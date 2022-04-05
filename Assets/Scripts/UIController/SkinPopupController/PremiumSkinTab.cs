using System.Collections.Generic;
using System.Linq;
using Scriptable;
using Sound;
using TMPro;
using Truongtv.Services.IAP;
using Truongtv.Utilities;
using UIController.Popup;
using UnityEngine;
using UnityEngine.UI;
using UserDataModel;

namespace UIController.SkinPopupController
{
    public class PremiumSkinTab : SkinBoard
    {
        [SerializeField] private Button  unlockByViewAdButton,dailyGiftOnlyButton;
        [SerializeField] private TextMeshProUGUI viewAdValue;
        [SerializeField] private string skuId;
        private List<SkinInfo> _skinDatas;
        public override void Init(SkinPopup skinPopup, SkinData skinData, SkinItem prefab)
        {
            unlockByViewAdButton.onClick.RemoveAllListeners();
            unlockByViewAdButton.onClick.AddListener(ViewAdToUnlock);
            dailyGiftOnlyButton.onClick.RemoveAllListeners();
            dailyGiftOnlyButton.onClick.AddListener(DailyGiftOnly);
            unlockByViewAdButton.gameObject.SetActive(false);
            base.Init(skinPopup,skinData,prefab);
        }
        protected override void SpawnSkinItem(SkinData skinData, SkinItem prefab)
        {
            skinItems= new List<SkinItem>();
            _skinDatas = skinData.skins.FindAll(a =>
                a.location == LocationType.Premium);

            container.RemoveAllChild();
            foreach (var skinInfo in _skinDatas)
            {
                var item = Instantiate(prefab, container);
                item.Init(skinInfo,group);
                skinItems.Add(item);
            }
        }
        protected override void OnToggleOn(SkinItem item)
        {
            base.OnToggleOn(item);
            if (UserDataController.IsSkinUnlock(item.skinName))
            {
                unlockByViewAdButton.gameObject.SetActive(false);
            }
            else
            {
                if (item.priceType == UnlockSkinType.DailyGift)
                {
                    unlockByViewAdButton.gameObject.SetActive(false);
                    dailyGiftOnlyButton.gameObject.SetActive(true);
                }
                else
                {
                    unlockByViewAdButton.gameObject.SetActive(true);
                    dailyGiftOnlyButton.gameObject.SetActive(false);
                    viewAdValue.text = "UNLOCK ALL";
                }
                
               
            }
        }

        

        private void ViewAdToUnlock()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            //TODO buy Iap
            NetWorkHelper.PurchaseProduct(skuId, (result, sku) =>
            {
                if(!result) return;
                for (var i = 0; i < _skinDatas.Count; i++)
                {
                    UserDataController.UnlockSkin(_skinDatas[i].skinName);
                    UserDataController.UpdateSelectedSkin(_skinDatas[i].skinName);
                    tryNowButton.gameObject.SetActive(false);
                    unlockByViewAdButton.gameObject.SetActive(false);
                    selectedObj.SetActive(true);
                    
                }

                foreach (var item in skinItems)
                {
                    item.SetUnlock();
                    item.GetComponent<Toggle>().isOn = true;
                    item.GetComponent<Toggle>().onValueChanged.Invoke(true);
                }
                var data = _skinDatas.Select(a => a.skinName).ToList();
                MenuPopupController.Instance.ShowUnlockSkinsPopup(data);
            });
        }
        private void DailyGiftOnly()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            MenuPopupController.Instance.ShowDailyGiftPopup();
        }

    }
}