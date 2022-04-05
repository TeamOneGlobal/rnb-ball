using System;
using DG.Tweening;
using Sound;
using TMPro;
using Truongtv.PopUpController;
using UnityEngine;
using UnityEngine.UI;

namespace UIController.Popup
{
    public class NotificationPopup : BasePopup
    {
        [SerializeField] private Transform container;
        [SerializeField] private TextMeshProUGUI titleTxt, descriptionTxt;
        [SerializeField] private Button closeButton,xButton;

        private bool _isInit;
        private Action _callback;
        public void Initialized(string title,string description,Action buttonAction = null)
        {
            titleTxt.text = title;
            descriptionTxt.text = description;
            _callback = buttonAction;
            RegisterEvent();
        }

        private void RegisterEvent()
        {
            if(_isInit) return;
            _isInit = true;
            openAction = OnStart;
            closeAction = OnClose;
            closeButton.onClick.RemoveAllListeners();
            xButton.onClick.AddListener(() =>
            {
                if(SoundMenuController.Instance!=null)
                    SoundMenuController.Instance.PlayButtonClickSound();
                if(SoundGamePlayController.Instance!=null)
                    SoundGamePlayController.Instance.PlayButtonClickSound();
                Close();
            });
            closeButton.onClick.AddListener(() =>
            {
                if(SoundMenuController.Instance!=null)
                    SoundMenuController.Instance.PlayButtonClickSound();
                if(SoundGamePlayController.Instance!=null)
                    SoundGamePlayController.Instance.PlayButtonClickSound();
                _callback?.Invoke();
                Close();
            });
        }
        private void OnStart()
        {
            container.localScale = Vector3.zero;
            if(SoundMenuController.Instance!=null)
                SoundMenuController.Instance.PlayPopupSound();
            if(SoundGamePlayController.Instance!=null)
                SoundGamePlayController.Instance.PlayPopupSound();
            container.DOScale(1, DURATION).SetUpdate(UpdateType.Normal,true).SetEase(Ease.OutBack);
        }

        private void OnClose()
        {
            if(SoundMenuController.Instance!=null)
                SoundMenuController.Instance.PlayPopupSound();
            if(SoundGamePlayController.Instance!=null)
                SoundGamePlayController.Instance.PlayPopupSound();
            container.DOScale(0, DURATION).SetUpdate(UpdateType.Normal,true).SetEase(Ease.InBack);
        }
    }
}