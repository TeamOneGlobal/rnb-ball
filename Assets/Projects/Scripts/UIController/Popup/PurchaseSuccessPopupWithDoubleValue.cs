using System;
using Sound;
using ThirdParties.Truongtv;
using TMPro;
using Truongtv.PopUpController;
using UnityEngine;
using UnityEngine.UI;
using UserDataModel;

namespace UIController.Popup
{
    public class PurchaseSuccessPopupWithDoubleValue : BasePopup
    {
       
        [SerializeField] private Button closeButton,doubleButton;

        [SerializeField] private GameObject coinValue,lifeValue, adCoin, adLife, blockAd;
        [SerializeField] private TextMeshProUGUI coinValueText,lifeValueText;

        public void Initialized(PurchaseSuccessPopup.PurchaseType type,long value = 0,Action closeCallback = null)
        {
            SoundMenuController.Instance.PlayCashSound();
            coinValue.SetActive(false);
            lifeValue.SetActive(false);
            adCoin.SetActive(false);
            adLife.SetActive(false);
            blockAd.SetActive(false);
            if (type ==  PurchaseSuccessPopup.PurchaseType.FreeCoin) // coin 
            {
                coinValue.SetActive(true);
                coinValueText.text = "+" + value;
            }
            else if (type == PurchaseSuccessPopup.PurchaseType.FreeLife) // life
            {
                lifeValue.SetActive(true);
                lifeValueText.text = "+" + value;
            }
            else if (type ==  PurchaseSuccessPopup.PurchaseType.BlockAd) // block ad
            {
                blockAd.SetActive(true);
            }
            else if (type ==  PurchaseSuccessPopup.PurchaseType.ComboAdCoin) // ad coin
            {
                adCoin.SetActive(true);
            }
            else if (type ==  PurchaseSuccessPopup.PurchaseType.ComboAdLife) // ad life
            {
                adLife.SetActive(true);
            }
            else if (type ==  PurchaseSuccessPopup.PurchaseType.IapCoin) // iap coin
            {
                coinValue.SetActive(true);
                coinValueText.text = "+" + value;
            }
            else if (type ==  PurchaseSuccessPopup.PurchaseType.IapLife) // iap life
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
                if (type == PurchaseSuccessPopup.PurchaseType.FreeCoin || type == PurchaseSuccessPopup.PurchaseType.IapCoin || type == PurchaseSuccessPopup.PurchaseType.ComboAdCoin)
                {
                    CoinController.Instance.IncreaseCoin(closeButton.transform,value,closeCallback);
                }
                else if (type == PurchaseSuccessPopup.PurchaseType.FreeLife || type == PurchaseSuccessPopup.PurchaseType.IapLife || type == PurchaseSuccessPopup.PurchaseType.ComboAdLife)
                {
                    LifeController.Instance.Addlife(value);
                }
                
            });
            doubleButton.onClick.RemoveAllListeners();
            doubleButton.onClick.AddListener(() =>
            {
                GameServiceManager.Instance.adManager.ShowRewardedAd("menu_double_value", () =>
                {
                    Close();
                    if(SoundMenuController.Instance!=null)
                        SoundMenuController.Instance.PlayButtonClickSound();
                    if (type == PurchaseSuccessPopup.PurchaseType.FreeCoin || type == PurchaseSuccessPopup.PurchaseType.IapCoin || type == PurchaseSuccessPopup.PurchaseType.ComboAdCoin)
                    {
                        CoinController.Instance.IncreaseCoin(closeButton.transform,value*2,closeCallback);
                    }
                    else if (type == PurchaseSuccessPopup.PurchaseType.FreeLife || type == PurchaseSuccessPopup.PurchaseType.IapLife || type == PurchaseSuccessPopup.PurchaseType.ComboAdLife)
                    {
                        LifeController.Instance.Addlife(value*2);
                    }
                });
            });
        }
    }
}