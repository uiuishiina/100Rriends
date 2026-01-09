using UnityEngine;
using UnityEngine.SceneManagement;

public class TagTheGameButton : MonoBehaviour
{
    public void OnClickTilleBack()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void OnClickGameBack()
    {
        SceneManager.LoadScene("GameScene");
    }
}
