using UnityEngine;

[CreateAssetMenu(fileName = "Setting_Text_Data", menuName = "Game/Text/Setting/Normal")]
public class Setting_Text_Data : ScriptableObject
{
    [SerializeField, Header("テキストデータ")] public string TextData;
    [SerializeField, Header("サイド(主人公がしゃべるときtrue(仮))")] public bool Side;
    public Switch s;
    public bool IsLoad = false;
    [SerializeField, Header("シーンの名前")] public string SceneName = null;
}
