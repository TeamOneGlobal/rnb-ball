using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Scriptable;
using Sirenix.OdinInspector;
using Sound;
using Truongtv.PopUpController;
using UIController.Gift;
using UnityEngine;
using UnityEngine.UI;
using UserDataModel;
using Random = System.Random;

namespace UIController.Popup
{
    public class GiftPopup : BasePopup
    {
        [SerializeField] private Transform container;
        [SerializeField] private Button backButton;
        [SerializeField] private List<GiftItem> giftItems;
        [SerializeField] private GiftData giftData;
        private bool _isInit;

        public void Initialized()
        {
            RegisterEvent();
            var giftList = UserDataController.GetGiftList();
            if (giftList?.Count == 0) // first Init
            {
                var rnd = new Random();
                var data = giftData.giftList.OrderBy(x => rnd.Next()).Take(5);
                var saveData = data.Select(variable => giftData.giftList.IndexOf(variable)).ToList();

                UserDataController.SetGiftList(saveData);
                for (var i = 0; i < giftItems.Count; i++)
                {
                    giftItems[i].Init(giftData.giftList[saveData[i]], true);
                }
                OpenAll();
            }
            else
            {
                for (var i = 0; i < giftItems.Count; i++)
                {
                    giftItems[i].Init(giftData.giftList[giftList[i]]);
                }
            }
        }

        [Button]
        private void OpenAll()
        {
            int current = 0;
            int last = -1;
            DOTween.To(() => -1, x => current = x, 4, 3f).SetDelay(1f).SetEase(Ease.InQuad)
                .OnUpdate(() =>
                {
                    Debug.Log(current);
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
            _isInit = true;
            openAction = OnStart;
            closeAction = OnClose;
            backButton.onClick.AddListener(ClosePopUp);
        }

        private void OnStart()
        {
            container.localScale = Vector3.zero;
            SoundMenuController.Instance.PlayPopupSound();
            container.DOScale(1, DURATION).SetUpdate(UpdateType.Normal, true).SetEase(Ease.OutBack);
        }

        private void OnClose()
        {
            SoundMenuController.Instance.PlayPopupSound();
            container.DOScale(0, DURATION).SetUpdate(UpdateType.Normal, true).SetEase(Ease.InBack);
        }

        private void ClosePopUp()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            Close();
        }
    }
}