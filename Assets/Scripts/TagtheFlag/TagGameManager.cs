using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class TagGameManager : MonoBehaviour
{
    [Header("プレイヤー配列")] public GameObject[] Players;
    [Header("Point配列")] public GameObject[] Points;
    [SerializeField, Header("鬼")] public GameObject Damon;
    public GameObject pl;
    private bool end;
    bool IsStart = false;
    [SerializeField, Header("タイマー")] private float Time_;
    [SerializeField, Header("タイマー")] TextMeshProUGUI TimeText_;
    [SerializeField, Header("カウントダウン")] TextMeshProUGUI CountText_;
    [SerializeField, Header("カウントダウン")] TextMeshProUGUI TachiText_;

    private void Start()
    {
        pl.GetComponent<TagMove>().StartDamon();
        ChengeDamon(Players[2],false);
        StartCoroutine(StartCall(4));
        TachiText_.enabled = false;
    }
    public void ChengeDamon(GameObject gameObject,bool a = true)
    {
        Damon = gameObject;
        foreach (var G in Players) {
            G.GetComponent<TagMove>().SendChengeDamon(Damon);
        }
        if (a) { StartCoroutine(Taci()); }
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
    void End()
    {
        foreach (var G in Players)
        {
            G.GetComponent<TagMove>().IsStart = true;
        }
        SceneManager.LoadScene("ResultScene");
    }
}
