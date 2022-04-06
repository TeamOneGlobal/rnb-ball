using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Truongtv.PopUpController;
using UIController.Popup;
using UnityEngine;

namespace UIController
{
    public class MenuPopupController : PopupController
    {
        [SerializeField] private WinPopup winPopup;
        [SerializeField] private SkinPopup skinPopup;
        [SerializeField] private CongratsPopup congratsPopup;
        [SerializeField] private ShopPopup shopPopup;
        [SerializeField] private PurchaseSuccessPopup purchaseSuccessPopup;
        [SerializeField] private SkinExpiredPopup skinExpiredPopup;
        [SerializeField] private NotEnoughCoinPopup notEnoughCoinPopup;
        [SerializeField] private SpinPopup spinPopup;
        [SerializeField] private SpecialOfferPopup specialOfferPopup;
        [SerializeField] private PurchaseSuccessPopupWithDoubleValue purchaseSuccessPopupWithDoubleValue;
        [SerializeField] private MissionPopup missionPopup;
        [SerializeField] private RatingPopup ratingPopup;
        [SerializeField] private DailyGiftPopup dailyGiftPopup;
        [SerializeField] private GiftPopup giftPopup;
        private static MenuPopupController _instance;
        public static MenuPopupController Instance => _instance;
        private void Awake()
        {
            if (_instance == null)
                _instance = this;
        }
        
        public void ShowWinPopup()
        {
            if(winPopup.gameObject.activeSelf) return;
            winPopup.gameObject.SetActive(true);
            winPopup.Initialized();
            winPopup.Show(this);
        }

        public void ShowSkinPopup()
        {
            if(skinPopup.gameObject.activeSelf) return;
            skinPopup.gameObject.SetActive(true);
            skinPopup.Initialized();
            skinPopup.Show(this);
        }
        public void ShowCongratsPopup(string skin)
        {
            congratsPopup.gameObject.SetActive(true);
            congratsPopup.Initialized(skin);
            congratsPopup.Show(this);
        }

        public void ShowUnlockSkinsPopup(List<string> skins, Action callback = null)
        {
            congratsPopup.gameObject.SetActive(true);
            congratsPopup.Initialized(skins,0,callback);
            congratsPopup.Show(this);
        }
        public void ShowShopPopup()
        {
            shopPopup.Initialized();
            if(shopPopup.gameObject.activeSelf) return;
            shopPopup.gameObject.SetActive(true);
            shopPopup.Show(this);
        }

        public void ShowPurchaseSuccess(PurchaseSuccessPopup.PurchaseType type,long value,Action complete = null)
        {
            if(purchaseSuccessPopup.gameObject.activeSelf) return;
            purchaseSuccessPopup.gameObject.SetActive(true);
            purchaseSuccessPopup.Initialized(type,value,complete);
            purchaseSuccessPopup.Show(this);
        }

        public void ShowSkinExpired()
        {
            if(skinExpiredPopup.gameObject.activeSelf) return;
            skinExpiredPopup.gameObject.SetActive(true);
            skinExpiredPopup.Initialized();
            skinExpiredPopup.Show(this);
        }
        public void ShowNotEnoughCoinPopup(Action callback = null)
        {
            if(notEnoughCoinPopup.gameObject.activeSelf) return;
            notEnoughCoinPopup.gameObject.SetActive(true);
            notEnoughCoinPopup.Initialized(callback);
            notEnoughCoinPopup.Show(this);
        }

        public void ShowSpinPopup()
        {
            if(spinPopup.gameObject.activeSelf) return;
            spinPopup.gameObject.SetActive(true);
            spinPopup.Initialized();
            spinPopup.Show(this);
        }

        public void ShowSpecialOffer()
        {
            if(specialOfferPopup.gameObject.activeSelf) return;
            specialOfferPopup.gameObject.SetActive(true);
            specialOfferPopup.Initialized();
            specialOfferPopup.Show(this);
        }
        public void ShowPurchaseSuccessDoubleValue(PurchaseSuccessPopup.PurchaseType type,long value,Action complete = null)
        {
            if(purchaseSuccessPopupWithDoubleValue.gameObject.activeSelf) return;
            purchaseSuccessPopupWithDoubleValue.gameObject.SetActive(true);
            purchaseSuccessPopupWithDoubleValue.Initialized(type,value,complete);
            purchaseSuccessPopupWithDoubleValue.Show(this);
        }
        public void ShowMissionPopup()
        {
            if(missionPopup.gameObject.activeSelf) return;
            missionPopup.gameObject.SetActive(true);
            missionPopup.Initialized();
            missionPopup.Show(this);
        }
        public void ShowRatingPopup()
        {
            if(ratingPopup.gameObject.activeSelf) return;
            ratingPopup.gameObject.SetActive(true);
            ratingPopup.Initialized();
            ratingPopup.Show(this);
        }
        public void ShowDailyGiftPopup()
        {
            if(dailyGiftPopup.gameObject.activeSelf) return;
            dailyGiftPopup.gameObject.SetActive(true);
            dailyGiftPopup.Initialized();
            dailyGiftPopup.Show(this);
        }
        [Button]
        public void ShowGiftPopup()
        {
            if(giftPopup.gameObject.activeSelf) return;
            giftPopup.gameObject.SetActive(true);
            giftPopup.Initialized();
            giftPopup.Show(this);
        }
    }
}