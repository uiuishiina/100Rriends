using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogObject : MonoBehaviour
{
    public static GameObject Instance_;
    [SerializeField]public float Time_ = 0;
    public int FriendsCount_ { get; set; } = 0;
    private int savec = 0;

    public List<(string,int)> FrendsNames_ = new List<(string, int)>();
    public List<string> Scenename_ = new List<string>();

    bool IsStop = false;
    bool game = false;
    private void Awake() {
        if (Instance_ == null) {
            Instance_ = this.gameObject;
            DontDestroyOnLoad(Instance_);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        game = false;
        if(scene.name == "TitleScene") {
            FriendsCount_ = 60;
            Time_ = 0;
            savec = 0;
            FrendsNames_.Clear();
            Scenename_.Clear();
        }
        if (scene.name == "ResultScene") {
            
        }
        else if ((FriendsCount_+ savec) >= 100) {
            SceneManager.LoadScene("ResultScene");
        }
        if(scene.name == "GameScene") { game = true; AddFrends(0); return; }
        if(Scenename_.Contains(scene.name)) { return; }
        Scenename_.Add(scene.name);
    }
    private void Update()
    {
        Time_ += Time.deltaTime;
    }
    public void AddFrends(int count)
    {
        savec += count;
        if (game)
        {
            FriendsCount_ += savec;
            GameObject.Find("GameManager").GetComponent<GameManager>().UpCount(savec);
            savec = 0;
        }
    }
    public void AddFrendsName(string name,int count)
    {
        FrendsNames_.Add((name,count));
    }

    public void Result()
    {
        var T = GameObject.Find("TimerText");
        var min = ((int)Time_ % 3600) / 60;
        var sec = (int)Time_ % 60;
        if (T != null) {
            T.GetComponent<TextMeshProUGUI>().text = min.ToString("D2") + ":" + sec.ToString("D2"); 
        }
        //SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Destroy(Instance_);
    }
}
