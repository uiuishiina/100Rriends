using UnityEngine;

public class Frends_Data_Script : MonoBehaviour
{
    [SerializeField,Header("キャラクターデータ")]private Character_Data character_Data;
    [SerializeField, Header("会話データ")] private MakeConversation_Text_Data TextData;

    public MakeConversation_Text_Data GetTextData()
    {
        return TextData;
    }
    public Character_Data GetCharaData()
    {
        return character_Data;
    }
}
