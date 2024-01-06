using UnityEngine;

namespace TeamOne.Tracking
{
    [CreateAssetMenu(fileName = "MarketingTrackData", menuName = "TeamOne/GameSetting/LogEventData", order = 1)]
    public class MarketingTrackData : ScriptableObject
    {
        public string key;
        public string id;
        public string mTrackInterLoaded = "af_inters_successfullyloaded";
        public string mTrackInterShow = "af_inters_displayed";
        public string mTrackInterRequest = "af_inters_logicgame";
        public string mTrackRewardedLoaded = "af_rewarded_successfullyloaded";
        public string mTrackRewardedShow = "af_rewarded_displayed";
        public string mTrackRewardedRequest = "af_rewarded_logicgame";
        public string mTrackIap = "af_purchase";
        public string mTrackLevelArchive = "af_level_achieved";
        public string mTrackAdRevenue = "af_ad_impression";
        public string mRevenueParam = "af_revenue";
        public string mCurrencyCodeParam = "af_currency";
        public string mSkuIdParam = "af_content_id";
        public string mLevel = "af_level";
    }
}