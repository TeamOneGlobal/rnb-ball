// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("mMrC/faot3TbcDKz7xdaJezD0INMnekcLSXO3U6Sbt1x8iy7bXfQGB+AXU4lsj3ezYkGAhUQ5ycjTBsl8THa/+AVLf4CxQaViX2hDD7w/VRrrzmfekxj3Tn6zfNfNm9jKkrA0GWzjRReVuIryBXl259Thd4jWDQJfrbYEgQM9tGmNM4pp4AU+5EmHhndXlBfb91eVV3dXl5fixh4FgyUgG/dXn1vUllWddkX2ahSXl5eWl9cC31ERk1K4XxPakaNLfRgjDEzsLqyKCCYMheHCsaMub1etXDxmMx9SFVeIsEmBDMaTyWUlPtFhGqdQbG+GUTgztSN7rcWUl7pujnqsONMjqrjfCecDxWX9obtZeyjpAS29+IHE0+lZh7pWfuDKl1cXl9e");
        private static int[] order = new int[] { 12,8,3,3,6,7,13,8,12,13,13,11,13,13,14 };
        private static int key = 95;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
