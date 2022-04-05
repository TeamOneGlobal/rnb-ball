using DG.Tweening;
using GamePlay;
using Sound;
using Truongtv.PopUpController;
using Truongtv.Services.Ad;
using Truongtv.Services.Firebase;
using Truongtv.SoundManager;
using UnityEngine;
using UnityEngine.UI;
using UserDataModel;

namespace UIController.Popup
{
    public class PausePopup : BasePopup
    {
        [SerializeField] private Transform container;
        [SerializeField] private Toggle sound, music, vibrate;
        [SerializeField] private Button closeButton, homeButton, restartButton, skipButton, continueButton;
        private bool _isInit;
        public void Init()
        {
            RegisterEvent();
            sound.isOn = SoundManager.IsSoundSfx();
            sound.onValueChanged.Invoke(sound.isOn);
            music.isOn = SoundManager.IsSoundBgm();
            music.onValueChanged.Invoke(music.isOn);
        }

        private void RegisterEvent()
        {
            if(_isInit) return;
            closeCompleteAction = ()=>{};
            openAction = OnStart;
            closeAction = OnClose;
            sound.onValueChanged.AddListener(OnSoundChange);
            music.onValueChanged.AddListener(OnMusicChange);
            vibrate.onValueChanged.AddListener(OnVibrateChange);
            closeButton.onClick.AddListener(Resume);
            restartButton.onClick.AddListener(ReStartLevel);
            skipButton.onClick.AddListener(SkipLevel);
            continueButton.onClick.AddListener(Resume);
            homeButton.onClick.AddListener(Home);
            
            sound.onValueChanged.AddListener(a=>
            {
                SoundGamePlayController.Instance.PlayButtonClickSound();
            });
            music.onValueChanged.AddListener(a=>
            {
                SoundGamePlayController.Instance.PlayButtonClickSound();
            });
            vibrate.onValueChanged.AddListener(a=>
            {
                SoundGamePlayController.Instance.PlayButtonClickSound();
            });
            closeButton.onClick.AddListener(SoundGamePlayController.Instance.PlayButtonClickSound);
            restartButton.onClick.AddListener(SoundGamePlayController.Instance.PlayButtonClickSound);
            skipButton.onClick.AddListener(SoundGamePlayController.Instance.PlayButtonClickSound);
            continueButton.onClick.AddListener(SoundGamePlayController.Instance.PlayButtonClickSound);
            homeButton.onClick.AddListener(SoundGamePlayController.Instance.PlayButtonClickSound);
            
            
            _isInit = true;
        }
        
        private void OnSoundChange(bool isOn)
        {
            SoundManager.SetSoundSfx(isOn);
        }
        private void OnMusicChange(bool isOn)
        {
            SoundManager.SetSoundBgm(isOn);
        }
        private void OnVibrateChange(bool isOn)
        {
            
        }
        private void OnStart()
        {
            container.localScale = Vector3.zero;
            SoundGamePlayController.Instance.PlayPopupSound();
            container.DOScale(1, DURATION).SetUpdate(UpdateType.Normal,true).SetEase(Ease.OutBack);
        }

        private void OnClose()
        {
            SoundGamePlayController.Instance.PlayPopupSound();
            container.DOScale(0, DURATION).SetUpdate(UpdateType.Normal,true).SetEase(Ease.InBack);
            GamePlayController.Instance.TogglePause();
        }
        private void Resume()
        {
            Close();
        }

        private void ReStartLevel()
        {
            NetWorkHelper.ShowInterstitialAd(result =>
            {
                GamePlayController.Instance.TogglePause();
                FirebaseLogEvent.LogLevelLose(GamePlayController.Instance.level);
                LoadSceneController.LoadLevel(GamePlayController.Instance.level);
            });
           
        }

        private void Home()
        {
            NetWorkHelper.ShowInterstitialAd(result =>
            {
                GamePlayController.Instance.TogglePause();
                LoadSceneController.LoadMenu();
            });
            
        }

        private void SkipLevel()
        {
            NetWorkHelper.ShowRewardedAdInGame("Rewarded_PausePopup_SkipLevel", adResult:result =>
            {
                if (!result) return;
                ForceWin();
                Resume();
            });
            
        }

        private void ForceWin()
        {
            var level = GamePlayController.Instance.level;
            FirebaseLogEvent.LogLevelWin(level);
            if (level >= 3)
            {
                UserDataController.SetLevelWin(level, CoinCollector.Instance.total);
                LoadSceneController.LoadMenu();
            }
            else
            {
                SoundGamePlayController.Instance.PlayWinSound(() =>
                {
                    UserDataController.UpdateCoin(CoinCollector.Instance.total);
                    var maxLevel = UserDataController.UpdateLevel(level);
                    FirebaseLogEvent.SerUserMaxLevel(maxLevel);
                    UserDataController.ClearPreviousLevelData();
                    var newLevel = UserDataController.GetCurrentLevel();
                    LoadSceneController.LoadLevel(newLevel);
                        
                });
            }
        }
        
    }
}