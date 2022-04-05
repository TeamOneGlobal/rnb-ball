using System;
using Truongtv.Services.Ad;
using Truongtv.Services.Firebase;
using Truongtv.Services.IAP;
using UIController;
using UnityEngine;

public static class NetWorkHelper
{
    public static void ShowRewardedAdInMenu(string location, Action notHaveInternet = null, Action adNotLoad = null,
        Action<bool> adResult = null)
    {
        FirebaseLogEvent.LogClickViewRewardedAd(location);
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            notHaveInternet?.Invoke();
            MenuPopupController.Instance.ShowNotification("NO CONNECTION","No internet connection. Make sure to turn on your Wifi/Mobile data.");
        }
        else
        {
            if (!AdManager.Instance.IsRewardVideoLoaded())
            {
                adNotLoad?.Invoke();
                MenuPopupController.Instance.ShowNotification("AD NOT AVAILABLE","Ads is still coming. Please try again later.");
            }
            else
            {
                FirebaseLogEvent.LogStartViewRewardedAd(location);
                AdManager.Instance.ShowRewardVideo(location,result =>
                {
                    adResult?.Invoke(result);
                    if (result)
                    {
                        FirebaseLogEvent.LogFinishViewRewardedAd(location);
                    }
                });
            }
        }
    }
    
    public static void ShowRewardedAdInGame(string location, Action notHaveInternet = null, Action adNotLoad = null,
        Action<bool> adResult = null)
    {
        FirebaseLogEvent.LogClickViewRewardedAd(location);
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            notHaveInternet?.Invoke();
            GamePlayPopupController.Instance.ShowNotificationText("No internet connection. Make sure to turn on your Wifi/Mobile data.");
        }
        else
        {
            if (!AdManager.Instance.IsRewardVideoLoaded())
            {
                adNotLoad?.Invoke();
                GamePlayPopupController.Instance.ShowNotificationText("Ads is still coming. Please try again later.");
            }
            else
            {
                FirebaseLogEvent.LogStartViewRewardedAd(location);
                AdManager.Instance.ShowRewardVideo(location,result =>
                {
                    adResult?.Invoke(result);
                    if (result)
                    {
                        FirebaseLogEvent.LogFinishViewRewardedAd(location);
                    }
                });
            }
        }
    }

    public static void RequestBanner()
    {
        AdManager.Instance.RequesBanner();
    }
    public static void ShowBannerAd()
    {
        AdManager.Instance.ShowBannerAd();
    }
    public static void HideBannerAd()
    {
        AdManager.Instance.HideBannerAd();
    }

    public static void ShowInterstitialAd(
        Action<bool> adResult = null)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            adResult?.Invoke(false);
            return;
        };
        if (AdManager.Instance.IsInterstitialLoaded())
        {
            FirebaseLogEvent.LogViewInterstitial();
            AdManager.Instance.ShowInterstitial(result => { adResult?.Invoke(result); });
        }
        else
        {
            adResult?.Invoke(false);
        }
    }

    public static void PurchaseProduct(string sku,Action<bool,string> callback)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            MenuPopupController.Instance.ShowNotification("NO CONNECTION","No internet connection. Make sure to turn on your Wifi/Mobile data.");
        }
        else
        {
            IAPManager.Instance.PurchaseProduct(sku,callback);
        }
    }
}