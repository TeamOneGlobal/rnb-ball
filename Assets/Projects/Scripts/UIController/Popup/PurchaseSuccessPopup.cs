using System;
using Sound;
using TMPro;
using Truongtv.PopUpController;
using Truongtv.SoundManager;
using UnityEngine;
using UnityEngine.UI;
using UserDataModel;

namespace UIController.Popup
{
    public class PurchaseSuccessPopup : BasePopup
    {
        [SerializeField] private Button closeButton;

        [SerializeField] private GameObject coinValue,lifeValue, adCoin, adLife, blockAd;
        [SerializeField] private TextMeshProUGUI coinValueText,lifeValueText;

        public void Initialized(PurchaseType type,long value = 0,Action closeCallback = null)
        {
            SoundMenuController.Instance.PlayCashSound();
            coinValue.SetActive(false);
            lifeValue.SetActive(false);
            adCoin.SetActive(false);
            adLife.SetActive(false);
            blockAd.SetActive(false);
            if (type ==  PurchaseType.FreeCoin) // coin 
            {
                coinValue.SetActive(true);
                coinValueText.text = "+" + value;
            }
            else if (type == PurchaseType.FreeLife) // life
            {
                lifeValue.SetActive(true);
                lifeValueText.text = "+" + value;
            }
            else if (type ==  PurchaseType.BlockAd) // block ad
            {
                blockAd.SetActive(true);
            }
            else if (type ==  PurchaseType.ComboAdCoin) // ad coin
            {
                adCoin.SetActive(true);
            }
            else if (type ==  PurchaseType.ComboAdLife) // ad life
            {
                adLife.SetActive(true);
            }
            else if (type ==  PurchaseType.IapCoin) // iap coin
            {
                coinValue.SetActive(true);
                coinValueText.text = "+" + value;
            }
            else if (type ==  PurchaseType.IapLife) // iap life
            {
                lifeValue.SetActive(true);
                lifeValueText.text = "+" + value;
            }
            if ((int)type >= 2)
            {
                UserDataController.SetBlockAd();
            }
            closeButton.onClick.RemoveAllListeners(); 
            closeButton.onClick.AddListener(() =>
            {
                
                Close();
                if(SoundMenuController.Instance!=null)
                    SoundMenuController.Instance.PlayButtonClickSound();
                if(SoundGamePlayController.Instance!=null)
                    SoundGamePlayController.Instance.PlayButtonClickSound();
                if (type == PurchaseType.FreeCoin || type == PurchaseType.IapCoin || type == PurchaseType.ComboAdCoin)
                {
                    CoinController.Instance.IncreaseCoin(closeButton.transform,value,closeCallback);
                }
                else if (type == PurchaseType.FreeLife || type == PurchaseType.IapLife || type == PurchaseType.ComboAdLife)
                {
                    LifeController.Instance.Addlife(value);
                }
                
            });
        }

        public enum PurchaseType
        {
            FreeCoin,FreeLife,BlockAd,ComboAdCoin,ComboAdLife,IapCoin,IapLife
        }
    }
}