using System.Collections;
using DG.Tweening;
using Projects.Scripts.Models;
using ThirdParties.Truongtv;
using ThirdParties.Truongtv.AdsManager;
using ThirdParties.Truongtv.SoundManager;
using TMPro;
using Truongtv.PopUpController;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Scripts.UIController.Popup
{
    public class PopupViewAdToDoubleValue : BasePopup
    {
        [SerializeField] private Transform container;
        [SerializeField] private GameObject coinObj,lifeObj;
        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private Button watchAd, noThanks;
        private ItemData data;
        private void Start()
        {
            noThanks.onClick.AddListener(ClosePopup);
            watchAd.onClick.AddListener(WatchAd);
            openAction = OnStart;
            closeAction = OnClose;
        }

        public void Init(ItemData itemData)
        {
            GameServiceManager.Instance.adManager.HideBanner();
            data = itemData;
            coinObj.SetActive(itemData.coinValue>0);
            lifeObj.SetActive(itemData.lifeValue>0);
            noThanks.gameObject.SetActive(false);
            var value = itemData.coinValue > 0 ? itemData.coinValue : itemData.lifeValue;
            valueText.text = $"{value}";
            openAction += () =>
            {
                StartCoroutine(ShowNoThanks());
            };
        }

        private void ClosePopup()
        {
            GameServiceManager.Instance.adManager.ShowBanner();
            SoundManager.Instance.PlayButtonSound();
            if(data.coinValue>0)
                MenuController.Instance.UpdateCoin(data.coinValue,coinObj.transform);
            if(data.lifeValue>0)
                MenuController.Instance.UpdateLife(data.lifeValue,lifeObj.transform);
            Close();
        }

        private void WatchAd()
        {
            SoundManager.Instance.PlayButtonSound();
            GameServiceManager.Instance.adManager.ShowRewardedAd("double_value", () =>
            {
                if(data.coinValue>0)
                    MenuController.Instance.UpdateCoin(data.coinValue*2,coinObj.transform);
                if(data.lifeValue>0)
                    MenuController.Instance.UpdateLife(data.lifeValue*2,lifeObj.transform);
                Close();
            });
        }

        private IEnumerator ShowNoThanks()
        {
            yield return new WaitForSeconds(2f);
            noThanks.gameObject.SetActive(true);
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
