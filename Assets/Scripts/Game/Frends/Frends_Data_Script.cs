using UnityEngine;

public class Frends_Data_Script : MonoBehaviour
{
    [SerializeField,Header("キャラクターデータ")]private Character_Data character_Data;
    [SerializeField, Header("会話データ")] private MakeConversation_Text_Data[] TextData;
    [SerializeField, Header("声をかけた回数")] public int count = 0;
    public MakeConversation_Text_Data GetTextData()
    {
        count++;
        if (count > 10) { return TextData[1]; }
        return TextData[0];
    }
    public Character_Data GetCharaData()
    {
        return character_Data;
    }
}
