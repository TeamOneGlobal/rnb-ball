#if USING_UNITY_REMOTE_CONFIG
using Unity.RemoteConfig;
#endif
using System;
using Truongtv.Utilities;
using UnityEngine;

namespace Truongtv.Services.UnityRemoteConfig
{
    public class UnityRemoteConfigController : SingletonMonoBehavior<UnityRemoteConfigController>
    {
        #if USING_UNITY_REMOTE_CONFIG
        private struct T1 {};
        private struct T2 {};
        public ConfigResponse remoteStatus;
        public override void Awake()
        {
            base.Awake();
           
        }

        private void Start()
        {
            ConfigManager.FetchCompleted += ApplyRemoteSettings;
            ConfigManager.SetCustomUserID("some-user-id");
            ConfigManager.SetEnvironmentID("72a6f8fe-d9e6-4b0f-871a-19c4ef72e442");
            
            InvokeRepeating(nameof(Fetch),0f,30f);
            
        }

        void Fetch()
        {
            ConfigManager.FetchConfigs<T1,T2>(new T1(), new T2());
        }
        void ApplyRemoteSettings (ConfigResponse configResponse) {
            // Conditionally update settings, depending on the response's origin:
            remoteStatus = configResponse;
            
            switch (configResponse.requestOrigin) {
                case ConfigOrigin.Default:
                    Debug.Log ("No settings loaded this session; using default values.");
                    break;
                case ConfigOrigin.Cached:
                    Debug.Log ("No settings loaded this session; using cached values from a previous session.");
                    Debug.Log(ConfigManager.appConfig.GetInt("demo"));
                    Debug.Log(ConfigManager.appConfig.GetString("hero"));
                    break;
                case ConfigOrigin.Remote:
                    Debug.Log(ConfigManager.appConfig.assignmentID);
                    Debug.Log ("New settings loaded this session; update values accordingly.");
                    Debug.Log(ConfigManager.appConfig.GetInt("demo"));
                    Debug.Log(ConfigManager.appConfig.GetString("hero"));
                    break;
            }
        }
        #endif
    }
}