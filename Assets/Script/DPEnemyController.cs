using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DPEnemyController : MonoBehaviour
{
    [Header("発射設定")]
    [Tooltip("発射する力の強さ")]
    public float launchPower = 5f;

    [Tooltip("発射するまでの最小時間")]
    public float minLaunchInterval = 2f;

    [Tooltip("発射するまでの最大時間")]
    public float maxLaunchInterval = 5f;

    [Tooltip("ターゲット検索範囲")]
    public float detectionRange = 15f;

    [Tooltip("速度の減衰率（0-1、1に近いほど滑る）")]
    public float velocityDamping = 0.95f;

    [Tooltip("停止とみなす速度の閾値")]
    public float stopThreshold = 0.1f;

    [Header("落下検知")]
    [Tooltip("落下判定の高さ")]
    public float fallThreshold = -5f;

    private Rigidbody rb;
    private float launchTimer;
    private float nextLaunchTime;
    private bool isActive = false;
    private bool hasLaunched = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Rigidbodyの設定
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.linearDamping = 0f;

        // ゲーム開始前は物理演算を無効化
        rb.isKinematic = true;

        // 初期速度を完全にゼロに
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 最初の発射時間を設定
        nextLaunchTime = Random.Range(minLaunchInterval, maxLaunchInterval);
        launchTimer = 0f;
    }

    void Update()
    {
        // ゲームがプレイ中のみ動作
        isActive = DPGameManager.Instance != null && DPGameManager.Instance.IsPlaying;

        // ゲーム状態に応じてRigidbodyのKinematicを切り替え
        if (isActive && rb.isKinematic)
        {
            // ゲーム開始時に物理演算を有効化
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            //rb.angularVelocity = Vector3.zero;
        }
        else if (!isActive && !rb.isKinematic)
        {
            // ゲーム終了時に物理演算を無効化
            rb.isKinematic = true;
        }

        if (isActive)
        {
            // まだ発射していない、または停止している場合
            if (!hasLaunched || rb.linearVelocity.magnitude < stopThreshold)
            {
                launchTimer += Time.deltaTime;

                if (launchTimer >= nextLaunchTime)
                {
                    LaunchTowardsTarget();
                    launchTimer = 0f;
                    nextLaunchTime = Random.Range(minLaunchInterval, maxLaunchInterval);
                    hasLaunched = true;
                }
            }
        }

        // 落下判定
        if (transform.position.y < fallThreshold)
        {
            OnEnemyFell();
        }
    }

    void FixedUpdate()
    {
        // Kinematicモードの時は何もしない
        if (rb.isKinematic) return;

        // 速度を徐々に減衰
        rb.linearVelocity = new Vector3(
  rb.linearVelocity.x * velocityDamping,
  rb.linearVelocity.y,
  rb.linearVelocity.z * velocityDamping
);

        // 速度が閾値以下になったら完全停止
        if (rb.linearVelocity.magnitude < stopThreshold)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }

        // 移動方向を向く
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (horizontalVelocity.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(horizontalVelocity);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 10f);
        }
    }

    void LaunchTowardsTarget()
    {
        Vector3 targetDirection = Vector3.zero;

        // ランダムでプレイヤーまたは他の敵を狙う
        int choice = Random.Range(0, 2);

        if (choice == 0)
        {
            // プレイヤーを狙う
            targetDirection = GetDirectionToPlayer();
        }
        else
        {
            // 他の敵を狙う
            targetDirection = GetDirectionToNearestEnemy();
        }

        // ターゲットが見つからない場合はランダムな方向
        if (targetDirection == Vector3.zero)
        {
            targetDirection = GetRandomDirection();
        }

        // ランダムな角度のブレを追加（完璧に狙わない）
        float randomAngle = Random.Range(-15f, 15f);
        targetDirection = Quaternion.Euler(0, randomAngle, 0) * targetDirection;

        // 発射
        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0); // 現在の速度をリセット
        rb.linearVelocity = targetDirection * launchPower;
    }
    //velocity
    Vector3 GetDirectionToPlayer()
    {
        GameObject player = DPGameManager.Instance.GetPlayer();

        if (player != null)
        {
            Vector3 direction = (player.transform.position - transform.position);
            direction.y = 0;

            if (direction.magnitude <= detectionRange)
            {
                return direction.normalized;
            }
        }

        return Vector3.zero;
    }

    Vector3 GetDirectionToNearestEnemy()
    {
        List<GameObject> enemies = DPGameManager.Instance.GetActiveEnemies();
        GameObject nearestEnemy = null;
        float nearestDistance = detectionRange;

        foreach (GameObject enemy in enemies)
        {
            if (enemy != null && enemy != gameObject)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = enemy;
                }
            }
        }

        if (nearestEnemy != null)
        {
            Vector3 direction = (nearestEnemy.transform.position - transform.position);
            direction.y = 0;
            return direction.normalized;
        }

        return Vector3.zero;
    }

    Vector3 GetRandomDirection()
    {
        float randomAngle = Random.Range(0f, 360f);
        return Quaternion.Euler(0, randomAngle, 0) * Vector3.forward;
    }

    void OnCollisionEnter(Collision collision)
    {
        // プレイヤーまたは敵との衝突で速度を減衰
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            rb.linearVelocity = rb.linearVelocity * 0.3f;
        }
    }

    void OnEnemyFell()
    {
        // GameManagerに通知
        if (DPGameManager.Instance != null)
        {
            DPGameManager.Instance.OnEnemyDestroyed(gameObject);
        }

        // 敵を破壊
        Destroy(gameObject);
    }
}