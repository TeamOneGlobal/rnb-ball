using Sirenix.OdinInspector;
using Truongtv.Utilities;
using UnityEngine;

namespace Truongtv.Services.Ad
{
    [CreateAssetMenu(fileName = "AdSetting", menuName = "Truongtv/GameSetting/AdSetting", order = 1)]
    public class AdSetting:SingletonScriptableObject<AdSetting>
    {
#if USING_ADS
        #if USING_IRON_SOURCE
        [Title("Android",TitleAlignment = TitleAlignments.Left,HorizontalLine = true,Bold = true)]
        [SerializeField] private string androidAppKey;
        [SerializeField] private string[] androidRewardedVideoPlacements;
        [SerializeField] private string[] androidInterstitialPlacements;
        [OnInspectorGUI] private void Space1() { GUILayout.Space(100); }
        [Title("IOS",TitleAlignment = TitleAlignments.Left,HorizontalLine = true,Bold = true)]
        [SerializeField] private string iOSAppKey;
        [SerializeField] private string[] iOSRewardedVideoPlacements;
        [SerializeField] private string[] iOSInterstitialPlacements;

        public string GetAppKey()
        {
#if UNITY_EDITOR||UNITY_ANDROID
            return androidAppKey;
#elif UNITY_IPHONE||UNITY_IOS
            return iOSAppKey;
#endif
        }
        public string[] GetInterstitialPlacement()
        {
#if UNITY_EDITOR||UNITY_ANDROID
            return androidInterstitialPlacements;
#elif UNITY_IPHONE||UNITY_IOS
            return iOSInterstitialPlacements;
#endif
        }
        public string[] GetVideoRewardPlacement()
        {
#if UNITY_EDITOR||UNITY_ANDROID
            return androidRewardedVideoPlacements;
#elif UNITY_IPHONE||UNITY_IOS
            return iOSRewardedVideoPlacements;
#endif
        }
        #endif

#if USING_ADMOB
        
#endif
#endif

    }
}