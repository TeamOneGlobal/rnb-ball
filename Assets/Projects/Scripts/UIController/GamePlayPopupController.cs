using Sirenix.OdinInspector;
using Truongtv.PopUpController;
using UIController.Popup;
using UnityEngine;

namespace UIController
{
    public class GamePlayPopupController : PopupController
    {


        [SerializeField] private PausePopup pausePopup;
        [SerializeField] private LosePopup losePopup;
        [SerializeField] private NotificationInGame notificationInGame;
        private static GamePlayPopupController _instance;
        public static GamePlayPopupController Instance => _instance;
        private void Awake()
        {
            if (_instance == null)
                _instance = this;
        }

        public void ShowPausePopup()
        {
            pausePopup.gameObject.SetActive(true);
            pausePopup.Init();
            pausePopup.Show(this);
        }

        [Button]
        public void ShowLosePopup()
        {
            losePopup.gameObject.SetActive(true);
            losePopup.Init();
            losePopup.Show(this);
        }
        [Button]
        public void ShowNotificationText(string description)
        {
            notificationInGame.gameObject.SetActive(true);
            notificationInGame.Initialized(description);
            notificationInGame.Show(this);
        }
    }
}