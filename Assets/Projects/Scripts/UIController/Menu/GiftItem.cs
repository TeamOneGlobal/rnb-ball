using System.Collections.Generic;
using DG.Tweening;
using Projects.Scripts.UIController.Popup;
using Spine.Unity;
using ThirdParties.Truongtv;
using ThirdParties.Truongtv.AdsManager;
using ThirdParties.Truongtv.SoundManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Scripts.UIController.Menu
{
    public class GiftItem : MonoBehaviour
    {
        [SerializeField] private Image bg, door;
        [SerializeField] private Sprite normalBg, normalDoor;
        [SerializeField] private Sprite specialBg, specialDoor;
        [SerializeField] private GameObject specialEffect,claimedObj;
        [SerializeField] private TextMeshProUGUI numberText;
        [SerializeField] private BallMenuController red,blue;
        [SerializeField] private Button watchAdButton, claimButton;
        private PopupGift _controller;
        private string _item;
        private int unlockValue = 2;
        public void Init(string skin,PopupGift controller,bool firstOpen = false)
        {
            _controller = controller;
            _item = skin;
            
            if (GameDataManager.Instance.IsPremiumSkin(skin))
            {
                bg.sprite = specialBg;
                door.sprite = specialDoor;
                specialEffect.SetActive(true);
                unlockValue = 12;
            }
            else
            {
                bg.sprite = normalBg;
                door.sprite = normalDoor;
                specialEffect.SetActive(false);
                unlockValue = 6;
            }
            if (firstOpen)
            {
                door.gameObject.SetActive(true);
            }
            else
            {
                door.gameObject.SetActive(false);
            }
            red.ApplySkin(_item+"_1");
            red.PlayRandomMix();
            blue.ApplySkin(_item+"_2");
            blue.PlayRandomMix();
            UpdateValue();
            watchAdButton.onClick.RemoveAllListeners();
            claimButton.onClick.RemoveAllListeners();
            watchAdButton.onClick.AddListener(WatchAd);
            claimButton.onClick.AddListener(Claim);
            
        }
        //
        public void OpenDoor()
        {
            door.transform.DOLocalMoveY(370, 1f).OnComplete(() => door.gameObject.SetActive(false));
        }

        public void UpdateValue()
        {
            var unlockProgress = GameDataManager.Instance.GetAdProgress();
            numberText.text = unlockProgress + "/" + unlockValue;
            if (GameDataManager.Instance.IsSkinUnlock(_item))
            {
                claimButton.gameObject.SetActive(false);
                watchAdButton.gameObject.SetActive(false);
                numberText.gameObject.SetActive(false);
                claimedObj.SetActive(true);
            }
            else
            {
                numberText.gameObject.SetActive(true);
                if (unlockProgress >= unlockValue)
                {
                    claimButton.gameObject.SetActive(true);
                    watchAdButton.gameObject.SetActive(false);
                    claimedObj.SetActive(false);
                }
                else
                {
                    claimButton.gameObject.SetActive(false);
                    watchAdButton.gameObject.SetActive(true);
                    claimedObj.SetActive(false);
                }
            }
        }
        
        private void WatchAd()
        {
            SoundManager.Instance.PlayButtonSound();
            var adProgress = GameDataManager.Instance.GetAdProgress();
            GameServiceManager.Instance.adManager.ShowRewardedAd("rewarded_GiftPopup_UnlockSkin", () =>
            {
                adProgress++;
                GameDataManager.Instance.SetAdProgressCount(adProgress);
                _controller.onWatchAdOrClaimSkin?.Invoke();
            });
        }
        
        private void Claim()
        {
            SoundManager.Instance.PlayButtonSound();
            GameDataManager.Instance.UnlockSkin(_item);
            PopupMenuController.Instance.OpenPopupReceiveSkin(_item);
            var adProgress = GameDataManager.Instance.GetAdProgress();
            GameDataManager.Instance.SetAdProgressCount(adProgress - unlockValue);
            _controller.onWatchAdOrClaimSkin?.Invoke();
            UpdateValue();
        }
    }
}