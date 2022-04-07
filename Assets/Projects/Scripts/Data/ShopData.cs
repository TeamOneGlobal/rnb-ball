using System;
using System.Linq;
using Projects.Scripts.Models;
using Sirenix.OdinInspector;
using ThirdParties.Truongtv.IapManager;
using Truongtv.Utilities;
using UnityEngine;

namespace Projects.Scripts.Data
{
    [CreateAssetMenu(fileName = "ShopData", menuName = "Truongtv/ShopData", order = 0)]
    public class ShopData : SingletonScriptableObject<ShopData>
    {
        [ShowIf("@this.iapData == null")]public IAPData iapData;
        [FoldoutGroup("Android")]public ShopItemData[] shopItemsAndroid;
        [FoldoutGroup("Ios")]public ShopItemData[] shopItemsIos;
        public string[] GetAllSkuItem()
        {
            return iapData.GetSkuItems().Select(a => a.skuId).ToArray();
        }

        private ShopItemData[] GetAllShopItem()
        {
            #if UNITY_IOS||UNITY_IPHONE
            return shopItemsIos;
           
            #elif UNITY_ANDROID
            return shopItemsAndroid;
            #endif
            return null;
        }

        public ShopId[] GetAllId()
        {
            return GetAllShopItem().Select(a => a.id).ToArray();
        }

        public ShopItemData GetItemById(ShopId findId)
        {
            return GetAllShopItem().ToList().Find(a => a.id.Equals(findId));
        }
    }

    [Serializable]
    public class ShopItemData
    {
        public ShopId id;
        public PurchaseType purchaseType;
        [ShowIf(nameof(purchaseType), PurchaseType.Iap),ValueDropdown(nameof(GetAllSkuItem))] public string skuId;
        [ShowIf(nameof(purchaseType), PurchaseType.Ad)] public int adValue;
        public ItemData reward;

        private string[] GetAllSkuItem()
        {
            return ShopData.Instance.GetAllSkuItem();
        }
    }

    public enum PurchaseType
    {
        Iap,Ad
    }

    public enum ShopId
    {
        special_offer,coin,life,ad,subscription
    }
}