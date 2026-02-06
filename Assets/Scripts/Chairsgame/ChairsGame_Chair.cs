using Kouya;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kouya 
{
    public class ChairsGame_Chair : MonoBehaviour
    {
        Chairsgame_base c_Base;
        ChairGame_Player player;
        public bool isSit = false;
        // Start is called once before the first execution of Update after the MonoBehaviour is create    
        void OnTriggerEnter(Collider other)
        {
            Debug.Log("当たった相手:" + other.gameObject.name);
            if (other.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("椅子がとられました");
                isSit = true;
                other.gameObject.GetComponent<Chairsgame_base>().IsSitting(true);
                Debug.Log("椅子:" + isSit);
            }
            else if (other.gameObject.CompareTag("Player"))
            {
                Debug.Log("椅子をプレイヤーがとりました");
                isSit = true;
                other.gameObject.GetComponent<ChairGame_Player>().IsSitting(true);
                Debug.Log("椅子:" + isSit);
            }
        }
        /*
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("当たった相手:" + collision.gameObject.name);
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("椅子がとられました");
                isSit = true;
                
                Debug.Log("椅子:" + isSit);
            }
            else if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("椅子をプレイヤーがとりました");
                isSit = true;
                
                Debug.Log("椅子:" + isSit);
            }
        }*/
    }
}

