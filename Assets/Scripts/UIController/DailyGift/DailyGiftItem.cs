using System;
using Scriptable;
using ThirdParties.Truongtv;
using TMPro;
using UIController.Popup;
using UnityEngine;
using UnityEngine.UI;
using UserDataModel;

namespace UIController.DailyGift
{
    public class DailyGiftItem : MonoBehaviour
    {
        [SerializeField] private GameObject claimedObj, dayObj,claimButtonObj;
        [SerializeField] private Button claimButton;
        [SerializeField] private GameObject skinObj, coinObj, heartObj;
        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private SkeletonGraphicController red, blue;

        private Scriptable.DailyGift _dailyGift;
        private Action _claimAction;
        private int _index;
        public void Init(Scriptable.DailyGift dailyGift,DailyGiftStatus status,int index,Action claim = null)
        {
            _dailyGift = dailyGift;
            _index = index;
            _claimAction = claim;
            InitStatus(status);
            claimButton.onClick.RemoveAllListeners();
            claimButton.onClick.AddListener(ClaimReward);
            switch (_dailyGift.gifts[0].type)
            {
                case ItemType.Coin:
                    coinObj.SetActive(true);
                    heartObj.SetActive(false);
                    skinObj.SetActive(false);
                    valueText.text = $"{_dailyGift.gifts[0].value}";
                    break;
                case ItemType.Heart:
                    coinObj.SetActive(false);
                    heartObj.SetActive(true);
                    skinObj.SetActive(false);
                    valueText.text = $"{_dailyGift.gifts[0].value}";
                    break;
                case ItemType.Skin:
                    coinObj.SetActive(false);
                    heartObj.SetActive(false);
                    skinObj.SetActive(true);
                    valueText.text = string.Empty;
                    red.InitSkin(_dailyGift.gifts[0].value+"_1");
                    blue.InitSkin(_dailyGift.gifts[0].value+"_2");
                    break;
            }
        }

        private void ClaimReward()
        {
            _claimAction?.Invoke();
            UserDataController.ClaimReward(_index);
            for (var i = 0; i < _dailyGift.gifts.Count; i++)
            {
                var item = _dailyGift.gifts[i];
                switch (item.type)
                {
                    case ItemType.Coin:
                        MenuPopupController.Instance.ShowPurchaseSuccessDoubleValue(PurchaseSuccessPopup.PurchaseType.FreeCoin,item.value);
                        break;
                    case ItemType.Heart:
                        MenuPopupController.Instance.ShowPurchaseSuccessDoubleValue(PurchaseSuccessPopup.PurchaseType.FreeLife,item.value);
                        break;
                    case ItemType.Skin:
                        MenuPopupController.Instance.ShowCongratsPopup(item.value.ToString());
                        break;
                }
            }

            GameServiceManager.Instance.mobileNotification.DailyRewardResetReminder(false);
            InitStatus(DailyGiftStatus.Claimed);
        }

        private void InitStatus(DailyGiftStatus status)
        {
            Debug.Log(status);
            switch (status)
            {
                case DailyGiftStatus.Wait:
                    dayObj.SetActive(true);
                    claimedObj.SetActive(false);
                    claimButtonObj.SetActive(false);
                    claimButton.interactable = false;
                    break;
                case DailyGiftStatus.Ready:
                    dayObj.SetActive(false);
                    claimedObj.SetActive(false);
                    claimButtonObj.SetActive(true);
                    claimButton.interactable = true;
                    break;
                case DailyGiftStatus.Claimed:
                    dayObj.SetActive(false);
                    claimedObj.SetActive(true);
                    claimButtonObj.SetActive(false);
                    claimButton.interactable = false;
                    break;
            }
        }
        
    }

    public enum DailyGiftStatus
    {
        Wait,Ready,Claimed
    }
}