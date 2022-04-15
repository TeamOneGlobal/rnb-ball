using System;
using System.Collections.Generic;
using Projects.Scripts.Data;
using Sirenix.OdinInspector;
using UnityEngine;
using GiftData = Projects.Scripts.Data.GiftData;

namespace Projects.Scripts
{
    public class GameDataManager : MonoBehaviour
    {
        [SerializeField,FoldoutGroup("Game Setting")] public bool cheated = false;

        [SerializeField, FoldoutGroup("Game Setting")]
        public bool debugger = false;
        [FoldoutGroup("Constant Value")]
        [SerializeField,FoldoutGroup("Constant Value/InGame")]public int coinValueInGame = 100;
        [SerializeField,FoldoutGroup("Constant Value/InGame")]public int maxLevel = 1;
        [SerializeField, FoldoutGroup("Constant Value/InGame")]public bool showBannerInGame;
        [SerializeField, FoldoutGroup("Constant Value/InGame")]public float squidDifficulty = 1f;
        [SerializeField, FoldoutGroup("Constant Value/InGame")]public int magnetDuration = 15;
        [SerializeField, FoldoutGroup("Constant Value/Ad")]public int blockAdTime = 30;
        [SerializeField, FoldoutGroup("Constant Value/Ad")]public int minLevelShowInterstitial = 2;
        [SerializeField, FoldoutGroup("Constant Value/Ad")]public int showInterstitialPerLevel = 1;
        [SerializeField, FoldoutGroup("Constant Value/Ad")]public int adCoin = 1000;
        [SerializeField, FoldoutGroup("Constant Value/Ad")]public int adLife = 1;
        [SerializeField, FoldoutGroup("Constant Value/Ad")]public int checkInternetPerLevel = 3;
        [SerializeField, FoldoutGroup("Constant Value/Ad")]
        public string versionReview = "0.2.5";
        [SerializeField, FoldoutGroup("Constant Value/Menu")] public int freeSpinCooldownHours = 8;
        [SerializeField, FoldoutGroup("Constant Value/Menu")] public int levelShowSpecialOffer = 11;
        [SerializeField, FoldoutGroup("Constant Value/Menu")] public int showOfferPerLevel = 10;
        [SerializeField, FoldoutGroup("Constant Value/Menu")] public int levelShowDailyReward = 3;
        [SerializeField, FoldoutGroup("Constant Value/Menu")] public int levelShowRating = 14;
        [SerializeField, FoldoutGroup("Constant Value/Menu")] public int showRatingPerLevel = 10;
        [SerializeField, FoldoutGroup("Constant Value/Menu")] public int levelShowLottery = 9;
        [SerializeField, FoldoutGroup("Constant Value/Menu")] public int showLotteryPerLevel = 10;
        [SerializeField, FoldoutGroup("Constant Value/Menu")] public int showContinueAfterLevel = 10;
        [SerializeField, FoldoutGroup("Constant Value/Menu")] public int multipleCoinInMenu = 5;
        [SerializeField, FoldoutGroup("Game Data")] public StartData startData;
        [SerializeField, FoldoutGroup("Game Data")] public SkinData skinData;
        [SerializeField, FoldoutGroup("Game Data")] public ShopData shopData;
        [SerializeField, FoldoutGroup("Game Data")] public string termOfUseUrl, privacyPolicyUrl;
        [HideInInspector] public bool firstCreate;
        [HideInInspector] public string defaultSkin;
        private UserData _userData;

        private int _countLevelNoInternet;
        
        private const string DataKey = "ball_data_t1";
        private const string MagnetKey = "magnet";
        private const string LifeKey = "life";
        private const string LevelKey = "level";
        private const string CoinKey = "coin";
        private const string StarSpentKey = "star_spent";
        private const string PurchaseBlockAdKey = "purchase_block_ad";
        private const string CountAdSkinKey = "skin_{0}_ads";
        private const string SkinCountBuyByCoinKey = "count_skin_coin";
        private const string SkinCountBuyByStarKey = "count_skin_star";
        private const string SubscriptionKey = "subscription";
        private const string InfinityLifeKey = "infinityLife";
        private const string SpecialOfferKey = "special_offer";
        public static bool FirstOpen = true;
        private static GameDataManager _instance;
        public static GameDataManager Instance => _instance;
        public void Awake()
        {
            if (_instance != null)
                Destroy(_instance.gameObject);
            _instance = this ;
        }
        private void Start()
        {
            Application.targetFrameRate = 300;
        }

        #region GameData

        public float GetSquidLevelDifficulty()
        {
            float difficulty = squidDifficulty * (1 + _userData.squidLevel / 10f) / 100f;
            Debug.Log("remote squid difficulty: " + squidDifficulty);
            Debug.Log("squid game level difficulty: " + difficulty);
            return difficulty;
        }
        
        #endregion
        
        #region UserData

        #region Load and Save

        public void RemoteConfigLoaded()
        {
            _userData.unlockSkin.Remove("Base");
            UnlockSkin(defaultSkin);
            SaveData();
        }
        public void LoadUserData()
        {
            if (!ES3.KeyExists(DataKey))
            {
                _userData = new UserData();
                _userData.SetBaseData(startData);
                SaveData();
                firstCreate = true;
                return;
            }
            _userData = ES3.Load<UserData>(DataKey);
            firstCreate = false;

        }

        private void SaveData()
        {
            ES3.Save(DataKey,_userData);
        }

        #endregion

        #region Magnet

        public void IncreaseMagnetDuration(int duration)
        {
            if (!_userData.currency.ContainsKey(MagnetKey))
            {
                _userData.currency.Add(MagnetKey,0);
            }

            _userData.currency[MagnetKey] += duration;
            SaveData();
        }

        public int GetMagnetDuration()
        {
            if (_userData.currency.ContainsKey(MagnetKey)) return _userData.currency[MagnetKey];
            _userData.currency.Add(MagnetKey,0);
            SaveData();
            return _userData.currency[MagnetKey];
        }

        public void CountDownMagnet()
        {
            _userData.currency[MagnetKey]--;
            if (_userData.currency[MagnetKey] < 0)
                _userData.currency[MagnetKey] = 0;
            SaveData();
        }

        #endregion

        #region Level

        public int GetSquidGameLevel()
        {
            return _userData.squidLevel;
        }

        public void SetSquidGameLevel(int level)
        {
            if (level > 10)
            {
                level = 10;
            }
            _userData.squidLevel = level;
            SaveData();
        }
        
        public int GetCurrentLevel()
        {
            return _userData.currency[LevelKey];
        }

        public LevelPlayedInfo GetCurrentLevelInfo(int level )
        {
            if (_userData.levelPlayedInfoList.Exists(a => a.level == level))
            {
                return _userData.levelPlayedInfoList.Find(a => a.level == level);
            }

            return null;

        }
        public void GameResult(GameResult result,int level,int coins)
        {
            _userData.LastLevelData = new LastLevelData
            {
                result = result,
                level = level,
                coins =  coins,
            };
            if (_userData.levelPlayedInfoList.Exists(a => a.level == level))
            {
                var temp = _userData.levelPlayedInfoList.Find(a => a.level == level);
                if (!temp.win && result == Data.GameResult.Lose)
                {
                    temp.countGame++;
                }
            }
            else
            {
                var info = new LevelPlayedInfo
                {
                    level = level,
                    win = result == Data.GameResult.Win,
                    countGame = 1
                };
                _userData.levelPlayedInfoList.Add(info);
            }
            SaveData();
        }
        
        public LastLevelData GetLastLevelData()
        {
            return _userData.LastLevelData;
        }

        public void ResetLastLevelData()
        {
            _userData.LastLevelData = null;
            SaveData();
        }

        public void UpdateLastLevel()
        {
            if (_userData.currency[LevelKey] == _userData.LastLevelData.level)
            {
                _userData.currency[LevelKey]++;
            }
            
            _userData.LastLevelData = null;
            SaveData();
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                _countLevelNoInternet++;
            }
            else
            {
                _countLevelNoInternet = 0;
            }
        }

        public bool CanPlayWithoutInternet()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                _countLevelNoInternet = 0;
                return true;
            }
            return _countLevelNoInternet<=checkInternetPerLevel;
        }
        #endregion

        #region Stars
        public void SetLevelStars(int level, int stars)
        {
            var currentStar = GetLevelStar(level);
            if(currentStar>=stars) return;
            for (var i = 0; i < 4; i++)
            {
                if (_userData.stars[i].Contains(level))
                {
                    _userData.stars[i].Remove(level);
                }
            }
            _userData.stars[stars].Add(level);
            SaveData();
        }

        public int GetTotalStars()
        {
            var result = 0;
            for (var i = 0; i < 4; i++)
            {
                result += _userData.stars[i].Count*i ;
            }
            
            return result;
        }

        public int GetLevelStar(int level)
        {
            var currentStar = 0;
            for (var i = 0; i < 4; i++)
            {
                if (_userData.stars[i].Contains(level))
                    currentStar = i;
            }

            return currentStar;
        }

        private int GetStarSpent()
        {
            if (_userData.currency.ContainsKey(StarSpentKey)) return _userData.currency[StarSpentKey];
            _userData.currency.Add(StarSpentKey,0);
            SaveData();
            return _userData.currency[StarSpentKey];
        }

        public void UseStar(int value)
        {
            if (!_userData.currency.ContainsKey(StarSpentKey))
            {
                _userData.currency.Add(StarSpentKey,0);
            }

            _userData.currency[StarSpentKey] += value;
            SaveData();
        }
        public int GetCurrentStars()
        {
            return GetTotalStars() - GetStarSpent();
        }

        #endregion

        #region Life

        public bool LostLife()
        {
            if (IsInfinityLife()) return true;
            if(GetCurrentLife()==1) return false;
            _userData.currency[LifeKey]--;
            SaveData();
            return true;
        }

        public int GetCurrentLife()
        {
            return _userData.currency[LifeKey];
        }

        public void AddLife(int value)
        {
            if (value <= -1)
            {
                //set Infinity Life
            }
            else
            {
                _userData.currency[LifeKey] += value;
            }
            
            SaveData();
        }
        #endregion

        #region Coin

        public void UpdateCoin(int value)
        {
            if (!_userData.currency.ContainsKey(CoinKey))
            {
                _userData.currency.Add(CoinKey,0);
            }
            _userData.currency[CoinKey] += value;
            if (_userData.currency[CoinKey] < 0)
                _userData.currency[CoinKey] = 0;
            SaveData();
        }

        public int GetCurrentCoin()
        {
            if (!_userData.currency.ContainsKey(CoinKey))
            {
                _userData.currency.Add(CoinKey,0);
                SaveData();
            }

            return _userData.currency[CoinKey];
        }

        #endregion

        #region Ad

        public bool IsPurchaseBlockAd()
        {
            if (!_userData.trigger.ContainsKey(PurchaseBlockAdKey))
            {
                _userData.trigger.Add(PurchaseBlockAdKey,false);
                SaveData();
            }

            return _userData.trigger[PurchaseBlockAdKey];
        }

        public void PurchaseBlockAd()
        {
            if (!_userData.trigger.ContainsKey(PurchaseBlockAdKey))
            {
                _userData.trigger.Add(PurchaseBlockAdKey,true);
                
            }
            _userData.trigger[PurchaseBlockAdKey] = true;
            SaveData();
        }

        public bool CanShowInterstitialAd(int countLevel)
        {
            if (IsPurchaseBlockAd()) return false;
            if (GetCurrentLevel() < minLevelShowInterstitial) return false;
            if (countLevel % showInterstitialPerLevel != 0) return false;
            return true;
        }

        public bool CanShowAd()
        {
            return IsPurchaseBlockAd();
        }
        #endregion

        #region Skin

        public List<string> GetUnlockedSkin()
        {
            return _userData.unlockSkin;
        }

        public string GetCurrentSkin()
        {
            return string.IsNullOrEmpty(_userData.trySkin) ? _userData.currentSkin : _userData.trySkin;
        }

        public string GetSkinInGame()
        {
            return string.IsNullOrEmpty(_userData.trySkin) ? _userData.currentSkin : _userData.trySkin;
        }
        public void SetSkin(string skinName)
        {
            _userData.currentSkin = skinName;
            _userData.trySkin = string.Empty;
            SaveData();
        }

        public void SetTrySkin(string trySkin)
        {
            _userData.trySkin = trySkin;
            SaveData();
        }
        public void UnlockSkin(string skinName)
        {
            if(!_userData.unlockSkin.Contains(skinName))
                _userData.unlockSkin.Add(skinName);
            _userData.currentSkin = skinName;
            SaveData();
        }

        public bool IsSkinUnlock(string skin)
        {
            return GetUnlockedSkin().Contains(skin);
        }

        public bool IsPremiumSkin(string skin)
        {
            return skinData.IsSkinPremium(skin);
        }
        
        public void ViewAdToUnlockSkin(string skinName)
        {
            if (!_userData.currency.ContainsKey(string.Format(CountAdSkinKey, skinName)))
            {
                _userData.currency.Add(string.Format(CountAdSkinKey, skinName),0);
            }
            _userData.currency[string.Format(CountAdSkinKey, skinName)]++;
            SaveData();
        }

        public void BuySkinByCoin()
        {
            if (!_userData.currency.ContainsKey(SkinCountBuyByCoinKey))
            {
                _userData.currency.Add(SkinCountBuyByCoinKey,0);
            }

            _userData.currency[SkinCountBuyByCoinKey]++;
            SaveData();
        }

        public int GetCountSkinBuyByCoin()
        {
            if (!_userData.currency.ContainsKey(SkinCountBuyByCoinKey))
            {
                _userData.currency.Add(SkinCountBuyByCoinKey,0);
                SaveData();
            }

            return _userData.currency[SkinCountBuyByCoinKey];
        }

        public int GetCountSkinBuyByStar()
        {
            if (!_userData.currency.ContainsKey(SkinCountBuyByStarKey))
            {
                _userData.currency.Add(SkinCountBuyByStarKey,0);
                SaveData();
            }

            return _userData.currency[SkinCountBuyByStarKey];
        }
        public void BuySkinByStar()
        {
            if (!_userData.currency.ContainsKey(SkinCountBuyByStarKey))
            {
                _userData.currency.Add(SkinCountBuyByStarKey,0);
            }

            _userData.currency[SkinCountBuyByStarKey]++;
            SaveData();
        }
        #endregion

        #region Gift

        public bool CanRefresh()
        {
            return _userData.giftData != null &&
                   DateTime.UtcNow.Subtract(_userData.giftData.refreshTime).TotalDays >= 1;
        }
        public DateTime GetRefreshTime()
        {
            return _userData.giftData.refreshTime;
        }

        public void SetGiftList(List<string> data)
        {
            _userData.giftData.skinList = data;
            SaveData();
        }

        public void SetRefreshTime()
        {
            _userData.giftData.refreshTime = DateTime.UtcNow;
        }
        public  List<string> GetGiftList()
        {
            if (_userData.giftData == null || _userData.giftData.skinList.Count == 0)
            {
                _userData.giftData = new GiftData
                {
                    refreshTime = DateTime.UtcNow.Subtract(TimeSpan.FromDays(2)),
                    skinList = new List<string>(),
                    adProgress = 0
                };
                SaveData();
            }
            return _userData.giftData.skinList;
        }

        public int GetAdProgress()
        {
            return _userData.giftData.adProgress;
        }

        public void SetAdProgressCount(int val)
        {
            _userData.giftData.adProgress = val;
            SaveData();
        }
        #endregion

        #region LuckySpin

        public DateTime GetLastSpinTime()
        {
            return _userData.lastSpinTime;
        }

        public bool IsSpinAvailable()
        {
            return DateTime.Now.Subtract(_userData.lastSpinTime).TotalHours >= freeSpinCooldownHours;
        }

        public bool IsFirstSpin()
        {
            return DateTime.MinValue.Equals(_userData.lastSpinTime);
        }
        public void Spin()
        {
            _userData.lastSpinTime = DateTime.Now;
            SaveData();
        }
        #endregion

        #region DailyGift

        public int GetDailyIndex()
        {
            return _userData.dailyData.dayIndex;
        }

        public List<int> GetClaimedDays()
        {
            return _userData.dailyData.dayClaimed;
        }

        public DateTime GetLastClaimedTime()
        {
            return _userData.dailyData.lastRewardTime;
        }

        public void ClaimDailyReward(DailyGiftItem data, bool x2 = false)
        {
            _userData.dailyData.dayClaimed.Add(_userData.dailyData.dayIndex);
            _userData.dailyData.lastRewardTime = DateTime.UtcNow;

            SaveData();
        }

        public bool IsDailyEnable()
        {
            return _userData.dailyData.enableDaily;
        }
        
        public void SetNextDailyGift()
        {
            _userData.dailyData.dayIndex++;
            if (_userData.dailyData.dayIndex >= 7)
            {
                _userData.dailyData.enableDaily = false;
            }

            SaveData();

        }
        
        #endregion
        
        #region Shop

        public void ViewAd(string currency)
        {
            if (!_userData.currency.ContainsKey(currency))
            {
                _userData.currency.Add(currency,0);
            }
            _userData.currency[currency]++;
            SaveData();
        }

        public int GetAdValue(string currency)
        {
            if (!_userData.currency.ContainsKey(currency))
            {
                _userData.currency.Add(currency,0);
                SaveData();
            }
            return _userData.currency[currency];
        }

        public void ResetAdValue(string currency)
        {
            if (!_userData.currency.ContainsKey(currency))
            {
                _userData.currency.Add(currency,0);
            }
            _userData.currency[currency]=0;
            SaveData();
        }

        public void SetInSubscription(bool active)
        {
            if (!_userData.trigger.ContainsKey(SubscriptionKey))
            {
                _userData.trigger.Add(SubscriptionKey,false);
            }
            _userData.trigger[SubscriptionKey] = active;
            SaveData();
        }

        public void SetInfinityLife()
        {
            if (!_userData.trigger.ContainsKey(InfinityLifeKey))
            {
                _userData.trigger.Add(InfinityLifeKey,true);
            }
            _userData.trigger[InfinityLifeKey] = true;
            SaveData();
        }

        public bool IsInfinityLife()
        {
            if (!_userData.trigger.ContainsKey(InfinityLifeKey))
            {
                _userData.trigger.Add(InfinityLifeKey,false);
                SaveData();
            }

            return _userData.trigger[InfinityLifeKey];
        }

        public bool IsSubscription()
        {
            if (!_userData.trigger.ContainsKey(SubscriptionKey))
            {
                _userData.trigger.Add(SubscriptionKey,false);
                SaveData();
            }
            return _userData.trigger[SubscriptionKey];
        }

        public void SetBuySpecialOffer()
        {
            if (!_userData.trigger.ContainsKey(SpecialOfferKey))
            {
                _userData.trigger.Add(SpecialOfferKey,false);
            }
            _userData.trigger[SpecialOfferKey] = true;
            SaveData();
        }

        public bool IsBuySpecialOfferAvailable()
        {
            if (!_userData.trigger.ContainsKey(SpecialOfferKey))
            {
                _userData.trigger.Add(SpecialOfferKey,false);
                SaveData();
            }

            return !_userData.trigger[SpecialOfferKey] &&
                   DateTime.Now.Subtract(_userData.createTime).TotalDays < 3;
        }

        public TimeSpan GetSpecialOfferRemainTime()
        {
            return _userData.createTime.AddDays(3).Subtract(DateTime.Now);
        }
        #endregion

        #region Tutorial

        public int GetTutorialStep()
        {
            if (!_userData.currency.ContainsKey("tutorial"))
            {
                _userData.currency.Add("tutorial",1);
                SaveData();
            }
            return _userData.currency["tutorial"];
        }

        public void FinishStep()
        {
            _userData.currency["tutorial"]++;
            SaveData();
        }

        #endregion
        #endregion

    }
}