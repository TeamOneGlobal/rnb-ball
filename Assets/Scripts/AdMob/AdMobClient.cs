using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using Truongtv.Services.Ad;

namespace AdMob
{
    public class AdMobClient : IAdClient
    {
        private const string InterstitialAdAndroidId = "/6485410/red_blue_ball_interstitial";
        private const string InterstitialAdIosId = "ca-app-pub-9179752697212712/6412577833";
        private const string RewardedAdAndroidId = "/6485410/red_blue_ball_reward";
        private const string RewardedAdIosId = "ca-app-pub-9179752697212712/1351822848";
        private const string BannerAdAndroidId = "ca-app-pub-9179752697212712/7677897668";
        private const string BannerAdIosId = "ca-app-pub-9179752697212712/2865065434";
        private InterstitialAd _interstitialAd;
        private RewardedAd _rewardedAd;
        private BannerView bannerView;


        private Action<bool> _adCalllback;
        #region public function

        public void Initialized()
        {
            MobileAds.SetiOSAppPauseOnBackground(true);
            var deviceIds = new List<string>
            {
                AdRequest.TestDeviceSimulator, "40A41017391C077ACEDA5380D51D246E"
            };

            // Add some test device IDs (replace with your own device IDs).
#if UNITY_IPHONE
            deviceIds.Add("96e23e80653bb28980d3f40beb58915c");
#elif UNITY_ANDROID
#endif

            // Configure TagForChildDirectedTreatment and test device IDs.
            RequestConfiguration requestConfiguration =
                new RequestConfiguration.Builder()
                    .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.False)
                    .SetTestDeviceIds(deviceIds).build();

            MobileAds.SetRequestConfiguration(requestConfiguration);
            MobileAds.Initialize(HandleInitCompleteAction);
        }

        public void OnApplicationPause(bool isPause)
        {
        }



        public bool IsRewardVideoLoaded()
        {
            var result =  _rewardedAd.IsLoaded();
            if (!result)
            {
                RequestAndLoadRewardedAd();
            }
            return result;
        }

        public void RequestBanner()
        {

            if (bannerView != null)
            {
                bannerView.Destroy();
            }
            // Create a 320x50 banner at the top of the screen.
            #if UNITY_ANDROID
            this.bannerView = new BannerView(BannerAdAndroidId, AdSize.Banner, AdPosition.Bottom);
            #elif UNITY_IOS|| UNITY_IPHONE
             this.bannerView = new BannerView(BannerAdIosId, AdSize.Banner, AdPosition.Bottom);
            #endif
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();

            // Load the banner with the request.
            bannerView.LoadAd(request);

        }

        public void ShowBannerAd()
        {
            if (bannerView != null)
            {
                bannerView.Show();
            }
        }

        public void HideBannerAd()
        {
            if (bannerView != null)
            {
                bannerView.Hide();
            }
        }

        public void ShowRewardVideo(Action<bool> actionFinishAd = null)
        {
            _adCalllback = actionFinishAd;
            _rewardedAd.Show();
        }

        public bool IsInterstitialLoaded()
        {
            
            var result =  _interstitialAd.IsLoaded();
            if (!result)
            {
                RequestAndLoadInterstitialAd();
            }
            return result;
        }

        public void ShowInterstitial(Action<bool> actionFinishAd = null)
        {
            _adCalllback = actionFinishAd;
            _interstitialAd.Show();
        }

        #endregion

        #region private

        private void HandleInitCompleteAction(InitializationStatus initstatus)
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                RequestAndLoadInterstitialAd();
                RequestAndLoadRewardedAd();
            });
        }

        #region HELPER METHODS

        private AdRequest CreateAdRequest()
        {
            return new AdRequest.Builder()
                .AddKeyword("unity-admob-sample")
                .Build();
        }

        private string InterstitialAdId()
        {
#if UNITY_IOS|| UNITY_IPHONE
            return InterstitialAdIosId;
#else
            return Config.ADMOB_ID_INTER;
#endif
        }

        private string RewardedAdId()
        {
#if UNITY_IOS|| UNITY_IPHONE
            return RewardedAdIosId;
#else
            return Config.ADMOB_ID_REWARDED;
#endif
        }
        #endregion

        #region INTERSTITIAL ADS
        private void RequestAndLoadInterstitialAd()
        {
            _interstitialAd?.Destroy();
            _interstitialAd = new InterstitialAd(InterstitialAdId());
            _interstitialAd.OnAdClosed += OnInterstitialAdClosed;
            _interstitialAd.LoadAd(CreateAdRequest());
        }

        private void OnInterstitialAdClosed(object sender, EventArgs args)
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                _adCalllback?.Invoke(true);
                RequestAndLoadInterstitialAd();
            });
           
        }
        #endregion

        #region REWARDED ADS

        private void RequestAndLoadRewardedAd()
        {
            _rewardedAd = new RewardedAd(RewardedAdId());
            _rewardedAd.OnAdClosed += HandleRewardedAdClosed;
            _rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
            _rewardedAd.LoadAd(CreateAdRequest());
        }
        private void HandleRewardedAdClosed(object sender, EventArgs args)
        {
            MobileAdsEventExecutor.ExecuteInUpdate(async () =>
            {
                await UniTask.Delay(TimeSpan.FromMilliseconds(400));
                _adCalllback.Invoke(false);
                RequestAndLoadRewardedAd();
            });
        }

        private void HandleUserEarnedReward(object sender, Reward args)
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                _adCalllback.Invoke(true);
            });
        }
        #endregion
        #endregion
    }
}