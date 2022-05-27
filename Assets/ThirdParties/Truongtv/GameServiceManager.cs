using System;

using Sirenix.OdinInspector;
using ThirdParties.Truongtv.AdsManager;
using ThirdParties.Truongtv.LogManager;
using ThirdParties.Truongtv.Notification;
using ThirdParties.Truongtv.Rating;
using ThirdParties.Truongtv.RemoteConfig;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
using System.IO;
using System.Xml;
using ThirdParties.Truongtv.Utilities;
#endif
#if USING_LOG_FIREBASE||USING_REMOTE_FIREBASE
using Firebase;
using Firebase.Extensions;
#endif
namespace ThirdParties.Truongtv
{
    [RequireComponent(typeof(AdManager))]
    [RequireComponent(typeof(LogEventManager))]
    [RequireComponent(typeof(RatingHelper))]
    [RequireComponent(typeof(RemoteConfigManager))]
    [RequireComponent(typeof(MobileNotification))]
    [RequireComponent(typeof(IapManager.IapManager))]
    public class GameServiceManager : MonoBehaviour
    {
    #if UNITY_EDITOR
        [SerializeField]
        private AdService adService;
        [SerializeField]
        private LogService logService;
        [SerializeField]
        private RemoteConfigService remoteConfigService;
        [SerializeField]
        private RatingService ratingService;
        [SerializeField]
        private CloudMessagingService cloudMessagingService;
        [SerializeField]
        private IapService iapService;
#endif
        [HideInInspector] public AdManager adManager;
        [HideInInspector] public LogEventManager logEventManager;
        [HideInInspector] public RatingHelper ratingHelper;
        [HideInInspector] public RemoteConfigManager remoteConfigManager;
        [HideInInspector] public MobileNotification mobileNotification;
        [HideInInspector] public IapManager.IapManager iapManager;
        public static Action<RemoteConfigManager> FetchComplete;
        
        public static GameServiceManager Instance;

        public void Awake()
        {
            if (Instance != null)
                Destroy(Instance.gameObject);
            Instance = this;
            if (Application.isPlaying)
                DontDestroyOnLoad(gameObject);
            adManager = GetComponent<AdManager>();
            logEventManager = GetComponent<LogEventManager>();
            ratingHelper = GetComponent<RatingHelper>();
            remoteConfigManager = GetComponent<RemoteConfigManager>();
            mobileNotification = GetComponent<MobileNotification>();
            iapManager = GetComponent<IapManager.IapManager>();
        }

        private void Start()
        {
            remoteConfigManager.fetchComplete += FetchComplete;
            adManager.Init();
            iapManager.Init();
            #if USING_LOG_FIREBASE||USING_REMOTE_FIREBASE
            Debug.Log("co vao service firebase");
             FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                remoteConfigManager.Init();
                logEventManager.Init();
                mobileNotification.Init();
            });
            #else
            remoteConfigManager.Init();
            logEventManager.Init();
            mobileNotification.Init();
            #endif
            
        }
#if UNITY_EDITOR
        [OnInspectorGUI] private void Space2() { GUILayout.Space(20); }
        [Button(ButtonSizes.Large), GUIColor(0, 1, 0), HideIf(nameof(IsServiceUpToDate))] [InitializeOnLoadMethod]
        private void UpdateService()
        {
            if(IsServiceUpToDate()) return;
            var symbolList = DefineSymbol.GetAllDefineSymbols();
            symbolList.Remove(DefineSymbol.MaxSymbol);
            symbolList.Remove(DefineSymbol.AdMobSymbol);
            symbolList.Remove(DefineSymbol.IronSourceSymbol);
            symbolList.Remove(DefineSymbol.LogUnity);
            symbolList.Remove(DefineSymbol.LogFirebase);
            symbolList.Remove(DefineSymbol.RemoteFirebase);
            symbolList.Remove(DefineSymbol.RemoteUnity);
            symbolList.Remove(DefineSymbol.InAppReview);
            symbolList.Remove(DefineSymbol.FirebaseMessaging);
            symbolList.Remove(DefineSymbol.IAP);
            symbolList.Remove(DefineSymbol.UDP);
            switch (adService)
            {
                case AdService.None:
                    break;
                case AdService.AdMob:
                    symbolList.Add(DefineSymbol.AdMobSymbol);
                    break;
                case AdService.IronSource:

                    symbolList.Add(DefineSymbol.IronSourceSymbol);
                    break;
                case AdService.Max:

                    symbolList.Add(DefineSymbol.MaxSymbol);
                    break;
            }

            switch (logService)
            {
                case LogService.None:
                    break;
                case LogService.Firebase:
                    symbolList.Add(DefineSymbol.LogFirebase);
                    break;
                case LogService.Unity:
                    symbolList.Add(DefineSymbol.LogUnity);
                    break;
            }

            switch (remoteConfigService)
            {
                case RemoteConfigService.None:
                    break;
                case RemoteConfigService.Firebase:
                    symbolList.Add(DefineSymbol.RemoteFirebase);
                    break;
                case RemoteConfigService.Unity:
                    symbolList.Add(DefineSymbol.RemoteUnity);
                    break;
            }

            if (ratingService == RatingService.InApp)
            {
                symbolList.Add(DefineSymbol.InAppReview);
            }

            if (cloudMessagingService == CloudMessagingService.Firebase)
            {
                symbolList.Add(DefineSymbol.FirebaseMessaging);
            }

            switch (iapService)
            {
                case IapService.None:
                    break;
                case IapService.IAP:
                    symbolList.Add(DefineSymbol.IAP);
                    break;
                case IapService.UDP:
                    symbolList.Add(DefineSymbol.UDP);
                    break;
            }

            DefineSymbol.UpdateDefineSymbols(symbolList);
        }

        private bool IsServiceUpToDate()
        {
            var symbolList = DefineSymbol.GetAllDefineSymbols();
            switch (adService)
            {
                case AdService.None:
                    if (symbolList.Contains(DefineSymbol.AdMobSymbol)) return false;
                    if (symbolList.Contains(DefineSymbol.IronSourceSymbol)) return false;
                    if (symbolList.Contains(DefineSymbol.MaxSymbol)) return false;
                    break;
                case AdService.AdMob:
                    if (!symbolList.Contains(DefineSymbol.AdMobSymbol)) return false;

                    break;
                case AdService.IronSource:
                    if (!symbolList.Contains(DefineSymbol.IronSourceSymbol)) return false;
                    break;
                case AdService.Max:
                    if (!symbolList.Contains(DefineSymbol.MaxSymbol)) return false;
                    break;
            }

            switch (logService)
            {
                case LogService.None:
                    if (symbolList.Contains(DefineSymbol.LogFirebase)) return false;
                    if (symbolList.Contains(DefineSymbol.LogUnity)) return false;
                    break;
                case LogService.Firebase:

                    if (!symbolList.Contains(DefineSymbol.LogFirebase)) return false;
                    break;
                case LogService.Unity:
                    if (!symbolList.Contains(DefineSymbol.LogUnity)) return false;
                    break;
            }

            switch (remoteConfigService)
            {
                case RemoteConfigService.None:
                    if (symbolList.Contains(DefineSymbol.RemoteFirebase)) return false;
                    if (symbolList.Contains(DefineSymbol.RemoteUnity)) return false;
                    break;
                case RemoteConfigService.Firebase:
                    if (!symbolList.Contains(DefineSymbol.RemoteFirebase)) return false;
                    break;
                case RemoteConfigService.Unity:
                    if (!symbolList.Contains(DefineSymbol.RemoteUnity)) return false;
                    break;
            }

            switch (ratingService)
            {
                case RatingService.OpenLink:
                    if (symbolList.Contains(DefineSymbol.InAppReview)) return false;
                    break;
                case RatingService.InApp:
                    if (!symbolList.Contains(DefineSymbol.InAppReview)) return false;
                    break;
            }

            switch (cloudMessagingService)
            {
                case CloudMessagingService.None:
                    if (symbolList.Contains(DefineSymbol.FirebaseMessaging)) return false;
                    break;
                case CloudMessagingService.Firebase:
                    if (!symbolList.Contains(DefineSymbol.FirebaseMessaging)) return false;
                    break;
            }

            switch (iapService)
            {
                case IapService.None:
                    if (symbolList.Contains(DefineSymbol.IAP)) return false;
                    if (symbolList.Contains(DefineSymbol.UDP)) return false;
                    break;
                case IapService.IAP:
                    if (!symbolList.Contains(DefineSymbol.IAP)) return false;
                    break;
                case IapService.UDP:
                    if (!symbolList.Contains(DefineSymbol.UDP)) return false;
                    break;
            }

            return true;
        }
#endif
    }

    enum LogService
    {
        None,
        Firebase,
        Unity
    }

    enum AdService
    {
        None,
        AdMob,
        IronSource,
        Max
    }

    enum RemoteConfigService
    {
        None,
        Firebase,
        Unity
    }

    enum RatingService
    {
        OpenLink,
        InApp
    }

    enum CloudMessagingService
    {
        None,Firebase
    }

    enum IapService
    {
        None,IAP,UDP
    }
    enum AssetBundleDelivery
    {
        None,Store,Custom
    }
}