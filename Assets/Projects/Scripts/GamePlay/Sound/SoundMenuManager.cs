using ThirdParties.Truongtv.SoundManager;
using UnityEngine;

namespace Projects.Scripts.GamePlay.Sound
{
    public class SoundMenuManager : MonoBehaviour
    {
        private static SoundMenuManager _instance;
        public static SoundMenuManager Instance => _instance;

        [SerializeField] private SoundManager soundManager;
        [SerializeField] private AudioClip win,spinSound;
        [SerializeField] private AudioClip[] bgm;
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(_instance.gameObject);
            }

            _instance = this;
        }



        public void PlayWinSound()
        {
            soundManager.PlaySfx(win);
        }
        public void PlaySpinSound()
        {
            soundManager.PlaySfx(spinSound);
        }
        public void PlayBgmSound()
        {
            var r = Random.Range(0, bgm.Length);
            if(Bgm.Instance)
                Bgm.Instance.Play(bgm[r]);
        }

        public void StopBgmSound()
        {
            if (Bgm.Instance)
            {
                Bgm.Instance.Stop();
            }
        }
    }
}