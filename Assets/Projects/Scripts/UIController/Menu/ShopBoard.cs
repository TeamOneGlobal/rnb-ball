using System.Collections.Generic;
using Projects.Scripts.Data;
using Projects.Scripts.UIController.Popup;
using UnityEngine;

namespace Projects.Scripts.UIController.Menu
{
    public class ShopBoard : MonoBehaviour
    {
        [SerializeField] private PopupShop shop;
        [SerializeField] private List<ShopItem> shopItemList;

        public void Init()
        {
            foreach (var shopItem in shopItemList)
            {
                shopItem.Init(shop);
            }
        }
        public void PurchaseSuccessCallback(ShopItemData item)
        {
            foreach (var shopItem in shopItemList)
            {
                shopItem.OnOtherItemPurchase(item);
            }
        }
    }
}