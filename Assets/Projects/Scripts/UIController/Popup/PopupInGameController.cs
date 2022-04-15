using System;
using Truongtv.PopUpController;
using UnityEngine;

namespace Projects.Scripts.UIController.Popup
{
    public class PopupInGameController : MonoBehaviour
    {
        [SerializeField] private PopupController controller;
        [SerializeField] private PopupReceiveSkin popupReceiveSkin;
        [SerializeField] private PopupRevive popupRevive;
        [SerializeField] private PopupPause popupPause;
        private static PopupInGameController _instance;
        public static PopupInGameController Instance => _instance;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(_instance.gameObject);
            }

            _instance = this;
        }
        public void OpenPopupReceiveSkin(string skin,Action complete = null)
        {
            popupReceiveSkin.Initialized(skin,true,complete);
            popupReceiveSkin.gameObject.SetActive(true);
            popupReceiveSkin.Show(controller);
        }
        public void OpenPopupRevive(Action onWatchAd, Action onClose, string skin)
        {
            popupRevive.Initialized(onWatchAd,onClose, skin);
            popupRevive.gameObject.SetActive(true);
            popupRevive.Show(controller);
        }
        public void OpenPopupPause()
        {
            popupPause.gameObject.SetActive(true);
            popupPause.Init();
            popupPause.Show(controller);
        }
    }
}