using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace Kouya
{
    public class ChairGame_Player : MonoBehaviour
    {
        public GameObject CenterObj;
        public float rote_sp;
        private GameObject nearobj;
        bool Check;
        bool sp;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {



        }
        public void ClickMouse(bool i)
        {
            if (i == true)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    nearobj = searchTag(gameObject, "Chair");
                    Check = true;
                }
            }
            else
            {
                Check = false;
            }

        }
        // Update is called once per frame
        void Update()
        {
            ClickMouse(true);
            if (Check)
            {
                if (sp == false)
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

        public void IsSitting(bool v)
        {
            if (v == true)
            {
                sp = false;
            }
        }

    }
}