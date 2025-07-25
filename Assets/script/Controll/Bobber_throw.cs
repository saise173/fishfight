using System.Data.SqlTypes;
using UnityEngine;

public class Bobber_throw : MonoBehaviour
{
    //---------------------------
    //          追加

    private Vector3 throwPoint;
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


    private Rigidbody2D rb;
    private SpriteRenderer player1_renderer;
    bool already_throw = false;
    // 弾のゲームオブジェクト
    //---------------------------


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player1_renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {   
        throwPoint = transform.Find("throw_point").localPosition;
        if (Input.GetKeyDown(duplicateKey))
        {
            DuplicateObjectWithInitialVelocity();
        }
        // Vector2 position = transform.position;
        // if (Input.GetKey(KeyCode.L) && already == false)
        // {
        //     // 弾の生成
        //     Instantiate(BobberObj, transform.position + throwPoint, Quaternion.identity);
        //     already = true;
        //     if (player1_renderer.flipX == true)
        //     {
        //         rb.AddForce(new Vector2(-0.5f * Bobber_right_and_left_speedBoost, 0.1f * Bobber_up_speedBoost), ForceMode2D.Impulse);
        //     }
        //     if (player1_renderer.flipX == false)
        //     {
        //         rb.AddForce(new Vector2(0.5f * Bobber_right_and_left_speedBoost, 0.1f * Bobber_up_speedBoost), ForceMode2D.Impulse);
        //     }
        // }
        // transform.position = position;
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

            GameObject duplicatedObject = Instantiate(objectToDuplicatePrefab, transform.position + throwPoint, Quaternion.identity);

            // 2. 複製されたオブジェクトのRigidbody2Dを取得する
            Rigidbody2D rb2d = duplicatedObject.GetComponent<Rigidbody2D>();

            if (rb2d != null)
            {
                // 3. Rigidbody2Dのvelocity（速度）を設定する
                // これにより、複製された瞬間に指定された速度が与えられる
                rb2d.linearVelocity = initialVelocity;
                Debug.Log(duplicatedObject.name + "を複製し、速度 " + initialVelocity + " を適用しました。");
            }
            else
            {
                Debug.LogError("複製されたオブジェクトにRigidbody2Dがアタッチされていません,速度を設定できませんでした。", duplicatedObject);
            }
        }
    }
}    

