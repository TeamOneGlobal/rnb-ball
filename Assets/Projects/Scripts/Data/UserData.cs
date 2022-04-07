using System;
using System.Collections.Generic;
using UnityEngine;

namespace Projects.Scripts.Data
{
    [Serializable]
    public class UserData
    {
        public DateTime createTime;
        public Dictionary<string, int> currency;
        public Dictionary<string, bool> trigger;
        public Dictionary<int, List<int>> stars;
        public string currentSkin,trySkin;
        public List<string> unlockSkin;
        public LastLevelData LastLevelData;
        public GiftData giftData;
        public DateTime lastSpinTime;
        public DailyGiftTime dailyData;
        public List<LevelPlayedInfo> levelPlayedInfoList;
        public int squidLevel = 1;
        public UserData()
        {
            createTime = DateTime.Now;
            currency = new Dictionary<string, int>();
            trigger = new Dictionary<string, bool>();
            stars = new Dictionary<int, List<int>>();
            unlockSkin = new List<string>();
            for (int i = 0; i < 4; i++)
            {
                stars.Add(i,new List<int>());
            }

            LastLevelData = null;
            giftData = null;
            dailyData = new DailyGiftTime();
            levelPlayedInfoList = new List<LevelPlayedInfo>();
        }

        public void SetBaseData(StartData data)
        {
            currency.Add("life",data.life);
            currency.Add("level",data.level);
            currentSkin = data.currentSkin;
            unlockSkin.Clear();
            unlockSkin.Add(data.currentSkin);
        }
    }

    [Serializable]
    public class LastLevelData
    {
        public GameResult result;
        public int level;
        public int coins;
    }

    [Serializable]
    public class GiftData
    {
        public DateTime refreshTime;
        public List<string> skinList;
        public int adProgress;
    }

    public enum GameResult
    {
        None,Win,Lose

    }

    [Serializable]
    public class DailyGiftTime
    {
        public int dayIndex;
        public List<int> dayClaimed;
        public DateTime lastRewardTime;
        public bool enableDaily;

        public DailyGiftTime()
        {
            enableDaily = true;
            dayIndex = 1;
            lastRewardTime = DateTime.Today.AddDays(-1);
            dayClaimed = new List<int>();
        }
    }

    [Serializable]
    public class LevelPlayedInfo
    {
        public int level;
        public bool win;
        public int countGame;
    }
    
}