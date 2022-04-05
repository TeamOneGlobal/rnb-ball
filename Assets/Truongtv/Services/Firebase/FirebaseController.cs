using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.Messaging;
using Firebase.RemoteConfig;
using Truongtv.Utilities;
using UnityEngine;

namespace Truongtv.Services.Firebase
{
    public class FirebaseController : SingletonMonoBehavior<FirebaseController>
    {
        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                InitRemoteConfig();
                FirebaseMessaging.TokenReceived += OnTokenReceived;
                FirebaseMessaging.MessageReceived += OnMessageReceived;
            });
        }

        private void OnTokenReceived(object sender, TokenReceivedEventArgs token) {
            Debug.Log("Received Registration Token: " + token.Token);
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e) {
            Debug.Log("Received a new message from: " + e.Message.From);
        }

        private void InitRemoteConfig()
        {
            var defaults =new  Dictionary<string, object>();
            defaults.Add("INTERSTITIAL_MIN_LEVEL",3);
            defaults.Add("LEVEL_PER_INTERSTITIAL",2);
            defaults.Add("INTERSTITIAL_BLOCK_TIME",30);
            FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults);
            FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero).ContinueWithOnMainThread(FetchComplete);
        }
        private void FetchComplete(Task fetchTask) {
            if (fetchTask.IsCanceled) {
                Debug.Log("Fetch canceled.");
            } else if (fetchTask.IsFaulted) {
                Debug.Log("Fetch encountered an error.");
            } else if (fetchTask.IsCompleted) {
                Debug.Log("Fetch completed successfully!");
            }
            var info = FirebaseRemoteConfig.DefaultInstance.Info;
            switch (info.LastFetchStatus) {
                case LastFetchStatus.Success:
                    FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                        .ContinueWithOnMainThread(task => {
                            Debug.Log(string.Format("Remote data loaded and ready (last fetch time {0}).",
                                info.FetchTime));
                            OnFetchComplete();
                        });

                    break;
                case LastFetchStatus.Failure:
                    switch (info.LastFetchFailureReason) {
                        case FetchFailureReason.Error:
                            Debug.Log("Fetch failed for unknown reason");
                            break;
                        case FetchFailureReason.Throttled:
                            Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                            break;
                    }
                    break;
                case LastFetchStatus.Pending:
                    Debug.Log("Latest Fetch call still pending.");
                    break;
            }
        }

        private void OnFetchComplete()
        {
            Config.INTERSTITIAL_BLOCK_TIME =
                FirebaseRemoteConfig.DefaultInstance.GetValue("INTERSTITIAL_BLOCK_TIME").LongValue;
            Config.INTERSTITIAL_MIN_LEVEL =
                FirebaseRemoteConfig.DefaultInstance.GetValue("INTERSTITIAL_MIN_LEVEL").LongValue;
            Config.LEVEL_PER_INTERSTITIAL =
                FirebaseRemoteConfig.DefaultInstance.GetValue("LEVEL_PER_INTERSTITIAL").LongValue;
            Config.WIN_REWARD_COIN =
                FirebaseRemoteConfig.DefaultInstance.GetValue("WIN_REWARD_COIN").LongValue;
            Config.COIN_VALUE_IN_GAME =
                FirebaseRemoteConfig.DefaultInstance.GetValue("COIN_VALUE_IN_GAME").LongValue;
            Config.REWARDED_MAGNET_DURATION =
                FirebaseRemoteConfig.DefaultInstance.GetValue("REWARDED_MAGNET_DURATION").LongValue;
            Config.REWARDED_FREE_COIN =
                FirebaseRemoteConfig.DefaultInstance.GetValue("REWARDED_FREE_COIN").LongValue;
            Config.REWARDED_FREE_LIFE =
                FirebaseRemoteConfig.DefaultInstance.GetValue("REWARDED_FREE_LIFE").LongValue;
            Config.FREE_MAGNET_DURATION =
                FirebaseRemoteConfig.DefaultInstance.GetValue("FREE_MAGNET_DURATION").LongValue;
            Config.FREE_SPIN_COOLDOWN_HOURS =
                FirebaseRemoteConfig.DefaultInstance.GetValue("FREE_SPIN_COOLDOWN_HOURS").LongValue;
            Config.SHOW_REVIEW_AFTER_LEVEL =
                FirebaseRemoteConfig.DefaultInstance.GetValue("SHOW_REVIEW_AFTER_LEVEL").LongValue;
            Config.SHOW_REWARD_AFTER_LEVEL =
                FirebaseRemoteConfig.DefaultInstance.GetValue("SHOW_REWARD_AFTER_LEVEL").LongValue;
            Config.SHOW_GIFT_AFTER_LEVEL =
                FirebaseRemoteConfig.DefaultInstance.GetValue("SHOW_GIFT_AFTER_LEVEL").LongValue;
            Config.SHOW_REVIEW_PER_LEVEL =
                FirebaseRemoteConfig.DefaultInstance.GetValue("SHOW_REVIEW_PER_LEVEL").LongValue;
            Config.BALL_MAX_MOVE_SPEED =
                FirebaseRemoteConfig.DefaultInstance.GetValue("BALL_MAX_MOVE_SPEED").LongValue;
            
            Config.ADMOB_ID_INTER =
                FirebaseRemoteConfig.DefaultInstance.GetValue("ADMOB_ID_INTER").StringValue;
            Config.ADMOB_ID_REWARDED =
                FirebaseRemoteConfig.DefaultInstance.GetValue("ADMOB_ID_REWARDED").StringValue;
        }
    }
}