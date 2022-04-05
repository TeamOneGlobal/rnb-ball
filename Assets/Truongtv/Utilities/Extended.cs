using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Truongtv.Utilities
{
    public static class Extended 
    {
        public static void ApplicationSetting()
        {
            Application.backgroundLoadingPriority = ThreadPriority.Low;
            Application.targetFrameRate = 60;
            Application.runInBackground = true;
            Input.multiTouchEnabled = true;
#if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
#else
            Debug.unityLogger.logEnabled = false;
#endif
        }
        public static void UpdateCamera(this Canvas mainCanvas)
        {
            mainCanvas.worldCamera = Camera.main;
        }
        public static void RemoveAllChild(this Transform root, Func<GameObject, bool> condition = null)
        {
            if (root == null||root.childCount == 0)
            {
                return;
            }
            for (var i = root.childCount - 1; i >= 0; i--)
            {
                var t = root.GetChild(i);
                if (condition == null || condition(t.gameObject))
                {
                    UnityEngine.Object.Destroy(t.gameObject);
                }
            }
        }
        public static async UniTaskVoid LoadImageFromUrl(string imgUrl, Texture2D callback)
        {
            var result = await UnityWebRequest.Get(imgUrl).SendWebRequest();
            var rawBytes = result.downloadHandler.data;
            callback.LoadImage(rawBytes);
        }

        public static async UniTask<string> ImageUrlToBase64(string imgUrl)
        {
            var result = await UnityWebRequest.Get(imgUrl).SendWebRequest();
            var rawBytes = result.downloadHandler.data;
            var base64String = Convert.ToBase64String(rawBytes);
            return base64String;
        }
        // public static async UniTask<bool> IsInternetConnected()
        // {
        //     const string echoServer = "http://google.com";
        //     bool result;
        //     using (var request = UnityWebRequest.Head(echoServer))
        //     {
        //         request.timeout = 2;
        //         var data = await request.SendWebRequest();
        //         result = !data.isNetworkError && !data.isHttpError && data.responseCode == 200;
        //     }
        //     return result;
        // }

        public static bool IsInternetConnected()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }

    }
    
}
