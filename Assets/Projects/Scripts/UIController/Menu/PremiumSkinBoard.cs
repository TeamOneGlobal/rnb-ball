using System;
using ThirdParties.Truongtv;
using ThirdParties.Truongtv.AdsManager;
using ThirdParties.Truongtv.SoundManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Scripts.UIController.Menu
{
    public class PremiumSkinBoard : SkinBoard
    {
        [SerializeField] private Button trySkinButton,unlockByAdButton;
        [SerializeField] private TextMeshProUGUI priceText;
        private bool forceOpen;
        private int AdUnlock = 3;
        private void Awake()
        {
            trySkinButton.onClick.AddListener(TrySkin);
            unlockByAdButton.onClick.AddListener(UnlockByAd);
        }

        public override void Init()
        {
            base.Init();
            InitItem(GameDataManager.Instance.IsSubscription());
            forceOpen = GameDataManager.Instance.IsSubscription();
           
        }

        private void OpenSubscription()
        {
            
        }

        private void TrySkin()
        {
            SoundManager.Instance.PlayButtonSound();
            GameServiceManager.Instance.adManager.ShowRewardedAd("shop_try_skin", () =>
            {
                GameDataManager.Instance.SetTrySkin(skinSelected.GetSkinName());
                SelectCurrentSkin();
            });
        }

        private void UnlockByAd()
        {
            SoundManager.Instance.PlayButtonSound();
            var adProgress = GameDataManager.Instance.GetAdUnlockSkin(skinSelected.GetSkinName());
            GameServiceManager.Instance.adManager.ShowRewardedAd("shop_premium_unlock", () =>
            {
                GameDataManager.Instance.SetAdUnlockSkin(skinSelected.GetSkinName());
                priceText.text = GameDataManager.Instance.GetAdUnlockSkin(skinSelected.GetSkinName()) + "/" +
                                 AdUnlock;
                if (GameDataManager.Instance.GetAdUnlockSkin(skinSelected.GetSkinName()) >= AdUnlock)
                {
                    // unlock skin and set select
                    PopupMenuController.Instance.OpenPopupReceiveSkin(skinSelected.GetSkinName(), false,() =>
                    {
                        skinSelected.Unlock();
                        skinSelected.SetSelected();
                        SelectCurrentSkin();
                    });
                }
            });
        }

        protected override void OnToggleOn(SkinItem skinItem)
        {
            base.OnToggleOn(skinItem);
            var skin = skinItem.GetSkinName();
            if (GameDataManager.Instance.IsSkinUnlock(skin)||forceOpen)
            {
                if (GameDataManager.Instance.GetCurrentSkin().Equals(skin))
                {
                    selected.SetActive(true);
                    selectButton.gameObject.SetActive(false);
                    
                }
                else
                {
                    selected.SetActive(false);
                    selectButton.gameObject.SetActive(true);
                }
                trySkinButton.gameObject.SetActive(false);
                unlockByAdButton.gameObject.SetActive(false);
            }
            else
            {
                if (GameDataManager.Instance.GetCurrentSkin().Equals(skin))
                {
                    selected.SetActive(true);
                    selectButton.gameObject.SetActive(false);
                    trySkinButton.gameObject.SetActive(false);
                    unlockByAdButton.gameObject.SetActive(false);
                }
                else
                {
                    selected.SetActive(false);
                    selectButton.gameObject.SetActive(false);
                    trySkinButton.gameObject.SetActive(true);
                    unlockByAdButton.gameObject.SetActive(true);
                    priceText.text = GameDataManager.Instance.GetAdUnlockSkin(skinSelected.GetSkinName()) + "/" +
                                     AdUnlock;
                }
                
            }
        }
    }
}