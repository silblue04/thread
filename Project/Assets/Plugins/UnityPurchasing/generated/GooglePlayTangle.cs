#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("bsUngUITn1aV/St4HkjVV0+Q56WLLtHcF5P7X7AbK8HZJ2Loe9W59fHkOp/rfJTO1Emr1scO+6k8+TkTIN6IWCiPCfXwSqA0XUQEHzrAxGsmaEW8QZN8xQQLHmgTxYaF5RTydRWxjgVJ4awa3z+TwtTJXAJiQ7vrHZ6Qn68dnpWdHZ6enx+5Zm2eARGteVhThRbmpfzwKOSxPvbp757+CQ6dYAE1xYuhFhAHLqG1y39lnFa6fpFpUGhdEjMePySXxU8jjcVssNSvHZ69r5KZlrUZ1xlokp6enpqfnBHySSyJ91XRy2XhgksYmhtCXuOr1BB26TNNaipBANxBY/xy0o2TZGg/WfeOZl6xcqN/LcIaGUtcPXC7UcwyTQpQctIstJ2cnp+e");
        private static int[] order = new int[] { 5,7,6,6,11,12,7,9,8,9,12,12,12,13,14 };
        private static int key = 159;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
