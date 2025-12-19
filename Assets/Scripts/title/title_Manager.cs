using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class title_Manager : MonoBehaviour
{
    [SerializeField, Header("開始ボタン")] Button StartButton;
    [SerializeField, Header("設定ボタン")] Button SettingButton;
    [SerializeField, Header("戻りボタン")] Button BackButton;
    [SerializeField] GameObject Panel;
    [SerializeField,Header("音量スライダー")] Slider ValueSlider;
    [SerializeField, Header("オンロード")] DontDestroy Dont;
    //------  ------
    private void Start()
    {
        Panel.SetActive(false);
        StartButton.onClick.AddListener(() => {
            SceneManager.LoadScene("GameScene");
        });
        SettingButton.onClick.AddListener(() => {
            Panel.SetActive(true);
        });
        BackButton.onClick.AddListener(() => { 
            Panel.SetActive(false); 
        });
        //ValueSlider.onValueChanged.AddListener((float value) => { 
        //    Dont.data_.ChengeVolume(value);
        //});
    }
}
