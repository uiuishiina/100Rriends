using UnityEngine;

public class TagGameManager : MonoBehaviour
{
    [Header("プレイヤー配列")] public GameObject[] Players;
    [Header("Point配列")] public GameObject[] Points;
    [SerializeField, Header("鬼")] public GameObject Damon;
    public GameObject pl;
    private void Start()
    {
        pl.GetComponent<TagMove>().StartDamon();
    }
    public void ChengeDamon(GameObject gameObject)
    {
        Debug.Log("newDamon:" + gameObject);
        Damon = gameObject;
        foreach (var G in Players) {
            G.GetComponent<TagMove>().SendChengeDamon(Damon);
        }
    }
}
