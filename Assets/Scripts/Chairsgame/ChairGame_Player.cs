using System.ComponentModel.Design;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using static UnityEngine.Rendering.DebugUI;

namespace Kouya
{
    public class ChairGame_Player : MonoBehaviour
    {
        public GameObject CenterObj;
        public float rote_sp;
        private GameObject[] nearobj;
        bool CanSpace = false;
        bool Check = false;
        bool sp;
        public  bool sit;
        public GameObject image;
        private Vector3 ppp = new Vector3();
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            CenterObj = transform.parent.gameObject;
            Debug.Log("センターオブジェクト:" + CenterObj);
            ppp = transform.position;
           
        }
        public void ClickMouse(bool i)
        {
            if(i)
            {
                image.SetActive(true);
                Debug.Log("nu");
                CanSpace = true;
            }
           
        }
        void OnSpace(InputValue input)
        {
            
            Debug.Log("a");
            if (!CanSpace) { return; }
            image.SetActive(false);
            Debug.Log("b");
            Check = true;
        }
        // Update is called once per frame
        void Update()
        {
            nearobj = searchTag(gameObject, "Chair");
            if (sp)
            {
                Check = false ;
                return;
            }
            if (Check)
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
            else
            {
                var no = (CenterObj.transform.position - gameObject.transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(no, Vector3.up) * Quaternion.Euler(0, -90, 0);
                
                transform.RotateAround(CenterObj.transform.position, Vector3.up, rote_sp * Time.deltaTime);
            }
        }

        GameObject[] searchTag(GameObject nowobj, string tagname)
        {
            var o = GameObject.FindGameObjectsWithTag(tagname);
            // 2. 距離順に並べ替えて配列を取得（LINQ）
            GameObject[] sortedObjects = o.OrderBy(obj => Vector3.SqrMagnitude(obj.transform.position - transform.position)) // 距離の2乗で比較（高速）
                .ToArray();

            // 結果を表示（一番近いオブジェクト）
            if (sortedObjects.Length > 0)
            {
                //Debug.Log("最も近いオブジェクト: " + sortedObjects[0].name);
            }
            return sortedObjects;
        }

        public void IsSitting(bool v)
        {
            if (v)
            {
                sit = true;
               
                sp = true;
            }
        }

    }
}