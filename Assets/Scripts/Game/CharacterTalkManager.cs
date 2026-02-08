using NUnit.Framework.Interfaces;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
    [SerializeField] Image PlayerImage;
    [SerializeField] Image FriendsImage;
    [SerializeField,Header("ゲーム遷移")] GameObject[] GameButton;
    [Header("機能参照")]
    [SerializeField] GameManager gameManager;
    [SerializeField] text_cs conttext;

    [Header("データ参照")]
    [SerializeField] MakeConversation_Text_Data Textdata;
    [SerializeField] Character_Data CharacterData;
    [SerializeField, Header("プレイヤー")] Character_Data Player_Data;
    [SerializeField] ButtonAdd_ a;
    public enum TextMeshProMode { TextMeshPro, TextMeshProUGUI, TMP_Text }

    private IEnumerator colti;
    //------  変数  ------
    bool isTalk = false;
    bool PushBool = false;
    int num;
    private void Start()
    {
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
        a.ADDC(false);
        Talk();
    }
    void Talk()
    {
        StartCoroutine(Manage());
    }
    public void CTalk(MakeConversation_Text_Data data) { 
        StopAllCoroutines();
        colti = null;
        gameManager.c(false);
        StartCoroutine(CCOL(data.Datas,data.UseImage_));
    }
    IEnumerator CCOL(Setting_Text_Data[] data,bool i)
    {
        TalkUI.SetActive(true);
        colti = col(data, CharacterData,i);
        yield return StartCoroutine(colti);
    }
    IEnumerator Manage(){
        TalkUI.SetActive(true);
        if (Player_Data.Image != null) { PlayerImage.sprite = Player_Data.Image[0]; }
        if (CharacterData.Image != null) { FriendsImage.sprite = CharacterData.Image[0]; }
        colti = col(Textdata.Datas, CharacterData, Textdata.UseImage_);
        yield return StartCoroutine(colti);
        TalkUI.SetActive(false);
        gameManager.c(true);
        yield break;
    }

    IEnumerator col(Setting_Text_Data[] data,Character_Data charaData,bool usei)
    {
        foreach (var item in data)
        {
            if (usei)
            {
                PlayerImage.enabled = true;
                FriendsImage.enabled = true;
                PlayerImage.color = new Color(0.5f, 0.5f, 0.5f, 1);
                FriendsImage.color = new Color(0.5f, 0.5f, 0.5f, 1);
                if (item.Side)
                {
                    PlayerImage.sprite = Player_Data.Image[item.CHImageNum_];
                    Nametext.text = Player_Data.Name;
                    PlayerImage.color = new Color(1, 1, 1, 1);
                }
                else
                {
                    FriendsImage.sprite = charaData.Image[item.CHImageNum_];
                    Nametext.text = charaData.Name;
                    FriendsImage.color = new Color(1, 1, 1, 1);
                }
            }
            else
            {
                PlayerImage.enabled = false;
                FriendsImage.enabled = false;
            }
                Debug.Log("cclo");
            yield return StartCoroutine(conttext.TextActive(Talktext, item.TextData));
            if (item.s != null) {
                left.text = item.s.Switch_Data[0].Switched_Title;
                right.text = item.s.Switch_Data[1].Switched_Title;
                PushBool = false;
                CButton(true);
                yield return new WaitUntil(() => PushBool);
                PushBool = false;
                CButton(false);
                yield return StartCoroutine(col(item.s.Switch_Data[num].Switched_Data,charaData,usei));
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
            if (item.UpF_) { GameObject.Find("LogObject").GetComponent<LogObject>().AddFrends(item.FUP_); }
        }
        yield break;
    }

    void CButton(bool set)
    {
        LeftButton.SetActive(set);
        RightButton.SetActive(set);
    }
}
