using UnityEngine;
using System.Collections.Generic; // Listを使用するために必要

public class PlayerPlatformHandler : MonoBehaviour
{
    [Header("地面判定用レイヤー")]
    [Tooltip("プレイヤーが地面と判定するレイヤーを設定してください")]
    public LayerMask groundLayer;

    [Header("足場判定用レイヤー")]
    [Tooltip("足場として判定するオブジェクトのレイヤーを設定してください")]
    public LayerMask platformLayer;

    [Header("当たり判定を消すキー")]
    [Tooltip("足場の当たり判定を無効にするキーを設定してください")]
    public KeyCode disablePlatformKey = KeyCode.S; // デフォルトはSキー

    [Header("接地判定用設定")]
    [Tooltip("地面判定を行うためのOffset（プレイヤーの中心からのオフセット）")]
    public Vector2 groundCheckOffset = new Vector2(0f, -0.55f); // プレイヤーの足元に合わせる
    [Tooltip("地面判定を行うためのRadius")]
    public float groundCheckRadius = 0.3f; // 円の半径

    private bool isGrounded;
    private List<Collider2D> currentPlatforms = new List<Collider2D>(); // 現在触れている足場のコライダーを保持

    void Update()
    {
        CheckIsGrounded();

        if (isGrounded && Input.GetKey(disablePlatformKey))
        {
            // 地面に触れていて、かつ指定のキーが押されている間
            foreach (Collider2D platformCollider in currentPlatforms)
            {
                if (platformCollider != null && platformLayer == (platformLayer | (1 << platformCollider.gameObject.layer)))
                {
                    // 足場レイヤーに属するコライダーのみ無効化
                    platformCollider.enabled = false;
                }
            }
        }
        else
        {
            // キーが離されたか、地面から離れた場合
            foreach (Collider2D platformCollider in currentPlatforms)
            {
                if (platformCollider != null && platformLayer == (platformLayer | (1 << platformCollider.gameObject.layer)))
                {
                    // 足場レイヤーに属するコライダーのみ有効化
                    platformCollider.enabled = true;
                }
            }
        }
    }

    private void CheckIsGrounded()
    {
        // Physics2D.OverlapCircleAll を使用して、プレイヤーの足元にあるコライダーをすべて検出
        Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2)transform.position + groundCheckOffset, groundCheckRadius, groundLayer);
        isGrounded = colliders.Length > 0;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // プレイヤーが足場と衝突した際に、その足場のコライダーをリストに追加
        if (platformLayer == (platformLayer | (1 << collision.gameObject.layer)))
        {
            if (!currentPlatforms.Contains(collision.collider))
            {
                currentPlatforms.Add(collision.collider);
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // プレイヤーが足場から離れた際に、その足場のコライダーをリストから削除
        if (platformLayer == (platformLayer | (1 << collision.gameObject.layer)))
        {
            if (currentPlatforms.Contains(collision.collider))
            {
                currentPlatforms.Remove(collision.collider);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // シーンビューで接地判定用のGizmoを表示
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere((Vector2)transform.position + groundCheckOffset, groundCheckRadius);
    }
}