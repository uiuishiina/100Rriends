using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAdd_ : MonoBehaviour
{
    [SerializeField] public GameObject Addbutton_;
    [SerializeField] private GameObject pale;
    [SerializeField] private LogObject Log;
    public GameObject[] GameButtons;
    

    void Start()
    {
        pale.SetActive(false);
        if(GameObject.Find("LogObject").GetComponent<LogObject>() != null)
        {
            Log = GameObject.Find("LogObject").GetComponent<LogObject>();
            Addbutton_.SetActive(true);
            //if (log.Scenename_.Count != 0){
            //    Addbutton_.SetActive(true);

            //    //foreach (var name in log.Scenename_)
            //    //{
            //    //    //foreach (var button in GameButtons)
            //    //    //{
            //    //    //    //if (name == button.name)
            //    //    //    //{
            //    //    //    //    button.SetActive(true);
            //    //    //    //}
            //    //    //}

            //    //}
            //}
        }
    }
    private void FixedUpdate()
    {
        if (Log != null) {
            
            ActiveButton(Log.FriendsCount_);
        }
        
    }
    public void ADDC(bool a)
    {
        Addbutton_.SetActive(a);
    }
    void ActiveButton(int count)
    {
        if (count >= 5)
        {
            
            GameButtons[0].SetActive(true);
        }
        if (count >= 35)
        {
            GameButtons[1].SetActive(true);
        }
        if(count >= 60)
        {

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
