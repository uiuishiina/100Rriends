using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
public class CharacterTalkManager : MonoBehaviour
{
    //------  参照  ------
    [Header("UI参照")]
    [SerializeField] GameObject TalkUI;
    [SerializeField] TextMeshProUGUI Talktext;
    [SerializeField] TextMeshProUGUI Nametext;
    [SerializeField] GameObject LeftButton;
    [SerializeField] TextMeshProUGUI left;
    [SerializeField] GameObject RightButton;
    [SerializeField] TextMeshProUGUI right;
    [Header("機能参照")]
    [SerializeField] GameManager gameManager;
    [SerializeField] text_cs conttext;

    [Header("データ参照")]
    [SerializeField] MakeConversation_Text_Data Textdata;
    [SerializeField] Character_Data CharacterData;
    [SerializeField, Header("プレイヤー")] Character_Data Player_Data;
    public enum TextMeshProMode { TextMeshPro, TextMeshProUGUI, TMP_Text }

    //------  変数  ------
    bool isTalk = false;
    bool PushBool = false;
    int num;
    private void Start()
    {
        //StartCoroutine(col(data.Datas));
        LeftButton.GetComponent<Button>().onClick.AddListener(() => { Click(0); });
        RightButton.GetComponent<Button>().onClick.AddListener(() => { Click(1); });
        CButton(false);
        TalkUI.SetActive(false);
    }
    void Click(int i){
        num = i;
        PushBool = true;
    }
    void OnSpace(InputValue input){
        isTalk = true;
    }
    public void OnESC(InputValue inputValue)
    {
        gameManager.ESC();
    }

    public void SetObject(MakeConversation_Text_Data d, Character_Data c)
    {
        if (d == null) { Debug.LogWarning("TextDataNull"); }
        if (c == null) { Debug.LogWarning("CharaDataNull"); }
        Textdata = d;
        CharacterData = c;
        gameManager.c(false);
        Talk();
    }
    void Talk()
    {
        StartCoroutine(Manage());
    }
    IEnumerator Manage(){
        TalkUI.SetActive(true);
        yield return StartCoroutine(col(Textdata.Datas,CharacterData));
        TalkUI.SetActive(false);
        gameManager.c(true);
        yield break;
    }

    IEnumerator col(Setting_Text_Data[] data,Character_Data charaData)
    {
        foreach (var item in data)
        {
            if (item.Side)  {
                Nametext.text = Player_Data.Name;
            }
            else  {
                Nametext.text = CharacterData.Name;
            }
                yield return StartCoroutine(conttext.TextActive(Talktext, item.TextData));
            if (item.s != null) {
                left.text = item.s.Switch_Data[0].Switched_Title;
                right.text = item.s.Switch_Data[1].Switched_Title;
                PushBool = false;
                CButton(true);
                yield return new WaitUntil(() => PushBool);
                PushBool = false;
                CButton(false);
                yield return StartCoroutine(col(item.s.Switch_Data[num].Switched_Data,charaData));
            }
            else
            {
                isTalk = false;
                yield return new WaitUntil(() => isTalk);
                isTalk = false;
            }
            if (item.IsLoad)
            {
                SceneManager.LoadScene(item.SceneName);
            }
        }
        yield break;
    }

    void CButton(bool set)
    {
        LeftButton.SetActive(set);
        RightButton.SetActive(set);
    }
}
