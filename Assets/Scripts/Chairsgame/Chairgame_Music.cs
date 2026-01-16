using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

namespace Kouya
{
    public class Chairgame_Music : MonoBehaviour
    {
        ChairGame_Player player;
        Kouya.Chairsgame_base chairsgame_Base;
        public AudioSource audiosource;
        private float randomPlayTIme;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            randomPlayTIme = Random.Range(5f, 30f);

        }

        // Update is called once per frame
        void Update()
        {

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