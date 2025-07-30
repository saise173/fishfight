using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class player1_controll_beta : MonoBehaviour
{
    //[SerializeField, Header("玉の発射位置のオブジェクト名")]public string targetObjectName = "ex";
    [SerializeField, Header("左右の移動速度倍率")] private float right_and_left_speedBoost = 1;

    [SerializeField, Header("上下の移動速度倍率")] private float junp_speedBoost = 1;

    [SerializeField, Header("ジャンプ回数")] private int can_junp_count = 1;
    [SerializeField, Header("左右の最大速度(正の値でお願いします)")] private float right_and_left_max_speed = 10;

    [SerializeField, Header("下方向の最大速度(負の値でお願いします)")] private float donw_max_speed = -10;

    //[SerializeField, Header("ウキの左右移動速度倍率")] private float Bobber_right_and_left_speedBoost = 1;
    //[SerializeField, Header("ウキの上下移動速度倍率")] private float Bobber_up_speedBoost = 1;

    [Header("複製時に与える速度")]
    [Tooltip("複製されたオブジェクトに一度だけ与える速度")]
    public Vector2 initialVelocity = new Vector2(5f, 2f); // 右上方向に速度を与える例

    [Header("複製キー")]
    public KeyCode duplicateKey = KeyCode.L; // スペースキーで複製する例

    [Header("複製するオブジェクトのPrefab")]
    [Tooltip("複製したいオブジェクトのPrefabをここにドラッグ、ドロップしてください。Rigidbody2Dが必要です。")]
    public GameObject objectToDuplicatePrefab;

    public GameObject prefabToSpawn; // クローンする元のプレハブ
    public Collider2D targetCollider; // 衝突を無視したい相手のコライダーをInspectorから設定    

    private float speed = 0.05f;

    private int isGround;

    private bool look_right;

    private bool look_left;
    private Rigidbody2D rb;
    private SpriteRenderer player1_renderer;
    bool already_throw = false;

    private Vector3 right_throwPoint;

    private Vector3 left_throwPoint;

    GameObject duplicatedObject;

    // private BoxCollider boxCol;
    void Start()
    {
        float right_max_speed = right_and_left_max_speed;
        float left_max_speed = -right_and_left_max_speed;
        rb = GetComponent<Rigidbody2D>();
        player1_renderer = GetComponent<SpriteRenderer>();
        // Physics2D.IgnoreCollision(GetComponent<Collider2D>(), targetCollider, true);
        isGround = can_junp_count;
        // bobber_grounded bobber_grounded = GetComponent<bobber_grounded>();
        // GameObject otherGameObject = GameObject.Find("玉ウキ");
        // if (otherGameObject != null)
        // {
        //     bobber_grounded otherbobber = otherGameObject.GetComponent<bobber_grounded>();
        //     if (otherbobber != null)
        //     {
        //         Debug.Log("ScriptB (別のゲームオブジェクト): grounded = " + otherbobber.grounded);
        //     }
        //     else
        //     {
        //         Debug.LogError("OtherGameObject に ScriptA が見つかりません。");
        //     }
        // }        
    }

    void Update()
    {
        //GameObject targetObject = GameObject.Find(targetObjectName);
        Vector3 position = transform.position;
        right_throwPoint = transform.Find("throw_point_right").localPosition;
        left_throwPoint = transform.Find("throw_point_left").localPosition;
        //Debug.Log("現在のplayer1の現在位置" + position);
        observe();
        if (Input.GetKeyDown(duplicateKey))
        {
            DuplicateObjectWithInitialVelocity();
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (rb.linearVelocityX > -right_and_left_max_speed)
            {
                rb.AddForce(new Vector2(-0.5f * right_and_left_speedBoost, 0), ForceMode2D.Impulse);
                //position.x -= speed * speedBoost;
            }
            player1_renderer.flipX = true;
            look_right = true;
            look_left = false;
            //targetObject.transform.position =new Vector3(position.x-3,position.y, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (rb.linearVelocityX < right_and_left_max_speed)
            {
                rb.AddForce(new Vector2(0.5f * right_and_left_speedBoost, 0), ForceMode2D.Impulse);
                //position.x -= speed * speedBoost;
            }
            //position.x += speed * speedBoost;
            player1_renderer.flipX = false;
            look_left = true;
            look_right = false;
            //targetObject.transform.position =new Vector3(position.x+3,position.y, 0);
        }

        if (Input.GetKey(KeyCode.S))
        {
            player1_renderer.flipY = true;
            if (rb.linearVelocityY > donw_max_speed)
            {
                rb.AddForce(new Vector2(0, -2f * junp_speedBoost), ForceMode2D.Impulse);
            }
            else if (donw_max_speed < 0)
            {
                //rb.linearVelocityY = donw_max_speed;
            }
        }
        else
        {
            player1_renderer.flipY = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGround > 0)//&& isGround
        {
            rb.AddForce(new Vector2(0, 5f * junp_speedBoost), ForceMode2D.Impulse);
            isGround -= 1;
        }

        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            isGround = can_junp_count;
        }
        if (collision.gameObject.CompareTag("Bobber_player1"))
        {
            Destroy(collision.gameObject);
            already_throw = false;
            // 敵ヒット時の処理
        }
    }
    void DuplicateObjectWithInitialVelocity()
    {
        if (already_throw == false)
        {
            if (objectToDuplicatePrefab == null)
            {
                Debug.LogError("複製するPrefabが設定されていません");
                return;
            }

            // 1. オブジェクトを複製する
            // transform.position + (Vector3)spawnOffset: プレイヤーの位置からspawnOffset分ずらした位置
            // Quaternion.identity: 回転なし
            //GameObject duplicatedObject = Instantiate(objectToDuplicatePrefab, (Vector2)transform.position + spawnOffset, Quaternion.identity);
            if (look_left == true)
            {
                already_throw = true;
                GameObject duplicatedObject = Instantiate(objectToDuplicatePrefab, transform.position + right_throwPoint, Quaternion.identity);
                Vector3 bobboer_position = duplicatedObject.transform.position;
                Rigidbody2D rb2d = duplicatedObject.GetComponent<Rigidbody2D>();
                duplicatedObject.layer = LayerMask.NameToLayer("player1_bobber");
                if (rb2d != null && look_left == true)
                {
                    // 3. Rigidbody2Dのvelocity（速度）を設定する
                    // これにより、複製された瞬間に指定された速度が与えられる
                    rb2d.linearVelocityX = initialVelocity.x;
                    rb2d.linearVelocityY = initialVelocity.y;
                    Debug.Log(duplicatedObject.name + "を複製し、速度 X:" + initialVelocity.x + "    Y:" + initialVelocity.y + " を適用しました。");
                }
                else
                {
                    Debug.LogError("複製されたオブジェクトにRigidbody2Dがアタッチされていません,速度を設定できませんでした。", duplicatedObject);
                }
            }
            if (look_right == true)
            {
                already_throw = true;
                GameObject duplicatedObject = Instantiate(objectToDuplicatePrefab, transform.position + left_throwPoint, Quaternion.identity);
                Rigidbody2D rb2d = duplicatedObject.GetComponent<Rigidbody2D>();
                duplicatedObject.layer = LayerMask.NameToLayer("player1_bobber");
                if (rb2d != null && look_right == true)
                {
                    // 3. Rigidbody2Dのvelocity（速度）を設定する
                    // これにより、複製された瞬間に指定された速度が与えられる
                    rb2d.linearVelocityX = -initialVelocity.x;
                    rb2d.linearVelocityY = initialVelocity.y;
                    Debug.Log(duplicatedObject.name + "を複製し、速度 X:" + -initialVelocity.x + "    Y:" + initialVelocity.y + " を適用しました。");
                }
                else
                {
                    Debug.LogError("複製されたオブジェクトにRigidbody2Dがアタッチされていません,速度を設定できませんでした。", duplicatedObject);
                }
            }
        }
    }
    void observe()
    {
        // if (already_throw == true){
        // Vector3 bobboer_position =duplicatedObject.transform.position;
        // Debug.Log("玉ウキの現在地点:" + bobboer_position);
        // }
    }
}