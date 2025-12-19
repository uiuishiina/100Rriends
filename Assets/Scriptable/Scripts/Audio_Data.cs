using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Audio_Data", menuName = "Game/Audio")]
public class Audio_Data : ScriptableObject
{
    [SerializeField, Header("Й╣Р║ГfБ[Г^")] public AudioClip Resource;
    [SerializeField, Header("Й╣Р║В▓В╞В╠Т▓Ро")] public float SetVolume;
}
