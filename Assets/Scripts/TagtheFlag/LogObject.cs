using UnityEngine;
using UnityEngine.SceneManagement;

public class LogObject : MonoBehaviour
{
    public static GameObject Instance_;
    public MakeConversation_Text_Data Data_;
    public Character_Data CData_;
    public bool RC_;
    public int count;
    private void Awake()
    {
        if (Instance_ == null)
        {
            Instance_ = this.gameObject;
            DontDestroyOnLoad(Instance_);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            var g = GameObject.Find("CharacterTalkManager");
            if (g == null) { Debug.Log("NotFundCTM"); return; }
            if(Data_ == null) { return; }
            g.GetComponent<CharacterTalkManager>().SetObject(Data_,CData_);
        }
    }

    public void SetDataScript(MakeConversation_Text_Data data, Character_Data c)
    {
        Data_ = data;CData_ = c;
    }
    public void ADDCOUNT()
    {
        count++;
    }
}
