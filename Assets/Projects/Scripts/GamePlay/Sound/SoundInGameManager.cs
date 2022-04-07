using System;
using Sirenix.OdinInspector;
using ThirdParties.Truongtv.SoundManager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Projects.Scripts.GamePlay.Sound
{
    
    public class SoundInGameManager : MonoBehaviour
    {
        [SerializeField] private SoundManager soundManager;
        [SerializeField,FoldoutGroup("UI Sounds")] private AudioClip win,lose,changeTarget;
        [SerializeField] private AudioClip[] winSounds;
        [SerializeField] private AudioClip[] loseSound;
        [SerializeField, FoldoutGroup("UI Sounds")]
        private AudioClip[] bgm,bossFight;
        [FoldoutGroup("Game Play Sounds")]
        [SerializeField, FoldoutGroup("Game Play Sounds/Ball")]
        private AudioClip ballTrySkin, ballLanding, ballHurt, ballAttack, ballDie, ballCollectCoin,ballCollectStar,ballCollectHeart,ballCollectMagnet,ballWin,ballExplode,ballInCheckPoint,ballRevive;

        [SerializeField, FoldoutGroup("Game Play Sounds/Ball")]
        private AudioClip[] jumpClips;
        [SerializeField, FoldoutGroup("Game Play Sounds/Item")]
        private AudioClip spring;
        [SerializeField, FoldoutGroup("Game Play Sounds/Item")]
        private AudioClip cageBreakSound,ballRescueSound;
        private static SoundInGameManager _instance;
        public static SoundInGameManager Instance => _instance;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(_instance.gameObject);
            }

            _instance = this;
        }

        #region UI

        public void PlayChangeTargetSound()
        {
            soundManager.PlaySfx(changeTarget);
        }
        public void PlayBgmSound()
        {
            var r = Random.Range(0, bgm.Length);
            if(Bgm.Instance)
                Bgm.Instance.Play(bgm[r]);
        }

        public void PlayBossFightSound()
        {
            var r = Random.Range(0, bossFight.Length);
            if(Bgm.Instance)
                Bgm.Instance.Play(bossFight[r]);
        }
        public void PlayLoseSound(Action complete)
        {
            Bgm.Instance.Stop();
            var r = Random.Range(0, loseSound.Length);
            soundManager.PlaySfx(loseSound[r],onComplete:complete);
        }

        public void PlayWinSound(Action complete)
        {
            Bgm.Instance.Stop();
            var r = Random.Range(0, winSounds.Length);
            soundManager.PlaySfx(winSounds[r],onComplete:complete);
        }
        

        #endregion

        #region Item

        public void PlaySpringSound()
        {
            soundManager.PlaySfx(spring);
        }
        public void PlayCageBreakSound()
        {
            soundManager.PlaySfx(cageBreakSound);
        }
        public void PlayRescueSound()
        {
            soundManager.PlaySfx(ballRescueSound);
        }
        
        #endregion
    }
}