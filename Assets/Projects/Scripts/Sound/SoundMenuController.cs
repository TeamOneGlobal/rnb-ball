using ThirdParties.Truongtv.SoundManager;
using UnityEngine;

namespace Sound
{
    public class SoundMenuController : SoundManager
    {
        #region Hiden
        private static SoundMenuController _instance;
        public static SoundMenuController Instance => _instance;
        private new void Awake()
        {
            //base.Awake();
            if (_instance == null)
                _instance = this;
        }
        #endregion
        [SerializeField] private AudioClip popupSound;
        [SerializeField] private AudioClip backgroundMusic;
        [SerializeField] private AudioClip[] winSounds;
        [SerializeField] private AudioClip cashSound;
        [SerializeField] private AudioClip spinSound;
        private void Start()
        {
            PlayBgm(backgroundMusic);
        }
        public void PlayPopupSound()
        {
            PlaySfx(popupSound);
        }
        
        public void PlayWinSound()
        {
            var index = Random.Range(0, winSounds.Length);
            PlaySfx(winSounds[index]);
        }

        public void PlayCashSound()
        {
            PlaySfx(cashSound);
        }

        public void PlaySpinSound()
        {
            PlaySfx(spinSound);
        }
    }
}