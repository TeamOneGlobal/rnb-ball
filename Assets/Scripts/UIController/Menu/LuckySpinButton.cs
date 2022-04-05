using System;
using System.Collections.Generic;
using MEC;
using Truongtv.Utilities;
using UnityEngine;
using UserDataModel;

namespace UIController.Menu
{
    public class LuckySpinButton : SingletonMonoBehavior<LuckySpinButton>
    {
        [SerializeField] private GameObject noticeObj;


        private void Start()
        {
            Timing.RunCoroutineSingleton(CountDown().CancelWith(gameObject),gameObject,SingletonBehavior.Abort);
        }

        private IEnumerator<float> CountDown()
        {
            var lastTime = UserDataController.GetLastSpinTime();
            int totalSecond;
            do
            {
                var totalWaitSecond = Mathf.FloorToInt((float) DateTime.Now.Subtract(lastTime).TotalSeconds);
                totalSecond = Mathf.FloorToInt((float) TimeSpan.FromHours(Config.FREE_SPIN_COOLDOWN_HOURS)
                    .Subtract(TimeSpan.FromSeconds(totalWaitSecond)).TotalSeconds);
                if (totalSecond <= 0)
                {
                    break;
                }
                noticeObj.SetActive(false);
                yield return Timing.WaitForSeconds(1f);
                
               
            } while (totalSecond > 0);
            noticeObj.SetActive(true);
        }

        public void CheckTime()
        {
            Timing.RunCoroutineSingleton(CountDown().CancelWith(gameObject),gameObject,SingletonBehavior.Abort);
        }
    }
}