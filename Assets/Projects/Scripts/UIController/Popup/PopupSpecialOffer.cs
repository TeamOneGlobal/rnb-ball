using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Projects.Scripts.Data;
using Sirenix.OdinInspector;
using ThirdParties.Truongtv;
using ThirdParties.Truongtv.AdsManager;
using ThirdParties.Truongtv.SoundManager;
using TMPro;
using Truongtv.PopUpController;
using Truongtv.Services.IAP;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Scripts.UIController.Popup
{
    public class PopupSpecialOffer : BasePopup
    {
        [SerializeField,ValueDropdown(nameof(GetAllShopItem))] private ShopId shopId;
        [SerializeField] private Transform container;
        [SerializeField] private Button closeButton, buyButton,viewAdButton;
        [SerializeField] private TextMeshProUGUI valueText,valueAdText,timeText;
        private bool _isInit;
        private ShopItemData shopItem;
        private ShopId[] GetAllShopItem()
        {
            return ShopData.Instance.GetAllId();
        }

        public void Initialized()
        {
            GameServiceManager.Instance.adManager.HideBanner();
            shopItem = GameDataManager.Instance.shopData.GetItemById(shopId);
            RegisterEvent();
            if (shopItem.purchaseType == PurchaseType.Ad)
            {
                viewAdButton.gameObject.SetActive(true);
                buyButton.gameObject.SetActive(false);
                valueAdText.text = GameDataManager.Instance.GetAdValue("offer") + "/" + shopItem.adValue;
            }
            else
            {
                viewAdButton.gameObject.SetActive(false);
                buyButton.gameObject.SetActive(true);
                valueText.text = GameServiceManager.Instance.iapManager.GetItemLocalPriceString(shopItem.skuId);
            }
            closeButton.gameObject.SetActive(false);
            
        }

        private IEnumerator SpecialOfferCountDown()
        {
            while (GameDataManager.Instance.IsBuySpecialOfferAvailable())
            {
                var span = GameDataManager.Instance.GetSpecialOfferRemainTime();
                timeText.text =
                    $"{span.TotalHours:00}:{span.Minutes:00}:{span.Seconds:00}";
                yield return new WaitForSeconds(1);
            }
            Close();
        }
        private void OnStart()
        {
            container.localScale = Vector3.zero;
            SoundManager.Instance.PlayPopupOpenSound();
            container.DOScale(1, DURATION).SetUpdate(UpdateType.Normal, true).SetEase(Ease.OutBack);
            StartCoroutine(ShowClose());
            StartCoroutine(SpecialOfferCountDown());
        }

        private void OnClose()
        {
            GameServiceManager.Instance.adManager.ShowBanner();
            SoundManager.Instance.PlayPopupCloseSound();
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
            viewAdButton.onClick.AddListener(ViewAd);
        }

        private void ClosePopup()
        {
            SoundManager.Instance.PlayButtonSound();
            Close();
        }

        private void Buy()
        {
           GameServiceManager.Instance.iapManager.PurchaseProduct(shopItem.skuId, (result, sku) =>
            {
                if (!result) return;
                BuyItem();
            });
        }

        private void ViewAd()
        {
            SoundManager.Instance.PlayButtonSound();
            GameServiceManager.Instance.adManager.ShowRewardedAd("Rewarded_SpecialOffer", ()=>
            {
                GameDataManager.Instance.ViewAd("offer");
                var unlockProgress = GameDataManager.Instance.GetAdValue("offer");
                valueText.text = unlockProgress + "/" + shopItem.adValue;
                if (unlockProgress == shopItem.adValue)
                {
                    BuyItem();
                }
            });
        }

        private void BuyItem()
        {
            GameDataManager.Instance.PurchaseBlockAd();
            GameDataManager.Instance.SetBuySpecialOffer();
            MenuController.Instance.UpdateCoin(shopItem.reward.coinValue,buyButton.transform);
            PopupMenuController.Instance.OpenPopupReceiveSkin(shopItem.reward.skinName,false, () =>
            {
                MenuController.Instance.UpdateSkin(shopItem.reward.skinName);
            });
            Close();
        }

        private IEnumerator ShowClose()
        {
            yield return new WaitForSeconds(2f);
            closeButton.gameObject.SetActive(true);
        }
    }
}