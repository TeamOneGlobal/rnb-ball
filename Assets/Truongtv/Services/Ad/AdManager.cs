using System;
using AdMob;
using Truongtv.Services.Firebase;
using Truongtv.Utilities;
using UserDataModel;

namespace Truongtv.Services.Ad
{
    public class AdManager:SingletonMonoBehavior<AdManager>
    {
        private IAdClient _adClient;
        private DateTime _lastTimeInterstitialShow;
        private int _countLevel;
        #region Unity Function
        private void Start()
        {
            _adClient = new AdMobClient();
            _adClient.Initialized();
        }
        #endregion
        
        #region Public Function
        public void ShowRewardVideo(string placement = null, Action<bool> actionCloseAd=null)
        {
          
            _adClient.ShowRewardVideo((result) =>
            {
                actionCloseAd?.Invoke(result);
                
            });
        }

        public bool IsRewardVideoLoaded()
        {
            return _adClient.IsRewardVideoLoaded();
        }
        public void ShowInterstitial(Action<bool> actionCloseAd=null)
        {
            
            _lastTimeInterstitialShow = DateTime.Now;
            _countLevel++;
            _adClient.ShowInterstitial(actionCloseAd);
        }

        public void RequesBanner()
        {
            _adClient.RequestBanner();
        }

        public void ShowBannerAd()
        {
            _adClient.ShowBannerAd();
        }
        public void HideBannerAd()
        {
            _adClient.HideBannerAd();
        }

        public bool IsInterstitialLoaded()
        {
            if (UserDataController.IsBlockAd()) return false;
            if (DateTime.Now.Subtract(_lastTimeInterstitialShow).TotalSeconds < Config.INTERSTITIAL_BLOCK_TIME)
                return false;
            if (UserDataController.GetCurrentLevel() < Config.INTERSTITIAL_MIN_LEVEL) return false;
            if (_countLevel % Config.LEVEL_PER_INTERSTITIAL != 0) return false;
            
            return _adClient.IsInterstitialLoaded();
        }
        #endregion

       
    }
    public enum AdState
    {
        None,Loading,Loaded,Failed
    }
}
