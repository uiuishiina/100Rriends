using Kouya;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kouya
{

    public class ChairsGame_Chair : MonoBehaviour
    {
        Kouya.Chairsgame_base c_Base;
        ChairGame_Player player;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log("椅子のタグ" + this.gameObject.tag);
        }
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("当たった相手:" + collision.gameObject.name);
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("椅子がとられました");
                this.gameObject.tag = "usedChair";
                c_Base.IsSitting(true);
                Debug.Log("椅子のタグ" + this.gameObject.tag);
            }
            else if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("椅子をプレイヤーがとりました");
                this.gameObject.tag = "usedChair";
                player.IsSitting(true);
                Debug.Log("椅子のタグ" + this.gameObject.tag);
            }
        }
    }
}
