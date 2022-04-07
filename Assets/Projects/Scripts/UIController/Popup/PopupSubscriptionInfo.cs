using System;
using DG.Tweening;
using ThirdParties.Truongtv;
using ThirdParties.Truongtv.SoundManager;
using TMPro;
using Truongtv.PopUpController;
using Truongtv.Services.IAP;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Scripts.UIController.Popup
{
    public class PopupSubscriptionInfo : BasePopup
    {
        [SerializeField] private Transform container;
        [SerializeField] private Button privacyButton, termOfUseButton, confirmButton,closeButton;
        [SerializeField] private TextMeshProUGUI valueText;
        private void Start()
        {
            privacyButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayButtonSound();
                Application.OpenURL(GameDataManager.Instance.privacyPolicyUrl);
            });
            termOfUseButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayButtonSound();
                Application.OpenURL(GameDataManager.Instance.termOfUseUrl);
            });
            closeButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayButtonSound();
                Close();
            });
        }

        public void Init(string id,Action<Action> buyAction)
        {
            openAction = OnStart;
            closeAction = OnClose;
            valueText.text = GameServiceManager.Instance.iapManager.GetItemLocalPriceString(id)+ "/week";
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(() =>
            {
                
                SoundManager.Instance.PlayButtonSound();
                GameServiceManager.Instance.iapManager.PurchaseProduct(id, (result, sku) =>
                {
                    if(!result) return;
                    buyAction.Invoke(Close);
                });
            });
        }
        private void OnStart()
        {
            container.localScale = Vector3.zero;
            SoundManager.Instance.PlayPopupOpenSound();
            container.DOScale(1, DURATION).SetUpdate(UpdateType.Normal, true).SetEase(Ease.OutBack);
        }

        private void OnClose()
        {
            SoundManager.Instance.PlayPopupCloseSound();
            container.DOScale(0, DURATION).SetUpdate(UpdateType.Normal, true).SetEase(Ease.InBack);
        }
    }
}