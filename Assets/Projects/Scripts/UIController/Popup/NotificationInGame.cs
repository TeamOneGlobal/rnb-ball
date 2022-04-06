using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sound;
using TMPro;
using Truongtv.PopUpController;
using UnityEngine;

namespace UIController.Popup
{
    public class NotificationInGame : BasePopup
    {
        [SerializeField] private TextMeshProUGUI descriptionTxt;
        [SerializeField] private float stayDuration;
        private bool _isInit;
        public void Initialized(string description)
        {
            descriptionTxt.text = description;
            
            RegisterEvent();
        }

        private void RegisterEvent()
        {
            if(_isInit) return;
            _isInit = true;
            openAction = OnStart;
            closeAction = OnClose;
            openCompleteAction = async () => { 
                await UniTask.Delay(TimeSpan.FromSeconds(stayDuration));
                Close();
            };
        }
        private void OnStart()
        {
            SoundGamePlayController.Instance.PlayPopupSound();
        }

        private void OnClose()
        {
            SoundGamePlayController.Instance.PlayPopupSound();
        }
    }
}