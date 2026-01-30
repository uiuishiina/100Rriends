using UnityEngine;

public class Frends_Data_Script : MonoBehaviour
{
    [SerializeField,Header("キャラクターデータ")]private Character_Data character_Data;
    [SerializeField, Header("会話データ")] private MakeConversation_Text_Data[] TextData;
    [SerializeField, Header("会話データ")] private MakeConversation_Text_Data ResultData;
    [SerializeField, Header("声をかけた回数")] public int count = 0;
    public MakeConversation_Text_Data GetTextData()
    {

        var g = GameObject.Find("LogObject");
        if(g!= null) {
            count = g.GetComponent<LogObject>().count;
            g.GetComponent<LogObject>().ADDCOUNT();
        }
        // 
        //var g = GameObject.Find("LogObject");
        //g.GetComponent<LogObject>().SetDataScript(ResultData, character_Data);
        if (count == 0) { return TextData[0]; }

        return TextData[Random.Range(1, TextData.Length)];
    }
    public Character_Data GetCharaData()
    {
        return character_Data;
    }
    private void Start()
    {
        if (!character_Data.Body) { return; }
        var g = Instantiate(character_Data.Body, gameObject.transform);
    }
}
