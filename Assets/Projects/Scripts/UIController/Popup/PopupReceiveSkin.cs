using System;
using System.Collections;
using DG.Tweening;
using Spine.Unity;
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
    public class PopupReceiveSkin : BasePopup
    {
        [SerializeField] private Transform container;
        [SerializeField] private SkeletonGraphic red,blue;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Button continueButton,viewAdButton;
        [SerializeField] private GameObject firework;
        private bool _isInit;
        private string _currentSkin;
        public void Initialized(string skin,bool needWatchAd = false,Action complete=null)
        {
            
            _currentSkin = skin;
            closeAction =()=>
            {
                complete?.Invoke();
            };
            RegisterEvent();
            red.initialSkinName = skin+"_1";
            red.Initialize(true);
            blue.initialSkinName = skin+"_2";
            blue.Initialize(true);
            openAction = () =>
            {
                StartCoroutine(!needWatchAd ? SpawnFireWork() : ShowContinueButton()); 
            };
            if (!needWatchAd)
            {
                viewAdButton.gameObject.SetActive(false);
                continueButton.gameObject.SetActive(true);
                title.text = "YOU GOT A NEW SKIN";
                GameDataManager.Instance.UnlockSkin(skin);
            }
            else
            {
                title.text = "NEW SKIN";
                viewAdButton.gameObject.SetActive(true);
            }
            GameServiceManager.Instance.adManager.HideBanner();
        }
        private void RegisterEvent()
        {
            if(_isInit) return;
            _isInit = true;
            continueButton.onClick.AddListener(Continue);
            viewAdButton.onClick.AddListener(ViewAd);
        }
        private void Continue()
        {
            GameServiceManager.Instance.adManager.ShowBanner();
            SoundManager.Instance.PlayButtonSound();
            Close();
        }

        private void ViewAd()
        {
            SoundManager.Instance.PlayButtonSound();
            GameServiceManager.Instance.adManager.ShowRewardedAd("ad_skin", () =>
            {
                StopAllCoroutines();
                viewAdButton.gameObject.SetActive(false);
                continueButton.gameObject.SetActive(true);
                title.text = "YOU GOT A NEW SKIN";
                StartCoroutine(SpawnFireWork());
                GameDataManager.Instance.UnlockSkin(_currentSkin);
            });
        }
        private IEnumerator SpawnFireWork()
        {
            for (var i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(Random.Range(0.2f, 1f));
                var obj = Instantiate(firework,transform);
                obj.transform.localScale = new Vector3(100,100,100);
                obj.transform.position = new Vector3(Random.Range(-10f, 10f), Random.Range(2f, 6f), 100);
            }
        }

        private IEnumerator ShowContinueButton()
        {
            continueButton.gameObject.SetActive(false);
            yield return new WaitForSeconds(2f);
            continueButton.gameObject.SetActive(true);
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