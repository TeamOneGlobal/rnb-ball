using System;
using System.Collections.Generic;
using UnityEngine;
#if USING_ADJUST
using System.Threading.Tasks;
using com.adjust.sdk;
#endif

namespace TeamOne.Tracking
{
    public class MarketingTrackManager: MonoBehaviour
    {
        private MarketingTrackData _data;
        private static MarketingTrackManager _instance;

        public static MarketingTrackManager Instance => _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                _instance = null;
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            Init();
        }

        private async void Init()
        {
            _data = Resources.Load<MarketingTrackData>(nameof(MarketingTrackData));
#if USING_ADJUST
            await Task.Delay(50);
            AdjustConfig config = new AdjustConfig(_data.key,AdjustEnvironment.Production);
            config.setLogLevel(AdjustLogLevel.Info);
            Adjust.start(config);
#endif
        }


        private void TrackEvent(string eventName)
        {
#if USING_ADJUST
            AdjustEvent adjustEvent = new AdjustEvent(eventName);
            Adjust.trackEvent(adjustEvent);
#endif
        }

        private void TrackEvent(string eventName, Dictionary<string, object> parameter)
        {
#if USING_ADJUST
            AdjustEvent adjustEvent = new AdjustEvent(eventName);
            foreach (var keyValue in parameter)
            {
                adjustEvent.addCallbackParameter(keyValue.Key, keyValue.Value.ToString());
            }
            Adjust.trackEvent(adjustEvent);
#endif
        }

        public void TrackIap(string sku, float value, string currencyCode)
        {
#if USING_ADJUST
            AdjustEvent adjustEvent = new AdjustEvent(_data.mTrackIap);
            adjustEvent.setRevenue(value, currencyCode);
            adjustEvent.addCallbackParameter("sku",sku);
            Adjust.trackEvent(adjustEvent);
#endif
        }

        public void TrackAdRevenue(Dictionary<string, object> data)
        {
#if USING_ADJUST
            AdjustAdRevenue adjustAdRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAppLovinMAX);
            var value = float.Parse(data["value"].ToString());
            adjustAdRevenue.setRevenue(value, data["currency"].ToString());
            adjustAdRevenue.setAdImpressionsCount(1);
            adjustAdRevenue.setAdRevenueNetwork(data["ad_source"].ToString());
            adjustAdRevenue.setAdRevenueUnit(data["ad_unit_name"].ToString());
            adjustAdRevenue.setAdRevenuePlacement(data["ad_format"].ToString());
            Adjust.trackAdRevenue(adjustAdRevenue);
#endif
        }

        public void TrackLevelArchive(string level)
        {
            TrackEvent(_data.mTrackLevelArchive,new Dictionary<string, object>
            {
                {_data.mLevel, level}
            });
        }

        public void TrackAdLoad(AdType type)
        {
            switch (type)
            {
                case AdType.Reward:
                    TrackEvent(_data.mTrackRewardedLoaded);
                    break;
                case AdType.Inter:
                    TrackEvent(_data.mTrackInterLoaded);
                    break;
                default:
                    break;
            }
        }
        
        public void TrackAdShow(AdType type)
        {
            switch (type)
            {
                case AdType.Reward:
                    TrackEvent(_data.mTrackRewardedShow);
                    break;
                case AdType.Inter:
                    TrackEvent(_data.mTrackInterShow);
                    break;
                default:
                    break;
            }
        }
        
    }

    public enum AdType
    {
        Reward,
        Inter,
        Banner,
        MREC,
        AOA,
    }
}