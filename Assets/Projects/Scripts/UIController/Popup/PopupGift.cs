using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Projects.Scripts.Data;
using Projects.Scripts.GamePlay.Sound;
using Projects.Scripts.UIController.Menu;
using ThirdParties.Truongtv;
using ThirdParties.Truongtv.AdsManager;
using ThirdParties.Truongtv.SoundManager;
using TMPro;
using Truongtv.PopUpController;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Projects.Scripts.UIController.Popup
{
    public class PopupGift : BasePopup
    {
        [SerializeField] private Transform container;
        [SerializeField] private Button backButton,refreshButton,refreshAdButton;
        [SerializeField] private List<GiftItem> giftItems;
        private SkinData _skinData;
        [SerializeField] private TextMeshProUGUI timeText;
        private bool _isInit;
        public Action onWatchAdOrClaimSkin;
        public void Initialized()
        {
            RegisterEvent();
            _skinData = GameDataManager.Instance.skinData;
            var giftList = GameDataManager.Instance.GetGiftList();
            if (giftList?.Count == 0) // first Init
            {
                Refresh();
            }
            else
            {
                for (var i = 0; i < giftList.Count; i++)
                {
                    giftItems[i].Init(giftList[i],this);
                }
            }
            _isInit = true;
        }

        private IEnumerator CountDown()
        {
            while (!GameDataManager.Instance.CanRefresh())
            {
                var time = GameDataManager.Instance.GetRefreshTime().AddDays(1).Subtract(DateTime.UtcNow);
                timeText.text = "Refresh in: " + time.ToString(@"hh\:mm\:ss");
                yield return new WaitForSeconds(1);
            }
            refreshButton.gameObject.SetActive(true);
            refreshAdButton.gameObject.SetActive(false);
        }
        private void OpenAll(float delay)
        {
            int current = 0;
            int last = -1;
            DOTween.To(() => -1, x => current = x, 4, 3f).SetDelay(delay).SetEase(Ease.InQuad)
                .OnUpdate(() =>
                {
                    if (current != last)
                    {
                        if (current >= 0)
                            giftItems[current].OpenDoor();
                        last = current;
                    }
                });
        }

        private void RegisterEvent()
        {
            if (_isInit) return;
            onWatchAdOrClaimSkin += () =>
            {
                foreach (var item in giftItems)
                {
                    item.UpdateValue();
                }
            };
            openAction = OnStart;
            closeAction = OnClose;
            backButton.onClick.AddListener(ClosePopUp);
            refreshButton.onClick.AddListener(RefreshData);
            refreshAdButton.onClick.AddListener(RefreshDataAd);
        }


        private void OnStart()
        {
            container.localScale = Vector3.zero;
            SoundManager.Instance.PlayPopupOpenSound();
            container.DOScale(1, DURATION).SetUpdate(UpdateType.Normal, true).SetEase(Ease.OutBack);
            StartCoroutine(CountDown());
        }

        private void OnClose()
        {
            SoundManager.Instance.PlayPopupCloseSound();
            container.DOScale(0, DURATION).SetUpdate(UpdateType.Normal, true).SetEase(Ease.InBack);
        }

        private void ClosePopUp()
        {
            SoundManager.Instance.PlayButtonSound();
           // GameServiceManager.Instance.adManager.ShowBanner();
            StopAllCoroutines();
            Close();
        }

        private void Refresh()
        {
            var rnd = Random.Range(1, 3);
            var random = new System.Random();
            List<string> saveData = new List<string>();
            var premiumSkins = _skinData.GetAllPremiumSkin();
            List<SkinInfo> temp = new List<SkinInfo>();
            foreach (var skin in premiumSkins)
            {
                if (GameDataManager.Instance.IsSkinUnlock(skin.skinName))
                {
                    temp.Add(skin);
                }
            }
            foreach (var skinInfo in temp)
            {
                premiumSkins.Remove(skinInfo);
            }
            temp.Clear();
            var listPremiumSkins = premiumSkins.OrderBy(x => random.Next()).Take(rnd);
            saveData.AddRange(listPremiumSkins.Select(a=>a.skinName));
            var normalSkins = _skinData.GetNormalSkin();
            foreach (var skin in normalSkins)
            {
                if (GameDataManager.Instance.IsSkinUnlock(skin.skinName))
                {
                    temp.Add(skin);
                }
            }
            foreach (var skinInfo in temp)
            {
                normalSkins.Remove(skinInfo);
            }
            var listNormalSkin = normalSkins.OrderBy(x => random.Next()).Take(5 - rnd);
            saveData.AddRange(listNormalSkin.Select(a=>a.skinName));
            saveData.OrderBy(x => random.Next());
            GameDataManager.Instance.SetGiftList(saveData);
            var giftList = GameDataManager.Instance.GetGiftList();
            for (var i = 0; i < giftList.Count; i++)
            {
                giftItems[i].Init(giftList[i],this,true);
            }
            OpenAll(1);
            if (GameDataManager.Instance.CanRefresh())
            {
                refreshButton.gameObject.SetActive(true);
                refreshAdButton.gameObject.SetActive(false);
            }
            else
            {
                refreshButton.gameObject.SetActive(false);
                refreshAdButton.gameObject.SetActive(true);
                StartCoroutine(CountDown());
            }
        }
        
        private void RefreshData()
        {
            SoundManager.Instance.PlayButtonSound();
            GameDataManager.Instance.SetGiftList(new List<string>());
            GameDataManager.Instance.SetRefreshTime();
            Refresh();
            MenuController.Instance.CheckTimeCountDownGiftButton();
        }

        private void RefreshDataAd()
        {
            SoundManager.Instance.PlayButtonSound();
            GameServiceManager.Instance.adManager.ShowRewardedAd("Rewarded_Refresh_Gift",()=>
            {
                SoundManager.Instance.PlayButtonSound();
                GameDataManager.Instance.SetGiftList(new List<string>());
                GameDataManager.Instance.SetRefreshTime();
                Refresh();
                MenuController.Instance.CheckTimeCountDownGiftButton();
            });
        }

    }
}