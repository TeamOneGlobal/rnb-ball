using System;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

namespace UserDataModel
{
    public static class UserDataController
    {
        private const string UserInfoKey = "user_info";
        private static UserInfo _userInfo;

        #region Save and Load

        public static void LoadUserData()
        {
            if (!ES3.KeyExists(UserInfoKey) || ES3.Load<UserInfo>(UserInfoKey) == null)
            {
                _userInfo = new UserInfo();
                ES3.Save(UserInfoKey, _userInfo);
            }

            _userInfo = ES3.Load<UserInfo>(UserInfoKey);
        }

        private static void SaveData()
        {
            ES3.Save(UserInfoKey, _userInfo);
        }


        #endregion

        #region Level
        public static int GetCurrentLevel()
        {
            return _userInfo.currentLevel;
        }

        public static void SetCurrentLevel(int level)
        {
            _userInfo.currentLevel = level;
            SaveData();
        }
        public static int UpdateLevel(int level)
        {
            if (level < _userInfo.currentLevel) return _userInfo.currentLevel;
            _userInfo.currentLevel++;
            SaveData();
            return _userInfo.currentLevel;
        }

        public static void ForceUpdateLevel()
        {
            _userInfo.currentLevel = Config.MAX_LEVEL;
            SaveData();
        }


        #endregion

        #region Coin
        public static long GetCurrentCoin()
        {
            return _userInfo.coin;
        }
        public static void UpdateCoin(long value)
        {
            _userInfo.coin += value;
            SaveData();
        }



        #endregion

        #region Life

        public static long GetTotalLife()
        {
            return _userInfo.totalLife;
        }

        public static void UpdateLife(long value)
        {
            _userInfo.totalLife += value;
            SaveData();
        }


        #endregion

        #region Last Level
        public static void ClearPreviousLevelData()
        {
            _userInfo.lastLevel = null;
            SaveData();
        }

        public static bool IsLastLevelWin()
        {
            return _userInfo.lastLevel != null;
        }

        public static LastLevelInfo GetLastLevelData()
        {
            return _userInfo.lastLevel;
        }

        public static void SetLevelWin(int level, long coin)
        {
            _userInfo.lastLevel = new LastLevelInfo
            {
                lastCoin = coin,
                lastLevelWin = level
            };
            SaveData();
        }

        

        #endregion

        #region Magnet

        public static void UpdateMagnetDuration(long duration)
        {
            _userInfo.magnetDuration += duration;
            if (_userInfo.magnetDuration < 0) _userInfo.magnetDuration = 0;
            SaveData();
        }

        public static long GetMagneticDuration()
        {
            return _userInfo.magnetDuration;
        }

        #endregion

        #region Skins
        public static List<string> GetTotalUnlockSKins()
        {
            return _userInfo.unlockSkin;
        }

        public static void UpdateSelectedSkin(string skin)
        {
            _userInfo.currentSkin = skin;
            SaveData();
        }

        public static string GetSelectedSkin()
        {
            return string.IsNullOrEmpty(_userInfo.demoSkin) ? _userInfo.currentSkin : _userInfo.demoSkin;
        }

        public static void ClearDemoSkin()
        {
            _userInfo.demoSkin = string.Empty;
            SaveData();
        }

        public static void DemoSkin(string skin)
        {
            _userInfo.demoSkin = skin;
            SaveData();
        }

        public static void UnlockSkin(string skin)
        {
            if (_userInfo.unlockSkin.Contains(skin)) return;
            _userInfo.unlockSkin.Add(skin);
            SaveData();
        }

        public static bool IsSkinUnlock(string skin)
        {
            return _userInfo.unlockSkin.Contains(skin);
        }

        public static int GetUnlockProgress(string skin)
        {
            if (!_userInfo.unlockProgress.ContainsKey(skin))
            {
                _userInfo.unlockProgress.Add(skin, 0);
                SaveData();
                return 0;
            }

            return _userInfo.unlockProgress[skin];
        }

        public static bool IsInUnlockProgress(string skin)
        {
            return _userInfo.unlockProgress.ContainsKey(skin);
        }
        public static void SetUnlockProgress(string skin)
        {
            if (_userInfo.unlockProgress.ContainsKey(skin)) return;
            _userInfo.unlockProgress.Add(skin, 0);
            SaveData();
        }
        public static void UnlockProgress(string skin)
        {
            _userInfo.unlockProgress[skin]++;
            SaveData();
        }

        public static void BoughtSkin(string skin)
        {
            _userInfo.numberSkinBought++;
            UnlockSkin(skin);
        }

        public static int GetBoughSkinNumber()
        {
            return _userInfo.numberSkinBought;
        }
        public static bool IsDemoSkin()
        {
            return !string.IsNullOrEmpty(_userInfo.demoSkin);
        }
        #endregion

        #region Ad
        public static void SetBlockAd()
        {
            _userInfo.noAd = true;
            SaveData();
        }

        public static bool IsBlockAd()
        {
            return _userInfo.noAd;
        }
        

        #endregion

        #region Tutorial

        public static int GetTutorialStep()
        {
            return _userInfo.tutorialStep;
        }

        public static void SetFinishStep()
        {
            _userInfo.tutorialStep++;
            SaveData();
        }
        #endregion

        #region Spin
        public static DateTime GetLastSpinTime()
        {
            if (_userInfo.spinInfo != null) return _userInfo.spinInfo.lastSpinTime;
            _userInfo.spinInfo = new SpinInfo();
            SaveData();
            return _userInfo.spinInfo.lastSpinTime;
        }

        public static bool IsSpinAvailable()
        {
            var lastSpinTime = GetLastSpinTime();
            return DateTime.Now.Subtract(lastSpinTime).TotalHours >= Config.FREE_SPIN_COOLDOWN_HOURS;
        }
        public static long GetTotalSpin()
        {
            return _userInfo.spinInfo.totalSpinCount;
        }
        public static void Spin(bool free)
        {
            if (free)
            {
                _userInfo.spinInfo.lastSpinTime = DateTime.Now;
            }
            _userInfo.spinInfo.totalSpinCount++;
            SaveData();
        }

        #endregion

        #region IAP

        public static void SetBuySpecialOffer()
        {
            _userInfo.buySpecialOffer = true;
            SaveData();
        }

        public static bool IsBuySpecialOffer()
        {
            return _userInfo.buySpecialOffer;
        }
        #endregion

        #region DailyGift

        public static bool IsDailyRewardAvailable()
        {
            if (_userInfo.dailyRewardInfo == null)
            {
                _userInfo.dailyRewardInfo = new DailyRewardInfo
                {
                    dateCreate = DateTime.Now,
                    collected = new List<int>()
                };
                SaveData();
                return true;
            }
            return DateTime.Now.Subtract(_userInfo.dailyRewardInfo.dateCreate).TotalDays >= _userInfo.dailyRewardInfo.collected.Count;
        }
        public static int GetDailyReward()
        {
            if (_userInfo.dailyRewardInfo == null)
            {
                _userInfo.dailyRewardInfo = new DailyRewardInfo
                {
                    dateCreate = DateTime.Now,
                    collected = new List<int>()
                };
                SaveData();
            }

            var delta =Mathf.CeilToInt( (float)DateTime.Now.Subtract(_userInfo.dailyRewardInfo.dateCreate).TotalDays);
            if (delta == 0) delta = 1;
            return delta;
        }

        public static List<int> GetDailyRewardReceivedDay()
        {
            return _userInfo.dailyRewardInfo.collected;
        }

        public static void ClaimReward(int day)
        {
            _userInfo.dailyRewardInfo.collected.Add(day);
            SaveData();
        }
        #endregion

        #region Gift

        public static void SetGiftList(List<int> data)
        {
            _userInfo.giftItem = data;
            SaveData();
        }

        public static List<int> GetGiftList()
        {
            if (_userInfo.giftItem == null || _userInfo.giftItem.Count == 0)
            {
                _userInfo.giftItem = new List<int>();
                SaveData();
               
            }
            return _userInfo.giftItem;
        }

        public static bool IsRating()
        {
            return _userInfo.rate;
        }

        public static void SetRating()
        {
            _userInfo.rate = true;
            SaveData();
        }
        #endregion
    }
}