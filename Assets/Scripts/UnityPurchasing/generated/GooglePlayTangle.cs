// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("N9H/wO/1FuEij95axLuUGACXUFI3zkPdRvqR3WS/iMwxpbtupup4/RF4fXrWJFkfRD/yZCZdi/N9uMlPSRRpVX2T3E7cwZ45oujCNaRUCSTyGLdM6DfpJd7l8Oppy6bQh7KjiROQnpGhE5CbkxOQkJEUKKzIAPv+W9KfPTfXXAgUNhRr8y8GkItYFzpXaoi+c+4CGl5M65bLYU3jv6xtsVsOT3NUJcsJ32DfenGlNIFPESK/1oiUAF+Cg+cGWG4vcpdXxmjn8+5Zn7h9U7yhSvhc9fmRQCvUZb5lnKETkLOhnJeYuxfZF2ackJCQlJGSUqpZpqNYIvTcyn9EdlCAngBkJA9y7EydudsJixxjqnilcFRwNMMRGKnOJRh1vRs4TpOSkJGQ");
        private static int[] order = new int[] { 6,3,10,10,8,10,11,7,10,10,12,11,13,13,14 };
        private static int key = 145;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
