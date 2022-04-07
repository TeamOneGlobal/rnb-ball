using DG.Tweening;
using Projects.Scripts.Data;
using Projects.Scripts.UIController.Menu;
using ThirdParties.Truongtv;
using ThirdParties.Truongtv.AdsManager;
using ThirdParties.Truongtv.SoundManager;
using Truongtv.PopUpController;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Scripts.UIController.Popup
{
    public class PopupShop : BasePopup
    {
        [SerializeField] private Transform container;
        [SerializeField] private Button closeButton;
       
        [SerializeField] private Toggle shopToggle, premiumToggle, rescueToggle, coinToggle;
        [SerializeField] private ShopBoard shopTab;
        [SerializeField] private SkinBoard premiumTab, rescueTab, coinTab;
        private bool _isInit;

        public void Initialized()
        {
            GameServiceManager.Instance.adManager.HideBanner();
            RegisterEvent();
            
        }

        public void PurchaseSuccessCallback(ShopItemData item)
        {
            shopTab.PurchaseSuccessCallback(item);
        }

        public void ActiveClose(bool active)
        {
            closeButton.interactable = active;
        }
        private void RegisterEvent()
        {
            if(_isInit) return;
            _isInit = true;
            openAction = OnStart;
            closeAction = OnClose;
            closeButton.onClick.AddListener(ClosePopup);
            shopToggle.onValueChanged.AddListener(OnShopTabSelected);
            premiumToggle.onValueChanged.AddListener(OnPremiumTabSelected);
            rescueToggle.onValueChanged.AddListener(OnRescueTabSelected);
            coinToggle.onValueChanged.AddListener(OnCoinTabSelected);
        }

        private void ClosePopup()
        {
            GameServiceManager.Instance.adManager.ShowBanner();
            SoundManager.Instance.PlayButtonSound();
             Close();
        }
        private void OnStart()
        {
            container.localScale = Vector3.zero;
            SoundManager.Instance.PlayPopupOpenSound();
            container.DOScale(1, DURATION).SetUpdate(UpdateType.Normal,true).SetEase(Ease.OutBack);
            shopToggle.isOn = true;
            shopToggle.onValueChanged.Invoke(true);
        }

        private void OnClose()
        {
            SoundManager.Instance.PlayPopupCloseSound();
            container.DOScale(0, DURATION).SetUpdate(UpdateType.Normal,true).SetEase(Ease.InBack);
        }

        private void OnShopTabSelected(bool isSelect)
        {
            if (isSelect)
            {
                shopTab.gameObject.SetActive(true);
                shopTab.Init();
                premiumTab.gameObject.SetActive(false);
                rescueTab.gameObject.SetActive(false);
                coinTab.gameObject.SetActive(false);
            }
            else
            {
                shopTab.gameObject.SetActive(false);
            }
        }
        private void OnPremiumTabSelected(bool isSelect)
        {
            if (isSelect)
            {
                shopTab.gameObject.SetActive(false);
                premiumTab.gameObject.SetActive(true);
                premiumTab.Init();
                rescueTab.gameObject.SetActive(false);
                coinTab.gameObject.SetActive(false);
                
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
                shopTab.gameObject.SetActive(false);
                premiumTab.gameObject.SetActive(false);
                rescueTab.gameObject.SetActive(true);
                coinTab.gameObject.SetActive(false);
                rescueTab.Init();
            }
            else
            {
                rescueTab.gameObject.SetActive(false);
            }
        }
        private void OnCoinTabSelected(bool isSelect)
        {
            if (isSelect)
            {
                shopTab.gameObject.SetActive(false);
                premiumTab.gameObject.SetActive(false);
                rescueTab.gameObject.SetActive(false);
                coinTab.gameObject.SetActive(true);
                coinTab.Init();
            }
            else
            {
                coinTab.gameObject.SetActive(false);
            }
        }
    }
}