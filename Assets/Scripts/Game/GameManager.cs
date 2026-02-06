using System.Collections;
using TMPro;
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
    [SerializeField] Button Back;
    [SerializeField] GameObject SettingPanel;
    [SerializeField, Header("タイマー")] float Time_ = 0;
    [SerializeField, Header("タイマー")] TextMeshProUGUI TimeText_;
    [SerializeField] TextMeshProUGUI CountText_;
    [SerializeField] TextMeshProUGUI UpText_;
    LogObject Log;    
    bool end = false;
    private bool isPause = false;
    private bool S;
    private void Start()
    {
        var f = true;
        player.enabled = f;
        Ui.enabled = !f;
        Panel.enabled = !f;
        UpText_.enabled = false;
        Title.onClick.AddListener(() => {
            SceneManager.LoadScene("TitleScene");
        });
        Setting.onClick.AddListener(() => {
            SettingPanel.SetActive(true);
        });
        Back.onClick.AddListener(() => {
            SettingPanel.SetActive(false);
        });
        pausePanel.SetActive(false);
        SettingPanel.SetActive(false);
        Log = GameObject.Find("LogObject").GetComponent<LogObject>();
    }

    public void c(bool a)
    {
        player.enabled = a;
        Ui.enabled = !a;
    }

    private void P(bool pause)
    {
        if (pause) {
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
        SettingPanel.SetActive(false);
        Time.timeScale = isPause ? 0f : 1f;
    }
    void End()
    {
        SceneManager.LoadScene("ResultScene");
    }
    public void UpCount(int value)
    {
        UpText_.enabled = true;
        UpText_.text = "+" + value.ToString();
        StartCoroutine(COuntTi());
    }
    IEnumerator COuntTi()
    {
        UpText_.color = new Color(1, 1, 1, 0);
        for (int i = 0; i < 5; i++)
        {
            UpText_.color += new Color(0, 0, 0, 0.2f);
            yield return new WaitForSeconds(0.1f);
        }
        for (int i = 0; i < 5; i++)
        {
            UpText_.color -= new Color(0, 0, 0, 0.2f);
            yield return new WaitForSeconds(0.1f);
        }
        UpText_.enabled = false;
        CountText_.text = Log.FriendsCount_.ToString();
    }
    private void Update()
    {
        var min = ((int)Log.Time_ % 3600) / 60;
        var sec = (int)Log.Time_ % 60;
        TimeText_.text = min.ToString("D2") + ":" + sec.ToString("D2");
        
    }
}
