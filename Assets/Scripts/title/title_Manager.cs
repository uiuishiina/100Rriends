using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using Button = UnityEngine.UI.Button;


public class title_Manager : MonoBehaviour
{
    [SerializeField, Header("開始ボタン")] Button StartButton;
    [SerializeField, Header("設定ボタン")] Button SettingButton;
    [SerializeField, Header("戻りボタン")] Button BackButton;
    [SerializeField, Header("エンドボタン")] Button EndButton;
    [SerializeField] GameObject Panel;
    [SerializeField,Header("音量スライダー")] Slider ValueSlider;
    [SerializeField, Header("オンロード")] DontDestroy Dont;
    public GameObject panel;
    public Button PButton;
    //------  ------
    private void Start()
    {
        Panel.SetActive(false);
        panel.SetActive(false);
        StartButton.enabled = false;
        StartButton.onClick.AddListener(() => {
            SceneManager.LoadScene("GameScene");
        });
        SettingButton.onClick.AddListener(() => {
            Panel.SetActive(true);
        });
        BackButton.onClick.AddListener(() => { 
            Panel.SetActive(false); 
        });
        EndButton.onClick.AddListener(() => {
#if UNITY_EDITOR
            // エディタ上でゲームを停止
            UnityEditor.EditorApplication.isPlaying = false;
#else
            // ビルド後のアプリを終了
            Application.Quit();
#endif
        });
        //ValueSlider.onValueChanged.AddListener((float value) => { 
        //    Dont.data_.ChengeVolume(value);
        //});

        PButton.onClick.AddListener(() => { StartCoroutine(INVPanel()); });
    }

    IEnumerator INVPanel()
    {
        panel.SetActive(true);
        panel.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        for(int i = 0; i < 10; i++)
        {Debug.Log("消えたよ");
            panel.GetComponent<Image>().color += new Color(0, 0, 0, 0.1f);
            yield return new WaitForEndOfFrame();
        }
        StartButton.enabled = true;
        yield break;
    }
}
