using DG.Tweening;
using Sirenix.OdinInspector;
using ThirdParties.Truongtv;
using ThirdParties.Truongtv.AdsManager;
using ThirdParties.Truongtv.SoundManager;
using TMPro;
using Truongtv.PopUpController;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Scripts.UIController.Menu
{
    public class CoinSkinBoard : SkinBoard
    {
        [SerializeField] private Button unlockButton,viewAdButton;
        [SerializeField] private TextMeshProUGUI valueText;
        public override void Init()
        {
            base.Init();
            InitItem();
            viewAdButton.onClick.RemoveAllListeners();
            viewAdButton.onClick.AddListener(ViewAd);
            unlockButton.onClick.RemoveAllListeners();
            unlockButton.onClick.AddListener(Unlock);
            var count = GameDataManager.Instance.GetCountSkinBuyByCoin();
            var value = GameDataManager.Instance.skinData.baseCoinValue +
                        count * GameDataManager.Instance.skinData.increaseCoinValue;
            valueText.text = $"{value}";
        }

        private void ViewAd()
        {
            SoundManager.Instance.PlayButtonSound();
            GameServiceManager.Instance.adManager.ShowRewardedAd("shop_try_skin", () =>
            {
                GameDataManager.Instance.SetTrySkin(skinSelected.GetSkinName());
                SelectCurrentSkin();
            });
        }

        private void Unlock()
        {
            SoundManager.Instance.PlayButtonSound();
            var count = GameDataManager.Instance.GetCountSkinBuyByCoin();
            var value = GameDataManager.Instance.skinData.baseCoinValue +
                        count * GameDataManager.Instance.skinData.increaseCoinValue;
            if (GameDataManager.Instance.GetCurrentCoin() <value)
            {
                PopupController.Instance.ShowToast("Not enough Coin");
            }
            else
            {
                GameDataManager.Instance.BuySkinByCoin();
                MenuController.Instance.UpdateCoin(-value);
                RandomUnlock();
            }
             count = GameDataManager.Instance.GetCountSkinBuyByCoin();
             value = GameDataManager.Instance.skinData.baseCoinValue +
                        count * GameDataManager.Instance.skinData.increaseCoinValue;
            valueText.text = $"{value}";
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
                viewAdButton.gameObject.SetActive(false);
            }
            else
            {
                if (GameDataManager.Instance.GetCurrentSkin().Equals(skin))
                {
                    selected.SetActive(true);
                    selectButton.gameObject.SetActive(false);
                    unlockButton.gameObject.SetActive(false);
                    viewAdButton.gameObject.SetActive(false);
                }
                else
                {
                    selected.SetActive(false);
                    selectButton.gameObject.SetActive(false);
                    unlockButton.gameObject.SetActive(true);
                    viewAdButton.gameObject.SetActive(true);
                }
                
            }
        }

    }
}