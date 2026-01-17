using UnityEngine;

public class Frends_Data_Script : MonoBehaviour
{
    [SerializeField,Header("キャラクターデータ")]private Character_Data character_Data;
    [SerializeField, Header("会話データ")] private MakeConversation_Text_Data[] TextData;
    [SerializeField, Header("声をかけた回数")] public int count = 0;
    public MakeConversation_Text_Data GetTextData()
    {
        
        if (count == 0 ) { count++; return TextData[0]; }
        
        return TextData[Random.Range(1,TextData.Length)];
    }
    public Character_Data GetCharaData()
    {
        return character_Data;
    }
    private void Start()
    {
        if (!character_Data.Body) { return; }
        Instantiate(character_Data.Body,gameObject.transform);
    }
}
