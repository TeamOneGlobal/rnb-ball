using System.Collections.Generic;
using DG.Tweening;
using Scriptable;
using Sound;
using Truongtv.PopUpController;
using UIController.DailyGift;
using UnityEngine;
using UnityEngine.UI;
using UserDataModel;

namespace UIController.Popup
{
    public class DailyGiftPopup : BasePopup
    {
        [SerializeField] private Transform container;
        [SerializeField] private List<DailyGiftItem> dailyGiftItems;
        [SerializeField] private Day7 day7;
        [SerializeField] private Button backButton;
        [SerializeField] private DailyGiftData data;
        private bool _isInit;
        public void Initialized()
        {
            RegisterEvent();
            var listReceived = UserDataController.GetDailyRewardReceivedDay();
            var day = UserDataController.GetDailyReward();
            for (var i = 0; i < dailyGiftItems.Count; i++)
            {
                if (i + 1 <= day)
                {
                    if (listReceived.Contains(i + 1))
                    {
                        dailyGiftItems[i].Init(data.data[i],DailyGiftStatus.Claimed,i+1);
                    }
                    else
                    {
                        dailyGiftItems[i].Init(data.data[i],DailyGiftStatus.Ready,i+1);
                    }
                    
                }
                else if (i + 1 > day)
                {
                    dailyGiftItems[i].Init(data.data[i],DailyGiftStatus.Wait,i+1);
                }
            }
            if (7 <= day)
            {
                if (listReceived.Contains(7))
                {
                    day7.Init(data.data[6],DailyGiftStatus.Claimed);
                }
                else
                {
                    day7.Init(data.data[6],DailyGiftStatus.Ready);
                }
                
            }
            else if (7 >= day)
            {
                day7.Init(data.data[6],DailyGiftStatus.Wait);
            }
        }
        private void RegisterEvent()
        {
            if(_isInit) return;
            _isInit = true;
            openAction = OnStart;
            closeAction = OnClose;
            backButton.onClick.AddListener(ClosePopUp);
            
            
        }
        private void OnStart()
        {
            container.localScale = Vector3.zero;
            SoundMenuController.Instance.PlayPopupSound();
            container.DOScale(1, DURATION).SetUpdate(UpdateType.Normal,true).SetEase(Ease.OutBack);
        }

        private void OnClose()
        {
            SoundMenuController.Instance.PlayPopupSound();
            container.DOScale(0, DURATION).SetUpdate(UpdateType.Normal,true).SetEase(Ease.InBack);
        }

        private void ClosePopUp()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            Close();
        }
    }
}