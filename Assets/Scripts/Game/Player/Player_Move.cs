using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Move : MonoBehaviour
{
    [SerializeField] float Speed;
    [SerializeField] float RotateSpeed;
    [SerializeField, Header("å©ÇΩñ⁄ÇÃëÃ")] GameObject Body;
    [SerializeField] PlayerTalkController PlayerTalkController;
    [SerializeField] GameManager GameManager;
    Rigidbody rb;
    //-------  ïœêî  ------
    Vector3 Inputvec;
    //------  ------

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void OnMove(InputValue input)
    {
        var v = input.Get<Vector2>();
        Inputvec = new Vector3(v.x, 0, v.y);
    }

    void OnSpace(InputValue input)
    {
        PlayerTalkController.OnSpace();
    }
    public void OnESC(InputValue inputValue)
    {
        GameManager.ESC();
    }

    private void Update()
    {
        rb.linearVelocity = new Vector3(Inputvec.x * Speed, rb.linearVelocity.y, Inputvec.z * Speed);

        //------  ëÃ  ------
        if (Inputvec.magnitude <= 0.1f){
            return;
        }
        var angle = Mathf.Atan2(Inputvec.x, Inputvec.z) * Mathf.Rad2Deg;
        var q = Quaternion.Euler(0, angle, 0);
        Body.transform.localRotation=Quaternion.RotateTowards(Body.transform.localRotation, q, RotateSpeed);
    }
}
