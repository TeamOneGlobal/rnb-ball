using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using MEC;
using Sound;
using Spine.Unity;
using Truongtv.PopUpController;
using Truongtv.Services.Ad;
using Truongtv.SoundManager;
using UIController.Scene;
using UnityEngine;
using UnityEngine.UI;
using UserDataModel;
using Random = UnityEngine.Random;

namespace UIController.Popup
{
    public class CongratsPopup : BasePopup
    {

        [SerializeField] private SkeletonGraphic red, blue;
        [SerializeField] private Button gotItButton, okButton,skipButton;
        [SerializeField] private GameObject tryItText, congratText;
        [SerializeField] private GameObject firework;
        private bool _isInit;
        private string _skin;
        private List<string> _skinList;
        private int _indexSkin;
        public void Initialized(string skin)
        {
            RegisterEvent();
            _skin = skin;
            _skinList = null;
            red.initialSkinName = skin + "_1";
            red.Initialize(true);
            blue.initialSkinName = skin + "_2";
            blue.Initialize(true);
            Timing.RunCoroutine(SpawnFireWork().CancelWith(gameObject));
            StepOne();
        }
        public void Initialized(List<string> skin,int currentIndex,Action afterClose = null)
        {
            _indexSkin = currentIndex;
            RegisterEvent();
            
            closeAction = afterClose;
            _skinList = skin;
            red.initialSkinName = _skinList[_indexSkin] + "_1";
            red.Initialize(true);
            blue.initialSkinName = _skinList[_indexSkin] + "_2";
            blue.Initialize(true);
            Timing.RunCoroutineSingleton(SpawnFireWork().CancelWith(gameObject),gameObject,SingletonBehavior.Abort);
            StepTwo();

        }

        private void RegisterEvent()
        {
            if(_isInit) return;
            _isInit = true;
            gotItButton.onClick.AddListener(GotIt);
            okButton.onClick.AddListener(()=>
            {
                MenuScene.Instance.UpdateBall();
                Confirm();
            });
            skipButton.onClick.AddListener(Skip);
        }
        private void StepOne()
        {
            gotItButton.gameObject.SetActive(true);
            okButton.gameObject.SetActive(false);
            tryItText.SetActive(true);
            congratText.SetActive(false);
            skipButton.gameObject.SetActive(true);
            skipButton.transform.localScale = Vector3.zero;
            skipButton.transform.DOScale(Vector3.one, 1f).SetDelay(2.5f);
            UserDataController.SetUnlockProgress(_skin);
        }

        private void StepTwo()
        {
           
            okButton.gameObject.SetActive(true);
            gotItButton.gameObject.SetActive(false);
            congratText.SetActive(true);
            skipButton.gameObject.SetActive(false);
            tryItText.SetActive(false);
        }

        private void GotIt()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            NetWorkHelper.ShowRewardedAdInMenu("Rewarded_CongratsPopup_ReceiveSkin", adResult:result =>
            {
                if(!result) return;
                UserDataController.UnlockSkin(_skin);
                UserDataController.UpdateSelectedSkin(_skin);
                MenuScene.Instance.UpdateBall();
                StepTwo();
            });
        }

        private void Skip()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            Close();
        }
        private void Confirm()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            
            if (_skinList == null)
            {
                Close();
                return;
            }
            _indexSkin++;
            if (_indexSkin>=_skinList.Count )
            {
                Close();
            }
            else
            {
                Initialized(_skinList,_indexSkin,closeAction);
            }
        }
        private IEnumerator<float> SpawnFireWork()
        {
            var count = 0;
            while (true)
            {
                yield return Timing.WaitForSeconds(Random.Range(0.2f, 1f));
                var obj = Instantiate(firework,transform);
                count++;
                obj.transform.localScale = new Vector3(100,100,100);
                obj.transform.position = new Vector3(Random.Range(-10f, 10f), Random.Range(2f, 6f), 100);
                if (count <= 5)
                {
                    obj.GetComponent<SimpleAudio>().Play().Forget();
                }
            }
        }
    }
}