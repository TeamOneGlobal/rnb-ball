using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GamePlay;
using Sound;
using ThirdParties.Truongtv;
using TMPro;
using Truongtv.PopUpController;
using UnityEngine;
using UnityEngine.UI;

namespace UIController.Popup
{
    public class LosePopup : BasePopup
    {
        [SerializeField] private Transform container;
        [SerializeField] private Button closeButton, replayButton, addLifeButton;
        [SerializeField] private CanvasGroup countTimeObject, loseObject;
        [SerializeField] private TextMeshProUGUI countDownText,addLifeText;
        private const int CountDownTime = 3;
        private int _remainTime;
        private bool _isInit;

        public void Init()
        {
            RegisterEvent();
            _remainTime = CountDownTime;
            replayButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
            replayButton.interactable = false;
            countTimeObject.alpha = 0f;
            loseObject.alpha = 0f;
            addLifeText.text = "+" + Config.REWARDED_FREE_LIFE;
        }

        private void RegisterEvent()
        {
            if(_isInit) return;
            openAction = OnStart;
            closeAction = OnClose;
            openCompleteAction = OnStartComplete;
            closeButton.onClick.AddListener(Home);
            replayButton.onClick.AddListener(RestartLevel);
            addLifeButton.onClick.AddListener(AddLife);
            closeButton.onClick.AddListener(SoundGamePlayController.Instance.PlayButtonClickSound);
            replayButton.onClick.AddListener(SoundGamePlayController.Instance.PlayButtonClickSound);
            addLifeButton.onClick.AddListener(SoundGamePlayController.Instance.PlayButtonClickSound);
            _isInit = true;
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
        }

        private async void OnStartComplete()
        {
            countTimeObject.DOFade(1, 0.25f).SetEase(Ease.Linear).SetUpdate(UpdateType.Normal, true);
            for (var i = _remainTime; i >=0;i--)
            {
                countDownText.text = $"{i}";
                await UniTask.Delay(TimeSpan.FromSeconds(1f),true);
            }
            countTimeObject.DOFade(0, 0.25f).SetEase(Ease.Linear).SetUpdate(UpdateType.Normal, true);
            loseObject.DOFade(1, 0.25f).SetEase(Ease.Linear).SetUpdate(UpdateType.Normal, true);
            replayButton.interactable = true;
            replayButton.GetComponent<Image>().DOFade(1, 0.35f).SetEase(Ease.Linear).SetUpdate(UpdateType.Normal, true);
        }
        private void Home()
        {
            GameServiceManager.Instance.adManager.ShowInterstitialAd(() =>
            {
                GamePlayController.Instance.LogicalResume();
                LoadSceneController.LoadMenu();
            });
            
        }
        private void RestartLevel()
        {
            GameServiceManager.Instance.adManager.ShowInterstitialAd(() =>
            {
                GamePlayController.Instance.LogicalResume();
                LoadSceneController.LoadLevel(GamePlayController.Instance.level);
            });
            
        }

        private void AddLife()
        {
            GameServiceManager.Instance.logEventManager.LogEvent("in_game_reward_click",new Dictionary<string, object>
            {
                {"reward_for","revive"},
                {"level","lv_"+GamePlayController.Instance.level}
            });
            GameServiceManager.Instance.adManager.ShowRewardedAd("in_game_revive", () =>
            {
                GameServiceManager.Instance.logEventManager.LogEvent("in_game_reward_finish",new Dictionary<string, object>
                {
                    {"reward_for","revive"},
                    {"level","lv_"+GamePlayController.Instance.level}
                });
                GamePlayController.Instance.LogicalResume();
                GamePlayController.Instance.SetCharacterRevive();
                SoundGamePlayController.Instance.ResumeBgm();
                Close();
            });
        }
    }
}