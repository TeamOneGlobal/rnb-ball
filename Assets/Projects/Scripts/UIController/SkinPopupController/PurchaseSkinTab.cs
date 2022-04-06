using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Scriptable;
using Sound;
using TMPro;
using Truongtv.Utilities;
using UIController.Popup;
using UnityEngine;
using UnityEngine.UI;
using UserDataModel;
using Random = UnityEngine.Random;

namespace UIController.SkinPopupController
{
    public class PurchaseSkinTab : SkinBoard
    {
        [SerializeField] private Button unlockButton;
        [SerializeField] private TextMeshProUGUI coinValue;
        [SerializeField] private long baseValue;
        [SerializeField] private long increaseValue;
        private long _currentValue;
        private List<SkinInfo> _skinInfos;
        private bool _isUnlockAll;
        public override void Init(SkinPopup skinPopup, SkinData skinData, SkinItem prefab)
        {
            unlockButton.onClick.RemoveAllListeners();
            unlockButton.onClick.AddListener(Unlock);
            UpdateValue();
            baseValue = skinData.baseCoinValue;
            increaseValue = skinData.increaseValue;
            unlockButton.gameObject.SetActive(false);
            base.Init(skinPopup,skinData,prefab);
            InitValue();

        }

        private void InitValue()
        {
            var unlockedSkin = UserDataController.GetTotalUnlockSKins();
            var lockedSkin = _skinInfos.Select(skinInfo => skinInfo.skinName).ToList();
            lockedSkin = lockedSkin.Except(unlockedSkin).ToList();
            _isUnlockAll = lockedSkin.Count == 0;
            if(_isUnlockAll)
                unlockButton.gameObject.SetActive(!_isUnlockAll);
        }
        protected override void SpawnSkinItem(SkinData skinData, SkinItem prefab)
        {
            skinItems= new List<SkinItem>();
            _skinInfos = skinData.skins.FindAll(a =>
                a.location == LocationType.Purchase);

            container.RemoveAllChild();
            foreach (var skinInfo in _skinInfos)
            {
                var item = Instantiate(prefab, container);
                item.Init(skinInfo,group);
                skinItems.Add(item);
            }
        }
        protected override void OnToggleOn(SkinItem item)
        {
            base.OnToggleOn(item);
            if (item.IsUnlock()|| _isUnlockAll)
            {
                unlockButton.gameObject.SetActive(false);
            }
            else
            {
                unlockButton.gameObject.SetActive(true);
            }
        }
        private void UpdateValue()
        {
            _currentValue = UserDataController.GetBoughSkinNumber() * increaseValue + baseValue;
            coinValue.text = ""+_currentValue;
        }
        private void Unlock()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            if (UserDataController.GetCurrentCoin() < _currentValue)
            {
                MenuPopupController.Instance.ShowNotEnoughCoinPopup(Unlock);
            }
            else
            {
                RandomPick().Forget();
                   
            }
        }

        private async UniTaskVoid RandomPick()
        {
            var unlockedSkin = UserDataController.GetTotalUnlockSKins();
            var lockedSkin = _skinInfos.Select(skinInfo => skinInfo.skinName).ToList();
            lockedSkin = lockedSkin.Except(unlockedSkin).ToList();
            var randomSkin = lockedSkin[0];
            if (lockedSkin.Count > 1)
            {
                for (var i = 0; i < 10; i++)
                {
                    var r = Random.Range(0, lockedSkin.Count);
                    var item2 = skinItems.Find(a => a.skinName == lockedSkin[r]);
                    item2.GetComponent<Toggle>().isOn = true;
                    await UniTask.Delay(TimeSpan.FromMilliseconds(250));
                }
            }
            randomSkin = lockedSkin[Random.Range(0, lockedSkin.Count)];
            UserDataController.BoughtSkin(randomSkin);
            UserDataController.UpdateSelectedSkin(randomSkin);
            tryNowButton.gameObject.SetActive(false);
            var item = skinItems.Find(a => a.skinName == randomSkin);
            selectedObj.SetActive(true);
            item.SetUnlock();
            item.GetComponent<Toggle>().isOn = true;
            item.GetComponent<Toggle>().onValueChanged.Invoke(true);
            if (item == Item)
            {
                unlockButton.gameObject.SetActive(false);
            }
            await UniTask.Delay(TimeSpan.FromMilliseconds(250));
            MenuPopupController.Instance.ShowUnlockSkinsPopup(new List<string>{randomSkin});
            CoinController.Instance.DecreaseCoin(_currentValue);
           
            UpdateValue();
            InitValue();
        }
    }
}