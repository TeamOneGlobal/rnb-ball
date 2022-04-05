using System;
using Truongtv.SoundManager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sound
{
    public class SoundGamePlayController : SoundManager
    {
        [SerializeField] private AudioClip backgroundMusic;
        [SerializeField] private AudioClip[] winSounds;
        [SerializeField] private AudioClip[] loseSound;
        [SerializeField] private AudioClip popupSound;
        [SerializeField] private AudioClip buttonChangeTargetSound;
        #region Hidden
        private static SoundGamePlayController _instance;
        public static SoundGamePlayController Instance => _instance;
        private new void Awake()
        {
            //base.Awake();
            if (_instance == null)
                _instance = this;
        }
        #endregion

        private void Start()
        {
            PlayBgm(backgroundMusic);
        }


        public void PlayPopupSound()
        {
            PlaySfx(popupSound);
        }
        public void PlayWinSound(Action oncomplete = null)
        {
            Pause(true);
            var index = Random.Range(0, winSounds.Length);
            PlaySfx(winSounds[index],onComplete: () =>
            {
                Pause(false);
                oncomplete?.Invoke();
            });
        }
        
        public void PlayLoseSound(Action oncomplete = null)
        {
            Pause(true);
            var index = Random.Range(0, loseSound.Length);
            PlaySfx(loseSound[index],onComplete:oncomplete);
        }

        public void ResumeBgm()
        {
            Pause(false);
        }
        public void PlayChangeTargetSound()
        {
            PlaySfx(buttonChangeTargetSound);
        }
    }
}