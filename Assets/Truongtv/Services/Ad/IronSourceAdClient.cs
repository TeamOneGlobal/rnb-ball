#if USING_IRON_SOURCE
using System;
using Cysharp.Threading.Tasks;
using Truongtv.Services.Ad;

public class IronSourceAdClient : IAdClient
{
    private Action<bool> _actionCloseAd;
    private AdState _rewardVideoState;
    private AdState _interstitialState;

    #region public function

    public void Initialized()
    {
        var adSetting = AdSetting.Instance;
            var appKey = adSetting.GetAppKey();
            
            IronSource.Agent.init(appKey);
            IronSource.Agent.setMetaData("Facebook_IS_CacheFlag","IMAGE");
            IronSource.Agent.setMetaData("AppLovin_AgeRestrictedUser","true");
            IronSource.Agent.setMetaData("AdMob_TFCD","true"); 
            IronSource.Agent.setMetaData("AdMob_TFUA","true");
            IronSource.Agent.shouldTrackNetworkState(true);
            IronSource.Agent.setAdaptersDebug(false);
            IronSource.Agent.validateIntegration();

            IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
            IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
            IronSourceEvents.onRewardedVideoAdStartedEvent += IronSourceEventsOnonRewardedVideoAdStartedEvent;
            IronSourceEvents.onRewardedVideoAdClickedEvent += IronSourceEventsOnonRewardedVideoAdClickedEvent;
            IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
            IronSourceEvents.onRewardedVideoAdEndedEvent += IronSourceEventsOnonRewardedVideoAdEndedEvent;
            IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
            IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;


            IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
            IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;        
            IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent; 
            IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent; 
            IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
            IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
            IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;
            
            RequestInterstitial();
    }

    public void OnApplicationPause(bool isPause)
    {
        IronSource.Agent.onApplicationPause(isPause);
    }

    public bool IsRewardVideoLoaded(string placement = null)
    {
        if (string.IsNullOrEmpty(placement))
            return _rewardVideoState == AdState.Loaded;
        return _rewardVideoState == AdState.Loaded &&
               !IronSource.Agent.isRewardedVideoPlacementCapped(placement);
    }

    public void ShowRewardVideo(string placement = null, Action<bool> actionCloseAd = null)
    {
        _actionCloseAd = null;
        _actionCloseAd = actionCloseAd;
        if (IsRewardVideoLoaded(placement))
        {
            if(string.IsNullOrEmpty(placement))
                IronSource.Agent.showRewardedVideo();
            else 
                IronSource.Agent.showRewardedVideo(placement);
        }
        else
            _actionCloseAd.Invoke(false);
    }

    public bool IsInterstitialLoaded(string placement = null)
    {
        switch (_interstitialState)
        {
            case AdState.Failed:
            case AdState.None:
                _interstitialState = AdState.Loading;
                IronSource.Agent.loadInterstitial();
                return false;
            case AdState.Loading:
                return false;
            case AdState.Loaded:
                return string.IsNullOrEmpty(placement) || !IronSource.Agent.isInterstitialPlacementCapped(placement);
            default:
                return false;
        }
    }

    public void ShowInterstitial(string placement = null, Action<bool> actionCloseAd = null)
    {
        _actionCloseAd = actionCloseAd;
        if (IsInterstitialLoaded(placement))
        {
            if (string.IsNullOrEmpty(placement))
            {
                IronSource.Agent.showInterstitial();
            }
            else
                IronSource.Agent.showInterstitial(placement);
                
        }
        else
        {
            _actionCloseAd?.Invoke(false);
            _actionCloseAd = null;
        }
    }
    
    

    #endregion
    #region Reward Video
        void RewardedVideoAdOpenedEvent()
        {
        }
        async void RewardedVideoAdClosedEvent()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.4f));
            if (_actionCloseAd == null) return;
            _actionCloseAd?.Invoke(false);
            _actionCloseAd = null;
        }
        void RewardedVideoAvailabilityChangedEvent(bool available)
        {
            _rewardVideoState = available ? AdState.Loaded : AdState.Failed;
        }

        void RewardedVideoAdRewardedEvent(IronSourcePlacement placement)
        {
            // if (placement != null)
            // {
            //     var placementName = placement.getPlacementName();
            //     var rewardName = placement.getRewardName();
            //     var rewardAmount = placement.getRewardAmount();
            // }
            _actionCloseAd?.Invoke(true);
            _actionCloseAd = null;
        }

        void RewardedVideoAdShowFailedEvent(IronSourceError error)
        {
            _actionCloseAd?.Invoke(false);
            _actionCloseAd = null;
        }
        private void IronSourceEventsOnonRewardedVideoAdStartedEvent()
        {
        }

        private void IronSourceEventsOnonRewardedVideoAdEndedEvent()
        {
        }

        private void IronSourceEventsOnonRewardedVideoAdClickedEvent(IronSourcePlacement obj)
        {
        }

        #endregion
    #region Interstitial
        private void RequestInterstitial()
        {
            _interstitialState = AdState.Loading;
            IronSource.Agent.loadInterstitial();
        }
        private void InterstitialAdLoadFailedEvent (IronSourceError error)
        {
            _interstitialState = AdState.Failed;
        }
        private void InterstitialAdReadyEvent() {
            _interstitialState = AdState.Loaded;
        }
        private void InterstitialAdShowSucceededEvent() {
        }
        private void InterstitialAdShowFailedEvent(IronSourceError error) {
            _actionCloseAd?.Invoke(false);
        }
        private void InterstitialAdClickedEvent () {
        }
        private void InterstitialAdClosedEvent ()
        {
            RequestInterstitial();
            _actionCloseAd?.Invoke(true);
        }
        
        private void InterstitialAdOpenedEvent() {
        }

        

        #endregion
}
#endif