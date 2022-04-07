using System;
using Projects.Scripts.Models;
using Projects.Scripts.UIController.Popup;
using Sirenix.OdinInspector;
using Truongtv.PopUpController;
using UnityEngine;

namespace Projects.Scripts.UIController
{
    public class PopupMenuController : MonoBehaviour
    {
        [SerializeField] private PopupController controller;
        [SerializeField] private PopupGift popupGift;
        [SerializeField] private PopupSelectLevel popupSelectLevel;
        [SerializeField] private PopupReceiveSkin popupReceiveSkin;
        [SerializeField] private PopupShop popupShop;
        [SerializeField] private PopupLuckySpin popupLuckySpin;
        [SerializeField] private PopupSpecialOffer popupSpecialOffer;
        [SerializeField] private PopupDailyGift popupDaily;
        [SerializeField] private PopupViewAdToDoubleValue popupDoubleReward;
        [SerializeField] private PopupLottery popupLottery;
        [SerializeField] private PopupSubscriptionInfo popupSubscriptionInfo;
        [SerializeField] private PopupRate popupRate;
        private static PopupMenuController _instance;
        public static PopupMenuController Instance => _instance;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(_instance.gameObject);
            }

            _instance = this;
        }
       
        public void OpenPopupGift()
        {
            popupGift.Initialized();
            popupGift.gameObject.SetActive(true);
            popupGift.Show(controller);
        }
        public void OpenPopupSelectLevel()
        {
            popupSelectLevel.Initialized();
            popupSelectLevel.gameObject.SetActive(true);
            popupSelectLevel.Show(controller);
        }

        [Button]
        public void OpenPopupShop()
        {
            if( popupShop.gameObject.activeSelf) return;
            popupShop.Initialized();
            popupShop.gameObject.SetActive(true);
            popupShop.Show(controller);
        }

        public void OpenPopupSpin()
        {
            popupLuckySpin.Initialized();
            popupLuckySpin.gameObject.SetActive(true);
            popupLuckySpin.Show(controller);
        }

        [Button]
        public void OpenPopupDailyGift()
        {
            if (!GameDataManager.Instance.IsDailyEnable())
            {
                return;
            }
            if (!popupDaily.IsInit)
            {
                popupDaily.Inititialized();
            }
            popupDaily.gameObject.SetActive(true);
            popupDaily.Show(controller);
        }

        public void OpenPopupSpecialOffer()
        {
            popupSpecialOffer.Initialized();
            popupSpecialOffer.gameObject.SetActive(true);
            popupSpecialOffer.Show(controller);
        }

       
        public void OpenPopupReceiveSkin(string skin,bool needWatchAd = false,Action complete = null)
        {
            popupReceiveSkin.Initialized(skin,needWatchAd,complete);
            popupReceiveSkin.gameObject.SetActive(true);
            popupReceiveSkin.Show(controller);
        }

        public void OpenPopupDoubleReward(ItemData data)
        {
            popupDoubleReward.Init(data);
            popupDoubleReward.gameObject.SetActive(true);
            popupDoubleReward.Show(controller);
        }

        public void OpenLottery()
        {
            popupLottery.Init();
            popupLottery.gameObject.SetActive(true);
            popupLottery.Show(controller);
        }
        public void OpenSubscriptionInfo(string id,Action<Action> complete )
        {
            popupSubscriptionInfo.Init(id,complete);
            popupSubscriptionInfo.gameObject.SetActive(true);
            popupSubscriptionInfo.Show(controller);
        }
        [Button]
        public void OpenRate()
        {
            popupRate.gameObject.SetActive(true);
            popupRate.Show(controller);
        }
    }
}