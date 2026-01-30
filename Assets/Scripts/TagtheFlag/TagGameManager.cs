using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class TagGameManager : MonoBehaviour
{
    [Header("プレイヤー配列")] public GameObject[] Players;
    [Header("Point配列")] public GameObject[] Points;
    public GameObject pl;
    [Header("鬼")] public List<GameObject> Damon = new List<GameObject>();
    public Texture ONI;
    
    private bool end;
    bool IsStart = false;
    [SerializeField, Header("タイマー")] private float Time_;
    [SerializeField, Header("タイマー")] TextMeshProUGUI TimeText_;
    [SerializeField, Header("カウントダウン")] TextMeshProUGUI CountText_;
    [SerializeField, Header("カウントダウン")] TextMeshProUGUI TachiText_;

    private void Start()
    {
        pl.GetComponent<TagMove>().StartDamon();
        ChengeDamon(Players[0],false);
        StartCoroutine(StartCall(4));
        TachiText_.enabled = false;
    }
    public void ChengeDamon(GameObject gameObject,bool a = true)
    {
        Damon.Add(gameObject);
        foreach (var G in Players)
        {
            G.GetComponent<TagMove>().SendChengeDamon(Damon);
        }
        foreach (var p in Players)
        {
            if(!p.GetComponent<TagMove>().IsDemon)
            {
                if (a) { StartCoroutine(Taci()); }
                return;
            }
        }
        End(true);
    }
    IEnumerator Taci()
    {
        TachiText_.text = "タッチ";
        TachiText_.enabled = true;
        for (int i = 10; i > 0; i--){
            TachiText_.alpha = i * 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        TachiText_.enabled = false;
    }
    IEnumerator StartCall(int value)
    {
        CountText_.text = value.ToString();
        for (int i = value; i > 0; i--)
        {
            CountText_.text = (i - 1).ToString();
            if ((i - 1)==0) { CountText_.text = "START!"; }
            yield return new WaitForSeconds(1);
        }
        foreach (var G in Players)
        {
            G.GetComponent<TagMove>().IsStart = false;
        }
        CountText_.enabled = false;
        IsStart = true;
    }
    private void Update()
    {
        if (!IsStart){
            return;
        }
        if (end) { return; }
        if (Time_ < 0) { end = true; End(); }
        Time_ -= Time.deltaTime;
        TimeText_.text = "Time:"+((int)Time_).ToString();
    }
    void End(bool ans = false)
    {
        foreach (var G in Players)
        {
            G.GetComponent<TagMove>().IsStart = true;
        }
        var te = "";
        if (ans) { te = "クリア！"; }
        else { te = "失敗"; }
        StartCoroutine(Endcol(te, ans));
    }
    IEnumerator Endcol(string text,bool ans)
    {
        CountText_.enabled = true;
        CountText_.text = text;
        for (int i = 0; i < 3; i++) {
            yield return new WaitForSeconds(1);
        }
        if(GameObject.Find("LogObject").GetComponent<LogObject>() != null) {
            GameObject.Find("LogObject").GetComponent<LogObject>().AddFrends(Players.Length - 1);
        }
        if (ans) { SceneManager.LoadScene("GameScene"); }
        else { SceneManager.LoadScene("GameScene"); }
    }
}
