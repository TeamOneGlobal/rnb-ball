using System.Collections.Generic;
using DG.Tweening;
using GamePlay;
using Sound;
using TMPro;
using Truongtv.PopUpController;
using Truongtv.Services.IAP;
using UIController.Scene;
using UnityEngine;
using UnityEngine.UI;
using UserDataModel;

namespace UIController.Popup
{
    public class SpecialOfferPopup : BasePopup
    {
        [SerializeField] private Transform container;
        [SerializeField] private Button closeButton, buyButton;
        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private string skuId;
        private bool _isInit;


        public void Initialized()
        {
            RegisterEvent();
            valueText.text = IAPManager.Instance.GetItemLocalPriceString(skuId);
        }

        private void OnStart()
        {
            container.localScale = Vector3.zero;
            SoundMenuController.Instance.PlayPopupSound();
            container.DOScale(1, DURATION).SetUpdate(UpdateType.Normal, true).SetEase(Ease.OutBack);
        }

        private void OnClose()
        {
            SoundMenuController.Instance.PlayPopupSound();
            container.DOScale(0, DURATION).SetUpdate(UpdateType.Normal, true).SetEase(Ease.InBack);
        }

        private void RegisterEvent()
        {
            if (_isInit) return;
            _isInit = true;
            openAction = OnStart;
            closeAction = OnClose;
            closeButton.onClick.AddListener(ClosePopup);
            buyButton.onClick.AddListener(Buy);
        }

        private void ClosePopup()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            Close();
        }

        private void Buy()
        {
            NetWorkHelper.PurchaseProduct(skuId, (result, sku) =>
            {
                if (!result) return;
                UserDataController.UnlockSkin("19");
                UserDataController.UnlockSkin("20");
                UserDataController.SetBuySpecialOffer();
                MenuScene.Instance.UpdateBall();
                Close();
                MenuScene.Instance.HideOfferButton();
                MenuPopupController.Instance.ShowUnlockSkinsPopup(new List<string>{"19","20"}, () =>
                {
                    MenuPopupController.Instance.ShowPurchaseSuccess(PurchaseSuccessPopup.PurchaseType.ComboAdCoin,50000);
                });
            });
        }
    }
}