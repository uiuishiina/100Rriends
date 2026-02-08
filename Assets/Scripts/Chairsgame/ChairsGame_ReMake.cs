using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace Kouya
{
    public class ChairsGame_ReMake : MonoBehaviour
    {
        /*
         椅子取りゲームで作るべきもの
        ・椅子までの移動　済
        ・椅子をとられた反応　済
        ・回転（音楽なってるとき）済
        ・椅子を取られた時の対象の変更
        ・椅子を少なくしてゲームのリスタート 済
      　  一連をコルーチン操作でやってみる　済
        ・リザルト 半済　未着手：誰が残ったか
        次やること
        ・キャラの情報追加
         */
        public bool OK = false;
        public bool St = false;
        public bool end = false;
        public bool check = false;
        Kouya.ChairGame_Player player;
        Kouya.Chairsgame_base enemy;
        ChairsGame_Chair CC;
        public AudioSource audiosource;
        private float randomPlayTIme;//音楽再生時間
        private float randomWaitTIme;//待機時間
        public Vector3 centerPoint = Vector3.zero;
        [SerializeField]
        private GameObject CreateObj;//生成するオブジェクト
        [SerializeField]
        private GameObject CreateCharaObj;//生成するキャラオブジェクト
        [SerializeField]
        private GameObject CreatePlayerObj;//生成するプレイヤー
        [SerializeField]
        public GameObject centobj; //円の中心になるオブジェクト
        [SerializeField]
        public int CreateCount = 7;//椅子を生成する数
        [SerializeField]
        public float radius = 3.0f;//生成する円の半径
        [SerializeField]
        public int CreateCharaCount = 9;//キャラを生成する数
        [SerializeField]
        public float C_radius = 6.0f;//生成する円の半径(キャラ)
        [SerializeField]
        public float repeat = 2f;
        [SerializeField]
        Text TimerText;
        [SerializeField]
        Text ResultText;
        float limitTime = 3f;
        [SerializeField]
        public GameObject ExpPanel;
        [SerializeField]
        public GameObject Panel;
        [SerializeField]
        public GameObject ResultPanel;
        [SerializeField]
        public GameObject WaitPanel;
        GameObject[] enemys;
        GameObject players;
       public GameObject Eximage;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Awake()
        {
            // SceneManager.LoadScene(1);//１は仮の番号　実装する際は実際のシーン番号に変更してください
        }
        void Start()
        {
            Debug.Log("ChairsGame_ReMakeが読み込まれました。");
            randomPlayTIme = Random.Range(5f, 30f);//音楽を流す時間設定
            randomWaitTIme = Random.Range(0f, 2f);//待機時間設定
            centerPoint = transform.position;
            WaitPanel.SetActive(false);
            ResultPanel.SetActive(false);
            StartCoroutine(GameMaster());
        }

        // Update is called once per frame
        void Update()
        {
           //---------------椅子の数を確認----------------------------
            GameObject[] Chairs = GameObject.FindGameObjectsWithTag("Chair");
            foreach(var c in Chairs)
            {
                if (!c.GetComponent<ChairsGame_Chair>().isSit)
                {
                    return;
                }
               
            }
            check = true;
        }
        public void P_Know()//説明画面の確認ボタン用
        {
            OK = true;
            Debug.Log("説明画面が押されました");
        }
        //--------------------ゲームをまとめるコルーチン------------------
        IEnumerator GameMaster()
        {
            Debug.Log("GameMasterが読み込まれました");
            yield return StartCoroutine(ExpGame());
            while (CreateCount > 1)
            {
                check = false;  
                Debug.Log("ゲームループ開始");
                yield return StartCoroutine(CreateChair());
                yield return StartCoroutine(CharaCreate());
                yield return StartCoroutine(StartGame());
                yield return StartCoroutine(MusicController()); 
                yield return new WaitUntil(() => check);
                yield return StartCoroutine(ResetGame());           
                CreateCount -= 2;
                CreateCharaCount -= 2;
            }
            Debug.Log("ゲームループ終了");
            yield return new WaitUntil(() => end);
            yield return StartCoroutine(EndedGame());
            yield break;
        }
        //--------------------ゲーム説明画面のコルーチン------------------
        IEnumerator ExpGame()
        {
            ExpPanel.SetActive(true);
            yield return new WaitUntil(() => OK);
            OK = false;
            ExpPanel.SetActive(false);
            yield break;
        }
        //--------------------ゲームリセットのコルーチン------------------
        IEnumerator ResetGame()
        {
            Debug.Log("ResetGameが読み込まれました");
            players = GameObject.FindGameObjectWithTag("Player");
            if(players.GetComponent<ChairGame_Player>().sit)
            {
                WaitPanel.SetActive(true);
                Transform Parent_t = centobj.transform;
                for (int i = Parent_t.childCount - 1; i >= 0; i--)
                {
                    Destroy(Parent_t.GetChild(i).gameObject);
                }
                yield return new WaitForSeconds(5f);
                WaitPanel.SetActive(false);
                yield break;
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(EndedGame());
            }
            
           
        }
        //--------------------ゲーム終了のコルーチン----------------------
        IEnumerator EndedGame()
        {
            Debug.Log("EndedGameが読み込まれました");
            ResultPanel.SetActive(true);
            ResultText.text = "Finish!!";
            yield return new WaitForSeconds(2f);
            ResultText.text = "";
            yield return new WaitForSeconds(3f);
            var G = GameObject.Find("LogObject");
            if (G != null)
            {
                G.GetComponent<LogObject>().AddFrends(40);
            }
            SceneManager.LoadScene("GameScene");
            yield break;
        }
        //--------------------ゲームの開始前のコルーチン------------------
        IEnumerator StartGame()
        {
            Panel.SetActive(true);
            Debug.Log("StartGameが読み込まれました");
            yield return new WaitForSeconds(1f);
            Debug.Log("ループ中");

            while (limitTime > 0)
            {
                limitTime -= Time.deltaTime;
                TimerText.text = limitTime.ToString("F0");
            }
            
            if (limitTime < 0)
            {
                limitTime = 0;
                St = true;
            }
            yield return new WaitUntil(() => St);
            limitTime = 3f;
            St = false;
            Panel.SetActive(false);
            yield break;
        }
        //--------------------音楽を流して止めるコルーチン----------------
        IEnumerator MusicController()
        {
            players = GameObject.FindGameObjectWithTag("Player");
            enemys = GameObject.FindGameObjectsWithTag("Enemy");

            Debug.Log("MusicControllerが読み込まれました");
            audiosource.Play();       
             ChairGame_Player Cp = players.GetComponent<ChairGame_Player>();
             if (Cp != null)
             {
                Cp.image = Eximage;
                 Debug.Log("プレイヤーのスクリプトがあります");
                 Cp.ClickMouse(false);
             }
             else
             {
                 Debug.Log("プレイヤーのスクリプトがありません");
             }
            
            foreach (GameObject e_obj in enemys)
            {
                Chairsgame_base Cb = e_obj.GetComponent<Chairsgame_base>();
                if (Cb != null)
                {
                    Debug.Log("NPCのスクリプトがありまス");
                    Cb.isMoving(false);
                }
                else
                {
                    Debug.Log("NPCのスクリプトがありません");
                }
            }
            Debug.Log("音楽が流れ始めました。");
            yield return new WaitForSeconds(randomPlayTIme);
            audiosource.Stop();
            Debug.Log("音楽が止まりました。");
            //プレイヤー用処理（デバッグ中でoff）
              if (Cp != null)
              {
                    Debug.Log("プレイヤーのスクリプトがあります");
                  Cp.ClickMouse(true);
              }
             else
             {
                Debug.Log("プレイヤーのスクリプトがありません");
             }
         //   yield return new WaitForSeconds(randomWaitTIme);
            foreach (GameObject e_obj in enemys)
            {
                Chairsgame_base Cb = e_obj.GetComponent<Chairsgame_base>();
                if (Cb != null)
                {
                    Debug.Log("NPCのスクリプトがありまス");
                    Cb.isMoving(true);
                }
                else
                {
                    Debug.Log("NPCのスクリプトがありません");
                }
            }
            yield return new WaitUntil(()=>check);//この処理のところに椅子がないのを確認
            yield break;
        }
        //--------------------椅子を円形に生成するコルーチン--------------
        IEnumerator CreateChair()
        {
            Debug.Log("CreateChairが読み込まれました");
            Debug.Log("椅子の数:" + CreateCount);
            var oneCycle = 2.0f * Mathf.PI;
            if (CreateCount >= 1)
            {
                for (var i = 0; i < CreateCount; i++)
                {
                    float angle = (float)i / CreateCount * oneCycle;

                    float x = Mathf.Cos(angle) * radius;
                    float y = Mathf.Sin(angle) * radius;
                    /*
                    var point = ((float)i / CreateCount) * oneCycle;
                    var repeatPoint = point * repeat;

                    var x = Mathf.Cos(repeatPoint) * radius;
                    var y = Mathf.Sin(repeatPoint) * radius;
                    */
                    var position = centerPoint + new Vector3(x, 0, y);
                    var Q = Quaternion.identity;
                    var q = new Quaternion(Q.x, 180, Q.z, Q.w);
                    var obj = Instantiate(CreateObj, position,q, transform);
                    CreateObj.tag = "Chair";

                    Vector3 dirFromCenter = (obj.transform.position - centerPoint).normalized;

                    dirFromCenter.y = 0;
                    if (dirFromCenter.sqrMagnitude > 0.001f)
                    {
                        obj.transform.rotation = Quaternion.LookRotation(-dirFromCenter);
                    }
                }

                Debug.Log("椅子の生成が終わりました。");
            }
            else
            {
                end = true;
                // StartCoroutine(EndedGame());//ゲーム終了
            }
            yield break;
        }
        //--------------------キャラを円形に生成するコルーチン--------------
        IEnumerator CharaCreate()
        {
            Debug.Log("CharaCreateが読み込まれました");
            Debug.Log("キャラの数:" + CreateCharaCount);
            var oneCycle = 2.0f * Mathf.PI;
            if (CreateCharaCount >= 2)
            {
                for (var i = 0; i < CreateCharaCount; i++)
                {
                    float angle = (float)i / CreateCharaCount * oneCycle;

                    float x = Mathf.Cos(angle) * C_radius;
                    float y = Mathf.Sin(angle) * C_radius;
                    /*
                    var point = ((float)i / CreateCharaCount) * oneCycle;
                    var repeatPoint = point * repeat;

                    var x = Mathf.Cos(repeatPoint) * C_radius;
                    var y = Mathf.Sin(repeatPoint) * C_radius;
                    */
                    var position = centerPoint + new Vector3(x, 0, y);
                    GameObject spawneObj;
                    if (i == 0)
                    {
                        spawneObj = Instantiate(CreatePlayerObj, position, Quaternion.identity, transform);
                    }
                    else
                    { 
                        spawneObj = Instantiate(CreateCharaObj, position, Quaternion.identity, transform);
                    }

                    Vector3 dirFromCenter = (spawneObj.transform.position - centerPoint).normalized;
                    spawneObj.transform.rotation = Quaternion.LookRotation(-dirFromCenter);

                    dirFromCenter.y = 0;

                }
                Debug.Log("キャラの生成が終わりました。");

            }
            else
            {
                end = true;
                // StartCoroutine(EndedGame());//ゲーム終了
            }
            yield break;

        }
    }
}