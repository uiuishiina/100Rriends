using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerInput player;
    [SerializeField] PlayerInput Ui;

    private void Start()
    {
        var f = true;
        player.enabled = f;
        Ui.enabled = !f;
    }

    public void c(bool a)
    {
        player.enabled = a;
        Ui.enabled = !a;
    }
}
