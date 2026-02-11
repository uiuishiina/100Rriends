using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DPGameManager : MonoBehaviour
{
    public static DPGameManager Instance;

    [Header("ゲーム設定")]
    [Tooltip("ゲームの制限時間（秒）")]
    public float gameTime = 30f;

    [Tooltip("スタートカウントダウンの時間（秒）")]
    public float startCountdownTime = 3f;

    [Header("敵の設定")]
    [Tooltip("敵のプレハブ")]
    public GameObject enemyPrefab;

    [Tooltip("同時に存在できる最大敵数")]
    public int maxEnemies = 10;

    [Tooltip("敵のリスポーン間隔（秒）")]
    public float enemyRespawnInterval = 2f;

    [Tooltip("敵の出現範囲 X")]
    public Vector2 enemySpawnRangeX = new Vector2(-5f, 5f);

    [Tooltip("敵の出現範囲 Z")]
    public Vector2 enemySpawnRangeZ = new Vector2(-5f, 5f);

    [Tooltip("敵の出現高さ Y")]
    public float enemySpawnHeight = 0f;

    [Header("スコア設定")]
    [Tooltip("敵を倒したときの獲得スコア")]
    public int scorePerEnemy = 10;

    // ゲーム状態
    public enum GameState
    {
        Tutorial,      // 説明画面
        Countdown,     // スタートカウントダウン
        Playing,       // ゲームプレイ中
        GameOver       // ゲーム終了
    }

    private GameState currentState = GameState.Tutorial;
    private float remainingTime;
    private float countdownTimer;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private Coroutine enemySpawnCoroutine;

    public bool IsPlaying => currentState == GameState.Playing;
    public float RemainingTime => remainingTime;
    public float CountdownTimer => countdownTimer;
    public GameState CurrentState => currentState;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        remainingTime = gameTime;
        SetGameState(GameState.Tutorial);
    }

    void Update()
    {
        switch (currentState)
        {
            case GameState.Countdown:
                UpdateCountdown();
                break;
            case GameState.Playing:
                UpdateGameTime();
                break;
        }
    }

    void UpdateCountdown()
    {
        countdownTimer -= Time.deltaTime;

        if (countdownTimer <= 0)
        {
            StartGame();
        }
    }

    void UpdateGameTime()
    {
        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0)
        {
            remainingTime = 0;
            EndGame(false);
        }
    }

    public void OnStartButtonPressed()
    {
        if (currentState == GameState.Tutorial)
        {
            SetGameState(GameState.Countdown);
            countdownTimer = startCountdownTime;
        }
    }

    void StartGame()
    {
        SetGameState(GameState.Playing);
        remainingTime = gameTime;
        enemySpawnCoroutine = StartCoroutine(SpawnEnemiesRoutine());
    }

    public void EndGame(bool playerFell)
    {
        if (currentState != GameState.Playing) return;

        SetGameState(GameState.GameOver);

        if (enemySpawnCoroutine != null)
        {
            StopCoroutine(enemySpawnCoroutine);
        }

        if (DPUIManager.Instance != null)
        {
            if (playerFell)
            {
                DPUIManager.Instance.ShowGameOver("プレイヤーが落下しました！");
            }
            else
            {
                DPUIManager.Instance.ShowGameOver("Time up");
            }
        }
    }

    void SetGameState(GameState newState)
    {
        currentState = newState;

        if (DPUIManager.Instance != null)
        {
            DPUIManager.Instance.UpdateGameState(currentState);
        }
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        // 初期の敵を生成
        for (int i = 0; i < maxEnemies; i++)
        {
            SpawnEnemy();
        }

        while (currentState == GameState.Playing)
        {
            yield return new WaitForSeconds(enemyRespawnInterval);

            // 敵の数が最大数より少ない場合、新しい敵を生成
            if (activeEnemies.Count < maxEnemies)
            {
                SpawnEnemy();
            }
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null) return;

        float randomX = Random.Range(enemySpawnRangeX.x, enemySpawnRangeX.y);
        float randomZ = Random.Range(enemySpawnRangeZ.x, enemySpawnRangeZ.y);

        Vector3 spawnPosition = new Vector3(randomX, enemySpawnHeight, randomZ);
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        activeEnemies.Add(enemy);
    }

    public void OnEnemyDestroyed(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
            DPScoreManager.Instance.AddScore(scorePerEnemy);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void MoveGameScene() {
        var g = GameObject.Find("AudioManager");
        if (g != null)
        {
            g.GetComponent<AudioManager>().PLAYSE();
            g.GetComponent<AudioManager>().stopbgm();
        }
        var G = GameObject.Find("LogObject");
        if (G != null) {
            G.GetComponent<LogObject>().AddFrends(DPScoreManager.Instance.CurrentScore/10);
        }
        SceneManager.LoadScene("GameScene");
    }
    public List<GameObject> GetActiveEnemies()
    {
        return new List<GameObject>(activeEnemies);
    }

    public GameObject GetPlayer()
    {
        return GameObject.FindGameObjectWithTag("Player");
    }
}

