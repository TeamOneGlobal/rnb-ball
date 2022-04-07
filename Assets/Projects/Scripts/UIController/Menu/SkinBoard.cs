using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Projects.Scripts.Data;
using Projects.Scripts.UIController.Popup;
using Sirenix.OdinInspector;
using Spine.Unity;
using ThirdParties.Truongtv.SoundManager;
using Truongtv.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Scripts.UIController.Menu
{
    public class SkinBoard : MonoBehaviour
    {
        [SerializeField] private LocationType location;
        [SerializeField] private ToggleGroup group;
        [SerializeField] private Transform container;
        [SerializeField] private SkinItem prefab;
        [SerializeField] private SkeletonGraphic red,blue;
        [SerializeField] protected Button selectButton;
        [SerializeField] protected GameObject selected;
        [SerializeField] protected PopupShop shop;
        protected List<SkinItem> skinItemList;
        protected SkinItem skinSelected;
        protected void InitItem(bool forceOpen = false)
        {
            container.RemoveAllChild();
            skinItemList = new List<SkinItem>();
            var skinList = GameDataManager.Instance.skinData.GetSkinByLocation(location).OrderBy(a=>a.unlockValue);
            bool isActiveOne = false;
            var unlockSkins = GameDataManager.Instance.GetUnlockedSkin();
            foreach (var skinInfo in skinList)
            {
                var clone = Instantiate(prefab, container) as SkinItem ;
                if (clone == null) continue;
                clone.Init(skinInfo,group,forceOpen);
                clone.onToggleOn = OnToggleOn;
                if (!isActiveOne && !unlockSkins.Contains(clone.GetSkinName()))
                {
                    isActiveOne = true;
                    clone.SetSelected();
                }
                skinItemList.Add(clone);
            }
        }

        public virtual void Init()
        {
            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(OnSelectClick);
        }
        protected virtual void OnToggleOn(SkinItem skinItem)
        {
            skinSelected = skinItem;
            red.initialSkinName = skinSelected.GetSkinName()+"_1";
            red.Initialize(true);
            blue.initialSkinName = skinSelected.GetSkinName()+"_2";
            blue.Initialize(true);
        }

        private void OnSelectClick()
        {
            SoundManager.Instance.PlayButtonSound();
            GameDataManager.Instance.SetSkin(skinSelected.GetSkinName());
            SelectCurrentSkin();
        }

        protected void SelectCurrentSkin()
        {
            foreach (var item in skinItemList)
            {
                item.SetUsing(false);
            }
            OnToggleOn(skinSelected);
            skinSelected.SetUsing(true);
            MenuController.Instance.UpdateSkin(skinSelected.GetSkinName());
        }

        
        [Button]
        protected void RandomUnlock()
        {
            shop.ActiveClose(false);
            var unlockSkins = GameDataManager.Instance.GetUnlockedSkin();
            var ListItemNotUnlock = skinItemList.FindAll(a => !unlockSkins.Contains(a.GetSkinName()));
            if (ListItemNotUnlock.Count == 1)
            {
                ListItemNotUnlock[0].SetSelected();
                GameDataManager.Instance.UnlockSkin(ListItemNotUnlock[0].GetSkinName());
                SelectCurrentSkin();
            }
            else if (ListItemNotUnlock.Count >1)
            {
                var index = 0;
                DOTween.To(() => 0, x => index = x, 15, 3f).SetEase(Ease.InOutQuad).OnUpdate(
                    () =>
                    {
                        ListItemNotUnlock[index%ListItemNotUnlock.Count].SetSelected();
                    }).OnComplete(() =>
                {
                    var item = ListItemNotUnlock[index % ListItemNotUnlock.Count];
                    item.SetSelected();
                    UnlockSkin();
                    shop.ActiveClose(true);
                });
            }
        }

        protected void UnlockSkin()
        {
            PopupMenuController.Instance.OpenPopupReceiveSkin(skinSelected.GetSkinName(), false,() =>
            {
                skinSelected.Unlock();
                skinSelected.SetSelected();
                SelectCurrentSkin();
            });
            
        }
    }
}