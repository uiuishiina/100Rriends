using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class title_Manager : MonoBehaviour
{
    [SerializeField, Header("開始ボタン")] Button StartButton;


    //------  ------
    private void Start()
    {
        StartButton.onClick.AddListener(() => {
            SceneManager.LoadScene("GameScene");
        });
    }
}
