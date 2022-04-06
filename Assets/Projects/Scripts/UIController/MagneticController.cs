using System;
using System.Collections.Generic;
using GamePlay;
using MEC;
using TMPro;
using Truongtv.Utilities;
using UnityEngine;
using UserDataModel;

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
            
            var duration = UserDataController.GetMagneticDuration();
            if (duration > 0)
            {
                magnetObj.SetActive(true);
                Timing.RunCoroutineSingleton(CountDown().CancelWith(magnetObj),magnetObj, behaviorOnCollision: SingletonBehavior.Abort);
                GamePlayController.Instance.MagnetCallback(true);
            }
        }

        private IEnumerator<float> CountDown()
        {
            var duration = UserDataController.GetMagneticDuration();
            do
            {
                durationText.text = TimeSpan.FromSeconds(duration).ToString(@"mm\:ss");
                yield return Timing.WaitForSeconds(1f);
                UserDataController.UpdateMagnetDuration(-1);
                duration = UserDataController.GetMagneticDuration();
            } while (duration >= 1);
            magnetObj.SetActive(false);
            GamePlayController.Instance.MagnetCallback(false);
        }

        public bool IsActiveMagnetic()
        {
            var duration = UserDataController.GetMagneticDuration();
            return duration > 0;
        }
    }
}