using System;
using Projects.Scripts.Data;
using Projects.Scripts.Models;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Scripts.UIController.Popup
{
    public class DailyComponent : MonoBehaviour
    {
        [SerializeField] private Sprite todaySprite, notTodaySprite;
        [SerializeField] private Image bgImage;
        [SerializeField] private GameObject coinObj, heartObj, tickObj,grayObj;
        [SerializeField] private SkeletonGraphic red,blue;
        [SerializeField] private TextMeshProUGUI valueText;

        private DailyGiftItem _dailyGift;
        private Action _claimAction;

        public void Init(DailyGiftItem dailyGift, DailyGiftStatus status, bool today = false, Action claimAction = null)
        {
            _dailyGift = dailyGift;
            _claimAction = claimAction;
            if (_dailyGift.itemData.Count > 1)
            {
                foreach (var item in _dailyGift.itemData)
                {
                    InitGift(item);
                }
            }
            else
            {
                InitGift(_dailyGift.itemData[0]);
            }
            
            InitStatus(status);
            if (today)
            {
                if (bgImage)
                {
                    bgImage.sprite = todaySprite;
                    bgImage.SetNativeSize();
                }
                if (grayObj)
                {
                    grayObj.SetActive(false);
                }
            }
            else
            {
                if (bgImage)
                {
                    bgImage.sprite = notTodaySprite;
                    bgImage.SetNativeSize();
                }
            }
        }

        public void SetClaimed()
        {
            _claimAction?.Invoke();
            InitStatus(DailyGiftStatus.Claimed);
            // CustomNotification.Instance.DailyRewardResetReminder(false);
        }
        
        void InitGift(ItemData item)
        {
            if (item.coinValue != 0)
            {
                coinObj.SetActive(true);
                valueText.text = "+ " + item.coinValue + " COINS" + "\n";
            }
            
            if (item.lifeValue != 0)
            {
                heartObj.SetActive(true);
                valueText.text +=  $"+ {item.lifeValue} LIVES";
            }

            
            if (!string.IsNullOrEmpty(item.skinName))
            {
                red.gameObject.SetActive(true);
                red.Skeleton.SetToSetupPose();
                red.initialSkinName = item.skinName+"_1";
                red.Initialize(true);
                blue.gameObject.SetActive(true);
                blue.Skeleton.SetToSetupPose();
                blue.initialSkinName = item.skinName+"_2";
                blue.Initialize(true);
                valueText.gameObject.SetActive(item.lifeValue != 0);
            }

        }
        
        private void InitStatus(DailyGiftStatus status)
        {
            switch (status)
            {
                case DailyGiftStatus.Wait:
                case DailyGiftStatus.Ready:
                    grayObj.SetActive(false);
                    tickObj.SetActive(false);
                    break;
                case DailyGiftStatus.Claimed:
                    grayObj.SetActive(false);
                    tickObj.SetActive(true);
                    break;
                case DailyGiftStatus.NotClaimed:
                    grayObj.SetActive(true);
                    tickObj.SetActive(false);
                    break;
            }
        }

        public enum DailyGiftStatus
        {
            Wait,
            Ready,
            Claimed,
            NotClaimed
        }
    }
}