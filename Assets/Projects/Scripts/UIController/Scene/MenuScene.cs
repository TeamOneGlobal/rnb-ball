

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Scriptable;
using Sound;
using ThirdParties.Truongtv;
using ThirdParties.Truongtv.AdsManager;
using TMPro;
using Truongtv.SoundManager;
using Truongtv.Utilities;
using UIController.Popup;
using UnityEngine;
using UnityEngine.UI;
using UserDataModel;
using Random = UnityEngine.Random;

namespace UIController.Scene
{
    public class MenuScene : SingletonMonoBehavior<MenuScene>
    {
        // [SerializeField] private CanvasGroup bottomRight, topRight;
        //
        // [SerializeField] private Button startButton,
        //     skinButton,
        //     heartAdButton,
        //     shopButton,
        //     buyCoinButton,
        //     specialOfferButton,
        //     luckyButton,missionButton,giftButton;

        [SerializeField] private BallAnimationMenu red, blue;
        [SerializeField] public SkinData skinData;
        private static bool firstTime;
        [SerializeField] private Image background, grade;
        [SerializeField] private Sprite factoryBg, factoryGrade;
        
        [SerializeField] private TextMeshProUGUI coinText, lifeText, starText,doubleValueText,multipleText;
        [SerializeField] private Button squidGameButton,giftButton,  spinButton, shopButton,specialOfferButton;
        [SerializeField] private Button 
            nextSkinButton,
            prevSkinButton,
            adLifeButton,
            adCoinButton,
            addMoreLifeButton,
            addMoreCoinButton;
        [SerializeField] private Button playButton, selectLevelButton, buySkinButton, trySkinButton,nextButton,doubleValueButton,continueButton,skipLevelButton;
        [SerializeField] private Transform levelWinBanner, levelLoseBanner;
        [SerializeField] private GameObject wheelNotiObj, giftNotiObj,firework;
        [SerializeField] private TextMeshProUGUI specialOfferTimeText, adLifeValueText, adCoinValueText;
        [SerializeField] private ParticleGold particleGold;
        [SerializeField] private ParticleGold particleHeart, particleStar;
        private int _currentSkinIndex;
        private List<string> _allSkinNames;
        private Sequence _increaseCoin, _increaseLife,_increaseStar;
        
        private void Start()
        {
            Init();
        }

        private void Init()
        {
            giftButton.onClick.AddListener(OnGiftButtonClick);
            spinButton.onClick.AddListener(OnSpinButtonClick);
            shopButton.onClick.AddListener(OnShopButtonClick);
            specialOfferButton.onClick.AddListener(OnSpecialButtonClick);
            playButton.onClick.AddListener(OnSelectLevelButtonClick);
            nextButton.onClick.AddListener(OnSelectLevelButtonClick);
            continueButton.onClick.AddListener(OnSelectLevelButtonClick);
            doubleValueButton.onClick.AddListener(OnDoubleRewardClick);
            skipLevelButton.onClick.AddListener(OnSkipLevelClick);
            selectLevelButton.onClick.AddListener(OnSelectLevelButtonClick);
            nextSkinButton.onClick.AddListener(OnNextSkinButtonClick);
            prevSkinButton.onClick.AddListener(OnPrevSkinButtonClick);
            buySkinButton.onClick.AddListener(OnBuySkinButtonClick);
            trySkinButton.onClick.AddListener(OnTrySkinButtonClick);
            adLifeButton.onClick.AddListener(OnAdLifeButtonClick);
            adCoinButton.onClick.AddListener(OnAdCoinButtonClick);
            addMoreLifeButton.onClick.AddListener(OnAddMoreLifeButtonClick);
            addMoreCoinButton.onClick.AddListener(OnAddMoreCoinButtonClick);
            
        }
        public void UpdateBall()
        {
            var skin = UserDataController.GetSelectedSkin();
            red.ApplySkin(skin + "_1");
            blue.ApplySkin(skin + "_2");
            red.PlayRandomState();
            blue.PlayRandomState();
        }

        // #region button event
        //
        // private async void StartLevel()
        // {
        //     await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        //     LoadSceneController.LoadLevel(UserDataController.GetCurrentLevel());
        // }
        //
        // private void OpenSkinPopup()
        // {
        //     SoundMenuController.Instance.PlayButtonClickSound();
        //     MenuPopupController.Instance.ShowSkinPopup();
        //     //ShowTopRight(false);
        // }
        //
        // private void AddHeart()
        // {
        //     SoundMenuController.Instance.PlayButtonClickSound();
        //     GameServiceManager.Instance.adManager.ShowRewardedAd("menu_free_heart", ()=>
        //     {
        //         MenuPopupController.Instance.ShowPurchaseSuccess(PurchaseSuccessPopup.PurchaseType.FreeLife, 3);
        //     });
        // }
        //
        // private void OpenShop()
        // {
        //     SoundMenuController.Instance.PlayButtonClickSound();
        //     MenuPopupController.Instance.ShowShopPopup();
        // }
        //
        // private void OpenOffer()
        // {
        //     SoundMenuController.Instance.PlayButtonClickSound();
        //     MenuPopupController.Instance.ShowSpecialOffer();
        // }
        //
        // private void OpenLuckyPopup()
        // {
        //     SoundMenuController.Instance.PlayButtonClickSound();
        //     MenuPopupController.Instance.ShowSpinPopup();
        // }
        //
        // public void HideOfferButton()
        // {
        //     specialOfferButton.gameObject.SetActive(false);
        // }
        //
        // private void OpenMissionPopup()
        // {
        //     SoundMenuController.Instance.PlayButtonClickSound();
        //     MenuPopupController.Instance.ShowMissionPopup();
        // }
        // private void OpenGiftPopup()
        // {
        //     SoundMenuController.Instance.PlayButtonClickSound();
        //     MenuPopupController.Instance.ShowGiftPopup();
        // }
        // #endregion
        #region Button Event

        private void OnGiftButtonClick()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            MenuPopupController.Instance.ShowGiftPopup();
        }

        private void OnSpinButtonClick()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            MenuPopupController.Instance.ShowSpinPopup();
        }
       
        private void OnShopButtonClick()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            MenuPopupController.Instance.ShowShopPopup();
        }
        private void OnSpecialButtonClick()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            MenuPopupController.Instance.ShowSpecialOffer();
        }
        private void OnSelectLevelButtonClick()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            MenuPopupController.Instance.ShowMissionPopup();
        }
        private void OnBuySkinButtonClick()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            GameServiceManager.Instance.adManager.ShowRewardedAd("menu_get_skin", () =>
            {
                MenuPopupController.Instance.OpenPopupReceiveSkin(_allSkinNames[_currentSkinIndex],complete: () =>
                {
                    UpdateSkin(_allSkinNames[_currentSkinIndex]);
                });
            });
        }
        private void OnTrySkinButtonClick()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            GameServiceManager.Instance.adManager.ShowRewardedAd("menu_try_skin", () =>
            {
                UserDataController.DemoSkin(_allSkinNames[_currentSkinIndex]);
                LoadSceneController.LoadLevel(UserDataController.GetCurrentLevel());       
            });
        }
        private void OnNextSkinButtonClick()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            _currentSkinIndex++;
            _currentSkinIndex %= _allSkinNames.Count;
            UpdateSkin(_allSkinNames[_currentSkinIndex]);
        }
        private void OnPrevSkinButtonClick()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            _currentSkinIndex--;
            if (_currentSkinIndex < 0)
                _currentSkinIndex += _allSkinNames.Count;
            _currentSkinIndex %= _allSkinNames.Count;
            UpdateSkin(_allSkinNames[_currentSkinIndex]);
        }
        private void OnAdLifeButtonClick()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            GameServiceManager.Instance.adManager.ShowRewardedAd("menu_life", () =>
            {
                UpdateLife(GameDataManager.Instance.adLife, adLifeButton.transform);
            });
        }
        private void OnAdCoinButtonClick()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            GameServiceManager.Instance.adManager.ShowRewardedAd("menu_coin", () =>
            {
                UpdateCoin(GameDataManager.Instance.adCoin,adCoinButton.transform);
            });
        }

        private void OnAddMoreLifeButtonClick()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            MenuPopupController.Instance.OpenPopupShop();
        }

        private void OnAddMoreCoinButtonClick()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            MenuPopupController.Instance.OpenPopupShop();
        }

        private void OnDoubleRewardClick()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            GameServiceManager.Instance.adManager.ShowRewardedAd("menu_double_value", () =>
            {
                UpdateCoin(_lastLevelData.coins*GameDataManager.Instance.multipleCoinInMenu);
                StopCoroutine(ShowContinueButton());
                playButton.gameObject.SetActive(true);
                nextButton.gameObject.SetActive(false);
                doubleValueButton.gameObject.SetActive(false);
                skipLevelButton.gameObject.SetActive(false);
                continueButton.gameObject.SetActive(false);
            });
        }

        private void OnSkipLevelClick()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            GameServiceManager.Instance.adManager.ShowRewardedAd("menu_skip_level", () =>
            {
                GameDataManager.Instance.SetTrySkin(string.Empty);
                UpdateStar(GameDataManager.Instance.GetCurrentLevel(),3);
                GameDataManager.Instance.UpdateLastLevel();
                
                playButton.gameObject.SetActive(true);
                nextButton.gameObject.SetActive(false);
                doubleValueButton.gameObject.SetActive(false);
                skipLevelButton.gameObject.SetActive(false);
                continueButton.gameObject.SetActive(false);
            });
        }
        #endregion
        
       #region Public function
        public void UpdateCoin(int value,Transform from = null)
        {
            if(value==0) return;
            var currentCoin = GameDataManager.Instance.GetCurrentCoin();
            GameDataManager.Instance.UpdateCoin(value);
            var newCoinValue =  GameDataManager.Instance.GetCurrentCoin();
            if(_increaseCoin.IsActive())
                _increaseCoin.Kill(true);
            _increaseCoin = DOTween.Sequence();
            _increaseCoin.Append(DOTween.To(() => currentCoin, x => currentCoin = x, newCoinValue, 1f).SetEase(Ease.InOutSine));
            if (from != null && value>0)
            {
                particleGold.transform.position = from.position;
                particleGold.gameObject.SetActive(true);
                particleGold.Play();
            }
            _increaseCoin.OnUpdate(() => { coinText.text = "" + currentCoin; });
            _increaseCoin.OnComplete(() => { coinText.text = "" + newCoinValue; });
            _increaseCoin.Play();
        }

        public void UpdateStar(int level, int star,Transform from = null)
        {
            var currentStars = GameDataManager.Instance.GetCurrentStars();
            GameDataManager.Instance.SetLevelStars(level,star);
            var newStarsValue =  GameDataManager.Instance.GetCurrentStars();
            if(_increaseLife.IsActive())
                _increaseLife.Kill(true);
            _increaseStar = DOTween.Sequence();
            _increaseStar.Append(DOTween.To(() => currentStars, x => currentStars = x, newStarsValue, 1f).SetEase(Ease.InOutSine));
            var delta = newStarsValue - currentStars;
            if (from != null && delta>0)
            {
                particleStar.transform.position = from.position;
                particleStar.gameObject.SetActive(true);
                particleStar.Play(delta);
            }
            _increaseStar.OnUpdate(() => { starText.text = "" + currentStars; });
            _increaseStar.OnComplete(() => { starText.text = "" + newStarsValue; });
        }

        public void UseStars(int value)
        {
            var currentStars = GameDataManager.Instance.GetCurrentStars();
            GameDataManager.Instance.UseStar(value);
            var newStarsValue =  GameDataManager.Instance.GetCurrentStars();
            if(_increaseLife.IsActive())
                _increaseLife.Kill(true);
            _increaseStar = DOTween.Sequence();
            _increaseStar.Append(DOTween.To(() => currentStars, x => currentStars = x, newStarsValue, 1f).SetEase(Ease.InOutSine));
            _increaseStar.OnUpdate(() => { starText.text = "" + currentStars; });
            _increaseStar.OnComplete(() => { starText.text = "" + newStarsValue; });
        }
        public void UpdateLife(int value,Transform from = null)
        {
            if(value<=0) return;
            var currentLife = GameDataManager.Instance.GetCurrentLife();
            GameDataManager.Instance.AddLife(value);
            var newLifeValue =  GameDataManager.Instance.GetCurrentLife();
            if(_increaseLife.IsActive())
                _increaseLife.Kill(true);
            if (from != null)
            {
                particleHeart.transform.position = from.position;
                particleHeart.gameObject.SetActive(true);
                particleHeart.Play(value);
            }
            _increaseLife = DOTween.Sequence();
            _increaseLife.Append(DOTween.To(() => currentLife, x => currentLife = x, newLifeValue, 1f).SetEase(Ease.InOutSine));
            _increaseLife.OnUpdate(() => { lifeText.text = "" + currentLife; });
            _increaseLife.OnComplete(() => { lifeText.text = "" + newLifeValue; });
        }
        public void CheckTimeCountDownGiftButton()
        {
            
        }
        
        public void UpdateSkin(string skin)
        {
            _currentSkinIndex = _allSkinNames.IndexOf(skin);
            red.ApplySkin(skin + "_1");
            blue.ApplySkin(skin + "_2");
            red.PlayRandomState();
            blue.PlayRandomState();
            var unlockSkins = UserDataController.Instance.GetUnlockedSkin();
            if (unlockSkins.Contains(skin))
            {
                trySkinButton.gameObject.SetActive(false);
                buySkinButton.gameObject.SetActive(false);
            }
            else
            {
                var canBuySkins = GameDataManager.Instance.skinData.GetSkinNameCanBuyDirectly();
                if (canBuySkins.Contains(skin))
                {
                    buySkinButton.gameObject.SetActive(true);
                    trySkinButton.gameObject.SetActive(false);
                }
                else
                {
                    buySkinButton.gameObject.SetActive(false);
                    trySkinButton.gameObject.SetActive(true);
                }
            }
        }

        private void UpdateLevelResult()
        {
            _lastLevelData = GameDataManager.Instance.GetLastLevelData();
            if (_lastLevelData == null || _lastLevelData.result == GameResult.None)
            {
                levelLoseBanner.gameObject.SetActive(false);
                levelWinBanner.gameObject.SetActive(false);
                playButton.gameObject.SetActive(true);
                nextButton.gameObject.SetActive(false);
                doubleValueButton.gameObject.SetActive(false);
                skipLevelButton.gameObject.SetActive(false);
                continueButton.gameObject.SetActive(false);
                var currentLevel = GameDataManager.Instance.GetCurrentLevel();
                var lastClaimedTime = GameDataManager.Instance.GetLastClaimedTime();
                if (currentLevel >= GameDataManager.Instance.levelShowDailyReward &&
                    DateTime.Now.Date.Subtract(lastClaimedTime.Date).TotalDays >= 1)
                {
                    #if UNITY_IOS|| UNITY_IPHONE
                    if (!Application.version.Equals(GameDataManager.Instance.versionReview))
                        PopupMenuController.Instance.OpenPopupDailyGift();
                    #else
                    PopupMenuController.Instance.OpenPopupDailyGift();
                    #endif
                    
                }
                AdManager.Instance.ShowBanner();
                return;
            }
            
            if (_lastLevelData.result == GameResult.Lose)
            {
                levelLoseBanner.gameObject.SetActive(true);
                levelWinBanner.gameObject.SetActive(false);
                playButton.gameObject.SetActive(false);
                nextButton.gameObject.SetActive(false);
                doubleValueButton.gameObject.SetActive(false);
                skipLevelButton.gameObject.SetActive(true);
                continueButton.gameObject.SetActive(false);
                StartCoroutine(ShowContinueButton());
                ball.PlayLose();
                AdManager.Instance.ShowBanner();
            }
            if (_lastLevelData.result == GameResult.Win)
            {
                var currentLevel = GameDataManager.Instance.GetCurrentLevel();
                levelLoseBanner.gameObject.SetActive(false);
                levelWinBanner.gameObject.SetActive(true);
                StartCoroutine(SpawnFireWork());
                UpdateCoin(_lastLevelData.coins,trySkinButton.transform);
                UpdateStar(_lastLevelData.level,_lastLevelData.stars,trySkinButton.transform);
                GameDataManager.Instance.UpdateLastLevel();
                doubleValueText.text = $"{_lastLevelData.coins*GameDataManager.Instance.multipleCoinInMenu}";
                multipleText.text = "x" + GameDataManager.Instance.multipleCoinInMenu;
                if (GameDataManager.Instance.GetCurrentLevel() < GameDataManager.Instance.showContinueAfterLevel)
                {
                    playButton.gameObject.SetActive(false);
                    nextButton.gameObject.SetActive(true);
                    doubleValueButton.gameObject.SetActive(false);
                    skipLevelButton.gameObject.SetActive(false);
                    continueButton.gameObject.SetActive(false);
                }
                else
                {
                    playButton.gameObject.SetActive(false);
                    nextButton.gameObject.SetActive(false);
                    doubleValueButton.gameObject.SetActive(true);
                    skipLevelButton.gameObject.SetActive(false);
                    continueButton.gameObject.SetActive(false);
                    StartCoroutine(ShowContinueButton());
                }

                if (_lastLevelData.level == currentLevel)
                {
                    if (currentLevel>=GameDataManager.Instance.levelShowRating&&(currentLevel - GameDataManager.Instance.levelShowRating) %
                        GameDataManager.Instance.showRatingPerLevel == 0)
                    {
                        PopupMenuController.Instance.OpenRate();
                        AdManager.Instance.ShowBanner();
                        return;
                    }

                    if (currentLevel>=GameDataManager.Instance.levelShowSpecialOffer&&(currentLevel - GameDataManager.Instance.levelShowSpecialOffer) %
                        GameDataManager.Instance.showOfferPerLevel == 0 && GameDataManager.Instance.IsBuySpecialOfferAvailable())
                    {
                         #if UNITY_IOS|| UNITY_IPHONE
                        if (!Application.version.Equals(GameDataManager.Instance.versionReview))
                        {
                            PopupMenuController.Instance.OpenPopupSpecialOffer(); 
                            AdManager.Instance.HideBanner();
                        }
#else
                    PopupMenuController.Instance.OpenPopupSpecialOffer(); 
                        AdManager.Instance.HideBanner();
                    #endif
                        
                        return;
                    }
                        
                    var lastClaimedTime = GameDataManager.Instance.GetLastClaimedTime();
                    if (currentLevel >= GameDataManager.Instance.levelShowDailyReward &&
                        DateTime.Now.Date.Subtract(lastClaimedTime.Date).TotalDays >= 1)
                    {
#if UNITY_IOS|| UNITY_IPHONE
                        if (!Application.version.Equals(GameDataManager.Instance.versionReview))
                            PopupMenuController.Instance.OpenPopupDailyGift();
#else
                    PopupMenuController.Instance.OpenPopupDailyGift();
#endif
                        AdManager.Instance.ShowBanner();
                        return;
                    }

                    if (currentLevel >= GameDataManager.Instance.levelShowLottery &&
                        (currentLevel - GameDataManager.Instance.levelShowLottery) %
                        GameDataManager.Instance.showLotteryPerLevel == 0)
                    {
                        PopupMenuController.Instance.OpenLottery();
                        AdManager.Instance.HideBanner();
                        return;
                    }
                }
            }

        }
        private IEnumerator SpawnFireWork()
        {
            for (var i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(Random.Range(0.2f, 1f));
                var obj = Instantiate(firework,transform);
                obj.transform.localScale = new Vector3(100,100,100);
                obj.transform.position = new Vector3(Random.Range(-10f, 10f), Random.Range(2f, 6f), 100);
            }
        }

        private IEnumerator ShowContinueButton()
        {
            yield return new WaitForSeconds(2f);
            continueButton.gameObject.SetActive(true);
        }
        #endregion
    }
}