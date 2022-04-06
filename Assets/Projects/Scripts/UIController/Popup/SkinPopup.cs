using DG.Tweening;
using Scriptable;
using Sound;
using Spine.Unity;
using Truongtv.PopUpController;
using UIController.Scene;
using UIController.SkinPopupController;
using UnityEngine;
using UnityEngine.UI;

namespace UIController.Popup
{
    public class SkinPopup : BasePopup
    {
        [SerializeField] private Transform container;
        [SerializeField] private Button closeButton;
        [SerializeField] private SkinItem itemPrefab;
        [SerializeField] private SkinData skinData;
        [SerializeField] private Toggle premiumToggle,rescueToggle, purchaseToggle;
        [SerializeField] private SkinBoard premiumTab, rescueTab, purchaseTab;
        [SerializeField] private BallAnimationInSkinPopup red, blue;
        private bool _isInit;

        public void Initialized(int tabSelect = 0)
        {
            RegisterEvent();
            if (tabSelect == 0)
            {
                premiumToggle.isOn = true;
                premiumToggle.onValueChanged.Invoke(true);
            }

            else if (tabSelect == 1)
            {
                rescueToggle.isOn = true;
                rescueToggle.onValueChanged.Invoke(true);
            }

            else if (tabSelect == 2)
            {
                purchaseToggle.isOn = true;
                purchaseToggle.onValueChanged.Invoke(true);
            }
        }

        private void RegisterEvent()
        {
            if (_isInit) return;
            _isInit = true;
            openAction = OnStart;
            closeAction = OnClose;
            closeCompleteAction = () => { MenuScene.Instance.ShowTopRight(true);};
            closeButton.onClick.AddListener(() =>
            {
                SoundMenuController.Instance.PlayButtonClickSound();
                MenuScene.Instance.UpdateBall();
                Close();
            });
            premiumToggle.onValueChanged.AddListener(OnPremiumTabSelected);
            rescueToggle.onValueChanged.AddListener(OnRescueTabSelected);
            purchaseToggle.onValueChanged.AddListener(OnPurchaseTabSelected);
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
        private void OnPremiumTabSelected(bool isSelect)
        {
            if (isSelect)
            {
                SoundMenuController.Instance.PlayButtonClickSound();
                premiumTab.Init(this,skinData,itemPrefab);
                premiumTab.gameObject.SetActive(true);
                purchaseTab.gameObject.SetActive(false);
                rescueTab.gameObject.SetActive(false);
            }
            else
            {
                premiumTab.gameObject.SetActive(false);
            }
        }
        private void OnRescueTabSelected(bool isSelect)
        {
            if (isSelect)
            {
                SoundMenuController.Instance.PlayButtonClickSound();
                rescueTab.Init(this,skinData,itemPrefab);
                rescueTab.gameObject.SetActive(true);
                premiumTab.gameObject.SetActive(false);
                purchaseTab.gameObject.SetActive(false);
            }
            else
            {
                rescueTab.gameObject.SetActive(false);
            }
        }
        private void OnPurchaseTabSelected(bool isSelect)
        {
            if (isSelect)
            {
                SoundMenuController.Instance.PlayButtonClickSound();
                purchaseTab.Init(this,skinData,itemPrefab);
                purchaseTab.gameObject.SetActive(true);
                premiumTab.gameObject.SetActive(false);
                rescueTab.gameObject.SetActive(false);
            }
            else
            {
                purchaseTab.gameObject.SetActive(false);
            }
        }


        public void OnItemSelected(string skin)
        {
            red.skeleton.initialSkinName = skin + "_1";
            red.skeleton.Initialize(true);
            blue.skeleton.initialSkinName = skin + "_2";
            blue.skeleton.Initialize(true);

            
            red.Play();
            blue.Play();
            if (skinData.IsSkinPremium(skin))
            {
                blue.PlayAnim("ingame_premium_"+skin,1);
                red.PlayAnim("ingame_premium_"+skin,1);
            }
        }
    }
}