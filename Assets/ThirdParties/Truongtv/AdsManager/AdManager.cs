using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Projects.Scripts;
using Truongtv.PopUpController;
using Truongtv.Services.Ad;
using UnityEngine;

namespace ThirdParties.Truongtv.AdsManager
{
    public class AdManager:MonoBehaviour
    {
        private IAdClient _adClient;
        private DateTime _lastTimeInterstitialShow;
        private int _countLevel;
        public bool pauseByIapAndAd = false;
        #region Unity Function
        public void Init()
        {
            #if USING_MAX
            _adClient = new MaxAdClient();
            #elif USING_ADMOB
            _adClient = new AdMobClient();
            #elif USING_IRON_SOURCE
            _adClient = new IronSourceAdClient();
            #else
            _adClient = new EditorAdClient();
            #endif
            _adClient.Initialized();
        }
        #endregion
        
        #region Private Function

        private void ShowRewardVideo(string placement = null, Action<bool> actionCloseAd=null)
        {
            
            _adClient.ShowRewardVideo((result) =>
            {
                if(result)
                    _lastTimeInterstitialShow = DateTime.Now;
                actionCloseAd?.Invoke(result);
                
            });
        }

        private bool IsRewardVideoLoaded()
        {
            return _adClient.IsRewardVideoLoaded();
        }

        private void ShowInterstitial(Action<bool> actionCloseAd=null)
        {
            
            _lastTimeInterstitialShow = DateTime.Now;
            _countLevel++;
            _adClient.ShowInterstitial(actionCloseAd);
        }

        private bool IsInterstitialLoaded()
        {
            return _adClient.IsInterstitialLoaded();
        }

        private bool IsInterstitialAvailableToShow()
        {
            return !GameDataManager.Instance.IsPurchaseBlockAd() && GameDataManager.Instance.CanShowInterstitialAd() && DateTime.Now.Subtract(_lastTimeInterstitialShow).TotalSeconds>GameDataManager.Instance.blockAdTime;
        }
        
        #endregion
        #region Public Function
        public void ShowBanner(Action<bool> result = null)
        {
            if (GameDataManager.Instance.cheated)
            {
                result?.Invoke(false);
                return;
            }
            if(GameDataManager.Instance.IsPurchaseBlockAd()) return;
            _adClient.ShowBannerAd(result);
        }

        public void HideBanner()
        {
            _adClient.HideBannerAd();
        }
        public void ShowInterstitialAd(
            Action adResult = null)
        {
            if (GameDataManager.Instance.cheated)
            {
                adResult?.Invoke();
                return;
            }
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                adResult?.Invoke();
                return;
            }
            if (IsInterstitialLoaded() && IsInterstitialAvailableToShow())
            {
                pauseByIapAndAd = true;
                ShowInterstitial(result =>
                {
                    pauseByIapAndAd = false;
                    GameServiceManager.Instance.logEventManager.LogEvent("ads_interstitial");
                    adResult?.Invoke();
                });
            }
            else
            {
                adResult?.Invoke();
            }
        }
        
        public void ShowRewardedAd(string location, Action adResult = null)
        {
            
            if (GameDataManager.Instance.cheated)
            {
                adResult?.Invoke();
                return;
            }
            GameServiceManager.Instance.logEventManager.LogEvent("ads_reward_click",new Dictionary<string, object>
            {
                {"reward_for",location}
            });
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                //PopupController.Instance.ShowNoInternet();
                GameServiceManager.Instance.logEventManager.LogEvent("ads_reward_fail",new Dictionary<string, object>
                {
                    {"cause","no_internet"}
                });
                _countLevel++;
                return;
            }
            if (!IsRewardVideoLoaded())
            {
                //PopupController.Instance.ShowToast("Ads is still coming. Please try again later.");
                GameServiceManager.Instance.logEventManager.LogEvent("ads_reward_fail",new Dictionary<string, object>
                {
                    {"cause","no_ad"}
                });
                return;
            }
            pauseByIapAndAd = true;
            ShowRewardVideo(location,result =>
            {
                pauseByIapAndAd = false;
                if (!result)
                {
                    GameServiceManager.Instance.logEventManager.LogEvent("ads_reward_fail",new Dictionary<string, object>
                    {
                        {"cause","not_complete"}
                    });
                    return;
                }
                adResult?.Invoke();
                GameServiceManager.Instance.logEventManager.LogEvent("ads_reward_complete",new Dictionary<string, object>
                {
                    {"reward_for",location}
                });
            });
        }

        public bool IsAppOpenAdLoaded()
        {
            return _adClient != null && _adClient.IsAppOpenAdLoaded();
        }

        public void ShowAppOpenAd()
        {
            if (pauseByIapAndAd)
            {
                pauseByIapAndAd = false;
                return;
            }
            _adClient.ShowAppOpenAd();
        }

        public void OnApplicationPause(bool pauseStatus)
        {
            if (!GameDataManager.Instance.activeOpenAd) return;
            if (!pauseStatus)
            {
                ShowAppOpenAd();
            }
        }

        public async void ShowAppOpenAdColdStart(float duration)
        {
            if(!GameDataManager.Instance.activeOpenAd) return;
            var time = 0f;
            while (!IsAppOpenAdLoaded() && time<duration)
            {
                time += 0.1f;
                await Task.Delay(TimeSpan.FromSeconds(0.1f));
            }
            if (time < duration)
            {
                ShowAppOpenAd();
            }
        }
        #endregion

       
    }

    
}
