using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public Transform object1; // 1つ目のオブジェクト
    public Transform object2; // 2つ目のオブジェクト

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component not found on this GameObject.");
            enabled = false; // スクリプトを無効にする
        }

        //幅の設定
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;

        //色の設定        
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
    }

    void Update()
    {
        // GameObject player1 = GameObject.Find("player1");
        // // 見つかったオブジェクトの座標を取得
        // Vector3 player1Position = player1.transform.position;
        // Debug.Log("player1の位置: " + player1Position);
        GameObject bobboer = GameObject.Find("玉ウキ");
        // // 見つかったオブジェクトの座標を取得
        Vector3 bobboerPosition = bobboer.transform.position;
        Debug.Log("bobberの位置: " + bobboerPosition);
        if (object1 != null && object2 != null)
        {
            // Line Rendererの開始点と終了点を設定
            lineRenderer.SetPosition(0, object1.position);
            lineRenderer.SetPosition(1, bobboerPosition);
            //幅の設定
            lineRenderer.startWidth = 0.5f;
            lineRenderer.endWidth = 0.5f;

            //色の設定        
            lineRenderer.startColor = Color.black;
            lineRenderer.endColor = Color.black;            
        }
        // if (bobboerPosition != null && player1Position != null)
        // {
        //     // Line Rendererの開始点と終了点を設定
        //     lineRenderer.SetPosition(0, player1Position);
        //     lineRenderer.SetPosition(1, bobboerPosition);
        // }             
    }
}