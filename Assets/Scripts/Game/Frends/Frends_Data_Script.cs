using UnityEngine;

public class Frends_Data_Script : MonoBehaviour
{
    [SerializeField,Header("キャラクターデータ")]private Character_Data character_Data;
    [SerializeField, Header("会話データ")] private MakeConversation_Text_Data[] TextData;
    [SerializeField, Header("会話データ")] private MakeConversation_Text_Data ResultData;
    [SerializeField, Header("声をかけた回数")] public int count = 0;

    public MakeConversation_Text_Data GetTextData()
    {
        count++;
        if (count == 1) { return TextData[0]; }
        if(TextData.Length == 1) { return TextData[0]; }
        if (count == 2) { return TextData[1]; }
        if (TextData.Length == 2) { return TextData[1]; }
        return TextData[Random.Range(2, TextData.Length)];
    }
    public Character_Data GetCharaData()
    {
        return character_Data;
    }
    private void Start()
    {
        if (!character_Data.Body) { return; }
        var g = Instantiate(character_Data.Body, gameObject.transform);
        if (GameObject.Find("LogObject").GetComponent<LogObject>() != null) {
            var data = GameObject.Find("LogObject").GetComponent<LogObject>().FrendsNames_.Find(name => name.Item1 == character_Data.Name);
            if(data != default) {
                count = data.Item2;
            }
        }
    }
    private void OnDestroy()
    {
        GameObject.Find("LogObject").GetComponent<LogObject>().AddFrendsName(character_Data.Name,count);
    }
}
