using Sirenix.OdinInspector;
using Truongtv.Utilities;
using UnityEngine;

namespace Truongtv.Services.IAP
{
    [CreateAssetMenu(fileName = "IAPSetting", menuName = "Truongtv/GameSetting/IAPSetting", order = 1)]
    public class IAPSetting : SingletonScriptableObject<IAPSetting>
    {

        [BoxGroup("Android",CenterLabel = true),ListDrawerSettings(Expanded = true),SerializeField]private SkuItem[] androidSkuId;
        [BoxGroup("IOS",CenterLabel = true),ListDrawerSettings(Expanded = true),SerializeField]private SkuItem[] iosSkuId;
        public SkuItem[] GetSkuItems()
        {
#if UNITY_EDITOR||UNITY_ANDROID
            return androidSkuId;
#elif UNITY_IOS||UNITY_IPHONE
            return iosSkuId;
#endif
        }
        

        


        
    }
    
}
