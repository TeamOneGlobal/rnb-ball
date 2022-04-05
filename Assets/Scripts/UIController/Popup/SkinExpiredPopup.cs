using System.Collections.Generic;
using DG.Tweening;
using Scriptable;
using Sound;
using Spine.Unity;
using TMPro;
using Truongtv.PopUpController;
using UIController.Scene;
using UnityEngine;
using UnityEngine.UI;
using UserDataModel;

namespace UIController.Popup
{
    public class SkinExpiredPopup : BasePopup
    {
        [SerializeField] private Transform container;
        [SerializeField] private Button closeButton, buyByCoinButton, tryButton,buyByAdButton;
        [SerializeField] private TextMeshProUGUI coinValue, adValue;
        [SerializeField] private SkeletonGraphic red, blue;
        [SerializeField] private SkinData skinData;
        private SkinInfo _skinInfo;
        private bool _isInit;
        private string _skinName;
        public void Initialized()
        {
            RegisterEvent();
            _skinName = UserDataController.GetSelectedSkin();
            _skinInfo = skinData.skins.Find(a => a.skinName.Equals(_skinName));
            red.initialSkinName = _skinName + "_1";
            red.Initialize(true);
            blue.initialSkinName = _skinName + "_2";
            blue.Initialize(true);
            if (_skinInfo.unlockType == UnlockSkinType.Coin)
            {
                buyByCoinButton.gameObject.SetActive(true);
                buyByAdButton.gameObject.SetActive(false);
                coinValue.text = ""+UserDataController.GetBoughSkinNumber()*skinData.increaseValue + skinData.baseCoinValue;
            }
            
            else
            {
                buyByCoinButton.gameObject.SetActive(false);
                buyByAdButton.gameObject.SetActive(false);
            }
            
        }

        private void RegisterEvent()
        {
            if(_isInit) return;
            _isInit = true;
            openAction = OnStart;
            closeAction = OnClose;
            closeButton.onClick.AddListener(ClosePopup);
            buyByCoinButton.onClick.AddListener(BuyByCoinNow);
            buyByAdButton.onClick.AddListener(BuyByAdNow);
            tryButton.onClick.AddListener(TryNow);
        }
        private void OnStart()
        {
            container.localScale = Vector3.zero;
            SoundMenuController.Instance.PlayPopupSound();
            container.DOScale(1, DURATION).SetUpdate(UpdateType.Normal,true).SetEase(Ease.OutBack);
        }

        private void OnClose()
        {
            SoundMenuController.Instance.PlayPopupSound();
            container.DOScale(0, DURATION).SetUpdate(UpdateType.Normal,true).SetEase(Ease.InBack);
        }
        private void TryNow()
        {
            SoundMenuController.Instance.PlayButtonClickSound(); 
            NetWorkHelper.ShowRewardedAdInMenu("Rewarded_ExpiredPopup_TrySkin", adResult:result =>
            {
                if(!result) return;
                LoadSceneController.LoadLevel(UserDataController.GetCurrentLevel());
            });
        }

        private void ClosePopup()
        {
            SoundMenuController.Instance.PlayButtonClickSound(); 
            UserDataController.ClearDemoSkin();
            MenuScene.Instance.UpdateBall();
            Close();
        }

        private void BuyByCoinNow()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            var currentValue = UserDataController.GetBoughSkinNumber() * skinData.increaseValue +
                               skinData.baseCoinValue;
            if (UserDataController.GetCurrentCoin() < currentValue)
            {
                //TODO: show not enough coin
                MenuPopupController.Instance.ShowNotEnoughCoinPopup(BuyByCoinNow);
            }
            else{
                MenuPopupController.Instance.ShowUnlockSkinsPopup(new List<string>{_skinName});
                CoinController.Instance.DecreaseCoin(currentValue);
                UserDataController.BoughtSkin(_skinName);
                UserDataController.UpdateSelectedSkin(_skinName);
                MenuScene.Instance.UpdateBall();
                Close();
            }
        }

        private void BuyByAdNow()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            NetWorkHelper.ShowRewardedAdInMenu("Rewarded_SkinPopup_PremiumSkinTab_UnlockSKin", adResult:result =>
            {
                if(!result) return;
                UserDataController.UnlockProgress(_skinName);
                if (UserDataController.GetUnlockProgress(_skinName) == _skinInfo.unlockValue)
                {
                    UserDataController.UnlockSkin(_skinName);
                    UserDataController.UpdateSelectedSkin(_skinName);
                    MenuPopupController.Instance.ShowUnlockSkinsPopup(new List<string>{_skinName});
                    MenuScene.Instance.UpdateBall();
                    Close();
                }
                else
                {
                    Initialized();
                }
            });

        }

       
    }
}