using UnityEngine;
using UnityEngine.InputSystem;

public class TagMove : MonoBehaviour
{
    //------  éQè∆  ------
    protected TagGameManager gameManager;
    protected GameObject Damon;

    protected Touch_script touch_Script;
    protected GameObject Body;
    float rote = 5;
    //------  ãSë§Ç©ì¶Ç∞ÇÈë§  ------
    public float Speed { private set; get; } = 5;
    public bool IsDemon { private set; get; } = false;

    //------  player  ------
    private Rigidbody rb;
    private Vector2 inputvec;

    virtual protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    virtual protected void Start()
    {
        gameManager = GameObject.Find("TagGameManager").GetComponent<TagGameManager>();
        touch_Script = transform.Find("Touch").GetComponent<Touch_script>();
        if (touch_Script == null) { Debug.LogError("NotFound,Touch_script"); return; }
        Body = transform.Find("Body").gameObject;
        if (Body == null) { Debug.LogError("NotFound,Touch_script"); return; }
    }
    private void OnMove(InputValue inputValue)
    {
        inputvec = inputValue.Get<Vector2>();
    }
    private void OnSpace(InputValue inputValue)
    {
        var o = touch_Script.Get().GetComponent<TagMove>();
        if (o == null) return;
        o.Touch(this.gameObject);
    }
    private void Update()
    {
        rb.linearVelocity = new Vector3(inputvec.x * Speed, rb.linearVelocity.y, inputvec.y * Speed);
        if (inputvec.magnitude < 0.1) {
            return;
        }
        var angle = Mathf.Atan2(inputvec.x, inputvec.y) * Mathf.Rad2Deg;
        var q = Quaternion.Euler(0, angle, 0);
        Body.transform.localRotation = Quaternion.RotateTowards(Body.transform.localRotation, q, rote);
    }

    public void Touch(GameObject Player)
    {
        var s = Player.GetComponent<TagMove>().IsDemon;
        if (!s)  {
            return;
        }
    }
}
