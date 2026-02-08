using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Audio_Data", menuName = "Game/Audio")]
public class Audio_Data : ScriptableObject
{
    [SerializeField, Header("音声データ")] public AudioClip Title;
    [SerializeField, Header("音声データ")] public AudioClip Talk;
    [SerializeField, Header("音声データ")] public AudioClip TagtheFlag;
    [SerializeField, Header("音声データ")] public AudioClip ResultFlag;
    [SerializeField, Header("音声データ")] public AudioClip kesibato;
    [SerializeField, Header("音声ごとの調整")] public float SetVolume;
}
