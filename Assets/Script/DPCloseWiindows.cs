using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DPCloseWiindows : MonoBehaviour
{
    public GameObject pausePanel;
    private bool isPause = false;

    void OnESC(InputValue inputValue)
    {
        TogglePause();
    }
     
    void TogglePause() 
    {
        isPause = !isPause;
        pausePanel.SetActive(isPause);

        Time.timeScale = isPause ? 0f : 1f;
    }
    public void ReturnTotitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleSceme");
    }
}
