using System.Collections.Generic;
using System.Linq;
using Com.LuisPedroFonseca.ProCamera2D;
using DG.Tweening;
using Projects.Scripts.UIController.Menu;
using ThirdParties.Truongtv.SoundManager;
using Truongtv.PopUpController;
using UnityEngine;
using UnityEngine.UI;
using UpdateType = DG.Tweening.UpdateType;

namespace Projects.Scripts.UIController
{
    public class PopupSelectLevel : BasePopup
    {
        [SerializeField] private Transform container;
        [SerializeField] private List<LevelItem> itemList;
        [SerializeField] private Button closeButton, nextButton, preButton,playButton;
        [SerializeField] private GameObject pageIconPrefab;
        [SerializeField] private List<GameObject> pageIconList,pageIconDisplay;
        private int _currentPage,_totalPage;
        private bool _isInit;
        private int numberItemOnPage = 15;
        public void Initialized()
        {
            RegisterEvent();
            _currentPage = Mathf.FloorToInt((GameDataManager.Instance.GetCurrentLevel()*1f-1) / numberItemOnPage);
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
            playButton.onClick.AddListener(OnPlayButtonClick);
        }
        private void OnStart()
        {
            container.localScale = Vector3.zero;
            SoundManager.Instance.PlayPopupOpenSound();
            container.DOScale(1, DURATION).SetUpdate(UpdateType.Normal,true).SetEase(Ease.OutBack);
        }

        private void OnClose()
        {
            SoundManager.Instance.PlayPopupCloseSound();
            container.DOScale(0, DURATION).SetUpdate(UpdateType.Normal,true).SetEase(Ease.InBack);
        }

        private void ClosePopUp()
        {
            SoundManager.Instance.PlayButtonSound();
            Close();
        }

        private void OnPrevClick()
        {
            if (_currentPage == 0)
                return;
            _currentPage--;
            InitListItem();
        }

        private void OnNextClick()
        {
            Debug.Log("_currentPage = "+_currentPage);
            Debug.Log("_totalPage = "+_totalPage);
            if (_currentPage == _totalPage-1)
            {
                return;
            }
            _currentPage++;
            InitListItem();
        }

        private void InitListItem()
        {
            
            if (GameDataManager.Instance.maxLevel % numberItemOnPage == 0)
            {
                _totalPage = GameDataManager.Instance.maxLevel / numberItemOnPage;
            }
            else
            {
                _totalPage = Mathf.CeilToInt(GameDataManager.Instance.maxLevel * 1f / numberItemOnPage);
            }
            var currentLevel = GameDataManager.Instance.GetCurrentLevel();
            for (var i = 0; i <itemList.Count; i++)
            {
                var level = _currentPage * numberItemOnPage + i + 1;
                var levelStar = GameDataManager.Instance.GetLevelStar(level);
                if (level <= GameDataManager.Instance.maxLevel)
                {
                    itemList[i].gameObject.SetActive(true);
                    itemList[i].Init(level,currentLevel,levelStar,false);
                }
                else
                    itemList[i].gameObject.SetActive(false);
            }

            
            for (var i = 0; i < pageIconList.Count; i++)
            {
                pageIconList[i].SetActive(i<_totalPage);
            }
            for (var i = 0; i < pageIconDisplay.Count; i++)
            {
                pageIconDisplay[i].SetActive(false);
            }
            if (_totalPage <= pageIconDisplay.Count)
            {
                pageIconDisplay[_currentPage].SetActive(true);
            }
            else
            {
                if (_currentPage < pageIconDisplay.Count)
                {
                    pageIconDisplay[_currentPage].SetActive(true);
                }
                else
                {
                    if (_currentPage == _totalPage)
                    {
                        pageIconDisplay.Last().SetActive(true);
                    }
                    else
                    {
                        pageIconDisplay[pageIconDisplay.Count-2].SetActive(true);
                    }
                }
            }
        }

        private void OnPlayButtonClick()
        {
            SoundManager.Instance.PlayButtonSound();
            if (!GameDataManager.Instance.CanPlayWithoutInternet())
            {
                PopupController.Instance.ShowNoInternet();
                return;
            }
            ProCamera2DTransitionsFX.Instance.OnTransitionExitEnded += () =>
            {
                var level = GameDataManager.Instance.GetCurrentLevel();
                if (level > GameDataManager.Instance.maxLevel)
                    level = GameDataManager.Instance.maxLevel;
                LoadSceneController.LoadLevel(level);
            };
            ProCamera2DTransitionsFX.Instance.TransitionExit();
        }
    }
}