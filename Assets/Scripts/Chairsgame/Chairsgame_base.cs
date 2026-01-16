using UnityEngine;
using System.Collections;


namespace Kouya {
    public class Chairsgame_base : MonoBehaviour
    {
        ChairsGame_ReMake ChairsGame_Remake;
        public GameObject CenterObj;
        public float rote_sp;
        public bool sp = true;
        private GameObject nearobj;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Awake()
        {

        }
        void Start()
        {

            CenterObj = transform.parent.gameObject;
            Debug.Log("センターオブジェクト:" + CenterObj);
            nearobj = searchTag(gameObject, "Chair");

        }

        // Update is called once per frame
        void Update()
        {           
            MovePos(false);
        }
        private void FixedUpdate()
        {
            nearobj = searchTag(gameObject, "Chair"); 
            Debug.Log(nearobj);
        }
        public void MovePos(bool i)
        {
            if (i)
            {
                if(sp == true)
                {
                    transform.LookAt(nearobj.transform);
                    transform.Translate(Vector3.forward * Time.deltaTime);
                }
                
            }
            else
            {
                transform.RotateAround(CenterObj.transform.position, Vector3.up, rote_sp * Time.deltaTime);
            }
        }
        public void IsSitting(bool i)
        {
            if(i == true)
            {
                sp = false; 
            }
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
        GameObject searchTag(GameObject nowobj, string tagname)
        {
            float tmpDis = 0;
            float nearDis = 0;

            GameObject targetobj = null;
            foreach (GameObject obs in GameObject.FindGameObjectsWithTag(tagname))
            {
                tmpDis = Vector3.Distance(obs.transform.position, nowobj.transform.position);

                if (nearDis == 0 || nearDis > tmpDis)
                {
                    nearDis = tmpDis;
                    targetobj = obs;
                }
            }
            return targetobj;
        }

    }



};
