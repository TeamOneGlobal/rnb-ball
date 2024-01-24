#if USING_MAX
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamOne.Tracking;
using Truongtv.Services.Ad;
using UnityEngine;

namespace ThirdParties.Truongtv.AdsManager
{
    public class MaxAdClient : IAdClient
    {
        private const string MaxSdkKey = "_mvVQTBIbzvJoHiX7np3dCoIiei9V8Bp4sttwDEq88ULZ38FG5rUNM93JFgVgMOp0Q2gBKN3RTvvr21GdDrztB";

#if UNITY_ANDROID
        private const string INTERSTITIAL_AD_UNIT_ID = "638e28190a7f84f5";
        private const string REWARDED_AD_UNIT_ID = "e70fe0ff20981503";
        private const string BANNER_AD_UNIT_ID = "7ac1e798c7927ad8";
        // private const string MREC_AD_UNIT_ID = "";
        private const string APP_OPEN_AD_UNIT_ID = "281d7ccabcf4f1e1";
#elif UNITY_IOS
        private const string INTERSTITIAL_AD_UNIT_ID = "b3411ee1ed0756a5";
        private const string REWARDED_AD_UNIT_ID = "bbdcd5a4287b451c";
        private const string BANNER_AD_UNIT_ID = "71974066cca86660";
        // private const string MREC_AD_UNIT_ID = "";
        private const string APP_OPEN_AD_UNIT_ID = "c7b009297d71bb8b";
#endif
        private Action<bool> _adCallback;
        #region public method

        public void Initialized()
        {
            MaxSdkCallbacks.OnSdkInitializedEvent += async (sdkConfiguration) =>
            {
                Debug.Log("MAX SDK Initialized");
                
                InitAppOpenAd();
                InitializeInterstitialAds();
                InitializeRewardedAds();
                InitializeBannerAds();
                // InitializeMRecAds();
                LoadAppOpenAd();
                await Task.Delay(TimeSpan.FromSeconds(4f));
                LoadInterstitial();
                LoadRewardedAd();
                LoadBannerAd();
                // LoadMRECAd();

            };
            
            MaxSdk.SetHasUserConsent(true);
            MaxSdk.SetIsAgeRestrictedUser(false);
            MaxSdk.SetSdkKey(MaxSdkKey);
            MaxSdk.InitializeSdk();
        }

        public void OnApplicationPause(bool isPause)
        {
        }

        public bool IsRewardVideoLoaded()
        {
            return MaxSdk.IsRewardedAdReady(REWARDED_AD_UNIT_ID);
        }

        public void ShowRewardVideo(Action<bool> actionFinishAd = null)
        {
           // GameServiceManager.MarketingTrackEvent("af_rewarded_ad_eligible");
            if (MaxSdk.IsRewardedAdReady(REWARDED_AD_UNIT_ID))
            {
                //GameServiceManager.MarketingTrackEvent("af_rewarded_api_called");
                _adCallback = actionFinishAd;
                MaxSdk.ShowRewardedAd(REWARDED_AD_UNIT_ID);
            }
            else
            {
                actionFinishAd?.Invoke(false);
            }
        }

        public bool IsInterstitialLoaded()
        {
            return MaxSdk.IsInterstitialReady(INTERSTITIAL_AD_UNIT_ID);
        }

        public void ShowInterstitial(Action<bool> actionFinishAd = null)
        {
            //GameServiceManager.MarketingTrackEvent("af_inters_ad_eligible");
            if (MaxSdk.IsInterstitialReady(INTERSTITIAL_AD_UNIT_ID))
            {
                //GameServiceManager.MarketingTrackEvent("af_inters_api_called");

                _adCallback = actionFinishAd;
                MaxSdk.ShowInterstitial(INTERSTITIAL_AD_UNIT_ID);
            }
            else
            {
                actionFinishAd?.Invoke(false);
            }
        }
        public void ShowBannerAd(Action<bool> actionFinishAd = null)
        {
            MaxSdk.ShowBanner(BANNER_AD_UNIT_ID);
        }
        public void HideBannerAd()
        {
            MaxSdk.HideBanner(BANNER_AD_UNIT_ID);
        }

        // public void ShowMRECAd(MRECPosition mrecPosition)
        // {
        //     var pos = (int) mrecPosition;
        //
        //     MaxSdk.UpdateMRecPosition(MREC_AD_UNIT_ID, (MaxSdkBase.AdViewPosition) pos);
        //     MaxSdk.ShowMRec(MREC_AD_UNIT_ID);
        // }
        //
        // public void HideMRECAd()
        // {
        //     MaxSdk.HideMRec(MREC_AD_UNIT_ID);
        // }
        public bool IsAppOpenAdLoaded()
        {
            // if (!MaxSdk.IsAppOpenAdReady(APP_OPEN_AD_UNIT_ID))
            // {
            //     LoadAppOpenAd();
            // }
            return MaxSdk.IsAppOpenAdReady(APP_OPEN_AD_UNIT_ID);
        }

        public void ShowAppOpenAd()
        {
            MaxSdk.ShowAppOpenAd(APP_OPEN_AD_UNIT_ID);
        }
        #endregion

        #region Interstitial Ads


        private void InitializeInterstitialAds()
        {
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnAdLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnAdLoadFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnAdDisplayedEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnAdHiddenEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnAdFailedToDisplayEvent;
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
            LoadInterstitial();
        }
        private void LoadInterstitial()
        {
            MaxSdk.LoadInterstitial(INTERSTITIAL_AD_UNIT_ID);
        }
        #endregion

        #region Rewarded Ads

        private int _rewardedRetryAttempt,_interRetryAttempt,_aoaRetryAttempt;

        private void InitializeRewardedAds()
        {
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnAdLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnAdHiddenEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnAdReceivedRewardEvent;
            
        }

        private void LoadRewardedAd()
        {
            MaxSdk.LoadRewardedAd(REWARDED_AD_UNIT_ID);
        }

        #endregion

        #region Banner ads

        private void InitializeBannerAds()
        {
            if (string.IsNullOrEmpty(BANNER_AD_UNIT_ID)) return;
            MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnAdLoadedEvent;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnAdLoadFailedEvent;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
           
        }

        private void LoadBannerAd()
        {
            if (string.IsNullOrEmpty(BANNER_AD_UNIT_ID)) return;
            MaxSdk.CreateBanner(BANNER_AD_UNIT_ID, MaxSdkBase.BannerPosition.BottomCenter);
            MaxSdk.SetBannerExtraParameter(BANNER_AD_UNIT_ID, "adaptive_banner", "true");
            MaxSdk.SetBannerBackgroundColor(BANNER_AD_UNIT_ID, new Color(1f, 1f, 1f, 0f));
        }
        #endregion

        // #region MRECs Ads
        //
        // private void InitializeMRecAds()
        // {
        //     
        //     MaxSdkCallbacks.MRec.OnAdLoadedEvent += OnAdLoadedEvent;
        //     MaxSdkCallbacks.MRec.OnAdLoadFailedEvent += OnAdLoadFailedEvent;
        //     MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
        // }
        //
        // private void LoadMRECAd()
        // {
        //     if (string.IsNullOrEmpty(MREC_AD_UNIT_ID)) return;
        //     MaxSdk.CreateMRec(MREC_AD_UNIT_ID, MaxSdkBase.AdViewPosition.Centered);
        // }
        // #endregion

        #region AppOpenAd

        private void InitAppOpenAd()
        {
            if (string.IsNullOrEmpty(APP_OPEN_AD_UNIT_ID)) return;
            MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += OnAppOpenDismissedEvent;
            MaxSdkCallbacks.AppOpen.OnAdLoadFailedEvent += OnAdLoadFailedEvent;
            MaxSdkCallbacks.AppOpen.OnAdDisplayedEvent += OnAdDisplayedEvent;
            MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
        }

        private void LoadAppOpenAd()
        {
            if (string.IsNullOrEmpty(APP_OPEN_AD_UNIT_ID)) return;
            MaxSdk.LoadAppOpenAd(APP_OPEN_AD_UNIT_ID);
        }
        private void OnAppOpenDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            LoadAppOpenAd();
        }

       

        #endregion

        #region Helper

        private void OnAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo impressionData)
        {
            double revenue = impressionData.Revenue;
            var dictParameter = new Dictionary<string, object>
            {
                {"ad_platform", "AppLovin"},
                {"ad_source", impressionData.NetworkName},
                {"ad_unit_name", impressionData.AdUnitIdentifier},
                {"ad_format", impressionData.AdFormat},
                {"value", revenue},
                {"currency", "USD"}
            };
            GameServiceManager.Instance.logEventManager.LogEvent("ad_impression", dictParameter);
            
            MarketingTrackManager.Instance.TrackAdRevenue(dictParameter);
            
        }

        private void OnAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (adUnitId == REWARDED_AD_UNIT_ID)
            {
                _rewardedRetryAttempt = 0;
                MarketingTrackManager.Instance.TrackAdLoad(AdType.Reward);
            }

            if (adUnitId == INTERSTITIAL_AD_UNIT_ID)
            {
                _interRetryAttempt = 0;
                MarketingTrackManager.Instance.TrackAdLoad(AdType.Inter);
            }
            if (adUnitId == APP_OPEN_AD_UNIT_ID)
                _aoaRetryAttempt = 0;
        }

        private async void OnAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            // if (adUnitId == MREC_AD_UNIT_ID)
            // {
            //     MaxSdk.CreateMRec(MREC_AD_UNIT_ID, MaxSdkBase.AdViewPosition.Centered);
            // }
            //
            // else
            // {
                if (adUnitId == REWARDED_AD_UNIT_ID)
                {
                    _rewardedRetryAttempt++;
                    var retryDelay = Math.Pow(2, Math.Min(6, _rewardedRetryAttempt));
                    await Task.Delay(TimeSpan.FromSeconds(retryDelay));
                    LoadRewardedAd();
                }
                else if (adUnitId == INTERSTITIAL_AD_UNIT_ID)
                {
                    _interRetryAttempt++;
                    var retryDelay = Math.Pow(2, Math.Min(6, _interRetryAttempt));
                    await Task.Delay(TimeSpan.FromSeconds(retryDelay));
                    LoadInterstitial();
                }
                else if (adUnitId == APP_OPEN_AD_UNIT_ID)
                {
                    _aoaRetryAttempt++;
                    var retryDelay = Math.Pow(2, Math.Min(6, _aoaRetryAttempt));
                    await Task.Delay(TimeSpan.FromSeconds(retryDelay));
                    LoadAppOpenAd();
                }
            // }
        }

        private void OnAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (adUnitId == REWARDED_AD_UNIT_ID)
            {
                MarketingTrackManager.Instance.TrackAdShow(AdType.Reward);
            }
            else if (adUnitId == INTERSTITIAL_AD_UNIT_ID)
            {
                MarketingTrackManager.Instance.TrackAdShow(AdType.Inter);
            }
        }

        private void OnAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
            MaxSdkBase.AdInfo adInfo)
        {
            
            if (adUnitId == REWARDED_AD_UNIT_ID)
                LoadRewardedAd();
            else if (adUnitId == INTERSTITIAL_AD_UNIT_ID)
            {
                _adCallback?.Invoke(false);
                LoadInterstitial();
            }
            else if (adUnitId == APP_OPEN_AD_UNIT_ID)
            {
                LoadAppOpenAd();
            }
        }
        private void OnAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (adUnitId == REWARDED_AD_UNIT_ID)
                LoadRewardedAd();
            else if (adUnitId == INTERSTITIAL_AD_UNIT_ID)
            {
                _adCallback?.Invoke(true);
                LoadInterstitial();
            }
        }

        private void OnAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
        {
            _adCallback?.Invoke(true);
        }

        #endregion
    }
}
#endif