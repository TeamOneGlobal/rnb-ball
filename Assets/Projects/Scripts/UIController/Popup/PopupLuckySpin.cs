using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Projects.Scripts.Data;
using Projects.Scripts.GamePlay.Sound;
using Projects.Scripts.Models;
using Projects.Scripts.UIController.Menu;
using Sirenix.OdinInspector;
using ThirdParties.Truongtv;
using ThirdParties.Truongtv.AdsManager;
using ThirdParties.Truongtv.Notification;
using ThirdParties.Truongtv.SoundManager;
using TMPro;
using Truongtv.PopUpController;
using Truongtv.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Projects.Scripts.UIController.Popup
{
    public class PopupLuckySpin : BasePopup
    {
        [SerializeField] private Transform container;
        [SerializeField] private LuckySpinData data;
        [SerializeField] private SpinItem prefab;
        [SerializeField] private List<SpinItem> itemList;
        [SerializeField] private Button closeButton, adSpinButton, freeSpinButton;
        [SerializeField] private Transform rollObj;
        [SerializeField] private TextMeshProUGUI timeRemain;
        [SerializeField] private BallMenuController red, blue;
        private bool _isInit;
      
        public void Initialized()
        {
            RegisterEvent();
            
            rollObj.RemoveAllChild();
            itemList = new List<SpinItem>();
            
            var skinNotUnlock = GameDataManager.Instance.skinData.GetNormalSkin().Select(a => a.skinName)
                .Except(GameDataManager.Instance.GetUnlockedSkin()).ToList();
            
            for (var i = 0; i < data.spinData.Count; i++)
            {
                var itemData = data.spinData[i];
                if (itemData.isSkin)
                {
                    if (skinNotUnlock.Count > 0)
                    {
                        var r = Random.Range(0, skinNotUnlock.Count);
                        itemData.itemData.skinName = skinNotUnlock[r];
                        skinNotUnlock.RemoveAt(r);
                    }
                    else
                    {
                        var temp = new SpinItemData {itemData = new ItemData {coinValue = 2000}, isSkin = true};
                        itemData = temp;
                        //itemData.itemData.coinValue = 2000;
                    }
                }

                var item = Instantiate(prefab, rollObj);
                var transform1 = item.transform;
                transform1.localPosition = Vector3.zero;
                transform1.localScale = Vector3.one;
                transform1.localEulerAngles = new Vector3(0, 0, 67.5f + i * 45);
                item.Init(itemData);
                itemList.Add(item);
            }
            closeAction = () =>
            {
                StopAllCoroutines();
                OnClose();
            };
            openAction = () =>
            {
                OnStart();
                if (!GameDataManager.Instance.IsSpinAvailable()) // no free spin, show countdown
                {
                    SetOnCountDown();
                }
                else
                {
                    SetFreeSpinAvailable();
                }
            };
        }

        private void SetOnCountDown()
        {
            freeSpinButton.gameObject.SetActive(false);
            adSpinButton.gameObject.SetActive(true);
            timeRemain.gameObject.SetActive(true);
            StartCoroutine(CountDown());
        }

        private void SetFreeSpinAvailable()
        {
            timeRemain.gameObject.SetActive(false);
            freeSpinButton.gameObject.SetActive(true);
            adSpinButton.gameObject.SetActive(false);
        }

        private IEnumerator CountDown()
        {
            var lastTime = GameDataManager.Instance.GetLastSpinTime();
            int totalSecond;
            do
            {
                var totalWaitSecond = Mathf.FloorToInt((float) DateTime.Now.Subtract(lastTime).TotalSeconds);
                totalSecond = Mathf.FloorToInt((float) TimeSpan
                    .FromHours(GameDataManager.Instance.freeSpinCooldownHours)
                    .Subtract(TimeSpan.FromSeconds(totalWaitSecond)).TotalSeconds);
                timeRemain.text = TimeSpan.FromSeconds(totalSecond).ToString(@"hh\:mm\:ss");
                if (totalSecond <= 0)
                {
                    break;
                }

                yield return new WaitForSeconds(1f);
            } while (totalSecond > 0);

            SetFreeSpinAvailable();
        }

        private void RegisterEvent()
        {
            if (_isInit) return;
            _isInit = true;
            closeButton.onClick.AddListener(OnCloseButtonClick);
            adSpinButton.onClick.AddListener(OnAdSpinButtonClick);
            freeSpinButton.onClick.AddListener(OnFreeSpinButtonClick);
        }

        private void UpdateTimeRemain()
        {
        }

        private void OnCloseButtonClick()
        {
            SoundManager.Instance.PlayButtonSound();
            Close();
        }

        private void OnAdSpinButtonClick()
        {
            SoundManager.Instance.PlayButtonSound();
            GameServiceManager.Instance.adManager.ShowRewardedAd("Rewarded_Spin", () =>
            {
                adSpinButton.interactable = false;
                Spin();
            });
        }

        private void OnFreeSpinButtonClick()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                PopupController.Instance.ShowNoInternet();
                return;
            }
            adSpinButton.interactable = false;
            GameDataManager.Instance.Spin();
            Spin();
            SetOnCountDown();
            // LuckySpinButton.Instance.CheckTime();
            MobileNotification.Instance.SetLuckySpinReminder();
        }

        private void Spin()
        {
            int currentIndex = 0;
            if (GameDataManager.Instance.IsFirstSpin()) // quay lần đầu tặng skin free
            {
                var itemData = data.spinData.Find(a => a.isFirstReward);
                var index = data.spinData.IndexOf(itemData);
                currentIndex = index;
            }
            else
            {
                var randomIndex = Random.Range(0, data.spinData.Count);
                currentIndex = randomIndex;
            }

            var angle = -currentIndex * 45 - 3600 + 22.5f;
            var tempAngle = rollObj.transform.localEulerAngles.z;

            rollObj.DOLocalRotate(new Vector3(0, 0, angle), 5f, RotateMode.FastBeyond360)
                .SetRelative(false)
                .SetEase(Ease.InOutCubic)
                .OnUpdate(() =>
                {
                    var delta = tempAngle - rollObj.transform.localEulerAngles.z;
                    if (delta <= -45f)
                    {
                        tempAngle = rollObj.transform.localEulerAngles.z;
                        SoundMenuManager.Instance.PlaySpinSound();
                    }
                    else if (delta >= 45f)
                    {
                        tempAngle = rollObj.transform.localEulerAngles.z;
                        SoundMenuManager.Instance.PlaySpinSound();
                    }
                })
                .OnComplete(() => ShowResult(currentIndex));
        }

        private void ShowResult(int index)
        {
            adSpinButton.interactable = true;
            var spinItem = data.spinData[index];
            if (!spinItem.itemData.skinName.Equals(string.Empty))
                PopupMenuController.Instance.OpenPopupReceiveSkin(spinItem.itemData.skinName,true);
            else
                PopupMenuController.Instance.OpenPopupDoubleReward(spinItem.itemData);
        }
        private void OnStart()
        {
            container.localScale = Vector3.zero;
            SoundManager.Instance.PlayPopupOpenSound();
            container.DOScale(1, DURATION).SetUpdate(UpdateType.Normal, true).SetEase(Ease.OutBack);
            red.PlayRandomMix();
            blue.PlayRandomMix();
        }

        private void OnClose()
        {
            SoundManager.Instance.PlayPopupCloseSound();
            container.DOScale(0, DURATION).SetUpdate(UpdateType.Normal, true).SetEase(Ease.InBack);
        }
    }
}