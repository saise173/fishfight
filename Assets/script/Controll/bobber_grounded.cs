using UnityEngine;

public class bobber_grounded : MonoBehaviour
{
    private bool grounded = false;

    private Rigidbody2D rb;

    private Vector3 position;

    private LineRenderer lineRenderer;
    // private BoxCollider boxCol;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {   
        lineRenderer = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        position = transform.position;
        Debug.Log("玉ウキの現在地点:" + position);
        if (grounded == true)
        {
            rb.linearVelocity = Vector3.zero;
            rb.gravityScale = 0;
        }

        GameObject player1 = GameObject.Find("player1");
        if (player1 != null)
        {
            // 見つかったオブジェクトの座標を取得
            Vector3 player1Position = player1.transform.position;
            Debug.Log("player1の位置: " + player1Position);
        if (player1 != null && position != null)
        {
            // Line Rendererの開始点と終了点を設定
            lineRenderer.SetPosition(0, player1Position);
            lineRenderer.SetPosition(1, position);
        }                  
        }     
    
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            gameObject.layer = LayerMask.NameToLayer(default);
            grounded = true;
        }
        if (collision.gameObject.CompareTag("wall"))
        {
            gameObject.layer = LayerMask.NameToLayer(default);
            grounded = true;
        }
        if (collision.gameObject.CompareTag("player2"))
        {
            gameObject.layer = LayerMask.NameToLayer(default);
        }                             
    }    
}
