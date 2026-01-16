using System.Collections;
using UnityEngine;
public class DPGameManager : MonoBehaviour
{
    public enum GameState {PlayerTurn, EnemyTurn, WaitMoving }
    public GameState currentState = GameState.PlayerTurn;

    public Rigidbody playerRb;
    public Rigidbody enemyRb;

    private float stopThreshold = 0.05f;
    private bool isObjectMoving = false;

    private void Update()
    {
        if (currentState == GameState.WaitMoving)
        {
            CheckObjectsStopped();
        }
    }
    public void StartPlayerMove()
    {
        if (currentState == GameState.PlayerTurn) 
        {
            isObjectMoving = true;
            currentState = GameState.WaitMoving;
            StartCoroutine(WaitAndChangeTurn(GameState.EnemyTurn));
        }
    }
    private void CheckObjectsStopped() 
    {
        bool playerStopped = playerRb.linearVelocity.magnitude < stopThreshold;
        bool enemyStopped = enemyRb.linearVelocity.magnitude < stopThreshold;

        if (playerStopped && enemyStopped && isObjectMoving) 
        {
            isObjectMoving = false;
        }
    }

    private IEnumerator WaitAndChangeTurn(GameState nextState)
    {
        yield return new WaitForSeconds(0.1f);

        while (playerRb.linearVelocity.magnitude > stopThreshold || enemyRb.linearVelocity.magnitude > stopThreshold)
        {
            yield return null;
        }
        currentState = nextState;

        if (currentState == GameState.EnemyTurn)
        {
            Debug.Log("敵のターン開始");
            EnemyMove();
        }
        else
        {
            Debug.Log("プレイヤーのターン開始");
        }
    }
    private void EnemyMove() 
    {
        Vector3 directionToPlayer = (playerRb.transform.position - enemyRb.transform.position).normalized;
        float randomForce = Random.Range(0.5f, 1.0f);
        float enemyForceMultiplier = 30f;

        enemyRb.AddForce(directionToPlayer * randomForce * enemyForceMultiplier, ForceMode.Impulse);

        StartCoroutine(WaitAndChangeTurn(GameState.PlayerTurn));
    }
}
             

