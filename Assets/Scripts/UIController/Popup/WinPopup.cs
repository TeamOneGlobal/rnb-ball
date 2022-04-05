using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MEC;
using Sound;
using ThirdParties.Truongtv;
using TMPro;
using Truongtv.PopUpController;
using Truongtv.SoundManager;
using UIController.Scene;
using UnityEngine;
using UnityEngine.UI;
using UserDataModel;
using Random = UnityEngine.Random;

namespace UIController.Popup
{
    public class WinPopup : BasePopup
    {
        [SerializeField] private TextMeshProUGUI levelText, nextLevelText, doubleCoinText;
        [SerializeField] private Button homeButton, x2CoinButton, nextLevelButton;

        [SerializeField] private GameObject ribbon;
        [SerializeField] private GameObject firework;
        private bool _isInit;
        private LastLevelInfo _lastLevel;

        public void Initialized()
        {
            _lastLevel = UserDataController.GetLastLevelData();
            levelText.text = "Level " + _lastLevel.lastLevelWin;
            doubleCoinText.text = $"{(_lastLevel.lastCoin +(_lastLevel.lastLevelWin == UserDataController.GetCurrentLevel()? Config.WIN_REWARD_COIN:0)) * 2}";

            RegisterEvent();
            Timing.RunCoroutine(SpawnFireWork().CancelWith(gameObject));

            IncreaseCoinEffect();
        }

        private void UpdateDb()
        {
            var maxLevel = UserDataController.UpdateLevel(_lastLevel.lastLevelWin);
            //FirebaseLogEvent.SerUserMaxLevel(maxLevel);
            UserDataController.ClearPreviousLevelData();
        }

        private void RegisterEvent()
        {
            if (_isInit) return;
            _isInit = true;
            openAction = () =>
            {
                SoundMenuController.Instance.PlayPopupSound();
                SoundMenuController.Instance.PlayWinSound();
                ribbon.transform.localScale = Vector3.zero;
                x2CoinButton.transform.localScale = Vector3.zero;
                nextLevelButton.transform.localScale = Vector3.zero;
                nextLevelButton.gameObject.SetActive(true);
                ribbon.transform.DOScale(Vector3.one, 0.5f);
                x2CoinButton.transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
                {
                    x2CoinButton.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.5f).SetEase(Ease.Linear)
                        .SetLoops(-1, LoopType.Yoyo);
                });
                nextLevelButton.transform.DOScale(Vector3.one, 0.5f).SetDelay(2.5f).SetUpdate(UpdateType.Normal, true);
            };
            closeAction = () =>
            {
                // TODO: add coin effect

                MenuScene.Instance.ShowBottomRight();
            };

            openCompleteAction = () => { };
            homeButton.onClick.AddListener(() =>
            {
                SoundMenuController.Instance.Pause(false);
                Close();
            });
            homeButton.onClick.AddListener(SoundMenuController.Instance.PlayButtonClickSound);
            x2CoinButton.onClick.AddListener(DoubleCoin);
            x2CoinButton.onClick.AddListener(SoundMenuController.Instance.PlayButtonClickSound);
            nextLevelButton.onClick.AddListener(NextLevel);
            nextLevelButton.onClick.AddListener(SoundMenuController.Instance.PlayButtonClickSound);
        }


        private async void IncreaseCoinEffect()
        {
            var currentLevel = UserDataController.GetCurrentLevel();
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            UpdateDb();
            nextLevelText.text = "LEVEL " + UserDataController.GetCurrentLevel();
            CoinController.Instance.IncreaseCoin(null, _lastLevel.lastCoin+ ((currentLevel+1)== UserDataController.GetCurrentLevel()?Config.WIN_REWARD_COIN:0));
            await UniTask.Delay(TimeSpan.FromSeconds(1.5f));
        }

        private bool _isSpawn = true;

        private IEnumerator<float> SpawnFireWork()
        {
            var count = 0;
            while (_isSpawn)
            {
                if (gameObject == null || !gameObject.activeSelf) break;
                yield return Timing.WaitForSeconds(Random.Range(0.2f, 1f));
                if (!gameObject.activeSelf) break;
                var obj = Instantiate(firework);
                count++;
                obj.transform.position = new Vector3(Random.Range(-10f, 10f), Random.Range(2f, 6f), 100);
                if (count <= 5)
                {
                    obj.GetComponent<SimpleAudio>().Play().Forget();
                }
            }
        }

        private void OnDestroy()
        {
            _isSpawn = false;
        }

        private void DoubleCoin()
        {
            
            GameServiceManager.Instance.adManager.ShowRewardedAd("menu_double_coin", () =>
            {
                x2CoinButton.interactable = false;
                x2CoinButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                Transform transform1;
                (transform1 = x2CoinButton.transform).DOKill();
                transform1.localScale = Vector3.one;
                UserDataController.UpdateCoin(_lastLevel.lastCoin);
                CoinController.Instance.IncreaseCoin(x2CoinButton.transform,
                    _lastLevel.lastCoin + Config.WIN_REWARD_COIN);
            });
        }

        private void NextLevel()
        {
            LoadSceneController.LoadLevel(UserDataController.GetCurrentLevel());
        }
    }
}