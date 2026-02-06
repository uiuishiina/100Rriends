using UnityEngine;
using UnityEngine.InputSystem;

public class DPDragShoot : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 dragStartPos;
    private float forceMultiplier = 3f;
    [SerializeField] DPGameManager DgameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = 1f;
        rb.angularDamping = 1f;
        
    }
    void OnMouseDown()
    {
        // マウスの位置をワールド座標に変換して保存
        dragStartPos = GetMouseWorldPosition();
        Debug.Log("OnMD");
    }
    // マウスボタンが離されたとき
    void OnMouseUp()
    {
        Vector3 dragReleasePos = GetMouseWorldPosition();
        // 引っ張った方向（リリース - スタート）の逆方向がCubeの進む方向
        Vector3 forceDirection = dragStartPos - dragReleasePos;
        // 力を加える
        // ForceMode.Impulse は瞬間的な力を加えるのに適しています
        rb.AddForce(forceDirection * forceMultiplier, ForceMode.Impulse);
        //DgameManager.StartPlayerMove();
    }
    // マウスのスクリーン座標をワールド座標に変換するヘルパー関数
    private Vector3 GetMouseWorldPosition()
    {
        // Z座標はカメラからの距離（ステージの高さ）に合わせる
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.transform.position.y - transform.position.y; // Y軸を合わせる
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, transform.position); // Y=0 の平面
        if (groundPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return transform.position; // エラー時のフォールバック
    }
}