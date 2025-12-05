using UnityEngine;
using UnityEngine.AI;

public class NaviMove : TagMove
{
    private NavMeshAgent Agent;
    private GameObject[] Points;
    private GameObject[] Players;
    private GameObject Target;
    protected override void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }
    protected override void Start()
    {
        gameManager = GameObject.Find("TagGameManager").GetComponent<TagGameManager>();
        touch_Script = transform.Find("Touch").GetComponent<Touch_script>();
        if (touch_Script == null) { Debug.LogError("NotFound,Touch_script"); return; }
        Body = transform.Find("Body").gameObject;
        if (Body == null) { Debug.LogError("NotFound,Touch_script"); return; }
        Points = gameManager.Points;
        Players = gameManager.Players;
    }
    private void Update()
    {
        if (IsDemon)
        {
            
        }
        else
        {

        }
    }
}
