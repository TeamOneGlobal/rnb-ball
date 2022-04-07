using System;
using ThirdParties.Truongtv;
using ThirdParties.Truongtv.AdsManager;
using ThirdParties.Truongtv.SoundManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Scripts.UIController.Menu
{
    public class RescueSkinBoard : SkinBoard
    {
        [SerializeField] private Button unlockButton;
        [SerializeField] private TextMeshProUGUI unlockText;
        public override void Init()
        {
            base.Init();
            InitItem();
            
            unlockButton.onClick.RemoveAllListeners();
            unlockButton.onClick.AddListener(Unlock);
        }

        private void Unlock()
        {
            SoundManager.Instance.PlayButtonSound();
            GameServiceManager.Instance.adManager.ShowRewardedAd("shop_unlock_skin", UnlockSkin);
        }
        protected override void OnToggleOn(SkinItem skinItem)
        {
            base.OnToggleOn(skinItem);
            var skin = skinItem.GetSkinName();
            if (GameDataManager.Instance.IsSkinUnlock(skin))
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
                unlockButton.gameObject.SetActive(false);
                unlockText.text = string.Empty;
                
            }
            else
            {
                selected.SetActive(false);
                selectButton.gameObject.SetActive(false);
                unlockButton.gameObject.SetActive(true);
                if (GameDataManager.Instance.GetCurrentLevel() > skinSelected.GetSkinInfo().unlockValue)
                    unlockText.text = string.Empty;
                else
                    unlockText.text = "Unlock at Level "+skinSelected.GetSkinInfo().unlockValue;
            }
        }
    }
}