#if USING_UDP_IN_APP_PURCHASE && USING_IN_APP_PURCHASE

using System;
using UnityEngine;
using UnityEngine.UDP;

namespace Truongtv.Services.IAP
{
    public class UdpInAppPurchase:IInitListener,IPurchaseListener,IPaymentService
    {
        private bool isInit;
    private event Action<bool,string> PurchaseActionCallback;
    private Inventory _inventory;

    public UdpInAppPurchase()
    {
        StoreService.Initialize(this);
    }
    public void OnInitialized(UserInfo userInfo)
    {
        Debug.Log("Initialization succeeded");
        StoreService.QueryInventory(this);
    }

    public void OnInitializeFailed(string message)
    {
        Debug.Log("Initialization failed: " + message);
        isInit = false;
    }
    public void OnPurchase(PurchaseInfo purchaseInfo)
    {
        var productInfo = _inventory.GetProductInfo(purchaseInfo.ProductId);
        if (productInfo.Consumable.HasValue && productInfo.Consumable.Value)
        {
            StoreService.ConsumePurchase(purchaseInfo,this);
        }
        else PurchaseActionCallback?.Invoke(true,purchaseInfo.ProductId);
    }

    public void OnPurchaseFailed(string message, PurchaseInfo purchaseInfo)
    {
        Debug.Log("Purchase Failed: " + message);
        PurchaseActionCallback?.Invoke(true,purchaseInfo.ProductId);
    }

    public void OnPurchaseRepeated(string productCode)
    {
        // Some stores don't support queryInventory.
        Debug.Log("OnPurchaseRepeated: " + productCode);
    }

    public void OnPurchaseConsume(PurchaseInfo purchaseInfo)
    {
        PurchaseActionCallback?.Invoke(true,purchaseInfo.ProductId);
    }

    public void OnPurchaseConsumeFailed(string message, PurchaseInfo purchaseInfo)
    {
        Debug.Log("Consume Failed: " + message);
        PurchaseActionCallback?.Invoke(false,purchaseInfo.ProductId);
    }

    public void OnQueryInventory(Inventory inventory)
    {
        _inventory = inventory;
        isInit = true;
    }

    public void OnQueryInventoryFailed(string message)
    {
        isInit = false;
    }

    public bool IsInitialized()
    {
        return isInit;
    }

    public void PurchaseProduct(string productId)
    {
        StoreService.Purchase(productId,"",this);
    }

    public void UpdateCallback(Action<bool, string> purchaseActionCallback)
    {
        PurchaseActionCallback += purchaseActionCallback;
    }

    public string GetItemLocalPriceString(string id)
    {
        var productInfo = _inventory.GetProductInfo(id);
        return productInfo.Price;
    }

    public void RestorePurchase()
    {
        
    }

    public string GetItemLocalCurrency(string id)
    {
        var productInfo = _inventory.GetProductInfo(id);
        return productInfo.Currency;
    }

    public decimal GetItemLocalPrice(string id)
    {
        var productInfo = _inventory.GetProductInfo(id);
        return productInfo.PriceAmountMicros;
    }

    }
}
#endif