using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]

public class DPPlayer : MonoBehaviour
{
    [Header("移動設定")]
    [Tooltip("ドラッグの力の倍率")]
    public float dragPowerMultiplier = 0.1f;
    [Tooltip("最大ドラッグ距離（ピクセル）")]
    public float maxDragDistance = 200f;
    [Tooltip("速度の減衰率（0-1、1に近いほど滑る）")]
    public float velocityDamping = 0.95f;
    [Tooltip("停止とみなす速度の閾値")]
    public float stopThreshold = 0.1f;
    [Header("落下検知")]
    [Tooltip("落下判定の高さ")]
    public float fallThreshold = -5f;
    [Header("ドラッグ表示")]
    [Tooltip("ドラッグライン表示")]
    public bool showDragLine = true;
    [Tooltip("ドラッグライン色")]
    public Color dragLineColor = Color.yellow;
    private Rigidbody rb;
    private bool canMove = false;
    // マウスドラッグ用の変数
    private bool isDragging = false;
    private Vector3 dragStartPosition;
    private Vector3 dragStartWorldPosition;
    private Camera mainCamera;
    private LineRenderer lineRenderer;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        // Rigidbodyの設定
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.linearDamping = 0f; // 空気抵抗は使わず、自分で減衰を制御
        // 初期速度を完全にゼロに
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        // LineRendererの設定（ドラッグライン表示用）
        if (showDragLine)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = dragLineColor;
            lineRenderer.endColor = dragLineColor;
            lineRenderer.positionCount = 2;
            lineRenderer.enabled = false;
        }
    }
    void Update()
    {
        // ゲームがプレイ中のみ移動可能
        canMove = DPGameManager.Instance != null && DPGameManager.Instance.IsPlaying;
        {
            isDragging = false;
            if (lineRenderer != null)
            {
                lineRenderer.enabled = false;
            }
        }
        // 落下判定

        if (transform.position.y < fallThreshold)
        {
            OnPlayerFell();
        }

    }



    void FixedUpdate()
    {
        // ゲームがプレイ中でない場合は速度をゼロに固定
        if (!canMove) {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            rb.angularVelocity = Vector3.zero;
            return;
        }
        if (!isDragging) {
            // 速度を徐々に減衰
            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x * velocityDamping,
                rb.linearVelocity.y,
                rb.linearVelocity.z * velocityDamping
            );

            // 速度が閾値以下になったら完全停止
            if (rb.linearVelocity.magnitude < stopThreshold){
                rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            }

            // 移動方向を向く
            Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            if (horizontalVelocity.magnitude > 0.1f) {
                Quaternion targetRotation = Quaternion.LookRotation(horizontalVelocity);
                rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 10f);
            }
        }
    }

    public void OnLeftButton(InputAction.CallbackContext context)
    {
        Debug.Log("押し始め");
        if (!canMove) { return; }
        if (context.started)
        {
            
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            // プレイヤーをクリックしたかチェック
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    isDragging = true;
                    dragStartPosition = Mouse.current.position.ReadValue();
                    dragStartWorldPosition = transform.position;
                    // 現在の速度をリセット
                    rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
                    if (lineRenderer != null)
                    {
                        lineRenderer.enabled = true;
                    }
                }
            }
        }
        if (context.performed)
        {
            Debug.Log("長押し確定！");
            Vector3 currentMousePos = Mouse.current.position.ReadValue();
            Vector3 dragDelta = dragStartPosition - currentMousePos;
            // ドラッグ距離を制限
            if (dragDelta.magnitude > maxDragDistance)
            {
                dragDelta = dragDelta.normalized * maxDragDistance;
            }
            Vector3 worldDragEnd = dragStartWorldPosition + ScreenToWorldDirection(dragDelta) * 0.01f;
            lineRenderer.SetPosition(0, transform.position + Vector3.up * 0.5f);
            lineRenderer.SetPosition(1, worldDragEnd + Vector3.up * 0.5f);
        }
        if (context.canceled)
        {
            Debug.Log("離した");
            isDragging = false;
            // ドラッグ方向と距離を計算
            Vector3 dragEndPosition = Mouse.current.position.ReadValue();
            Vector3 dragDelta = dragStartPosition - dragEndPosition; // 引っ張った方向の逆
            // ドラッグ距離を制限
            float dragDistance = dragDelta.magnitude;
            if (dragDistance > maxDragDistance)
            {
                dragDelta = dragDelta.normalized * maxDragDistance;
                dragDistance = maxDragDistance;
            }
            // スクリーン座標をワールド座標の力に変換
            Vector3 force = ScreenToWorldDirection(dragDelta) * dragPowerMultiplier;
            // 力を加える
            rb.linearVelocity = new Vector3(force.x, rb.linearVelocity.y, force.z);
            if (lineRenderer != null)
            {
                lineRenderer.enabled = false;
            }
        }
    }

    //void HandleMouseInput()
    //{
    //    // マウスボタンが押された時
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit;

    //        // プレイヤーをクリックしたかチェック
    //        if (Physics.Raycast(ray, out hit))
    //        {
    //            if (hit.collider.gameObject == gameObject)
    //            {
    //                isDragging = true;
    //                dragStartPosition = Input.mousePosition;
    //                dragStartWorldPosition = transform.position;
    //                // 現在の速度をリセット
    //                rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
    //                if (lineRenderer != null)
    //                {
    //                    lineRenderer.enabled = true;
    //                }
    //            }
    //        }
    //    }
    //    // マウスボタンが離された時
    //    else if (Input.GetMouseButtonUp(0) && isDragging)
    //    {
    //        isDragging = false;
    //        // ドラッグ方向と距離を計算
    //        Vector3 dragEndPosition = Input.mousePosition;
    //        Vector3 dragDelta = dragStartPosition - dragEndPosition; // 引っ張った方向の逆
    //        // ドラッグ距離を制限
    //        float dragDistance = dragDelta.magnitude;
    //        if (dragDistance > maxDragDistance) {
    //            dragDelta = dragDelta.normalized * maxDragDistance;
    //            dragDistance = maxDragDistance;
    //        }
    //        // スクリーン座標をワールド座標の力に変換
    //        Vector3 force = ScreenToWorldDirection(dragDelta) * dragPowerMultiplier;
    //        // 力を加える
    //        rb.linearVelocity = new Vector3(force.x, rb.linearVelocity.y, force.z);
    //        if (lineRenderer != null) {
    //            lineRenderer.enabled = false;
    //        }
    //    }
    //    // ドラッグ中の表示更新
    //    if (isDragging && lineRenderer != null){
    //        Vector3 currentMousePos = Input.mousePosition;
    //        Vector3 dragDelta = dragStartPosition - currentMousePos;
    //        // ドラッグ距離を制限
    //        if (dragDelta.magnitude > maxDragDistance) {
    //            dragDelta = dragDelta.normalized * maxDragDistance;
    //        }
    //        Vector3 worldDragEnd = dragStartWorldPosition + ScreenToWorldDirection(dragDelta) * 0.01f;
    //        lineRenderer.SetPosition(0, transform.position + Vector3.up * 0.5f);
    //        lineRenderer.SetPosition(1, worldDragEnd + Vector3.up * 0.5f);
    //    }
    //}
    
    Vector3 ScreenToWorldDirection(Vector3 screenDelta) {
        if (mainCamera == null) return Vector3.zero;
        // カメラの「右方向」と「上方向」を取得
        Vector3 cameraRight = mainCamera.transform.right;
        Vector3 cameraUp = mainCamera.transform.up;
        // 地面(XZ平面)で動かしたいので、高さ成分(y)を0にして平坦にする
        cameraRight.y = 0f;
        cameraUp.y = 0f;
        // 正規化（角度によって入力が弱くなるのを防ぐ）
        cameraRight.Normalize();
        cameraUp.Normalize();
        // 画面の左右ドラッグ(x)をカメラの右方向に、
        // 画面の上下ドラッグ(y)をワールドの奥行き方向（カメラの頭上の向き）に割り当てる
        return (cameraRight * screenDelta.x) + (cameraUp * screenDelta.y);
    }
    void OnCollisionEnter(Collision collision) {
        // プレイヤーまたは敵との衝突で速度を減衰
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player")) {
            // 衝突時に速度を大幅に減衰
            rb.linearVelocity = rb.linearVelocity * 0.3f;
        }
    }
    void OnPlayerFell() {
        if (DPGameManager.Instance != null) {
            DPGameManager.Instance.EndGame(true);
        }
        // プレイヤーの操作を無効化
        canMove = false;
    }
}