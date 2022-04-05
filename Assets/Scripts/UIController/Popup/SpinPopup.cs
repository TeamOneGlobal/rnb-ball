using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using MEC;
using Notification;
using Scriptable;
using Sirenix.OdinInspector;
using Sound;
using TMPro;
using Truongtv.PopUpController;
using Truongtv.Utilities;
using UIController.Menu;
using UIController.SpinController;
using UnityEngine;
using UnityEngine.UI;
using UserDataModel;
using Random = UnityEngine.Random;

namespace UIController.Popup
{
    public class SpinPopup : BasePopup
    {
        [SerializeField] private Transform container;
        [SerializeField] private LuckySpinData data;
        [SerializeField] private SpinItem prefab;
        [SerializeField] private List<SpinItem> itemList;
        [SerializeField] private TextMeshProUGUI timeRemain,spinRemain;
        [SerializeField] private Button closeButton, adSpinButton, freeSpinButton;
        [SerializeField] private Color normalColor, coolDownColor;
        [SerializeField] private Material grayScale;
        [SerializeField] private Transform rollObj;
        private bool _isInit;
        private int _lastIndex;
        public void Initialized()
        {
            RegisterEvent();
            _lastIndex = 0;
            if (!UserDataController.IsSpinAvailable()) // no free spin, show countdown
            {
                SetOnCountDown();
            }
            else
            {
                SetFreeSpinAvailable();
            }
            rollObj.RemoveAllChild();
            itemList = new List<SpinItem>();
            for (var i = 0; i < data.data.Count; i++)
            {
                var itemData = data.data[i];
                if (itemData.type == ItemType.Skin &&
                    UserDataController.GetTotalUnlockSKins().Contains(itemData.value.ToString()))
                {
                    itemData.type = ItemType.Coin;
                    itemData.value = 2000;
                }
                var item = Instantiate(prefab, rollObj);
                var transform1 = item.transform;
                transform1.localPosition = Vector3.zero;
                transform1.localScale = Vector3.one;
                transform1.localEulerAngles = new Vector3(0,0,90+i*45);
                item.Init(itemData);
                itemList.Add(item);
            }
        }

        private void SetOnCountDown()
        {
            freeSpinButton.interactable = false;
            spinRemain.text = "FREE SPIN";
            spinRemain.color = coolDownColor;
            freeSpinButton.GetComponent<Image>().material = grayScale;
            timeRemain.color = coolDownColor;
            timeRemain.gameObject.SetActive(true);
            Timing.RunCoroutine(CountDown().CancelWith(gameObject));
        }

        private void SetFreeSpinAvailable()
        {
            timeRemain.gameObject.SetActive(false);
            freeSpinButton.interactable = true;
            spinRemain.text = "FREE SPIN";
            spinRemain.color = normalColor;
            freeSpinButton.GetComponent<Image>().material = null;
        }

        private IEnumerator<float> CountDown()
        {
            var lastTime = UserDataController.GetLastSpinTime();
            int totalSecond;
            do
            {
                var totalWaitSecond = Mathf.FloorToInt((float) DateTime.Now.Subtract(lastTime).TotalSeconds);
                totalSecond = Mathf.FloorToInt((float) TimeSpan.FromHours(Config.FREE_SPIN_COOLDOWN_HOURS)
                    .Subtract(TimeSpan.FromSeconds(totalWaitSecond)).TotalSeconds);
                timeRemain.text = TimeSpan.FromSeconds(totalSecond).ToString(@"hh\:mm\:ss");
                if (totalSecond <= 0)
                {
                    break;
                }
                yield return Timing.WaitForSeconds(1f);
               
            } while (totalSecond > 0);
            
            SetFreeSpinAvailable();
        }
        private void RegisterEvent()
        {
            if(_isInit) return;
            _isInit = true;
            closeButton.onClick.AddListener(OnCloseButtonClick);
            adSpinButton.onClick.AddListener(OnAdSpinButtonClick);
            freeSpinButton.onClick.AddListener(OnFreeSpinButtonClick);
        }

        private void UpdateTimeRemain()
        {
            
        }
        private void OnCloseButtonClick()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            Close();
        }

        private void OnAdSpinButtonClick()
        {
            SoundMenuController.Instance.PlayButtonClickSound();
            NetWorkHelper.ShowRewardedAdInMenu("Rewarded_Spin",adResult: result =>
            {
                if(!result) return;
                adSpinButton.interactable = false;
                UserDataController.Spin(false);
                Spin();
            });
        }

        private void OnFreeSpinButtonClick()
        {
            UserDataController.Spin(true);
            Spin();
            CustomNotification.Instance.SetLuckySpinReminder();
            SetOnCountDown();
            LuckySpinButton.Instance.CheckTime();
        }

        private void Spin()
        {
            int currentIndex = 0;
            if (UserDataController.GetTotalSpin() == 1) // quay lần đầu tặng skin free
            {
                var itemData = data.data.Find(a => a.isFirstReward);
                var index = data.data.IndexOf(itemData);
                currentIndex = index;
            }
            else
            {
               
                var randomIndex = Random.Range(0, data.data.Count);
                while (data.data[randomIndex].neverReward)
                {
                    randomIndex = Random.Range(0, data.data.Count);
                }
               
                currentIndex = randomIndex;
            }
            var angle = -  currentIndex* 45-3600;
            var tempAngle = rollObj.transform.localEulerAngles.z;
           
            rollObj.DOLocalRotate(new Vector3(0, 0, angle), 5f,RotateMode.FastBeyond360)
                .SetRelative(false)
                .SetEase(Ease.InOutCubic)
                .OnUpdate(() =>
                {
                    var delta = tempAngle - rollObj.transform.localEulerAngles.z;
                    if (delta <= -45f)
                    {
                        tempAngle = rollObj.transform.localEulerAngles.z;
                        SoundMenuController.Instance.PlaySpinSound();
                    }
                    else if (delta >= 45f)
                    {
                        tempAngle = rollObj.transform.localEulerAngles.z;
                        SoundMenuController.Instance.PlaySpinSound();
                    }
                })
                .OnComplete(()=>ShowResult(currentIndex));
            _lastIndex = currentIndex;
        }

        private void ShowResult(int index)
        {
            adSpinButton.interactable = true;
            var spinItem = data.data[index];
            if (spinItem.type == ItemType.Skin)
            {
                MenuPopupController.Instance.ShowCongratsPopup(spinItem.value.ToString());
            }
            else if(spinItem.type == ItemType.Coin)
            {
                MenuPopupController.Instance.ShowPurchaseSuccessDoubleValue(PurchaseSuccessPopup.PurchaseType.FreeCoin,spinItem.value);
            }
            else
            {
                MenuPopupController.Instance.ShowPurchaseSuccessDoubleValue(PurchaseSuccessPopup.PurchaseType.FreeLife,spinItem.value);
            }
        }
    }
}