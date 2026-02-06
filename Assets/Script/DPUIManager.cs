using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DPUIManager : MonoBehaviour
{
    public static DPUIManager Instance;

    [Header("UI パネル")]
    public GameObject tutorialPanel;
    public GameObject gameplayPanel;
    public GameObject gameOverPanel;

    [Header("チュートリアル画面")]
    public TextMeshProUGUI tutorialText;
    public Button startButton;

    [Header("ゲームプレイ画面")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI countdownText;

    [Header("ゲームオーバー画面")]
    public TextMeshProUGUI gameOverMessageText;
    public TextMeshProUGUI finalScoreText;
    public Button restartButton;
    public Button MoveScene;

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
        // ボタンのイベント設定
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnRestartButtonClicked);
        }
        if (MoveScene != null)
        {
            MoveScene.onClick.AddListener(OnMoveSceneButtonClicked);
        }


        // 初期状態を設定
        UpdateGameState(DPGameManager.GameState.Tutorial);
    }

    void Update()
    {
        if (DPGameManager.Instance != null)
        {
            // カウントダウン中の表示更新
            if (DPGameManager.Instance.CurrentState == DPGameManager.GameState.Countdown)
            {
                UpdateCountdown(DPGameManager.Instance.CountdownTimer);
            }
            // ゲームプレイ中のタイマー更新
            else if (DPGameManager.Instance.CurrentState == DPGameManager.GameState.Playing)
            {
                UpdateTimer(DPGameManager.Instance.RemainingTime);

                // カウントダウンテキストを非表示
                if (countdownText != null)
                {
                    countdownText.gameObject.SetActive(false);
                }
            }
        }
    }

    public void UpdateGameState(DPGameManager.GameState state)
    {
        // すべてのパネルを非表示
        if (tutorialPanel != null) tutorialPanel.SetActive(false);
        if (gameplayPanel != null) gameplayPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        // 状態に応じてパネルを表示
        switch (state)
        {
            case DPGameManager.GameState.Tutorial:
                if (tutorialPanel != null) tutorialPanel.SetActive(true);
                SetupTutorial();
                break;

            case DPGameManager.GameState.Countdown:
            case DPGameManager.GameState.Playing:
                if (gameplayPanel != null) gameplayPanel.SetActive(true);
                if (state == DPGameManager.GameState.Countdown && countdownText != null)
                {
                    countdownText.gameObject.SetActive(true);
                }
                break;

            case DPGameManager.GameState.GameOver:
                if (gameOverPanel != null) gameOverPanel.SetActive(true);
                break;
        }
    }

    void SetupTutorial()
    {
        if (tutorialText != null)
        {
            tutorialText.text =
                "<size=36><b>ゲームルール</b></size>\n\n" +
                "<b>目標:</b> 敵をフィールドから落として高得点を目指そう!\n\n" +
                "<b>操作:</b>\n" +
                "• プレイヤーをクリックして引っ張る\n" +
                "• 飛ばしたい方向の逆にドラッグ\n" +
                "• 離すと飛んでいきます！\n\n" +
                "<b>ルール:</b>\n" +
                "• 制限時間は30秒\n" +
                "• 敵を落とすとスコア獲得\n" +
                "• プレイヤーが落ちるとゲームオーバー\n" +
                "• 敵とぶつかると速度が落ちる\n\n" +
                "<b>準備ができたら「スタート」を押してください</b>";
        }
    }

    void OnStartButtonClicked()
    {
        if (DPGameManager.Instance != null)
        {
            DPGameManager.Instance.OnStartButtonPressed();
        }
    }

    void OnRestartButtonClicked()
    {
        if (DPGameManager.Instance != null)
        {
            DPGameManager.Instance.RestartGame();
        }
    }
    public void OnMoveSceneButtonClicked()
    {
        if (DPGameManager.Instance != null)
        {
            DPGameManager.Instance.MoveGameScene();
        }
    }

    public void UpdateTimer(float timeRemaining)
    {
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(timeRemaining);
            timerText.text = $"Timer: {seconds}";

            // 残り時間が少ない場合は色を変更
            if (seconds <= 10)
            {
                timerText.color = Color.red;
            }
            else
            {
                timerText.color = Color.white;
            }
        }
    }

    public void UpdateCountdown(float countdown)
    {
        if (countdownText != null)
        {
            int countdownInt = Mathf.CeilToInt(countdown);

            if (countdownInt > 0)
            {
                countdownText.text = countdownInt.ToString();
                countdownText.fontSize = 120;
                countdownText.color = Color.yellow;
            }
            else
            {
                countdownText.text = "GO!";
                countdownText.fontSize = 100;
                countdownText.color = Color.green;
            }
        }
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"スコア: {score}";
        }
    }

    public void ShowGameOver(string message)
    {
        if (gameOverMessageText != null)
        {
            gameOverMessageText.text = message;
        }

        if (finalScoreText != null && DPScoreManager.Instance != null)
        {
            finalScoreText.text = $"最終スコア: {DPScoreManager.Instance.CurrentScore}";
        }
    }
}