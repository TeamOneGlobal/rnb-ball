using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace UnityEngine
{
    public class HttpHelper
    {
        public static async UniTask<string> GetRequest(string url)
        {
            var request = UnityWebRequest.Get(url);
            await request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log($"{request.error}: {request.downloadHandler.text}");
            } else
            {
                //Debug.Log(request.downloadHandler.text);
                return request.downloadHandler.text;
            }
            return null;
        }
    }
}