using UnityEngine;

[CreateAssetMenu(fileName = "Switch_Text_Data", menuName = "Game/Text/Setting/Switch")]
public class Switch : Setting_Text_Data
{
    [SerializeField, Header("分岐データ")] public Switch_Data[] Switch_Data;
}
[System.Serializable]
public class Switch_Data
{
    [SerializeField, Header("分岐のボタンの文字列")] public string Switched_Title;
    [SerializeField, Header("分岐後のテキストデータ")] public Setting_Text_Data[] Switched_Data;
}

//--------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------

//---------------------------------------------------------------------------------------------------------------

[CreateAssetMenu(fileName = "CharacterText_Data", menuName = "Game/Text/Saved/CharacterText_Data")]
public class Character_Text_Data : ScriptableObject
{
    //ケースバイケースのテキストデータまとめ
    [SerializeField, Header("データのキャラクターネーム(確認用)")] private string Name;
    [SerializeField, Header("設定済みのテキストデータ")] public MakeConversation_Text_Data[] DataList;
}