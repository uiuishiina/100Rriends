using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Mono.Cecil.Cil;
using UnityEngine.UIElements;
public class PlayerTalkController : MonoBehaviour
{
    [SerializeField] CharacterTalkManager manager;
    private MakeConversation_Text_Data textdata = null;
    private Character_Data charaData = null;
    public GameObject Frend;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Frends") {
            Frend = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Frends")
        {
            Frend = null;
        }
    }
    
    public void OnSpace()
    {
        Debug.Log("aaaaaa");
        if(Frend == null) { return; }
        textdata = Frend.GetComponent<Frends_Data_Script>().GetTextData();
        charaData = Frend.GetComponent<Frends_Data_Script>().GetCharaData();
        manager.SetObject(textdata, charaData);
    }
}