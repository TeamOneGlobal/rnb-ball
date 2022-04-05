using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Scriptable;
using Sound;
using TMPro;
using Truongtv.Services.Ad;
using Truongtv.Utilities;
using UIController.Popup;
using UnityEngine;
using UnityEngine.UI;
using UserDataModel;

namespace UIController.SkinPopupController
{
    public class RescueSkinTab : SkinBoard
    {
        [SerializeField] private Button unlockByLevelButton, spinOnlyButton, getItButton, dailyGiftOnlyButton;
        [SerializeField] private TextMeshProUGUI levelTxt;

        public override void Init(SkinPopup skinPopup, SkinData skinData, SkinItem prefab)
        {
            unlockByLevelButton.onClick.RemoveAllListeners();
            unlockByLevelButton.onClick.AddListener(UnlockByLevel);
            spinOnlyButton.onClick.RemoveAllListeners();
            spinOnlyButton.onClick.AddListener(SpinOnly);
            dailyGiftOnlyButton.onClick.RemoveAllListeners();
            dailyGiftOnlyButton.onClick.AddListener(DailyGiftOnly);
            getItButton.onClick.RemoveAllListeners();
            getItButton.onClick.AddListener(GetIt);
            unlockByLevelButton.gameObject.SetActive(false);
            spinOnlyButton.gameObject.SetActive(false);
            getItButton.gameObject.SetActive(false);
            dailyGiftOnlyButton.gameObject.SetActive(false);
            tryNowButton.gameObject.SetActive(false);

            base.Init(skinPopup, skinData, prefab);
        }


        protected override void SpawnSkinItem(SkinData skinData, SkinItem prefab)
        {
            skinItems = new List<SkinItem>();
            var data = skinData.skins.FindAll(a =>
                a.location == LocationType.Rescue);

            container.RemoveAllChild();
            foreach (var skinInfo in data)
            {
                var item = Instantiate(prefab, container);
                item.Init(skinInfo, group);
                skinItems.Add(item);
            }
        }

        protected override void OnToggleOn(SkinItem item)
        {
            base.OnToggleOn(item);
            if (UserDataController.IsSkinUnlock(item.skinName))
            {
                unlockByLevelButton.gameObject.SetActive(false);
                spinOnlyButton.gameObject.SetActive(false);
                getItButton.gameObject.SetActive(false);
                dailyGiftOnlyButton.gameObject.SetActive(false);
            }
            else
            {
                unlockByLevelButton.gameObject.SetActive(false);
                spinOnlyButton.gameObject.SetActive(false);
                getItButton.gameObject.SetActive(false);
                dailyGiftOnlyButton.gameObject.SetActive(false);
                switch (item.priceType)
                {
                    case UnlockSkinType.Level:
                        if (UserDataController.GetCurrentLevel() >= item.priceValue)
                        {
                            tryNowButton.gameObject.SetActive(false);
                            getItButton.gameObject.SetActive(true);
                        }
                        else
                        {
                            unlockByLevelButton.gameObject.SetActive(true);
                            tryNowButton.gameObject.SetActive(true);
                            UpdateValue();
                        }

                        break;
                    case UnlockSkinType.Spin:
                        if (UserDataController.IsInUnlockProgress(item.skinName))
                        {
                            getItButton.gameObject.SetActive(true);
                        }
                        else
                        {
                            spinOnlyButton.gameObject.SetActive(true);
                            tryNowButton.gameObject.SetActive(true);
                        }


                        break;
                    case UnlockSkinType.DailyGift:
                        if (UserDataController.IsInUnlockProgress(item.skinName))
                        {
                            getItButton.gameObject.SetActive(true);
                        }
                        else
                        {
                            dailyGiftOnlyButton.gameObject.SetActive(true);
                            tryNowButton.gameObject.SetActive(true);
                        }
                        break;
                }
            }
        }

        private void UpdateValue()
        {
            levelTxt.text = "UNLOCK AT LV." + Item.priceValue;
        }

        private async void UnlockByLevel()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            LoadSceneController.LoadLevel(UserDataController.GetCurrentLevel());
        }

        private void SpinOnly()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            MenuPopupController.Instance.ShowSpinPopup();
        }

        private void DailyGiftOnly()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            MenuPopupController.Instance.ShowDailyGiftPopup();
        }

        private void GetIt()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            NetWorkHelper.ShowRewardedAdInMenu("Rewarded_SkinPopup_GetIt", adResult: result =>
            {
                if (!result) return;
                UserDataController.UnlockSkin(Item.skinName);
                UserDataController.UpdateSelectedSkin(Item.skinName);
                tryNowButton.gameObject.SetActive(false);
                selectedObj.SetActive(true);
                Item.SetUnlock();
                Item.GetComponent<Toggle>().onValueChanged.Invoke(true);
                MenuPopupController.Instance.ShowUnlockSkinsPopup(new List<string>{Item.skinName});
            });
        }
    }
}