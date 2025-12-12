using System.Collections;
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
    public bool IsDemon { protected set; get; } = false;

    //------  player  ------
    private Rigidbody rb;
    private Vector2 inputvec;
    protected bool IsStop = false;

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
        if(IsStop) { return; }
        inputvec = inputValue.Get<Vector2>();
    }
    private void OnSpace(InputValue inputValue)
    {
        if(IsStop) { return; }
        var g = touch_Script.Get();
        if (g == null) return;
        var o = g.GetComponent<TagMove>();
        if (o == null) return;
        o.Touch(this.gameObject,IsDemon);
        if(IsDemon) IsDemon = false;
    }
    private void Update()
    {
        if (IsStop) { return; }
        rb.linearVelocity = new Vector3(inputvec.x * Speed, rb.linearVelocity.y, inputvec.y * Speed);
        if (inputvec.magnitude < 0.1) {
            return;
        }
        var angle = Mathf.Atan2(inputvec.x, inputvec.y) * Mathf.Rad2Deg;
        var q = Quaternion.Euler(0, angle, 0);
        Body.transform.localRotation = Quaternion.RotateTowards(Body.transform.localRotation, q, rote);
    }

    public void StartDamon()
    {
        IsDemon = true;
    }
    public void SendChengeDamon(GameObject D)
    {
        Damon = D;
        Debug.Log("CD"+this.gameObject);
    }
    virtual public void Touch(GameObject Player,bool Isd)
    {
        if (!Isd)  {
            return;
        }
        else
        {
            gameManager.ChengeDamon(this.gameObject);
            IsDemon = true;
            StartCoroutine(Stop());
        }
    }
    virtual protected IEnumerator Stop()
    {
        IsStop = true;
        for (int i = 0; i < 5; i++) {
            yield return new WaitForSeconds(1);
        }
        IsStop = false;
        Debug.Log("Stopoff");
        yield return null;
    }
}
