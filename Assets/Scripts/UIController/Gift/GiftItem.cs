using System.Collections.Generic;
using DG.Tweening;
using Scriptable;
using Sound;
using ThirdParties.Truongtv;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UserDataModel;

namespace UIController.Gift
{
    public class GiftItem : MonoBehaviour
    {
        [SerializeField] private Image bg, front, door1, door2;
        [SerializeField] private Sprite normalBg, normalFront, normalDoor1, normalDoor2;
        [SerializeField] private Sprite specialBg, specialFront, specialDoor1, specialDoor2;
        [SerializeField] private GameObject specialEffect,claimedObj;
        [SerializeField] private TextMeshProUGUI numberText;
        [SerializeField] private SkeletonGraphicController red, blue;
        [SerializeField] private Button watchAdButton, claimButton;
        private GiftInfo _item;
        public void Init(GiftInfo info,bool firstOpen = false)
        {
            _item = info;
            var unlockProgress = UserDataController.GetUnlockProgress(_item.skinName);
            numberText.text = unlockProgress + "/" + _item.unlockValue;
            if (UserDataController.IsSkinUnlock(_item.skinName))
            {
                claimButton.gameObject.SetActive(false);
                watchAdButton.gameObject.SetActive(false);
                claimedObj.SetActive(true);
            }
            else
            {
                if (unlockProgress == _item.unlockValue)
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
            
            if (_item.isSpecial)
            {
                bg.sprite = specialBg;
                front.sprite = specialFront;
                door1.sprite = specialDoor1;
                door2.sprite = specialDoor2;
                specialEffect.SetActive(true);
            }
            else
            {
                bg.sprite = normalBg;
                front.sprite = normalFront;
                door1.sprite = normalDoor1;
                door2.sprite = normalDoor2;
                specialEffect.SetActive(false);
            }
            red.InitSkin(_item.skinName+"_1");
            blue.InitSkin(_item.skinName+"_2");
            if (_item.skinName == "10" || _item.skinName == "12" || _item.skinName == "19" || _item.skinName == "20")
            {
                red.PlayAnim("ingame_premium_" + _item.skinName, 1, true);
                blue.PlayAnim("ingame_premium_" + _item.skinName, 1, true);
            }
            watchAdButton.onClick.RemoveAllListeners();
            claimButton.onClick.RemoveAllListeners();
            watchAdButton.onClick.AddListener(WatchAd);
            claimButton.onClick.AddListener(Claim);
            if (firstOpen)
            {
                door1.gameObject.SetActive(true);
                door2.gameObject.SetActive(true);
            }
            else
            {
                door1.gameObject.SetActive(false);
                door2.gameObject.SetActive(false);
            }
        }

        public void OpenDoor()
        {
            door1.gameObject.SetActive(true);
            door2.gameObject.SetActive(true);
            door1.transform.DOLocalMoveX(-225, 1f);
            door2.transform.DOLocalMoveX(275, 1f);
        }
        private void WatchAd()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            GameServiceManager.Instance.adManager.ShowRewardedAd("menu_gift_popup_unlock_skin", () =>
            {
                UserDataController.UnlockProgress(_item.skinName);
                var unlockProgress = UserDataController.GetUnlockProgress(_item.skinName);
                numberText.text = unlockProgress + "/" + _item.unlockValue;
                if (UserDataController.GetUnlockProgress(_item.skinName) == _item.unlockValue)
                {
                    UserDataController.UnlockSkin(_item.skinName);
                    UserDataController.UpdateSelectedSkin(_item.skinName);
                    MenuPopupController.Instance.ShowUnlockSkinsPopup(new List<string>{_item.skinName});
                }
            });
        }

        private void Claim()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            UserDataController.UnlockSkin(_item.skinName);
            UserDataController.UpdateSelectedSkin(_item.skinName);
            MenuPopupController.Instance.ShowUnlockSkinsPopup(new List<string>{_item.skinName});
        }
    }
}