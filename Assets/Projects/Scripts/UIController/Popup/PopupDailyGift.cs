using System;
using System.Collections.Generic;
using DG.Tweening;
using Projects.Scripts.Data;
using ThirdParties.Truongtv;
using ThirdParties.Truongtv.AdsManager;
using ThirdParties.Truongtv.Notification;
using ThirdParties.Truongtv.SoundManager;
using TMPro;
using Truongtv.PopUpController;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Scripts.UIController.Popup
{
    public class PopupDailyGift : BasePopup
    {
        [SerializeField] private Transform container;
        [SerializeField] private List<DailyComponent> _listDay;
        [SerializeField] private DailyGiftData _dailyGiftData;
        [SerializeField] private Button _claimButton, _claimX2Button, _closeButton;
        [SerializeField] private TextMeshProUGUI _countText;
       
        
        private DateTime lastClaimedTime;
        private TimeSpan remainTime;
        private bool isInit;
        public void Inititialized()
        {
            GameServiceManager.Instance.adManager.HideBanner();
            isInit = false;
            RegisterEvent();
            openAction = OnStart;
            closeAction = OnClose;
            lastClaimedTime = GameDataManager.Instance.GetLastClaimedTime();
            if (IsDailyGiftReady())
            {
                _countText.gameObject.SetActive(false);
                _claimButton.gameObject.SetActive(true);
                _claimX2Button.gameObject.SetActive(true);
            }
            var day = GameDataManager.Instance.GetDailyIndex();
            var dayClaimed = GameDataManager.Instance.GetClaimedDays();
            if (DateTime.UtcNow.Subtract(lastClaimedTime).TotalDays > 1 && dayClaimed.Contains(day))
            {
                GameDataManager.Instance.SetNextDailyGift();
                day++;
            }
            for (int i = 1; i <= _listDay.Count; i++)
            {
                if (i < day)
                {
                    if (dayClaimed.Contains(i))
                    {
                        _listDay[i - 1].Init(_dailyGiftData.data[i - 1],DailyComponent.DailyGiftStatus.Claimed);
                    }
                    else
                    {
                        _listDay[i - 1].Init(_dailyGiftData.data[i - 1],DailyComponent.DailyGiftStatus.NotClaimed);
                    }
                }
                else if (i == day)
                {
                    if (dayClaimed.Contains(i))
                    {
                        _listDay[i - 1].Init(_dailyGiftData.data[i - 1],DailyComponent.DailyGiftStatus.Claimed,true);
                        _claimButton.gameObject.SetActive(false);
                        _claimX2Button.gameObject.SetActive(false);
                        _countText.gameObject.SetActive(true);
                    }
                    else
                    {
                        _listDay[i - 1].Init(_dailyGiftData.data[i - 1],DailyComponent.DailyGiftStatus.Ready,true);
                        _claimButton.gameObject.SetActive(true);
                        _claimX2Button.gameObject.SetActive(!_dailyGiftData.data[i - 1].itemData.Exists(_ => !string.IsNullOrEmpty(_.skinName)));
                        _countText.gameObject.SetActive(false);
                    }
                }
                else
                {
                    _listDay[i - 1].Init(_dailyGiftData.data[i - 1],DailyComponent.DailyGiftStatus.Ready);
                }
            }
            isInit = true;
        }

        public bool IsInit
        {
            get => isInit;
        }

        public bool IsDailyGiftReady()
        {
            if (GetRemainTime().Ticks <= 0)
            {
                return true;
            }
            return false;
        }
        
        private TimeSpan GetRemainTime()
        {
            remainTime = TimeSpan.FromSeconds(lastClaimedTime.Date.AddDays(1).Subtract(DateTime.UtcNow).TotalSeconds);
            return remainTime;
        }
        
        private void FixedUpdate()
        {
            if (!_countText.gameObject.activeSelf)
            {
                return;
            }
            
            if (GetRemainTime().Ticks < 0)
            {
                _countText.gameObject.SetActive(false);
                _claimButton.gameObject.SetActive(true);
                _claimX2Button.gameObject.SetActive(true);
                return;
            }
            
            _countText.text = GetRemainTime().ToString(@"hh\:mm\:ss");
        }

        void RegisterEvent()
        {
            _closeButton.onClick.AddListener(() =>
            {
                GameServiceManager.Instance.adManager.ShowBanner();
                SoundManager.Instance.PlayButtonSound();
                Close();
            });
            _claimButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayButtonSound();
                if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    PopupController.Instance.ShowNoInternet();
                    return;
                }
                ClaimReward();
            });
            _claimX2Button.onClick.AddListener(OnWatchAd);
        }

        void ClaimReward(bool x2 = false)
        {
            var day = GameDataManager.Instance.GetDailyIndex();
            GameDataManager.Instance.ClaimDailyReward(_dailyGiftData.data[day - 1],x2);
            
            if (_dailyGiftData.data[day - 1].itemData.Count > 1)
            {
                for (int i = 0; i < _dailyGiftData.data[day - 1].itemData.Count; i++)
                {
                    Claim(_dailyGiftData.data[day - 1],i,x2);
                }
            }
            else
            {
                Claim(_dailyGiftData.data[day - 1],0,x2);
            }
            // _particleGold.transform.position = _listDay[day - 1].transform.position;
            // _particleHeart.transform.position = _listDay[day - 1].transform.position;
            var item = _dailyGiftData.data[day - 1].itemData;
            if (item.Count > 1)
            {
                foreach (var value in item)
                {
                    if (value.lifeValue != 0)
                    {
                        // _particleHeart.gameObject.SetActive(true);
                        // _particleHeart.Play(0.5f);
                    }
                    
                    if (value.coinValue != 0)
                    {
                        // _particleGold.gameObject.SetActive(true);
                        // _particleGold.Play(0.5f);
                    }
                }
            }
            else
            {
                if (item[0].coinValue != 0)
                {
                    // _particleGold.gameObject.SetActive(true);
                    // _particleGold.Play(0.5f);
                }

                if (item[0].lifeValue != 0)
                {
                    // _particleHeart.gameObject.SetActive(true);
                    // _particleHeart.Play(0.5f);
                }
            }
            lastClaimedTime = GameDataManager.Instance.GetLastClaimedTime();
            _listDay[day - 1].SetClaimed();
            _claimButton.gameObject.SetActive(false);
            _claimX2Button.gameObject.SetActive(false);
            _countText.gameObject.SetActive(true);
        }
        void Claim(DailyGiftItem data,int i, bool x)
        {
            if (data.itemData[i].coinValue > 0)
            {
                if (MenuController.Instance)
                {
                    MenuController.Instance.UpdateCoin(x ? 2*data.itemData[i].coinValue : data.itemData[i].coinValue,x?_claimX2Button.transform:_claimButton.transform);
                }
            }

            if (data.itemData[i].lifeValue > 0)
            {
                if (MenuController.Instance)
                {
                    MenuController.Instance.UpdateLife(x ? 2*data.itemData[i].lifeValue : data.itemData[i].lifeValue,x?_claimX2Button.transform:_claimButton.transform);
                }
            }
            if (!string.IsNullOrEmpty(data.itemData[i].skinName))
            {
                PopupMenuController.Instance.OpenPopupReceiveSkin(data.itemData[i].skinName,false);
            }
            MobileNotification.Instance.DailyRewardResetReminder(false);
        }
            
       
        void OnWatchAd()
        {
            SoundManager.Instance.PlayButtonSound();
            GameServiceManager.Instance.adManager.ShowRewardedAd("daily_gift_x2reward", () =>
            {
                ClaimReward(true);
            });
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
            container.DOScale(0, DURATION).SetUpdate(UpdateType.Normal, true).SetEase(Ease.InBack);
        }
    }
}