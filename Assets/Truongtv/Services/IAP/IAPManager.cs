using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Truongtv.Utilities;
using UnityEngine;

namespace Truongtv.Services.IAP
{
    public class IAPManager :SingletonMonoBehavior<IAPManager>
    {
        private IPaymentService _paymentService;
        private Action<bool, string> _purchaseCallback;
        [SerializeField] private IAPSetting settings;
        
        
        #region Private Function

        public override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }
        private void Start()
        {
            Initialized();
        }

        private void Initialized()
        {
            var SkuItem = settings.GetSkuItems();
            _paymentService = new UnityInAppPurchase(SkuItem);
            _paymentService.UpdateCallback(PurchaseActionCallback);
        }
        private void PurchaseActionCallback(bool isSuccess, string sku)
        {
            if (_purchaseCallback == null) return;
            _purchaseCallback?.Invoke(isSuccess, sku);
            _purchaseCallback = null;
        }
        

        #endregion

        #region Public Function

        private bool IsInitialized()
        {
            return _paymentService.IsInitialized();
        }
        public void PurchaseProduct(string sku, Action<bool, string> pAction)
        {
           #if UNITY_EDITOR
            pAction?.Invoke(true,sku);
            return;
            #endif
            
            if (!IsInitialized())
            {
                Initialized();
            }
            Act();
            async UniTaskVoid Act()
            {
                await UniTask.WaitUntil(IsInitialized);
                _purchaseCallback = pAction;
                _paymentService.PurchaseProduct(sku);
            }
           
        }
        public string GetItemLocalPriceString(string sku)
        {
            #if UNITY_EDITOR
            var list2 = settings.GetSkuItems().ToList();
            return list2.Find(a => a.skuId.Equals(sku)).defaultValue;
            #endif
            if (!IsInitialized()|| Application.internetReachability == NetworkReachability.NotReachable)
            {
                Initialized();
                var list = settings.GetSkuItems().ToList();
                return list.Find(a => a.skuId.Equals(sku)).defaultValue;
            }
            return _paymentService.GetItemLocalPriceString(sku);
        }
        public void RestorePurchase()
        {
            _paymentService.RestorePurchase();
        }


        public void PurchaseBlockAd(Action<bool, string> pAction)
        {
            //PurchaseProduct()
        }

        public void PurchaseComboAdCoin()
        {
            
        }

        public void PurchaseComboAdLife()
        {
            
        }
        #endregion
        

    }
}
