using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{
    public static GameObject Instance_;
    [SerializeField,Header("使用するミキサー")] private AudioMixer AudioMixer;
    [SerializeField,Header("再生するSEソース")] private AudioSource SESource;
    [SerializeField,Header("再生するBGMソース")] private AudioSource BGMSource;
    [SerializeField, Header("SEデータ")] private Audio_Data SEData_;
    [SerializeField, Header("BGMデータ")] private Audio_Data BGMData_;
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SESlider;

    private void Awake()
    {
        if (Instance_ == null)
        {
            Instance_ = this.gameObject;
            DontDestroyOnLoad(Instance_);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BGMSource.Stop();
        SESource.Stop();
        var g = GameObject.Find("SceneManager");
        if (g == null) { Debug.Log("Not Found SceneManager"); return; }
        AudioClip UseSE = null;
        AudioClip UseBGM = null;
        if (scene.name == "TitleScene") { UseSE = SEData_.Title; UseBGM = BGMData_.Title; }
        if (scene.name == "GameScene") { UseSE = SEData_.Talk; UseBGM = BGMData_.Talk; }
        if (scene.name == "TagtheFlagScene") { UseSE = SEData_.TagtheFlag; UseBGM = BGMData_.TagtheFlag; }
        if (scene.name == "ResultScene") { UseSE = SEData_.ResultFlag; UseBGM = BGMData_.ResultFlag; }
        BGMSlider = g.GetComponent<ReferenceManager>().BGMSlider;
        SESlider = g.GetComponent<ReferenceManager>().SESlider;
        EventTrigger SETrigger = SESlider.gameObject.AddComponent<EventTrigger>();
        EventTrigger BGMTrigger = BGMSlider.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        entry.callback.AddListener((data) => OnSESliderReleased());
        SETrigger.triggers.Add(entry);
        BGMTrigger.triggers.Add(entry);
        SESlider.onValueChanged.AddListener((float value) => { ChangeVolume(value, SESource,SEData_); });
        BGMSlider.onValueChanged.AddListener((float value) => { ChangeVolume(value, BGMSource,BGMData_); });

        SetSE(UseSE, SEData_.SetVolume);
        SetBGM(UseBGM, BGMData_.SetVolume);
        SetSliderValue();
        BGMSource.Play();
    }

    void SetSE(AudioClip data,float value)
    {
        SESource.clip = data;
        SESource.volume = value;
    }
    void SetBGM(AudioClip data, float value)
    {
        BGMSource.clip = data;
        BGMSource.volume = value;
    }
    void SetSliderValue()
    {
        BGMSlider.value = BGMData_.SetVolume;
        SESlider.value = SEData_.SetVolume;
    }

    void ChangeVolume(float volume, AudioSource Source, Audio_Data data)
    {
        Source.volume = volume;
        data.SetVolume = volume;
    }
    private void OnSESliderReleased()
    {
        // スライダーを離したときにテストSEだけを鳴らす
        if (SESource != null && SESource.clip != null) {
            SESource.PlayOneShot(SESource.clip);
        }
        else {
            Debug.LogWarning("TestSE AudioSource または clip が設定されていません。");
        }
    }

    public void PLAYSE()
    {
        SESource.PlayOneShot(SESource.clip);
    }
}
