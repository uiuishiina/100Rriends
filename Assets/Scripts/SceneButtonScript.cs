using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButtonScript : MonoBehaviour
{
    public void OnClick(string name)
    {
        SceneManager.LoadScene(name);
    }
}
