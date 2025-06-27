using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer2 : MonoBehaviour
{
    [SerializeField, Header("左右の移動速度倍率")]private float right_and_left_speedBoost = 1;

    [SerializeField, Header("上下の移動速度倍率")]private float junp_speedBoost = 1;

    [SerializeField, Header("ジャンプ回数")]private int can_junp_count = 1;
    private float speed = 0.05f;

    private int isGround;

    private Rigidbody2D rb;

    private SpriteRenderer player1_renderer;

    [SerializeField, Header("左右の最大速度(正の値でお願いします)")] private float right_and_left_max_speed = 10;

    [SerializeField, Header("下方向の最大速度(負の値でお願いします)")] private float donw_max_speed = -10;

    void Start()
    {
        float right_max_speed = right_and_left_max_speed;
        float left_max_speed = -right_and_left_max_speed;
        rb = GetComponent<Rigidbody2D>();
        player1_renderer = GetComponent<SpriteRenderer>();
        isGround = can_junp_count;
    }

    void Update()
    {
        Vector2 position = transform.position;
        if (Input.GetKey("left"))
        {
            if (rb.linearVelocityX >-right_and_left_max_speed) {
                rb.AddForce(new Vector2(-0.5f * right_and_left_speedBoost, 0), ForceMode2D.Impulse);
                //position.x -= speed * speedBoost;
            }
            player1_renderer.flipX = true;
        }
        if (Input.GetKey("right"))
        {   
            if (rb.linearVelocityX < right_and_left_max_speed) {
                rb.AddForce(new Vector2(0.5f * right_and_left_speedBoost, 0), ForceMode2D.Impulse);
                //position.x -= speed * speedBoost;
            }
            //position.x += speed * speedBoost;
            player1_renderer.flipX = false;
        }

        if (Input.GetKey("down"))
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

        if (Input.GetKeyDown("up") && isGround > 0)//&& isGround
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
    }
}