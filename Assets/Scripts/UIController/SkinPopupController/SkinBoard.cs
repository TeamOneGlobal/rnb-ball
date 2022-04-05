using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Scriptable;
using Sound;
using Truongtv.Services.Ad;
using UIController.Popup;
using UnityEngine;
using UnityEngine.UI;
using UserDataModel;

namespace UIController.SkinPopupController
{
    public class SkinBoard : MonoBehaviour
    {
        [SerializeField] protected Transform container;
        [SerializeField] protected List<SkinItem> skinItems;
        [SerializeField] protected Button tryNowButton;
        [SerializeField] protected GameObject selectedObj;
        [SerializeField] protected ToggleGroup group;
        protected SkinPopup SkinPopup;
        protected SkinItem Item;
        public virtual void Init(SkinPopup skinPopup, SkinData skinData, SkinItem prefab)
        {
            SkinPopup = skinPopup;
            group.SetAllTogglesOff();
            tryNowButton.onClick.RemoveAllListeners();
            tryNowButton.onClick.AddListener(TryNow);
            tryNowButton.gameObject.SetActive(false);
            SpawnSkinItem(skinData,prefab);
            bool selected = false;
            foreach (var skinItem in skinItems)
            {
                skinItem.onToggleOn = OnToggleOn;
                if (!skinItem.IsSelected())
                {
                    skinItem.GetComponent<Toggle>().isOn = false;
                    skinItem.GetComponent<Toggle>().onValueChanged.Invoke(false);
                }
                else
                {
                    selected = true;
                    skinItem.GetComponent<Toggle>().isOn = true;
                    skinItem.GetComponent<Toggle>().onValueChanged.Invoke(true);
                }
            }

            if (!selected)
            {
                skinItems[0].GetComponent<Toggle>().isOn = true;
                skinItems[0].GetComponent<Toggle>().onValueChanged.Invoke(true);
            }
        }

        protected virtual void SpawnSkinItem(SkinData skinData, SkinItem prefab)
        {
            
        }
        protected virtual void OnToggleOn(SkinItem item)
        {
            Item = item;
            SkinPopup.OnItemSelected(item.skinName);
            if (UserDataController.IsSkinUnlock(item.skinName))
            {
                UserDataController.UpdateSelectedSkin(item.skinName);
                tryNowButton.gameObject.SetActive(false);
                selectedObj.SetActive(true);
            }
            else
            {
                selectedObj.SetActive(false);
                tryNowButton.gameObject.SetActive(true);
            }
        }
        private void TryNow()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            NetWorkHelper.ShowRewardedAdInMenu("Rewarded_Rewarded_SkinPopup_TryNow", adResult:result =>
            {
                if(!result) return;
                UserDataController.DemoSkin(Item.skinName);
                LoadSceneController.LoadLevel(UserDataController.GetCurrentLevel());       
            });
             
        }
    }
}