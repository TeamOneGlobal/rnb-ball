using System.Collections.Generic;

namespace GamePlay
{
    public static class TagManager
    {
        #region Tag Game Object
        public static readonly List<string> PlayerTag = new List<string>{"Red","Blue"};
        public const string RED_TAG = "Red";
        public const string BLUE_TAG = "Blue";
        public const string MAGNET_TAG = "Magnet";
        public const string BOX_TAG = "Box";
        public const string DAMAGED_TAG = "Enemy";

        public static string[] GetAllTagHandle()
        {
            var result = new List<string>();
            result.AddRange(PlayerTag);
            result.Add(MAGNET_TAG);
            result.Add(DAMAGED_TAG);
            result.Add(BOX_TAG);
            return result.ToArray();
        }

       #endregion

        #region Tag Coroutine

        public static readonly string CameraAnim = "camera";

        #endregion
    }
}