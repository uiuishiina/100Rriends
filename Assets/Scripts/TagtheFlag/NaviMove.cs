using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class NaviMove : TagMove
{
    private NavMeshAgent Agent;
    private GameObject[] Points;
    private GameObject[] Players;
    public GameObject Target;
    public GameObject Image;
    protected override void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }
    protected override void Start()
    {
        gameManager = GameObject.Find("TagGameManager").GetComponent<TagGameManager>();
        touch_Script = transform.Find("Touch").GetComponent<Touch_script>();
        if (touch_Script == null) { Debug.LogError("NotFound,Touch_script"); return; }
        Body = GameObject.Instantiate(Data.Body, this.transform);
        if (Body == null) { Debug.LogError("NotFound,Touch_script"); return; }
        Agent = GetComponent<NavMeshAgent>();
        if (Agent == null) { Debug.LogError("NotFound,NavMesh"); }
        Points = gameManager.Points;
        Players = gameManager.Players;
    }
    private void Update()
    {
        var cpos = Camera.main.transform.position;
        var c = new Vector3(0, cpos.y, -1);
        Image.transform.rotation = UnityEngine.Quaternion.LookRotation(c);
        if (IsStart){ return;}
        if (IsStop) { return; } 
        if (Target == null){
            if (IsDemon){
                FindPlayer();
            }
            else{
                FindPoints(false);
            }
        }
        else{
            if (IsDemon){
                FindPlayer();
            }
            else{
                foreach (var g in Damon){
                    var l = Mathf.Abs((g.transform.position - transform.position).magnitude);
                    if (l < 10)
                    {
                        FindPoints(true);
                    }
                }
            }
        }
        if (Agent.remainingDistance < 2.0f){
            if (IsDemon){
                if (null!= touch_Script.Get()){
                    var g = touch_Script.Get();
                    if(null!= g.GetComponent<TagMove>())
                    {
                        g.GetComponent<TagMove>().Touch(this.gameObject, IsDemon);
                        FindPlayer();
                    }
                }
            }
            else FindPoints(false);
        }

        if (!Target) { return; }
        if (IsDemon) { Agent.speed = DSpeed; }
        else { Agent.speed = Speed; }
        Agent.SetDestination(Target.transform.position);
    }
    private void FindPlayer()
    {
        Target = null;
        var Min = 100f;
        GameObject T = null;
        Agent.isStopped = true;
        var pos = transform.position;
        foreach (var G in Players)
        {
            if (G == this.gameObject) continue;
            if (G.gameObject.GetComponent<TagMove>().IsDemon) { continue; }
            var dist = Vector3.Distance(pos, G.transform.position);
            if(Min > dist) {
                Min = dist;
                T = G;
            }
        }
        Target = T;
        Agent.isStopped = false;
    }
    private void FindPoints(bool away)
    {
        Target = null;
        var F = transform.forward;
        if (away) {
            var l = 100.0f;
            foreach (var g in Damon)
            {
                var pl = (transform.position - g.transform.position).magnitude;
                if (l> pl)
                {
                    l = pl;
                    F = transform.position - g.transform.position;
                }
            }
        }
        List<GameObject> v = new List<GameObject>();
        //³–Ê‚Ì‚â‚Â‚ð‚Æ‚é
        foreach (var G in Points) {
            var l = (G.transform.position - transform.position).normalized;
            var dot = Vector3.Dot(F, l);
            if (dot > 0.8f) {
                v.Add(G);
            }
        }
        //‚¢‚È‚©‚Á‚½‚ç‰¡‚Ü‚Å‚Ì‚â‚Â‚ð‚Æ‚é
        if (v.Count == 0) {
            foreach (var G in Points)
            {
                var l = (G.transform.position - transform.position).normalized;
                var dot = Vector3.Dot(F, l);
                if (dot > 0.5f)
                {
                    v.Add(G);
                }
            }
        }
        //‚³‚ç‚É‚¢‚È‚©‚Á‚½‚ç‘S•”
        if (v.Count == 0) {
            foreach (var G in Points) {
                v.Add(G);
            }
        }
        //
        var Min = 100f;
        GameObject T = null;
        Agent.isStopped = true;
        foreach (var G in v) {
            float dist = 0;
            var pos = transform.position;
            Agent.SetDestination(G.transform.position);
            foreach (var C in Agent.path.corners) {
                var C2 = C;
                dist += Vector3.Distance(pos, C2);
                pos = C2;
            }
            if (Min > dist) {
                Min = dist;
                T = G;
            }
        }
        Target = T;
        Agent.isStopped = false;
    }

    public override void Touch(GameObject Player, bool Isd)
    {
        if (!Isd)
        {
            return;
        }
        else
        {
            gameManager.ChengeDamon(this.gameObject);
            IsDemon = true;
            Agent.SetDestination(transform.position);
            Body.GetComponent<MeshRenderer>().materials[0].color = Color.yellow;
            Image.GetComponent<RawImage>().texture = gameManager.ONI;
            StartCoroutine(Stop());
        }
    }
    protected override IEnumerator Stop()
    {
        IsStop = true;
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(1);
        }
        FindPlayer();
        IsStop = false;
        Debug.Log("Stopoff");
        yield return null;
    }

    public override void SendChengeDamon(List<GameObject> D)
    {
        Damon = D;
    }
}
