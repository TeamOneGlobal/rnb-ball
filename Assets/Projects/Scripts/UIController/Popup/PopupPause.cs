using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GamePlay;
using Projects.Scripts.Data;
using ThirdParties.Truongtv;
using ThirdParties.Truongtv.AdsManager;
using ThirdParties.Truongtv.LogManager;
using ThirdParties.Truongtv.SoundManager;
using Truongtv.PopUpController;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Scripts.UIController
{
    public class PopupPause : BasePopup
    {
        [SerializeField] private Transform container;
        [SerializeField] private Button replayButton,homeButton,skipButton,closeButton;
        [SerializeField] private Toggle sfxToggle, bgmToggle, vibrateToggle;

        private void Awake()
        {
            replayButton.onClick.AddListener(OnReplayButtonClick);
            homeButton.onClick.AddListener(OnHomeButtonClick);
            skipButton.onClick.AddListener(OnSkipButtonClick);
            closeButton.onClick.AddListener(OnContinueButtonClick);
            sfxToggle.onValueChanged.AddListener(OnSfxToggleChange);
            bgmToggle.onValueChanged.AddListener(OnBgmToggleChange);
            vibrateToggle.onValueChanged.AddListener(OnVibrateToggleChange);
            openAction = OnStart;
            closeAction = OnClose;
        }

        public void Init()
        {
            sfxToggle.isOn = SoundManager.IsSfx();
            bgmToggle.isOn = SoundManager.IsBgm();
            vibrateToggle.isOn = PlayerPrefs.GetInt("vibration") == 0;
            GamePlayController.Instance.Pause();
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
            GamePlayController.Instance.Resume();
            container.DOScale(0, DURATION).SetUpdate(UpdateType.Normal, true).SetEase(Ease.InBack);
            
        }
        private void OnContinueButtonClick()
        {
            SoundManager.Instance.PlayButtonSound();
            Close();
        }

        private void OnReplayButtonClick()
        {
            SoundManager.Instance.PlayButtonSound();

            LoadingReplay();
        }
        
        public void LoadingReplay()
        {
            StartCoroutine(IELoadingReplay());
        }

        private IEnumerator IELoadingReplay()
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 0f;
            GamePlayController.Instance.Resume();
            GameServiceManager.Instance.adManager.HideBanner();
            GamePlayController.Instance.loadingFake.SetActive(true);
            yield return new WaitForSeconds(0.75f);
            GameServiceManager.Instance.adManager.ShowInterstitialAd(() =>
            {
                LoadSceneController.LoadLevel(GamePlayController.Instance.level);
            });
        }

        private void OnHomeButtonClick()
        {
            SoundManager.Instance.PlayButtonSound();
            
            LoadingHome();
        }
        
        public void LoadingHome()
        {
            StartCoroutine(IELoadingHome());
        }

        private IEnumerator IELoadingHome()
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 0f;
            GamePlayController.Instance.Resume();
            GameServiceManager.Instance.adManager.HideBanner();
            GamePlayController.Instance.loadingFake.SetActive(true);
            yield return new WaitForSeconds(0.75f);
            GameServiceManager.Instance.adManager.ShowInterstitialAd(LoadSceneController.LoadMenu);
        }

        private void OnSkipButtonClick()
        {
            SoundManager.Instance.PlayButtonSound();
            GamePlayController.Instance.Resume();
            GameServiceManager.Instance.logEventManager.LogEvent("count_ad_in_game_click",new Dictionary<string, object>
            {
                {"level","lv_"+GamePlayController.Instance.level},
                {"type","skip_level"}
            });
            GameServiceManager.Instance.adManager.ShowRewardedAd("in_game_skip_level", () =>
            {
                GameServiceManager.Instance.logEventManager.LogEvent("count_ad_in_game_finish",new Dictionary<string, object>
                {
                    {"level","lv_"+GamePlayController.Instance.level},
                    {"type","skip_level"}
                });
                GamePlayController.Instance.Win();
                Close();
            });
        }

        private void OnSfxToggleChange(bool value)
        {
            SoundManager.Instance.PlayButtonSound();
            SoundManager.Instance.SetSfx(value);
        }

        private void OnBgmToggleChange(bool value)
        {
            SoundManager.Instance.PlayButtonSound();
            SoundManager.Instance.SetBgm(value);
        }

        private void OnVibrateToggleChange(bool value)
        {
            SoundManager.Instance.PlayButtonSound();
            PlayerPrefs.SetInt("vibration", value?0:-1);
            PlayerPrefs.Save();
        }
        
    }
}