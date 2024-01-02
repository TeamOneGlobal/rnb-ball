using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Projects.Scripts.Models;
using Projects.Scripts.UIController.Menu;
using Spine.Unity;
using ThirdParties.Truongtv;
using ThirdParties.Truongtv.AdsManager;
using ThirdParties.Truongtv.SoundManager;
using Truongtv.PopUpController;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Scripts.UIController.Popup
{
    public class PopupLottery : BasePopup
    {
        [SerializeField] private Transform container;
        [SerializeField] private SkeletonGraphic best;
        [SerializeField] private Button close;
        [SerializeField] private List<ChestItem> chestList;
        [SerializeField] private List<ItemData> dataList;

        private string _bestGift;
        private int index;
        
        public void Init()
        {
            //GameServiceManager.Instance.adManager.HideBanner();
            var skins = GameDataManager.Instance.skinData.GetAllPremiumSkin().Select(a => a.skinName)
                .Except(GameDataManager.Instance.GetUnlockedSkin()).ToList();
            if (skins.Count == 0)
            {
                skins = GameDataManager.Instance.skinData.GetNormalSkin().Select(a => a.skinName)
                    .Except(GameDataManager.Instance.GetUnlockedSkin()).ToList();
                if (skins.Count == 0)
                {
                    skins = GameDataManager.Instance.skinData.GetAllSkinName()
                        .Except(GameDataManager.Instance.GetUnlockedSkin()).ToList();
                    if (skins.Count == 0)
                    {
                        return;
                    }
                }
            }
            openAction = OnStart;
            closeAction = OnClose;
            _bestGift = skins[Random.Range(0, skins.Count)];
            best.initialSkinName = _bestGift;
            best.Initialize(true);
            dataList.Find(a => !string.IsNullOrEmpty(a.skinName)).skinName = _bestGift; 
            close.onClick.RemoveAllListeners();
            close.onClick.AddListener(ClosePopup);
            foreach (var item in chestList)
            {
                item.Init(this);
            }
        }

        private void ClosePopup()
        {
            GameServiceManager.Instance.adManager.ShowBanner();
            SoundManager.Instance.PlayButtonSound();
            Close();
        }

        public ItemData GetItemData()
        {
            var result = index == 0 ? dataList.Find(a => string.IsNullOrEmpty(a.skinName)) : dataList[Random.Range(0,dataList.Count)];
            dataList.Remove(result);
            index++;
            return result;
        }

        public void OnChestOpen(ChestItem chest)
        {
            foreach (var chestItem in chestList)
            {
                if (chestItem != chest)
                {
                    chestItem.SetOpenByAd();
                }
            }
        }
        private void OnStart()
        {
            container.localScale = Vector3.zero;
            SoundManager.Instance.PlayPopupOpenSound();
            container.DOScale(1, DURATION).SetUpdate(UpdateType.Normal, true).SetEase(Ease.OutBack);
        }

        private void OnClose()
        {
            SoundManager.Instance.PlayPopupCloseSound();
            container.DOScale(0, DURATION).SetUpdate(UpdateType.Normal, true).SetEase(Ease.InBack);
        }
    }
}