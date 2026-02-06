using UnityEngine;
using System.Collections;
using System.Linq;


namespace Kouya {
    public class Chairsgame_base : MonoBehaviour
    {
        ChairsGame_ReMake ChairsGame_Remake;
        public GameObject CenterObj;
        public float rote_sp;
        public bool sp = false;
        private GameObject[] nearobj;
        bool m_st = false;
        public float randomwaittime;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Awake()
        {

        }
        void Start()
        {
            randomwaittime = Random.Range(0.5f, 2f);
            CenterObj = transform.parent.gameObject;
            Debug.Log("センターオブジェクト:" + CenterObj);
            nearobj = searchTag(gameObject, "Chair");

        }

        // Update is called once per frame
        void Update()
        {
            MovePos(m_st);
        }
        private void FixedUpdate()
        {
            nearobj = searchTag(gameObject, "Chair"); 
          
        }
        public void MovePos(bool i)
        {
            if (sp)
            {
                return;
            }
            if (i)
            {
                Invoke("movechair", randomwaittime);

            }
            else
            {
                var no = (CenterObj.transform.position - gameObject.transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(no, Vector3.up) * Quaternion.Euler(0, -90, 0);
                transform.RotateAround(CenterObj.transform.position, Vector3.up, rote_sp * Time.deltaTime);
            }
        }
        public void IsSitting(bool i)
        {
            if(i == true)
            {
                sp = true; 
            }
        }
        public void isMoving(bool s)
        {
            if(s)
            {
                m_st = true;
            }
            else
            {
                m_st = false;
            }
        }
        void movechair()
        {
            GameObject value = null;
            foreach (var g in nearobj)
            {
                if (!g.GetComponent<ChairsGame_Chair>().isSit)
                {
                    value = g; break;
                }
            }
            if (value == null) { return; }
            transform.LookAt(value.transform);
            transform.Translate(Vector3.forward * Time.deltaTime);
        }
        /*仮残し
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("当たった相手:" + collision.gameObject.name);
            if (collision.gameObject.CompareTag("Chair"))
            {
                sp = false;
            }
        }
        */
        GameObject[] searchTag(GameObject nowobj, string tagname)
        {
           
            var o = GameObject.FindGameObjectsWithTag(tagname);
            // 2. 距離順に並べ替えて配列を取得（LINQ）
            GameObject[] sortedObjects = o.OrderBy(obj => Vector3.SqrMagnitude(obj.transform.position - transform.position)) // 距離の2乗で比較（高速）
                .ToArray();

            // 結果を表示（一番近いオブジェクト）
            if (sortedObjects.Length > 0)
            {
               // Debug.Log("最も近いオブジェクト: " + sortedObjects[0].name);
            }
            //GameObject targetobj = null;
            //foreach (GameObject obs in GameObject.FindGameObjectsWithTag(tagname))
            //{
            //    tmpDis = Vector3.Distance(obs.transform.position, nowobj.transform.position);

            //    if (nearDis == 0 || nearDis > tmpDis)
            //    {
            //        nearDis = tmpDis;
            //        targetobj = obs;
            //    }
            //}
            return sortedObjects;
        }

    }



};
