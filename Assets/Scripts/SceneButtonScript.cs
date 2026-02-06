using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButtonScript : MonoBehaviour
{
    public void OnClick(string name)
    {
        SceneManager.LoadScene(name);
    }

    private void Start()
    {
        var l = GameObject.Find("LogObject");
        if(l != null) { l.GetComponent<LogObject>().Result(); }
    }
}
