using UnityEngine;


//-------------------------------------------------------------------------------------------------------------

//-------------------------------------------------------------------------------------------------------------

[CreateAssetMenu(fileName = "CharacterText_Data", menuName = "Game/Text/Saved/CharacterText_Data")]
public class Character_Text_Data : ScriptableObject
{
    //ケースバイケースのテキストデータまとめ
    [SerializeField, Header("データのキャラクターネーム(確認用)")] private string Name;
    [SerializeField, Header("設定済みのテキストデータ")] public MakeConversation_Text_Data[] DataList;
}