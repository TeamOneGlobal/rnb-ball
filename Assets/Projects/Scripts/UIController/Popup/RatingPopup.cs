using DG.Tweening;
using Sound;
using Truongtv.PopUpController;
using Truongtv.Utilities;
using UnityEngine;
using UnityEngine.UI;
using UserDataModel;

namespace UIController.Popup
{
    public class RatingPopup : BasePopup
    {
        [SerializeField] private Transform container;
        [SerializeField] private Slider slider;
        [SerializeField] private Button rateButton, laterButton;
        [SerializeField] private GameObject[] on;
        private bool _isInit;
        public void Initialized()
        {
            RegisterEvent();
        }

        private void RegisterEvent()
        {
            if(_isInit) return;
            _isInit = true;
            openAction = OnStart;
            closeAction = OnClose;
            rateButton.onClick.AddListener(Rate);
            laterButton.onClick.AddListener(ClosePopup);
            slider.onValueChanged.AddListener(OnValueChange);
        }

        private void OnValueChange(float value)
        {
            for (var i = 0; i < value; i++)
            {
                on[i].SetActive(true);
            }

            for (var i = (int)value; i < 5; i++)
            {
                on[i].SetActive(false);
            }
        }
        private void ClosePopup()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            Close();
        }

        private void Rate()
        {
            if (slider.value >= 4)
            {
                
                RatingHelper.Rate();
            }
            UserDataController.SetRating();
            ClosePopup();
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
    }
}