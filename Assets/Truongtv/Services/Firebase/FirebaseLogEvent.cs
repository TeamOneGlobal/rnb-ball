using Firebase.Analytics;
using UnityEngine;

namespace Truongtv.Services.Firebase
{
    public static class FirebaseLogEvent
    {
        #region Event name

        private const string LevelWinEventName = "Lv_{0}_W";
        private const string LevelLoseEventName = "Lv_{0}_L";
        private const string LevelStartEventName = "Lv_{0}_S";
        private const string InterstitialEventName = "InterAds";
        private const string ClickRewardedEventName = "Rewarded_C";
        private const string StartRewardedEventName = "Rewarded_S";
        private const string FinishRewardedEventName = "Rewarded_F";
        private const string ClickAdEventName = "{0}_C";
        private const string StartAdEventName = "{0}_S";
        private const string FinishAdEventName = "{0}_F";
        private const string CoinCollectEvent = "CoinEarned";
        private const string CoinUsedEvent = "CoinUsed";
        private const string CoinBalance = "CoinBalance";
        private const string SkinUsedInGame = "skinUsed";
        private const int MaxLevel = 50;

        private const string LevelEventName = "Lv";
        private const string RewardedEventName = "Rewarded";
        #endregion

        #region parameter

        private const string State = "State";
        private const string Value = "Value";
        private const string SkinName = "SkinName";
        #endregion

        #region Method

        public static void LogLevelStart(int level)
        {
            if (level > MaxLevel) return;
            LogEvent(string.Format(LevelStartEventName, level));
            LogLevel(level, "start");
        }
        public static void LogLevelWin(int level)
        {
            if (level > MaxLevel) return;
            LogEvent(string.Format(LevelWinEventName, level));
            LogLevel(level, "win");
        }
        public static void LogLevelLose(int level)
        {
            if (level > MaxLevel) return;
            LogEvent(string.Format(LevelLoseEventName, level));
            LogLevel(level, "lose");
        }
        public static void SerUserMaxLevel(int maxLevel)
        {
            FirebaseAnalytics.SetUserProperty("MaxLevel",maxLevel.ToString());
        }

       
        public static void LogViewInterstitial()
        {
            LogEvent(InterstitialEventName);
        }

        public static void LogClickViewRewardedAd(string location)
        {
            LogEvent(ClickRewardedEventName);
            LogEvent(string.Format(ClickAdEventName, location));
            LogRewardAdsEvent(location, "click");
        }
        public static void LogStartViewRewardedAd(string location)
        {
            LogEvent(StartRewardedEventName);
            LogEvent(string.Format(StartAdEventName, location));
            LogRewardAdsEvent(location, "start");
        }

        public static void LogFinishViewRewardedAd(string location)
        {
            LogEvent(FinishRewardedEventName);
            LogEvent(string.Format(FinishAdEventName, location));
            LogRewardAdsEvent(location, "finish");
        }

        public static void LogSkinUsed(string skin)
        {
            var param = new[]
            {
                new Parameter(SkinName,skin) 
            };
            LogEvent(SkinUsedInGame,param);
        }
        #region Truongtv custom log, not company

        private static void LogLevel(int level, string state)
        {
            if (level > MaxLevel) return;
            var param = new[]
            {
                new Parameter(State,"Lv_"+level+"_"+state) 
            };
            
            LogEvent(LevelEventName,param);
        }

        private static void LogRewardAdsEvent(string location,string state)
        {
            var param = new[]
            {
                new Parameter(State,location+"_"+state) 
            };
            
            LogEvent(RewardedEventName,param);
        }
        public static void LogCoinEarnEvent(long value)
        {
            var param = new[]
            {
                new Parameter(Value,value) 
            };
            LogEvent(CoinBalance,param);
            LogEvent(CoinCollectEvent,param);
        }
        public static void LogCoinUsedEvent(long value)
        {
            var param = new[]
            {
                new Parameter(Value,value) 
            };
            LogEvent(CoinBalance,param);
            LogEvent(CoinUsedEvent,param);
        }
        #endregion
        private static void LogEvent(string eventName, Parameter[] parameters)
        {
            FirebaseAnalytics.LogEvent(eventName, parameters);
            Debug.Log($" FirebaseAnalytics.LogEvent( {eventName} )");
        }

        private static void LogEvent(string eventName)
        {
            FirebaseAnalytics.LogEvent(eventName);
            Debug.Log($" FirebaseAnalytics.LogEvent( {eventName} )");
        }

       
        #endregion
    }
}