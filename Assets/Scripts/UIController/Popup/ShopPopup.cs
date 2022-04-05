using DG.Tweening;
using Sound;
using ThirdParties.Truongtv;
using TMPro;
using Truongtv.PopUpController;
using Truongtv.Services.IAP;
using UnityEngine;
using UnityEngine.UI;
using UserDataModel;

namespace UIController.Popup
{
    public class ShopPopup : BasePopup
    {
        [SerializeField] private Transform container;
        [SerializeField] private Button buyCoinAdButton, buyLifeAdButton, buyAdButton,closeButton;
        [SerializeField] private TextMeshProUGUI coinAdPrice, lifeAdPrice, noAdPrice;
        [SerializeField] private Image[] adImages;
        [SerializeField] private Material normalMaterial, grayScaleMaterial;
        [SerializeField] private string coinAdSku, lifeAdSku, blockAdSku;

        private bool _isInit;
        public void Initialized()
        {
            RegisterEvent();
            var isBlockAd = UserDataController.IsBlockAd();
            buyAdButton.interactable = !isBlockAd;
            foreach (var img in adImages)
            {
                img.material = isBlockAd ? grayScaleMaterial : normalMaterial;
            }

            coinAdPrice.text = GameServiceManager.Instance.iapManager.GetItemLocalPriceString(coinAdSku);
            lifeAdPrice.text = GameServiceManager.Instance.iapManager.GetItemLocalPriceString(lifeAdSku);
            noAdPrice.text = GameServiceManager.Instance.iapManager.GetItemLocalPriceString(blockAdSku);
        }

        private void RegisterEvent()
        {
            if(_isInit) return;
            _isInit = true;
            openAction = OnStart;
            closeAction = OnClose;
            buyCoinAdButton.onClick.AddListener(BuyAdCoin);
            buyLifeAdButton.onClick.AddListener(BuyAdLife);
            buyAdButton.onClick.AddListener(BuyBlockAd);
            closeButton.onClick.AddListener(ClosePopup);
        }

        private void ClosePopup()
        {
             SoundMenuController.Instance.PlayButtonClickSound();
             Close();
        }
        private void BuyAdCoin()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            GameServiceManager.Instance.iapManager.PurchaseProduct(coinAdSku, (result, sku) =>
            {
                if(!result)return;
                Initialized();
                UserDataController.SetBlockAd();
                MenuPopupController.Instance.ShowPurchaseSuccess(PurchaseSuccessPopup.PurchaseType.ComboAdCoin,50000);
            });
        }

        private void BuyAdLife()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            GameServiceManager.Instance.iapManager.PurchaseProduct(lifeAdSku, (result, sku) =>
            {
                if(!result)return;
                UserDataController.SetBlockAd();
                MenuPopupController.Instance.ShowPurchaseSuccess(PurchaseSuccessPopup.PurchaseType.ComboAdLife,50);
            });
        }

        private void BuyBlockAd()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            GameServiceManager.Instance.iapManager.PurchaseProduct(blockAdSku, (result, sku) =>
            {
                if(!result)return;
                Initialized();
                UserDataController.SetBlockAd();
                MenuPopupController.Instance.ShowPurchaseSuccess(PurchaseSuccessPopup.PurchaseType.BlockAd,50000);
            });
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

        public void RestorePurchase()
        {
            GameServiceManager.Instance.iapManager.RestorePurchase();
        }
    }
}