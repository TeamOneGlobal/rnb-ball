using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Truongtv.Utilities
{
    public static class Encryption
    {
        #region Gzip

        private static void CopyTo(Stream src, Stream dest)
        {
            var bytes = new byte[4096];
            int cnt;
            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        public static string EncryptByGZip(string inputStr)
        {
            var inputBytes = Encoding.UTF8.GetBytes(inputStr);
            using (var outputStream = new MemoryStream())
            {
                using (var gZipStream = new GZipStream(outputStream, CompressionMode.Compress))
                    gZipStream.Write(inputBytes, 0, inputBytes.Length);
                var outputBytes = outputStream.ToArray();
                var base64String = Convert.ToBase64String(outputBytes);
                return base64String;
            }
        }

        public static string DecryptByGZip(string str)
        {
            var bytes = Convert.FromBase64String(str);
            var msi = new MemoryStream(bytes);
            var mso = new MemoryStream();
            var gs = new GZipStream(msi, CompressionMode.Decompress);
            CopyTo(gs, mso);
            return Encoding.UTF8.GetString(mso.ToArray());
        }

        #endregion

        #region base64
        private static readonly byte[] EncryptKey = {
                168, 220, 184, 133, 78, 149, 8, 249, 171, 138, 98, 170, 95, 15, 211, 200, 51, 242, 4, 193, 219, 181, 232, 99, 16, 240, 142, 128, 29, 163, 245, 24, 204, 73, 173, 32, 214, 76, 31, 99, 91, 239, 232, 53, 138, 195, 93, 195, 185, 210, 155, 184, 243, 216, 204, 42, 138, 101, 100, 241, 46, 145, 198, 66, 11, 17, 19, 86, 157, 27, 132, 201, 246, 112, 121, 7, 195, 148, 143, 125, 158, 29, 184, 67, 187, 100, 31, 129, 64, 130, 26, 67, 240, 128, 233, 129, 63, 169, 5, 211, 248, 200, 199, 96, 54, 128, 111, 147, 100, 6, 185, 0, 188, 143, 25, 103, 211, 18, 17, 249, 106, 54, 162, 188, 25, 34, 147, 3, 222, 61, 218, 49, 164, 165, 133, 12, 65, 92, 48, 40, 129, 76, 194, 229, 109, 76, 150, 203, 251, 62, 54, 251, 70, 224, 162, 167, 183, 78, 103, 28, 67, 183, 23, 80, 156, 97, 83, 164, 24, 183, 81, 56, 103, 77, 112, 248, 4, 168, 5, 72, 109, 18, 75, 219, 99, 181, 160, 76, 65, 16, 41, 175, 87, 195, 181, 19, 165, 172, 138, 172, 84, 40, 167, 97, 214, 90, 26, 124, 0, 166, 217, 97, 246, 117, 237, 99, 46, 15, 141, 69, 4, 245, 98, 73, 3, 8, 161, 98, 79, 161, 127, 19, 55, 158, 139, 247, 39, 59, 72, 161, 82, 158, 25, 65, 107, 173, 5, 255, 53, 28, 179, 182, 65, 162, 17
            };
        public static string EncryptByBase64(string value)
        {
            var plainTextBytes = Xor(Encoding.UTF8.GetBytes(value), EncryptKey);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string DecryptByBase64(string value)
        {
            var base64EncodedBytes = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(Xor(base64EncodedBytes, EncryptKey));
        }

        private static byte[] Xor(byte[] input, byte[] key)
        {
            if (key == null || key.Length == 0)
            {
                return input;
            }
            var output = new byte[input.Length];
            var kPos = 0;
            var kUp = 0;
            var kk = 0;
            for (int pos = 0, n = input.Length; pos < n; ++pos)
            {
                output[pos] = (byte)(input[pos] ^ key[kPos] ^ kk);
                ++kPos;
                if (kPos < key.Length) continue;
                kPos = 0;
                kUp = (kUp + 1) % key.Length;
                kk = key[kUp];
            }
            return output;
        }

        #endregion
    }
}
