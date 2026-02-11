using Kouya;
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
    //[SerializeField,Header("再生するSEソース")] private AudioSource SESource_;
    //[SerializeField,Header("再生するBGMソース")] private AudioSource BGMSource_;
    [SerializeField, Header("SEデータ")] private Audio_Data SEData_;
    [SerializeField, Header("BGMデータ")] private Audio_Data BGMData_;
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SESlider;
    private ReferenceManager manager_;

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
        AudioClip UseSE = null;
        AudioClip UseBGM = null;
        if (scene.name == "TitleScene") { UseSE = SEData_.Title; UseBGM = BGMData_.Title; }
        if (scene.name == "GameScene") { UseSE = SEData_.Talk; UseBGM = BGMData_.Talk; }
        if (scene.name == "TagtheFlagScene") { UseSE = SEData_.TagtheFlag; UseBGM = BGMData_.TagtheFlag; }
        if (scene.name == "ResultScene") { UseSE = SEData_.ResultFlag; UseBGM = BGMData_.ResultFlag; }
        if (scene.name == "kesibato") { UseSE = SEData_.kesibato; UseBGM = BGMData_.kesibato; }
        if (scene.name == "ChairScene") { 
            GameObject.Find("Audio Source").GetComponent<Chairgame_Music>().SetBGMVolume(BGMData_.SetVolume);
            return; }
        var g = GameObject.Find("SceneManager");
        if (g == null) { Debug.Log("Not Found SceneManager"); return; }
        manager_ = g.GetComponent<ReferenceManager>();
        Debug.Log(manager_.SESource);
        BGMSlider = manager_.BGMSlider;
        SESlider = manager_.SESlider;
        EventTrigger SETrigger = SESlider.gameObject.AddComponent<EventTrigger>();
        EventTrigger BGMTrigger = BGMSlider.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        entry.callback.AddListener((data) => OnSESliderReleased());
        SETrigger.triggers.Add(entry);
        BGMTrigger.triggers.Add(entry);
        SESlider.onValueChanged.AddListener((float value) => { ChangeVolume(value, manager_.SESource, SEData_); });
        BGMSlider.onValueChanged.AddListener((float value) => { ChangeVolume(value, manager_.BGMSource, BGMData_); });

        SetSE(UseSE, SEData_.SetVolume);
        SetBGM(UseBGM, BGMData_.SetVolume);
        SetSliderValue();
        manager_.BGMSource.Play();
    }

    void SetSE(AudioClip data,float value)
    {
        if (!manager_) { return; }
        manager_.SESource.clip = data;
        manager_.SESource.volume = value;
    }
    void SetBGM(AudioClip data, float value)
    {
        if (!manager_) { return; }
        manager_.BGMSource.clip = data;
        manager_.BGMSource.volume = value;
    }
    void SetSliderValue()
    {
        BGMSlider.value = BGMData_.SetVolume*10;
        SESlider.value = SEData_.SetVolume*10;
    }

    void ChangeVolume(float volume, AudioSource Source, Audio_Data data)
    {
        Source.volume = volume/10;
        data.SetVolume = volume/10;
    }
    private void OnSESliderReleased()
    {
        // スライダーを離したときにテストSEだけを鳴らす
        if (manager_.SESource != null && manager_.SESource.clip != null) {
            manager_.SESource.PlayOneShot(manager_.SESource.clip);
        }
        else {
            Debug.LogWarning("TestSE AudioSource または clip が設定されていません。");
        }
    }
    public void stopbgm()
    {
        manager_.BGMSource.Stop();
    }

    public void PLAYSE()
    {
        manager_.SESource.PlayOneShot(manager_.SEClip);
    }
}
