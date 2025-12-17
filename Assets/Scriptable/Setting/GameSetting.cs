using UnityEngine;

public class GameSetting : ScriptableObject
{
    [SerializeField, Header("‰¹—Ê")] public float Volume = 0;
    public void ChengeVolume(float value)
    {
        Volume = value;
    }
}