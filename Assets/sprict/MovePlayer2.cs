using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer2 : MonoBehaviour
{
    [SerializeField, Header("移動速度")]
    private float speedBoost = 1;
    private float speed = 0.05f;

    private bool isGround = true;

    private Rigidbody2D rb;

    private SpriteRenderer player1_renderer;

    [SerializeField, Header("下方向の最大速度(負の値でお願いします)")] private float donw_max_speed = -10;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player1_renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector2 position = transform.position;
        if (Input.GetKey("left"))
        {
            position.x -= speed * speedBoost;
            player1_renderer.flipX = true;
        }
        if (Input.GetKey("right"))
        {
            position.x += speed * speedBoost;
            player1_renderer.flipX = false;
        }

        if (Input.GetKey("down"))
        {
            player1_renderer.flipY = true;
            if (rb.linearVelocityY > donw_max_speed)
            {
                rb.AddForce(new Vector2(0, -2f * speedBoost), ForceMode2D.Impulse);
            }
            else if(donw_max_speed<0)
            {
                rb.linearVelocityY = donw_max_speed;
            }
        }
        else
        {
            player1_renderer.flipY = false;
        }

        if (Input.GetKeyDown("up"))//&& isGround
        {
            rb.AddForce(new Vector2(0, 5f * speedBoost), ForceMode2D.Impulse);
            isGround = false;
        }

        transform.position = position;
    }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("floor"))
    //     {
    //         isGround = true;
    //     }
    // }
}