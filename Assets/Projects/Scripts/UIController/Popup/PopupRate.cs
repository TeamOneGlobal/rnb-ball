using DG.Tweening;
using ThirdParties.Truongtv;
using ThirdParties.Truongtv.Rating;
using ThirdParties.Truongtv.SoundManager;
using Truongtv.PopUpController;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Scripts.UIController.Popup
{
    public class PopupRate : BasePopup
    {
        [SerializeField] private Button rateButton;
        [SerializeField] private Transform container;
        [SerializeField] private Button closeButton;
        private void Start()
        {
            rateButton.onClick.AddListener(Rate);
            closeButton.onClick.AddListener(ClosePopup);
            openAction = OnStart;
            closeAction = OnClose;
        }

        private void Rate()
        {
            SoundManager.Instance.PlayButtonSound();
            GameServiceManager.Instance.ratingHelper.Rate();
            Close();
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
        private void ClosePopup()
        {
            SoundManager.Instance.PlayButtonSound();
            Close();
        }
    }
}
