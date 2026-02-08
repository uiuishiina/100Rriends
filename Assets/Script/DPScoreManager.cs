using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPScoreManager : MonoBehaviour
{
    public static DPScoreManager Instance;

    private int currentScore = 10;

    public int CurrentScore => currentScore;

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

    public void AddScore(int points)
    {
        currentScore += points;

        if (DPUIManager.Instance != null)
        {
            DPUIManager.Instance.UpdateScore(currentScore);
        }
    }

    public void ResetScore()
    {
        currentScore = 0;

        if (DPUIManager.Instance != null)
        {
            DPUIManager.Instance.UpdateScore(currentScore);
        }
    }
}
