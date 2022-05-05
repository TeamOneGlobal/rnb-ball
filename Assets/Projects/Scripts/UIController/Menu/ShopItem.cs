using System;
using Projects.Scripts.Data;
using Projects.Scripts.UIController.Popup;
using Sirenix.OdinInspector;
using ThirdParties.Truongtv;
using ThirdParties.Truongtv.AdsManager;
using ThirdParties.Truongtv.SoundManager;
using TMPro;
using Truongtv.Services.IAP;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Scripts.UIController.Menu
{
    public class ShopItem : MonoBehaviour
    {
        [SerializeField] private bool isSubscription;
        [SerializeField,ValueDropdown(nameof(GetAllShopItem))] private ShopId shopId;
        [SerializeField] private Button buyButton;
        [SerializeField] private TextMeshProUGUI price;
        [SerializeField] private GameObject adIcon,purchased;
        [SerializeField] private ShopItemData shopItem;
        private PopupShop _popupShop;
        private ShopId[] GetAllShopItem()
        {
            return ShopData.Instance.GetAllId();
        }

        public void Init(PopupShop shop)
        {
            _popupShop = shop;
            shopItem = GameDataManager.Instance.shopData.GetItemById(shopId);
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(OnBuyClick);
            if (shopItem.purchaseType == Data.PurchaseType.Ad)
            {
                adIcon.SetActive(true);
                price.text = GameDataManager.Instance.GetAdValue("shop_"+shopItem.id) + "/" + shopItem.adValue;
            }
            else
            {
                adIcon.SetActive(false);
                price.text = GameServiceManager.Instance.iapManager.GetItemLocalPriceString(shopItem.skuId);
            }

            if (shopItem.id == ShopId.ad && GameDataManager.Instance.IsPurchaseBlockAd())
            {
                purchased.SetActive(true);
                buyButton.interactable = false;
            }

            if (shopItem.id == ShopId.subscription && GameDataManager.Instance.IsSubscription())
            {
                purchased.SetActive(true);
                buyButton.interactable = false;
            }
        }

        public void OnOtherItemPurchase(ShopItemData item)
        {
            if (shopItem.id == ShopId.ad)
            {
                purchased.SetActive(true);
                buyButton.interactable = false;
            }
        }
        private void OnBuyClick()
        {
            SoundManager.Instance.PlayButtonSound();
            if (shopItem.purchaseType == Data.PurchaseType.Ad)
            {
                GameServiceManager.Instance.adManager.ShowRewardedAd("shop_"+shopItem.id, () =>
                {
                    GameDataManager.Instance.ViewAd("shop_"+shopItem.id);
                    var unlockProgress = GameDataManager.Instance.GetAdValue("shop_"+shopItem.id);
                    if (unlockProgress == shopItem.adValue)
                    {
                        BuyItem();
                        GameDataManager.Instance.ResetAdValue("shop_"+shopItem.id);
                        unlockProgress = GameDataManager.Instance.GetAdValue("shop_"+shopItem.id);
                    }
                    price.text = unlockProgress + "/" + shopItem.adValue;
                });
            }
            else
            {
                if (shopItem.id == ShopId.subscription)
                {
                    PopupMenuController.Instance.OpenSubscriptionInfo(shopItem.skuId,BuyItem);
                }
                else
                {
                    GameServiceManager.Instance.iapManager.PurchaseProduct(shopItem.skuId, (result, sku) =>
                    {
                        if(!result) return;
                        BuyItem();
                    });
                }
            }
        }

        private void BuyItem(Action complete = null)
        {
            _popupShop.PurchaseSuccessCallback(shopItem);
            GameDataManager.Instance.PurchaseBlockAd();
            if (shopItem.id == ShopId.subscription)
            {
                GameDataManager.Instance.SetInSubscription(true);
                purchased.SetActive(true);
                buyButton.interactable = false;
            }
            // else if (shopItem.id == ShopId.life)
            // {
            //     GameDataManager.Instance.SetInfinityLife();
            // }
            MenuController.Instance.UpdateCoin(shopItem.reward.coinValue,transform);
            MenuController.Instance.UpdateLife(shopItem.reward.lifeValue,transform);
            complete?.Invoke();
        }
    }
}