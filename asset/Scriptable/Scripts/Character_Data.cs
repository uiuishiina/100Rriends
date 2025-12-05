using UnityEngine;


[CreateAssetMenu(fileName = "Character_Data", menuName = "Game/Character_Data")]
public class Character_Data : ScriptableObject
{
    [Header("キャラクターネーム")] public string Name;
    [Header("キャラクターの画像")] public Sprite Image;
    [Header("キャラクター好感度")] public int LikeValue;
}