using System;
using System.Collections.Generic;
using Scriptable;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;
using UserDataModel;

namespace UIController.SkinPopupController
{
    public class SkinItem : MonoBehaviour
    {
        [SerializeField,OnValueChanged(nameof(OnValueChange)),ValueDropdown(nameof(GetAllSkinName))] public string skinName;
        [SerializeField] private SkeletonGraphic red, blue;
        [SerializeField] private Material grayScale, normal;
        [SerializeField] private GameObject unlockMark;
        [SerializeField] private Toggle toggle;
        [SerializeField] public UnlockSkinType priceType;
        [ShowIf("@this.priceType == UnlockSkinType.Level||this.priceType == UnlockSkinType.Ads")]public int priceValue;
        public Action<SkinItem> onToggleOn;
        public void Init(SkinInfo info, ToggleGroup group)
        {
            skinName = info.skinName;
            priceType = info.unlockType;
            priceValue = info.unlockValue;
            OnValueChange();
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    onToggleOn?.Invoke(this);
                }
            });
            toggle.group = group;
            if (UserDataController.IsSkinUnlock(skinName))
            {
                red.material = normal;
                blue.material = normal;
                unlockMark.SetActive(true);
            }
            else
            {
                red.material = grayScale;
                blue.material = grayScale;
                unlockMark.SetActive(false);
            }
        }

        public bool IsUnlock()
        {
            return UserDataController.IsSkinUnlock(skinName);
        }

        public bool IsSelected()
        {
            return UserDataController.GetSelectedSkin().Equals(skinName);
        }
        private List<string> GetAllSkinName()
        {
            var result = new List<string>();
            var totalSkin = red.SkeletonDataAsset.GetSkeletonData(true).Skins.Items;
            foreach (var skin in totalSkin)
            {
                result.Add(skin.Name.Split('_')[0]);
            }

            return result;
        }
        private void OnValueChange()
        {
            red.initialSkinName = skinName + "_1";
            blue.initialSkinName = skinName + "_2";
            red.Initialize(true);
            blue.Initialize(true);
        }

        public void SetUnlock()
        {
            red.material = normal;
            blue.material = normal;
            unlockMark.SetActive(true);
        }
    }

   
}