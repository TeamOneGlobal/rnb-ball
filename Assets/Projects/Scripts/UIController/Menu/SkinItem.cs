using System;
using Projects.Scripts.Data;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Scripts.UIController.Menu
{
    public class SkinItem : MonoBehaviour
    {
        [SerializeField]private Toggle toggle;
        [SerializeField] private SkeletonGraphic red,blue;
        [SerializeField] private GameObject locked, selected,usingObj;
        private SkinInfo _skinInfo;

        public Action<SkinItem> onToggleOn;
        public void Init(SkinInfo skinInfo, ToggleGroup group,bool forceOpen = false)
        {
            _skinInfo = skinInfo;
            toggle.group = group;
            red.initialSkinName = _skinInfo.skinName+"_1";
            red.Initialize(true);
            blue.initialSkinName = _skinInfo.skinName+"_2";
            blue.Initialize(true);
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    onToggleOn?.Invoke(this);
                }
            });
            locked.gameObject.SetActive(!GameDataManager.Instance.IsSkinUnlock(_skinInfo.skinName)&&!forceOpen);
            usingObj.gameObject.SetActive(GameDataManager.Instance.GetCurrentSkin().Equals(_skinInfo.skinName));
        }

        public void SetSelected()
        {
            toggle.isOn = true;
            toggle.onValueChanged.Invoke(true);
        }

        public void Unlock()
        {
            locked.gameObject.SetActive(false);
            usingObj.gameObject.SetActive(true);
            
        }
        public void SetUsing(bool value)
        {
            usingObj.SetActive(value);
        }
        public string GetSkinName()
        {
            return _skinInfo.skinName;
        }

        public SkinInfo GetSkinInfo()
        {
            return _skinInfo;
        }
    }
}