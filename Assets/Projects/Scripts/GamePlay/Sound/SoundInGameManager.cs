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
        [SerializeField,FoldoutGroup("UI Sounds")] private AudioClip win,lose;

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

        #region Ball
        public void PlayBallJumpSound()
        {
            var r = Random.Range(0, jumpClips.Length);
            soundManager.PlaySfx(jumpClips[r]);
        }

        public void PlayBallLandingSound()
        {
            soundManager.PlaySfx(ballLanding);
        }

        public void PlayBallDamagedSound()
        {
            soundManager.PlaySfx(ballHurt);
        }

        public void PlayBallAttackSound()
        {
            soundManager.PlaySfx(ballAttack);
        }

        public void PlayBallDieSound()
        {
            soundManager.PlaySfx(ballDie);
        }

        public void PlayBallCollectCoin()
        {
            soundManager.PlaySfx(ballCollectCoin);
        }

        public void PlayBallCollectStar()
        {
            soundManager.PlaySfx(ballCollectStar);
        }
        
        public void PlayBallCollectHeart()
        {
            soundManager.PlaySfx(ballCollectHeart);
        }
        public void PlayBallCollectMagnet()
        {
            soundManager.PlaySfx(ballCollectMagnet);
        }

        public void PlayBallTrySkin()
        {
            soundManager.PlaySfx(ballTrySkin);
        }

        public void PlayBallWinSound()
        {
            soundManager.PlaySfx(ballWin);
        }
        public void PlayBallExplode()
        {
            soundManager.PlaySfx(ballExplode);
        }
        public void PlayBallInCheckPoint()
        {
            soundManager.PlaySfx(ballInCheckPoint);
        }
        public void PlayBallRevive()
        {
            soundManager.PlaySfx(ballRevive);
        }
        
        #endregion

        #region UI

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
            soundManager.PlaySfx(lose,onComplete:complete);
        }

        public void PlayWinSound(Action complete)
        {
            Bgm.Instance.Stop();
            soundManager.PlaySfx(win,onComplete:complete);
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