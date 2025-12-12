using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerInput player;
    [SerializeField] PlayerInput Ui;
    [SerializeField] PlayerInput Panel;
    [SerializeField] GameObject pausePanel;
    [SerializeField] Button Title;
    [SerializeField] Button Setting;
    private bool isPause = false;
    private bool S;
    private void Start()
    {
        var f = true;
        player.enabled = f;
        Ui.enabled = !f;
        Panel.enabled = !f;
        Title.onClick.AddListener(() => {
            SceneManager.LoadScene("TitleScene");
        });

        pausePanel.SetActive(false);
    }

    public void c(bool a)
    {
        player.enabled = a;
        Ui.enabled = !a;
    }

    private void P(bool pause)
    {
        if (pause) {
            Debug.LogWarning("F");
            S = player.enabled;
            player.enabled = false;
            Ui.enabled = false;
            Panel.enabled = true;
        }
        else {
            player.enabled = S;
            Ui.enabled = !S;
            Panel.enabled = false;
        }
    }
    public void OnESC(InputValue inputValue)
    {
        ESC();
    }

    public void ESC()
    {
        TogglePause();
    }

    void TogglePause()
    {
        isPause = !isPause;
        P(isPause);
        pausePanel.SetActive(isPause);
        Time.timeScale = isPause ? 0f : 1f;
    }
}
