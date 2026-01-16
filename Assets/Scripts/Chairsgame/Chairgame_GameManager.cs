using System.Runtime.CompilerServices;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
//没マネージャー
namespace Kouya
{
    public class Chairgame_GameManager : MonoBehaviour
    {
        ChairGame_Player player;
        Kouya.Chairsgame_base chairsgame_Base;
        public AudioSource audiosource;
        private float randomPlayTIme;
        public Vector3 centerPoint = Vector3.zero;
        [SerializeField]
        private GameObject CreateObj;
        [SerializeField]
        public int CreateCount = 7;
        [SerializeField]
        public float radius = 3.0f;
        [SerializeField]
        public float repeat = 2f;
        [SerializeField]
        Text TimerText;
        float limitTime = 3;
        [SerializeField]
        public GameObject Panel;
        Chairgame_Music C_Music;
        Kouya.Chairsgame_base C_Base;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            randomPlayTIme = Random.Range(5f, 30f);
            centerPoint = transform.position;
            StartCoroutine(CreateChair());
            C_Music = GetComponent<Chairgame_Music>();
            C_Base = GetComponent<Kouya.Chairsgame_base>();
            StartGame();
        }

        // Update is called once per frame
        void Update()
        {
            limitTime -= Time.deltaTime;

            if (limitTime < 0)
            {
                limitTime = 0;
            }

            TimerText.text = limitTime.ToString("F0");
        }

        IEnumerator CreateChair()
        {
            Debug.Log("CreateChairが読み込まれました");
            var oneCycle = 2.0f * Mathf.PI;
            for (var i = 0; i < CreateCount; i++)
            {
                var point = ((float)i / CreateCount) * oneCycle;
                var repeatPoint = point * repeat;

                var x = Mathf.Cos(repeatPoint) * radius;
                var y = Mathf.Sin(repeatPoint) * radius;

                var position = centerPoint + new Vector3(x, 0, y);

                var obj = Instantiate(CreateObj, position, Quaternion.identity, transform);
                CreateObj.tag = "Chair";

                Vector3 dirFromCenter = (obj.transform.position - centerPoint).normalized;

                dirFromCenter.y = 0;
                if (dirFromCenter.sqrMagnitude > 0.001f)
                {
                    obj.transform.rotation = Quaternion.LookRotation(dirFromCenter);
                }
            }
            yield break;
        }

        IEnumerator PlayGame()
        {
            Panel.SetActive(false);
            Debug.Log("PlayGameが読み込まれました");
            if (CreateCount != 0)
            {
                AudioPlay();
            }
            CreateCount -= 1;

            yield break;
        }
        void StartGame()
        {
            StartCoroutine(CountDown());
        }
        IEnumerator CountDown()
        {
            Debug.Log("CountDownが読み込まれました");
            limitTime -= Time.deltaTime;

            if (limitTime < 0)
            {
                limitTime = 0;

            }
            TimerText.text = limitTime.ToString("F0");
            yield return StartCoroutine(PlayGame());
        }
        public void AudioPlay()
        {
            audiosource.Play();
            Invoke("StopMusic", randomPlayTIme);

        }
        void StopMusic()
        {
            audiosource.Stop();
            chairsgame_Base.MovePos(true);
            player.ClickMouse(true);
        }
    }
}
