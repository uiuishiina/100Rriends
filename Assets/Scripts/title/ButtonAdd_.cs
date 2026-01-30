using UnityEngine;

public class ButtonAdd_ : MonoBehaviour
{
    [SerializeField] public GameObject Addbutton_;
    [SerializeField] private GameObject pale;
    public GameObject[] GameButtons; 

    void Start()
    {
        Addbutton_.SetActive(false);
        pale.SetActive(false);
        if(GameObject.Find("LogObject").GetComponent<LogObject>() != null)
        {
            var log = GameObject.Find("LogObject").GetComponent<LogObject>();
            if (log.Scenename_.Count != 0){
                Addbutton_.SetActive(true);
                foreach (var name in log.Scenename_)
                {
                    foreach (var button in GameButtons)
                    {
                        if (name == button.name)
                        {
                            button.SetActive(true);
                        }
                    }

                }
            }
        }
    }

    public void Addbutton_OnClick()
    {
        Addbutton_.SetActive(false);
        pale.SetActive(true);
    }

    public void GameButton(MakeConversation_Text_Data data)
    {
        GameObject.Find("CharacterTalkManager").GetComponent<CharacterTalkManager>().CTalk(data);
        pale.SetActive(false);
    }
}
