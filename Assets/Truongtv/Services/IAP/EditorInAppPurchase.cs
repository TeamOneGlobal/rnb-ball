using System;
using UnityEngine;

namespace Truongtv.Services.IAP
{
    public class EditorInAppPurchase : IPaymentService
    {
        private event Action<bool,string> PurchaseActionCallback;
        public bool IsInitialized()
        {
            return true;
        }

        public void UpdateCallback(Action<bool, string> purchaseActionCallback)
        {
            PurchaseActionCallback = purchaseActionCallback;
        }

        public void PurchaseProduct(string sku)
        {
            PurchaseActionCallback?.Invoke(true,sku);
        }

        public string GetItemLocalPriceString(string sku)
        {
            return string.Empty;
        }

        public string GetItemLocalCurrency(string sku)
        {
            return "$";
        }

        public float GetItemLocalPrice(string sku)
        {
            return 0f;
        }

        public void RestorePurchase()
        {
            
        }
    }
}