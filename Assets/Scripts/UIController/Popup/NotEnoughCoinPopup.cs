using System;
using System.Runtime.CompilerServices;
using DG.Tweening;
using Sound;
using TMPro;
using Truongtv.PopUpController;
using Truongtv.Services.IAP;
using UnityEngine;
using UnityEngine.UI;

namespace UIController.Popup
{
    public class NotEnoughCoinPopup : BasePopup
    {
        [SerializeField] private Transform container;
        [SerializeField] private Button iapButton, adButton,closeButton;
        [SerializeField] private TextMeshProUGUI adValue,iapPrice;
        [SerializeField] private string sku;
        private bool _isInit;
        private Action _complete;
        public void Initialized(Action callback = null)
        {
            _complete = callback;
            adValue.text = "" + Config.REWARDED_FREE_COIN;
            iapPrice.text = IAPManager.Instance.GetItemLocalPriceString(sku);
            RegisterEvent();
        }
        private void OnStart()
        {
            container.localScale = Vector3.zero;
            SoundMenuController.Instance.PlayPopupSound();
            container.DOScale(1, DURATION).SetUpdate(UpdateType.Normal,true).SetEase(Ease.OutBack);
        }

        private void OnClose()
        {
            SoundMenuController.Instance.PlayPopupSound();
            container.DOScale(0, DURATION).SetUpdate(UpdateType.Normal,true).SetEase(Ease.InBack);
        }
        private void RegisterEvent()
        {
            if(_isInit) return;
            _isInit = true;
            openAction = OnStart;
            closeAction = OnClose;
            iapButton.onClick.AddListener(BuyIap);
            adButton.onClick.AddListener(BuyAd);
            closeButton.onClick.AddListener(ClosePopup);
        }

        private void ClosePopup()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            Close();
        }
        private void BuyIap()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            NetWorkHelper.PurchaseProduct(sku, (result, sku) =>
            {
                if(!result) return;
                Close();
                MenuPopupController.Instance.ShowPurchaseSuccess(PurchaseSuccessPopup.PurchaseType.IapCoin,10000,() =>
                {
                   // _complete?.Invoke();
                });
            });
        }

        private void BuyAd()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            NetWorkHelper.ShowRewardedAdInMenu("Rewarded_NotEnoughCoin",adResult: result =>
            {
                if(!result) return;
                Close();
                MenuPopupController.Instance.ShowPurchaseSuccess(PurchaseSuccessPopup.PurchaseType.FreeCoin,Config.REWARDED_FREE_COIN,() =>
                {
                   // _complete?.Invoke();
                });
            });
        }
    }
}