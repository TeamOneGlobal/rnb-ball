
using System;
using UnityEngine.Purchasing;

namespace Truongtv.Services.IAP
{
    [Serializable]
    public class SkuItem
    {
        public string skuId;
        public ProductType productType = ProductType.Consumable;
        public string defaultValue;
    }

    
}