using ThirdParties.Truongtv;
using ThirdParties.Truongtv.AdsManager;
using ThirdParties.Truongtv.SoundManager;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Scripts.UIController.Menu
{
    public class PremiumSkinBoard : SkinBoard
    {
        [SerializeField] private Button trySkinButton;
        private bool forceOpen;
        public override void Init()
        {
            base.Init();
            InitItem(GameDataManager.Instance.IsSubscription());
            forceOpen = GameDataManager.Instance.IsSubscription();
            trySkinButton.onClick.RemoveAllListeners();
            trySkinButton.onClick.AddListener(TrySkin);
        }

        private void OpenSubscription()
        {
            
        }

        private void TrySkin()
        {
            SoundManager.Instance.PlayButtonSound();
            GameServiceManager.Instance.adManager.ShowRewardedAd("shop_try_skin", () =>
            {
                GameDataManager.Instance.SetTrySkin(skinSelected.GetSkinName());
                SelectCurrentSkin();
            });
        }

        protected override void OnToggleOn(SkinItem skinItem)
        {
            base.OnToggleOn(skinItem);
            var skin = skinItem.GetSkinName();
            if (GameDataManager.Instance.IsSkinUnlock(skin)||forceOpen)
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
                trySkinButton.gameObject.SetActive(false);
            }
            else
            {
                if (GameDataManager.Instance.GetCurrentSkin().Equals(skin))
                {
                    selected.SetActive(true);
                    selectButton.gameObject.SetActive(false);
                    trySkinButton.gameObject.SetActive(false);
                }
                else
                {
                    selected.SetActive(false);
                    selectButton.gameObject.SetActive(false);
                    trySkinButton.gameObject.SetActive(true);
                }
                
            }
        }
    }
}