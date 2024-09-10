using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Com.LuisPedroFonseca.ProCamera2D;
using DG.Tweening;
using Projects.Scripts.Data;
using Projects.Scripts.GamePlay.Sound;
using Projects.Scripts.UIController.Menu;
using ThirdParties.Truongtv;
using ThirdParties.Truongtv.SoundManager;
using TMPro;
using Truongtv.PopUpController;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Projects.Scripts.UIController
{
    public class MenuController : MonoBehaviour
    {
        private static MenuController _instance;
        public static MenuController Instance => _instance;
        [SerializeField] private BallMenuController red,blue;
        [SerializeField] private TextMeshProUGUI coinText, lifeText,doubleValueText,multipleText;
        [SerializeField] private Button giftButton,  spinButton, shopButton,specialOfferButton;
        [SerializeField] private Button 
            nextSkinButton,
            prevSkinButton,
            adLifeButton,
            adCoinButton,
            addMoreLifeButton,
            addMoreCoinButton;
        [SerializeField] private Button playButton, selectLevelButton, buySkinButton, trySkinButton,selectSkinButton,nextButton,doubleValueButton,continueButton;
        [SerializeField] private Transform levelWinBanner, levelLoseBanner;
        [SerializeField] private GameObject wheelNotiObj, giftNotiObj,firework;
        [SerializeField] private TextMeshProUGUI specialOfferTimeText, adLifeValueText, adCoinValueText;
        [SerializeField] private ParticleGold particleGold;
        [SerializeField] private ParticleGold particleHeart;
        
        [SerializeField] private GameObject loadingFake;

        private int _currentSkinIndex;
        private List<string> _allSkinNames;
        private Sequence _increaseCoin, _increaseLife,_increaseStar;
        private LastLevelData _lastLevelData;
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(_instance.gameObject);
            }

            _instance = this;
            
            loadingFake.SetActive(true);
        }

        private IEnumerator Start()
        {
            Init();
            UpdateLevelResult();
            yield return new WaitForSeconds(0.5f);
            loadingFake.SetActive(false);
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
            selectLevelButton.onClick.AddListener(OnSelectLevelButtonClick);
            nextSkinButton.onClick.AddListener(OnNextSkinButtonClick);
            prevSkinButton.onClick.AddListener(OnPrevSkinButtonClick);
            buySkinButton.onClick.AddListener(OnBuySkinButtonClick);
            trySkinButton.onClick.AddListener(OnTrySkinButtonClick);
            selectSkinButton.onClick.AddListener(OnSelectSkinButtonClick);
            adLifeButton.onClick.AddListener(OnAdLifeButtonClick);
            adCoinButton.onClick.AddListener(OnAdCoinButtonClick);
            addMoreLifeButton.onClick.AddListener(OnAddMoreLifeButtonClick);
            addMoreCoinButton.onClick.AddListener(OnAddMoreCoinButtonClick);

            lifeText.text ="" + GameDataManager.Instance.GetCurrentLife();
            coinText.text = "" + GameDataManager.Instance.GetCurrentCoin();
            _allSkinNames = GameDataManager.Instance.skinData.GetAllSkinName();
            adLifeValueText.text = "+" + GameDataManager.Instance.adLife;
            adCoinValueText.text = "+" + GameDataManager.Instance.adCoin;
            
            if (GameDataManager.Instance.IsBuySpecialOfferAvailable())
            {
               
                StartCoroutine(SpecialOfferCountDown());
            }
            else
            {
                specialOfferButton.gameObject.SetActive(false);
                StopCoroutine(SpecialOfferCountDown());
            }
            RandomSkin();
            InvokeRepeating(nameof(CheckWheel),0f,1f);
            InvokeRepeating(nameof(CheckGift),0f,1f);
            SoundMenuManager.Instance.PlayBgmSound();
            
        }

        #region Button Event

        private void OnGiftButtonClick()
        {
            SoundManager.Instance.PlayButtonSound();
            PopupMenuController.Instance.OpenPopupGift();
        }
        private void OnSpinButtonClick()
        {
            SoundManager.Instance.PlayButtonSound();
            PopupMenuController.Instance.OpenPopupSpin();
        }
       
        private void OnShopButtonClick()
        {
            SoundManager.Instance.PlayButtonSound();
            PopupMenuController.Instance.OpenPopupShop();
        }
        private void OnSpecialButtonClick()
        {
            SoundManager.Instance.PlayButtonSound();
            PopupMenuController.Instance.OpenPopupSpecialOffer();
        }
 
        private void OnSelectLevelButtonClick()
        {
            SoundManager.Instance.PlayButtonSound();
            //PopupMenuController.Instance.OpenPopupSelectLevel();
            if (!GameDataManager.Instance.CanPlayWithoutInternet())
            {
                PopupController.Instance.ShowNoInternet();
                return;
            }
            ProCamera2DTransitionsFX.Instance.OnTransitionExitEnded += () =>
            {
                var level = GameDataManager.Instance.GetCurrentLevel();
                if (level > GameDataManager.Instance.maxLevel)
                    level = GameDataManager.Instance.maxLevel;
                LoadSceneController.LoadLevel(level);
            };
            ProCamera2DTransitionsFX.Instance.TransitionExit();
        }
        private void OnBuySkinButtonClick()
        {
            
            SoundManager.Instance.PlayButtonSound();
            GameServiceManager.Instance.adManager.ShowRewardedAd("menu_get_skin", () =>
            {
                PopupMenuController.Instance.OpenPopupReceiveSkin(_allSkinNames[_currentSkinIndex],complete: () =>
                {
                    UpdateSkin(_allSkinNames[_currentSkinIndex]);
                });
            });
        }
        private void OnTrySkinButtonClick()
        {
            SoundManager.Instance.PlayButtonSound();
            GameServiceManager.Instance.adManager.ShowRewardedAd("menu_try_skin", () =>
            {
                GameDataManager.Instance.SetTrySkin(_allSkinNames[_currentSkinIndex]);
                StartLevel();
            });
        }
        private void OnSelectSkinButtonClick()
        {
            SoundManager.Instance.PlayButtonSound();
            GameDataManager.Instance.SetSkin(_allSkinNames[_currentSkinIndex]);
            UpdateSkin(_allSkinNames[_currentSkinIndex]);
        }
        private void OnNextSkinButtonClick()
        {
            SoundManager.Instance.PlayButtonSound();
            _currentSkinIndex++;
            _currentSkinIndex %= _allSkinNames.Count;
            UpdateSkin(_allSkinNames[_currentSkinIndex]);
        }
        private void OnPrevSkinButtonClick()
        {
            SoundManager.Instance.PlayButtonSound();
            _currentSkinIndex--;
            if (_currentSkinIndex < 0)
                _currentSkinIndex += _allSkinNames.Count;
            _currentSkinIndex %= _allSkinNames.Count;
            UpdateSkin(_allSkinNames[_currentSkinIndex]);
        }
        private void OnAdLifeButtonClick()
        {
            SoundManager.Instance.PlayButtonSound();
            GameServiceManager.Instance.adManager.ShowRewardedAd("menu_life", () =>
            {
                UpdateLife(GameDataManager.Instance.adLife, adLifeButton.transform);
            });
        }
        private void OnAdCoinButtonClick()
        {
            SoundManager.Instance.PlayButtonSound();
            GameServiceManager.Instance.adManager.ShowRewardedAd("menu_coin", () =>
            {
                UpdateCoin(GameDataManager.Instance.adCoin,adCoinButton.transform);
            });
        }

        private void OnAddMoreLifeButtonClick()
        {
            SoundManager.Instance.PlayButtonSound();
            PopupMenuController.Instance.OpenPopupShop();
        }

        private void OnAddMoreCoinButtonClick()
        {
            SoundManager.Instance.PlayButtonSound();
            PopupMenuController.Instance.OpenPopupShop();
        }

        private void OnDoubleRewardClick()
        {
            SoundManager.Instance.PlayButtonSound();
            GameServiceManager.Instance.adManager.ShowRewardedAd("menu_double_value", () =>
            {
                UpdateCoin(_lastLevelData.coins*GameDataManager.Instance.multipleCoinInMenu);
                StopCoroutine(ShowContinueButton());
                playButton.gameObject.SetActive(true);
                nextButton.gameObject.SetActive(false);
                doubleValueButton.gameObject.SetActive(false);
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
            red.ApplySkin(skin+"_1");
            red.PlayRandomMix();
            blue.ApplySkin(skin+"_2");
            blue.PlayRandomMix();
            var unlockSkins = GameDataManager.Instance.GetUnlockedSkin();
            if (unlockSkins.Contains(skin))
            {
                trySkinButton.gameObject.SetActive(false);
                buySkinButton.gameObject.SetActive(false);
                selectSkinButton.gameObject.SetActive(!GameDataManager.Instance.GetCurrentSkin().Equals(skin));
            }
            else
            {
                selectSkinButton.gameObject.SetActive(false);
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
                GameServiceManager.Instance.adManager.ShowBanner();
                return;
            }
            
            if (_lastLevelData.result == GameResult.Lose)
            {
                levelLoseBanner.gameObject.SetActive(true);
                levelWinBanner.gameObject.SetActive(false);
                playButton.gameObject.SetActive(false);
                nextButton.gameObject.SetActive(false);
                doubleValueButton.gameObject.SetActive(false);
                continueButton.gameObject.SetActive(false);
                StartCoroutine(ShowPlayButton());
                red.PlayLose();
                blue.PlayLose();
                GameServiceManager.Instance.adManager.ShowBanner();
            }
            if (_lastLevelData.result == GameResult.Win)
            {
                var currentLevel = GameDataManager.Instance.GetCurrentLevel();
                levelLoseBanner.gameObject.SetActive(false);
                levelWinBanner.gameObject.SetActive(true);
                StartCoroutine(SpawnFireWork());
                UpdateCoin(_lastLevelData.coins,trySkinButton.transform);
                GameDataManager.Instance.UpdateLastLevel();
                doubleValueText.text = $"{_lastLevelData.coins*GameDataManager.Instance.multipleCoinInMenu}";
                multipleText.text = "x" + GameDataManager.Instance.multipleCoinInMenu;
                if (GameDataManager.Instance.GetCurrentLevel() < GameDataManager.Instance.showContinueAfterLevel)
                {
                    playButton.gameObject.SetActive(false);
                    nextButton.gameObject.SetActive(true);
                    doubleValueButton.gameObject.SetActive(false);
                    continueButton.gameObject.SetActive(false);
                }
                else
                {
                    playButton.gameObject.SetActive(false);
                    nextButton.gameObject.SetActive(false);
                    doubleValueButton.gameObject.SetActive(true);
                    continueButton.gameObject.SetActive(false);
                    StartCoroutine(ShowContinueButton());
                }

                if (_lastLevelData.level == currentLevel)
                {
                    if (currentLevel>=GameDataManager.Instance.levelShowRating&&(currentLevel - GameDataManager.Instance.levelShowRating) %
                        GameDataManager.Instance.showRatingPerLevel == 0)
                    {
                        PopupMenuController.Instance.OpenRate();
                        GameServiceManager.Instance.adManager.ShowBanner();
                        return;
                    }

                    if (currentLevel>=GameDataManager.Instance.levelShowSpecialOffer&&(currentLevel - GameDataManager.Instance.levelShowSpecialOffer) %
                        GameDataManager.Instance.showOfferPerLevel == 0 && GameDataManager.Instance.IsBuySpecialOfferAvailable())
                    {
                         #if UNITY_IOS|| UNITY_IPHONE
                        if (!Application.version.Equals(GameDataManager.Instance.versionReview))
                        {
                            PopupMenuController.Instance.OpenPopupSpecialOffer(); 
                            GameServiceManager.Instance.adManager.HideBanner();
                        }
#else
                    PopupMenuController.Instance.OpenPopupSpecialOffer(); 
                        //GameServiceManager.Instance.adManager.HideBanner();
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
                        GameServiceManager.Instance.adManager.ShowBanner();
                        return;
                    }

                    if (currentLevel == GameDataManager.Instance.showSpinLevel)
                    {
                        PopupMenuController.Instance.OpenPopupSpin();
                        return;
                    }
                    if (currentLevel == GameDataManager.Instance.showGiftLevel)
                    {
                        PopupMenuController.Instance.OpenPopupGift();
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
        private IEnumerator ShowPlayButton()
        {
            yield return new WaitForSeconds(2f);
            playButton.gameObject.SetActive(true);
        }
        #endregion
        
        #region Function

        private void RandomSkin()
        {
          
            var unlockSkins = GameDataManager.Instance.GetUnlockedSkin();
            var notUnlock = _allSkinNames.Except(unlockSkins).ToList();
            var currentSkin = notUnlock.Count > 0 ? notUnlock[Random.Range(0, notUnlock.Count)] : GameDataManager.Instance.GetCurrentSkin();
            _currentSkinIndex = _allSkinNames.IndexOf(currentSkin);
            UpdateSkin(currentSkin);

        }

        private void StartLevel()
        {
            SceneManager.LoadScene(GameDataManager.Instance.GetCurrentLevel());
        }

        private IEnumerator SpecialOfferCountDown()
        {
            while (GameDataManager.Instance.IsBuySpecialOfferAvailable())
            {
                var span = GameDataManager.Instance.GetSpecialOfferRemainTime();
                specialOfferTimeText.text =  $"{span.TotalHours:00}:{span.Minutes:00}:{span.Seconds:00}";
                yield return new WaitForSeconds(1);
            }
        }
        
        private void CheckWheel()
        {
            wheelNotiObj.SetActive(GameDataManager.Instance.IsSpinAvailable());
        }

        private void CheckGift()
        {
            giftNotiObj.SetActive(GameDataManager.Instance.CanRefresh());
        }
        #endregion
    }
}