using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GamePlay;
using Spine.Unity;
using ThirdParties.Truongtv;
using ThirdParties.Truongtv.AdsManager;
using ThirdParties.Truongtv.LogManager;
using ThirdParties.Truongtv.SoundManager;
using Truongtv.PopUpController;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Scripts.UIController.Popup
{
    public class PopupRevive : BasePopup
    {
        [SerializeField] private Transform container;
        [SerializeField] private GameObject adIcon;
        [SerializeField] private Sprite greenButtonSprite;
        [SerializeField] private SkeletonGraphic ball;
        [SerializeField] private Button watchAdButton,closeButton;
        [SerializeField] private bool isWatchAd = true;
        private Action _onClose, _onWatchAd;
        public void Initialized(Action onWatchAd, Action onClose, string skin)
        {
            
            _onClose = onClose;
            _onWatchAd = onWatchAd;
            closeButton.onClick.AddListener(OnClose);
            watchAdButton.onClick.AddListener(OnWatchAd);
            openAction += () =>
            {
                if (isWatchAd)
                {
                    adIcon.SetActive(true);
                    closeButton.gameObject.SetActive(false);
                }
                else
                {
                    adIcon.SetActive(false);
                    closeButton.gameObject.SetActive(true);
                    watchAdButton.GetComponentInChildren<Image>().sprite = greenButtonSprite;
                }
                OnStart();
            };
            openCompleteAction += () =>
            {
                if (isWatchAd)
                {
                    StartCoroutine(AllowClose());
                }
            };
            ball.initialSkinName = skin;
            ball.Initialize(true);
        }

        IEnumerator AllowClose()
        {
            yield return new WaitForSecondsRealtime(2);
            closeButton.gameObject.SetActive(true);
        }
        private void OnStart()
        {
            container.localScale = Vector3.zero;
            SoundManager.Instance.PlayPopupOpenSound();
            container.DOScale(1, DURATION).SetUpdate(UpdateType.Normal, true).SetEase(Ease.OutBack);
        }
        void OnClose()
        {
            closeCompleteAction = null;
            Close();
            _onClose?.Invoke();
        }
        
        void OnWatchAd()
        {
            if (isWatchAd)
            {
                GameServiceManager.Instance.logEventManager.LogEvent("count_ad_in_game_click", new Dictionary<string, object>
                {
                    {"level", "lv_" + GamePlayController.Instance.level},
                    {"type", "revive"}
                });
                GameServiceManager.Instance.adManager.ShowRewardedAd("in_game_revive", () =>
                {
                    GameServiceManager.Instance.logEventManager.LogEvent("count_ad_in_game_finish", new Dictionary<string, object>
                    {
                        {"level", "lv_" + GamePlayController.Instance.level},
                        {"type", "revive"}
                    });
                    if (!GameDataManager.Instance.showBannerInGame)
                        GameServiceManager.Instance.adManager.HideBanner();
                    Close();
                    _onWatchAd?.Invoke();
                });
            }
            else
            {
                if(!GameDataManager.Instance.showBannerInGame)
                    GameServiceManager.Instance.adManager.HideBanner();
                closeCompleteAction = null;
                Close();
                _onWatchAd?.Invoke();
            }
        }
    }
}