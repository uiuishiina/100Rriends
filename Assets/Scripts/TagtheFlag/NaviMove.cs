using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

public class NaviMove : TagMove
{
    private NavMeshAgent Agent;
    private GameObject[] Points;
    private GameObject[] Players;
    public GameObject Target;
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
        Agent = GetComponent<NavMeshAgent>();
        if (Agent == null) { Debug.LogError("NotFound,NavMesh"); }
        Points = gameManager.Points;
        Players = gameManager.Players;
    }
    private void Update()
    {
        if (IsStop) return;
        if (Target == null) return;
        if (!IsDemon) {
            var l = Mathf.Abs((Damon.transform.position - transform.position).magnitude);
            if (l < 5) {
                Debug.Log("TIkai");
                FindPoints(true);
            }
        }
        if (Agent.remainingDistance < 0.2f) {
            if (IsDemon) {
                var g = touch_Script.Get();
                if (g == null) return;Debug.Log("Eto");
                var o = g.GetComponent<TagMove>();
                if (o == null) return;
                o.Touch(this.gameObject, IsDemon);
                IsDemon = false;
                
            }
            else FindPoints(false);
        }
        Agent.destination = Target.transform.position;
    }
    private void FindPlayer()
    {
        Target = null;
        var Min = 100f;
        GameObject T = null;
        Agent.isStopped = true;
        foreach (var G in Players)
        {
            if (G == this.gameObject) continue;
            float dist = 0;
            var pos = transform.position;
            Agent.SetDestination(G.transform.position);
            foreach(var C in Agent.path.corners) {
                var C2 = C;
                dist += Vector3.Distance(pos, C2);
                pos = C2;
            }
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
            F = (transform.position - Damon.transform.position).normalized;
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


}
