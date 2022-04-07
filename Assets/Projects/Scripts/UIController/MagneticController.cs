using System;
using System.Collections.Generic;
using GamePlay;
using MEC;
using Projects.Scripts;
using TMPro;
using Truongtv.Utilities;
using UnityEngine;

namespace UIController
{
    public class MagneticController : SingletonMonoBehavior<MagneticController>
    {
        [SerializeField] private GameObject magnetObj;
        [SerializeField] private TextMeshProUGUI durationText;
        
        public void Init()
        {
           if(!IsActiveMagnetic()) return;
                ActiveMagnetic();
        }

        public void ActiveMagnetic()
        {
            
            var duration = GameDataManager.Instance.GetMagnetDuration();
            if (duration > 0)
            {
                magnetObj.SetActive(true);
                Timing.RunCoroutineSingleton(CountDown().CancelWith(magnetObj),magnetObj, behaviorOnCollision: SingletonBehavior.Abort);
                GamePlayController.Instance.MagnetCallback(true);
            }
        }

        private IEnumerator<float> CountDown()
        {
            var duration = GameDataManager.Instance.GetMagnetDuration();
            do
            {
                durationText.text = TimeSpan.FromSeconds(duration).ToString(@"mm\:ss");
                yield return Timing.WaitForSeconds(1f);
                GameDataManager.Instance.CountDownMagnet();
                duration = GameDataManager.Instance.GetMagnetDuration();
            } while (duration >= 1);
            magnetObj.SetActive(false);
            GamePlayController.Instance.MagnetCallback(false);
        }

        public bool IsActiveMagnetic()
        {
            var duration = GameDataManager.Instance.GetMagnetDuration();
            return duration > 0;
        }
    }
}