using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Truongtv.Utilities
{
    public class RatingHelper
    {
        public static void Rate()
        {
#if UNITY_ANDROID||UNITY_EDITOR
            if(_playReviewInfo!=null)
                ShowInAppReview();
            else
                Application.OpenURL($"https://play.google.com/store/apps/details?id={Application.identifier}");
#elif UNITY_IOS
                UnityEngine.iOS.Device.RequestStoreReview();
#endif
        }

#if UNITY_ANDROID||UNITY_EDITOR
        private static Google.Play.Review.ReviewManager _reviewManager;
        private static Google.Play.Review.PlayReviewInfo _playReviewInfo;
        public static async void RequestReviewInfo() // call this before show in app review
        {
            _reviewManager = new Google.Play.Review.ReviewManager();
           
            var requestFlowOperation =  _reviewManager.RequestReviewFlow();
            await requestFlowOperation;
            if (requestFlowOperation.Error != Google.Play.Review.ReviewErrorCode.NoError)
            {
                _playReviewInfo = requestFlowOperation.GetResult();
            }
        }

        private static async void ShowInAppReview()
        {
            var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
            await  launchFlowOperation;
            _playReviewInfo = null; // Reset the object
        }
#endif

    }

}
