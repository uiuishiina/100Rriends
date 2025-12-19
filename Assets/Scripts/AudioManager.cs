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
        var g = GameObject.Find("SceneManager");
        if (g == null) { Debug.Log("A"); return; }
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

        SetSE(SEData_);
        SetBGM(BGMData_);
        SetSliderValue();
    }

    void SetSE(Audio_Data data)
    {
        SESource.clip = data.Resource;
        SESource.volume = data.SetVolume;
    }
    void SetBGM(Audio_Data data)
    {
        BGMSource.clip = data.Resource;
        BGMSource.volume = data.SetVolume;
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
}
