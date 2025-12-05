using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneChenger : MonoBehaviour
{
    [SerializeField]
    private string SceneName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   public void ChengeScene()
    {
        SceneManager.LoadScene(SceneName);
    }
}
