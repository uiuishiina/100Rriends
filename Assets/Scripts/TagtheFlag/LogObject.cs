using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogObject : MonoBehaviour
{
    public static GameObject Instance_;
    [SerializeField]public float Time_ = 0;
    public int FriendsCount_ { get; private set; } = 0;

    public List<(string,int)> FrendsNames_ = new List<(string, int)>();
    public List<string> Scenename_ = new List<string>();
    private void Awake() {
        if (Instance_ == null) {
            Instance_ = this.gameObject;
            DontDestroyOnLoad(Instance_);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Debug.Log(FriendsCount_ + ":Count");
        if (scene.name == "ResultScene") {
            //SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
            Destroy(gameObject);
        }
        if (FriendsCount_ >= 100) {
            SceneManager.LoadScene("ResultScene");
        }
        if(scene.name == "GameScene") { return; }
        if(Scenename_.Contains(scene.name)) { return; }
        Scenename_.Add(scene.name);
    }
    private void Update()
    {
        Time_ += Time.deltaTime;
    }
    public void AddFrends(int count)
    {
        FriendsCount_ += count;
    }
    public void AddFrendsName(string name,int count)
    {
        FrendsNames_.Add((name,count));
    }
}
