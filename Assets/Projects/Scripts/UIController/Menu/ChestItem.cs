using Projects.Scripts.UIController.Popup;
using Spine.Unity;
using ThirdParties.Truongtv;
using ThirdParties.Truongtv.AdsManager;
using ThirdParties.Truongtv.SoundManager;
using TMPro;
using Truongtv.PopUpController;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Scripts.UIController.Menu
{
    public class ChestItem : MonoBehaviour
    {
        [SerializeField] private Button freeOpenButton, openByAdButton;
        [SerializeField] private GameObject openedObj,priceObj, coinObj;
        [SerializeField] private SkeletonGraphic ball,chest;
        [SerializeField] private TextMeshProUGUI coinValue;
        [SerializeField, SpineAnimation] private string openAnim;
        private PopupLottery _lottery;
        private bool _opened;
        
        public void Init(PopupLottery lottery)
        {
            _lottery = lottery;
            freeOpenButton.onClick.AddListener(FreeOpen);
            openByAdButton.onClick.AddListener(OpenByAd);
        }

        private void FreeOpen()
        {
            SoundManager.Instance.PlayButtonSound();
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                PopupController.Instance.ShowNoInternet();
                return;
            }
            Open();
        }

        private void OpenByAd()
        {
            SoundManager.Instance.PlayButtonSound();
            GameServiceManager.Instance.adManager.ShowRewardedAd("lottery_open", Open);
        }

        private void Open()
        {
            var item = _lottery.GetItemData();
            _lottery.OnChestOpen(this);
            _opened = true;
            freeOpenButton.gameObject.SetActive(false);
            openByAdButton.gameObject.SetActive(false);
            openedObj.SetActive(true);

            var entry =  chest.AnimationState.SetAnimation(0, openAnim, false);
            entry.Complete += trackEntry =>
            {
                if (item.coinValue > 0)
                {
                    coinObj.SetActive(true);
                    ball.gameObject.SetActive(false);
                    coinValue.text = $"{item.coinValue}";
                    priceObj.gameObject.SetActive(true);
                    MenuController.Instance.UpdateCoin(item.coinValue);
                }
                else
                {
                    ball.initialSkinName = item.skinName;
                    ball.Initialize(true);
                    PopupMenuController.Instance.OpenPopupReceiveSkin(item.skinName);
                    priceObj.gameObject.SetActive(true);
                    coinObj.SetActive(false);
                }
            }; 
        }
        public void SetOpenByAd()
        {
            if(_opened) return;
            freeOpenButton.gameObject.SetActive(false);
            openByAdButton.gameObject.SetActive(true);
            openedObj.SetActive(false);
        }
    }
}