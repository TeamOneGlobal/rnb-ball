#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Truongtv.Utilities;
using UnityEngine;

namespace Truongtv.Services.General
{
    
    [CreateAssetMenu(fileName = "ServiceSetting", menuName = "Truongtv/GameSetting/ServiceSetting", order = 1)]
    public class ServiceSetting:SingletonScriptableObject<ServiceSetting>
    {
        [BoxGroup("Addressable"),ToggleLeft] public bool usingAddressable;
        [BoxGroup("In App Review"),ToggleLeft] public bool usingInAppReview;
        [BoxGroup("Unity Remote Config"),ToggleLeft] public bool usingUnityRemoteConfig;
        [BoxGroup("Facebook"),ToggleLeft] public bool usingFacebookService;
        [BoxGroup("In App Purchase"),ToggleLeft] public bool usingIAPService;
        [BoxGroup("In App Purchase"),EnableIf("usingIAPService"),SerializeField] private IapType iapType;
        [BoxGroup("Advertising"),ToggleLeft] public bool usingAdService;
        [BoxGroup("Advertising"),EnableIf("usingAdService"),SerializeField] private AdType adType;
        [BoxGroup("Firebase"),ToggleLeft] public bool usingFirebaseService;
        [BoxGroup("Firebase"),ToggleLeft,EnableIf("usingFirebaseService")] public bool usingFirebaseAuth;
        [BoxGroup("Firebase"),ToggleLeft,EnableIf("usingFirebaseService")] public bool usingFirebaseAnalytics;
        [BoxGroup("Firebase"),ToggleLeft,EnableIf("usingFirebaseService")] public bool usingFirebaseRemoteConfig;
        [BoxGroup("Firebase"),ToggleLeft,EnableIf("usingFirebaseService")] public bool usingFirebaseDatabase;
        [BoxGroup("Firebase"),ToggleLeft,EnableIf("usingFirebaseService")] public bool usingFirebaseMessaging;

        private List<string> _defineSymbolList;
        [ResponsiveButtonGroup]
        private void Save()
        {
            _defineSymbolList = DefineSymbol.GetAllDefineSymbols();
            RemoveAll();
            if (usingAddressable)_defineSymbolList.Add(DefineSymbol.ADDRESSABLE_SYMBOL);
            if(usingInAppReview) _defineSymbolList.Add(DefineSymbol.IN_APP_REVIEW_SYMBOL);
            if(usingUnityRemoteConfig) _defineSymbolList.Add(DefineSymbol.UNITY_REMOTE_CONFIG_SYMBOL);
            if(usingFacebookService) _defineSymbolList.Add(DefineSymbol.FACEBOOK_SYMBOL);
            if (usingIAPService)
            {
                _defineSymbolList.Add(DefineSymbol.IN_APP_PURCHASE_SYMBOL);
                if (iapType == IapType.IAP)_defineSymbolList.Add(DefineSymbol.IAP_SYMBOL);
                else if (iapType == IapType.UDP) _defineSymbolList.Add(DefineSymbol.UDP_SYMBOL);
            }

            if (usingAdService)
            {
                _defineSymbolList.Add(DefineSymbol.AD_SYMBOL);
                if(adType== AdType.AdMob) _defineSymbolList.Add(DefineSymbol.AD_MOB_SYMBOL);
                else if(adType== AdType.IronSource) _defineSymbolList.Add(DefineSymbol.IRON_SOURCE_SYMBOL);
            }

            if (usingFirebaseService)
            {
                _defineSymbolList.Add(DefineSymbol.FIREBASE_SYMBOL);
                if(usingFirebaseAuth) _defineSymbolList.Add(DefineSymbol.FIREBASE_AUTH_SYMBOL);
                if(usingFirebaseAnalytics) _defineSymbolList.Add(DefineSymbol.FIREBASE_ANALYTICS_SYMBOL);
                if(usingFirebaseRemoteConfig) _defineSymbolList.Add(DefineSymbol.UNITY_REMOTE_CONFIG_SYMBOL);
                if(usingFirebaseDatabase) _defineSymbolList.Add(DefineSymbol.FIREBASE_DATABASE_SYMBOL);
                if(usingFirebaseMessaging) _defineSymbolList.Add(DefineSymbol.FIREBASE_MESSAGING_SYMBOL);
            }

            DefineSymbol.UpdateDefineSymbols(_defineSymbolList);
        }
        [ResponsiveButtonGroup]
        private void Cancel()
        {
            Init();
        }

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            _defineSymbolList = DefineSymbol.GetAllDefineSymbols();
            usingAddressable = _defineSymbolList.Contains(DefineSymbol.ADDRESSABLE_SYMBOL);
            usingUnityRemoteConfig = _defineSymbolList.Contains(DefineSymbol.UNITY_REMOTE_CONFIG_SYMBOL);
            usingFacebookService = _defineSymbolList.Contains(DefineSymbol.FACEBOOK_SYMBOL);
            usingInAppReview = _defineSymbolList.Contains(DefineSymbol.IN_APP_REVIEW_SYMBOL);
            usingAdService = _defineSymbolList.Contains(DefineSymbol.AD_SYMBOL);
            if (_defineSymbolList.Contains(DefineSymbol.AD_MOB_SYMBOL)) adType = AdType.AdMob;
            else if (_defineSymbolList.Contains(DefineSymbol.IRON_SOURCE_SYMBOL)) adType = AdType.IronSource;
            else adType = AdType.None;
            
            usingIAPService = _defineSymbolList.Contains(DefineSymbol.IN_APP_PURCHASE_SYMBOL);
            if (_defineSymbolList.Contains(DefineSymbol.IAP_SYMBOL)) iapType = IapType.IAP;
            else if (_defineSymbolList.Contains(DefineSymbol.UDP_SYMBOL)) iapType = IapType.UDP;
            else iapType = IapType.None;
            usingFirebaseService = _defineSymbolList.Contains(DefineSymbol.FIREBASE_SYMBOL);
            usingFirebaseAuth = _defineSymbolList.Contains(DefineSymbol.FIREBASE_AUTH_SYMBOL);
            usingFirebaseAnalytics = _defineSymbolList.Contains(DefineSymbol.FIREBASE_ANALYTICS_SYMBOL);
            usingFirebaseRemoteConfig = _defineSymbolList.Contains(DefineSymbol.FIREBASE_REMOTE_CONFIG_SYMBOL);
            usingFirebaseDatabase = _defineSymbolList.Contains(DefineSymbol.FIREBASE_DATABASE_SYMBOL);
            usingFirebaseMessaging = _defineSymbolList.Contains(DefineSymbol.FIREBASE_MESSAGING_SYMBOL);
        }
        private enum IapType
        {
            None,IAP,UDP
        }

        private enum AdType
        {
            None,AdMob,IronSource
        }

        private void RemoveAll()
        {
            _defineSymbolList.Remove(DefineSymbol.ADDRESSABLE_SYMBOL);
            _defineSymbolList.Remove(DefineSymbol.FACEBOOK_SYMBOL);
            _defineSymbolList.Remove(DefineSymbol.UNITY_REMOTE_CONFIG_SYMBOL);
            _defineSymbolList.Remove(DefineSymbol.IN_APP_REVIEW_SYMBOL);
            _defineSymbolList.Remove(DefineSymbol.AD_SYMBOL);
            _defineSymbolList.Remove(DefineSymbol.AD_MOB_SYMBOL);
            _defineSymbolList.Remove(DefineSymbol.IRON_SOURCE_SYMBOL);
            _defineSymbolList.Remove(DefineSymbol.IN_APP_PURCHASE_SYMBOL);
            _defineSymbolList.Remove(DefineSymbol.IAP_SYMBOL);
            _defineSymbolList.Remove(DefineSymbol.UDP_SYMBOL);
            _defineSymbolList.Remove(DefineSymbol.FIREBASE_SYMBOL);
            _defineSymbolList.Remove(DefineSymbol.FIREBASE_AUTH_SYMBOL);
            _defineSymbolList.Remove(DefineSymbol.FIREBASE_ANALYTICS_SYMBOL);
            _defineSymbolList.Remove(DefineSymbol.FIREBASE_REMOTE_CONFIG_SYMBOL);
            _defineSymbolList.Remove(DefineSymbol.FIREBASE_DATABASE_SYMBOL);
            _defineSymbolList.Remove(DefineSymbol.FIREBASE_MESSAGING_SYMBOL);
        }
    }
}
#endif