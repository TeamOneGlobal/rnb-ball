using System;
using Scriptable;
using TMPro;
using UIController.Popup;
using UnityEngine;
using UnityEngine.UI;
using UserDataModel;

namespace UIController.DailyGift
{
    public class Day7 : MonoBehaviour
    {
        [SerializeField] private GameObject claimedObj, claimButtonObj;
        [SerializeField] private Button claimButton;
        [SerializeField] private TextMeshProUGUI coinValue, lifeValue;
        [SerializeField] private SkeletonGraphicController red, blue;
        private Scriptable.DailyGift _dailyGift;
        private Action _claimAction;

        public void Init(Scriptable.DailyGift dailyGift,DailyGiftStatus status,Action claim = null)
        {
            _dailyGift = dailyGift;
            _claimAction = claim;
            InitStatus(status);
            claimButton.onClick.RemoveAllListeners();
            claimButton.onClick.AddListener(ClaimReward);
            int totalCoin = 0;
            int totalLife = 0;
            int skin = 0;
            for (var i = 0; i < _dailyGift.gifts.Count; i++)
            {
                var item = _dailyGift.gifts[i];
                switch (item.type)
                {
                    case ItemType.Coin:
                        totalCoin += item.value;

                        break;
                    case ItemType.Heart:
                        totalLife += item.value;

                        break;
                    case ItemType.Skin:
                        skin = item.value;

                        break;
                }

                coinValue.text = $"{totalCoin}";
                lifeValue.text = $"{totalLife}";
                red.InitSkin(skin+"_1");
                blue.InitSkin(skin+"_2");
            }
        }
        private void ClaimReward()
        {
            int totalCoin = 0;
            int totalLife = 0;
            int skin = 0;
            for (var i = 0; i < _dailyGift.gifts.Count; i++)
            {
                var item = _dailyGift.gifts[i];
                switch (item.type)
                {
                    case ItemType.Coin:
                        totalCoin += item.value;

                        break;
                    case ItemType.Heart:
                        totalLife += item.value;

                        break;
                    case ItemType.Skin:
                        skin = item.value;

                        break;
                }
            }
            UserDataController.ClaimReward(7);
            _claimAction?.Invoke();
            MenuPopupController.Instance.ShowCongratsPopup(skin.ToString());
            MenuPopupController.Instance.ShowPurchaseSuccessDoubleValue(PurchaseSuccessPopup.PurchaseType.FreeCoin,
                totalCoin,
                () =>
                {
                    MenuPopupController.Instance.ShowPurchaseSuccessDoubleValue(
                        PurchaseSuccessPopup.PurchaseType.FreeLife, totalLife, () =>
                        {
                            
                        });
                });
            InitStatus(DailyGiftStatus.Claimed);
        }

        private void InitStatus(DailyGiftStatus status)
        {
            switch (status)
            {
                case DailyGiftStatus.Wait:
                    claimedObj.SetActive(false);
                    claimButtonObj.SetActive(false);
                    claimButton.interactable = false;
                    break;
                case DailyGiftStatus.Ready:
                    claimedObj.SetActive(false);
                    claimButtonObj.SetActive(true);
                    claimButton.interactable = true;
                    break;
                case DailyGiftStatus.Claimed:
                    claimedObj.SetActive(true);
                    claimButtonObj.SetActive(false);
                    claimButton.interactable = false;
                    break;
            }
        }
    }
}