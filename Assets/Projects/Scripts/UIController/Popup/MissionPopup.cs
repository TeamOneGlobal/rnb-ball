using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GamePlay;
using Sound;
using Truongtv.PopUpController;
using UIController.Misson;
using UnityEngine;
using UnityEngine.UI;
using UserDataModel;

namespace UIController.Popup
{
    public class MissionPopup : BasePopup
    {
        [SerializeField] private Transform container;
        [SerializeField] private List<MissionItem> itemList;
        [SerializeField] private Button closeButton, nextButton, preButton;
        [SerializeField] private Material grayScale;
        [SerializeField] private int _currentPage;
        private bool _isInit;

        public void Initialized()
        {
            RegisterEvent();
            _currentPage = Mathf.FloorToInt((UserDataController.GetCurrentLevel()-1) / 12f);
            InitListItem();
        }

        private void RegisterEvent()
        {
            if(_isInit) return;
            _isInit = true;
            openAction = OnStart;
            closeAction = OnClose;
            closeButton.onClick.AddListener(ClosePopUp);
            nextButton.onClick.AddListener(OnNextClick);
            preButton.onClick.AddListener(OnPrevClick);
            
        }
        private void OnStart()
        {
            container.localScale = Vector3.zero;
            SoundMenuController.Instance.PlayPopupSound();
            container.DOScale(1, DURATION).SetUpdate(UpdateType.Normal,true).SetEase(Ease.OutBack);
        }

        private void OnClose()
        {
            SoundMenuController.Instance.PlayPopupSound();
            container.DOScale(0, DURATION).SetUpdate(UpdateType.Normal,true).SetEase(Ease.InBack);
        }

        private void ClosePopUp()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            Close();
        }

        private void OnPrevClick()
        {
            _currentPage--;
            if (_currentPage == 0)
            {
                preButton.interactable = false;
                preButton.GetComponent<Image>().material = grayScale;
            }
            nextButton.interactable = true;
            nextButton.GetComponent<Image>().material = null;
            InitListItem();
        }

        private void OnNextClick()
        {
            _currentPage++;
            if (_currentPage == Mathf.FloorToInt((Config.MAX_LEVEL-1)/12f))
            {
                nextButton.interactable = false;
                nextButton.GetComponent<Image>().material = grayScale;
            }
            preButton.interactable = true;
            preButton.GetComponent<Image>().material = null;
            InitListItem();
        }

        private void InitListItem()
        {
            for (var i = 0; i <itemList.Count; i++)
            {
                itemList[i].Init(_currentPage*12+i+1);
            }
        }
    }
}