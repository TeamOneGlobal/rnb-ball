using System;
using System.Collections.Generic;
using UnityEngine;

namespace UserDataModel
{
    [Serializable]
    public class UserInfo
    {
        public int currentLevel;
        public long totalLife;
        public long coin;
        public bool noAd;
        public LastLevelInfo lastLevel;
        public long magnetDuration;
        public string currentSkin;
        public string demoSkin;
        public int numberSkinBought;
        public List<string> unlockSkin;
        public List<string> watchAdToUnlockSkin;
        public Dictionary<string, int> unlockProgress;
        public int tutorialStep;
        public SpinInfo spinInfo;
        public bool buySpecialOffer;
        public DailyRewardInfo dailyRewardInfo;
        public List<int> giftItem;
        public bool rate;
        public UserInfo()
        {
            currentLevel = 1;
            totalLife = 1;
            coin = 0;
            magnetDuration = 0;
            noAd = false;
            lastLevel = null;
            currentSkin = "0";
            numberSkinBought = 0;
            unlockSkin = new List<string> {"0"};
            unlockProgress = new Dictionary<string, int>();
            watchAdToUnlockSkin = new List<string>();
            dailyRewardInfo = new DailyRewardInfo
            {
                dateCreate = DateTime.Now,
                collected = new List<int>()
            };
            giftItem = new List<int>();
        }
    }

    [Serializable]
    public class LastLevelInfo
    {
        public long lastCoin;
        public int lastLevelWin;
    }

    [Serializable]
    public class SpinInfo
    {
        public DateTime lastSpinTime;
        public int totalSpinCount;
    }

    [Serializable]
    public class DailyRewardInfo
    {
        public DateTime dateCreate;
        public List<int> collected;
    }
}