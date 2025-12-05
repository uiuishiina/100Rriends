using UnityEngine;

[CreateAssetMenu(fileName = "MakeConversation_Text_Data", menuName = "Game/Text/MakeConversation_Text_Data")]
public class MakeConversation_Text_Data : ScriptableObject
{
    [SerializeField, Header("会話シーンのタイトル")] public string SceneTitle;
    [SerializeField, Header("設定したテキストデータ")] public Setting_Text_Data[] Datas;
}

