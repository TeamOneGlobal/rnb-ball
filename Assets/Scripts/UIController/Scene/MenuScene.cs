

using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Scriptable;
using Sirenix.OdinInspector;
using Sound;
using ThirdParties.Truongtv;
using Truongtv.Utilities;
using UIController.Popup;
using UnityEngine;
using UnityEngine.UI;
using UserDataModel;

namespace UIController.Scene
{
    public class MenuScene : SingletonMonoBehavior<MenuScene>
    {
        [SerializeField] private CanvasGroup bottomRight, topRight;
        
        [SerializeField] private Button startButton,
            skinButton,
            heartAdButton,
            shopButton,
            buyCoinButton,
            specialOfferButton,
            luckyButton,missionButton,giftButton;

        [SerializeField] private BallAnimationMenu red, blue;
        [SerializeField] public SkinData skinData;
        private static bool firstTime;
        [SerializeField] private Image background, grade;
        [SerializeField] private Sprite factoryBg, factoryGrade;
        
        
        private void Start()
        {
            UserDataController.LoadUserData();
            if (UserDataController.GetCurrentLevel() > 50)
            {
                background.sprite = factoryBg;
                grade.sprite = factoryGrade;
            }
            Init();
            if (UserDataController.IsLastLevelWin())
            {
                var levelWin = UserDataController.GetLastLevelData();
                var skin = skinData.skins.FirstOrDefault(a =>
                    a.unlockType == UnlockSkinType.Level && a.unlockValue == levelWin.lastLevelWin);

                ShowWin();
                if (UserDataController.IsSpinAvailable()&& UserDataController.GetCurrentLevel()>=Config.MIN_LEVEL_SHOW_SPIN)
                {
                    MenuPopupController.Instance.ShowSpinPopup();
                }
                if (UserDataController.IsDemoSkin())
                {
                    MenuPopupController.Instance.ShowSkinExpired();
                }

                if (skin != null)
                {
                    MenuPopupController.Instance.ShowCongratsPopup(skin.skinName);
                }

                if (UserDataController.GetCurrentLevel() == Config.SHOW_REVIEW_AFTER_LEVEL)
                {
                    MenuPopupController.Instance.ShowRatingPopup();
                }

                if (levelWin.lastLevelWin == Config.SHOW_REWARD_AFTER_LEVEL)
                {
                    MenuPopupController.Instance.ShowDailyGiftPopup();
                }
                if (levelWin.lastLevelWin == Config.SHOW_GIFT_AFTER_LEVEL)
                {
                    MenuPopupController.Instance.ShowGiftPopup();
                }
            }
            else
            {
                red.PlayRandomState();
                blue.PlayRandomState();
                if (UserDataController.IsSpinAvailable() && UserDataController.GetCurrentLevel()>=Config.MIN_LEVEL_SHOW_SPIN)
                {
                    MenuPopupController.Instance.ShowSpinPopup();
                }

                if (UserDataController.GetCurrentLevel() > Config.SHOW_REWARD_AFTER_LEVEL &&
                    UserDataController.IsDailyRewardAvailable() && !firstTime)
                {
                    firstTime = true;
                    MenuPopupController.Instance.ShowDailyGiftPopup();
                }
            }
        }

        private void Init()
        {
            startButton.onClick.AddListener(SoundMenuController.Instance.PlayButtonClickSound);
            startButton.onClick.AddListener(StartLevel);
            skinButton.onClick.AddListener(OpenSkinPopup);
            heartAdButton.onClick.AddListener(AddHeart);
            shopButton.onClick.AddListener(OpenShop);
            buyCoinButton.onClick.AddListener(OpenShop);
            specialOfferButton.onClick.AddListener(OpenOffer);
            luckyButton.onClick.AddListener(OpenLuckyPopup);
            missionButton.onClick.AddListener(OpenMissionPopup);
            giftButton.onClick.AddListener(OpenGiftPopup);
            CoinController.Instance.UpdateCoin();
            LifeController.Instance.UpdateLife();
            if (UserDataController.IsBuySpecialOffer())
            {
                HideOfferButton();
            }
            UpdateBall();
            GameServiceManager.Instance.adManager.ShowBanner();
        }

        private void ShowWin()
        {
            HideBottomRight();
            red.PlayWin();
            blue.PlayWin();
            MenuPopupController.Instance.ShowWinPopup();
        }

        private void HideBottomRight()
        {
            bottomRight.DOFade(0, 0.5f).SetEase(Ease.Linear);
        }

        public void ShowBottomRight()
        {
            bottomRight.DOFade(1, 0.5f).SetEase(Ease.Linear);
        }

        public void ShowTopRight(bool isShow)
        {
            topRight.gameObject.SetActive(isShow);
        }

        public void UpdateBall()
        {
            var skin = UserDataController.GetSelectedSkin();
            red.ApplySkin(skin + "_1");
            blue.ApplySkin(skin + "_2");
            red.PlayRandomState();
            blue.PlayRandomState();
        }

        #region button event

        private async void StartLevel()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            LoadSceneController.LoadLevel(UserDataController.GetCurrentLevel());
        }

        private void OpenSkinPopup()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            MenuPopupController.Instance.ShowSkinPopup();
            //ShowTopRight(false);
        }

        private void AddHeart()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            GameServiceManager.Instance.adManager.ShowRewardedAd("menu_free_heart", ()=>
            {
                MenuPopupController.Instance.ShowPurchaseSuccess(PurchaseSuccessPopup.PurchaseType.FreeLife, 3);
            });
        }

        private void OpenShop()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            MenuPopupController.Instance.ShowShopPopup();
        }

        private void OpenOffer()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            MenuPopupController.Instance.ShowSpecialOffer();
        }

        private void OpenLuckyPopup()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            MenuPopupController.Instance.ShowSpinPopup();
        }

        public void HideOfferButton()
        {
            specialOfferButton.gameObject.SetActive(false);
        }

        private void OpenMissionPopup()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            MenuPopupController.Instance.ShowMissionPopup();
        }
        private void OpenGiftPopup()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            MenuPopupController.Instance.ShowGiftPopup();
        }
        #endregion
       
    }
}