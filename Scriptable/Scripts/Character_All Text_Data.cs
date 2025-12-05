using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllText_Data", menuName = "Game/Text/Saved/AllText_Data")]
public class Character_AllText_Data : ScriptableObject
{
    //データをまとめるやつ
    [SerializeField, Header("データのキャラクターネーム(確認用)")] private string Name;
    [SerializeField, Header("キャラクターテキストデータまとめ")] public Setting_Text_Data[] AllTextData;
}