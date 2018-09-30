using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


using System.IO;

namespace SterlingForexService.com.sbp.utility
{
    class AES
    {

        public static String Encrypt(String word, String key, String iv)
        {
            byte[] result = null;
            //string word = "Joscool";
            byte[] wordBytes = Encoding.UTF8.GetBytes(word);
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = Encoding.UTF8.GetBytes(key);
                    AES.IV = Encoding.UTF8.GetBytes(iv);

                    AES.Mode = System.Security.Cryptography.CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(wordBytes, 0, wordBytes.Length);
                        cs.Close();
                    }
                    byte[] encryptedBytes = ms.ToArray();
                    result = encryptedBytes;

                    return Convert.ToBase64String(result);
                    //return ByteArrayToString(encryptedBytes).ToUpper();
                }
            }

        }
        public static string Decrypt(String word, String key, String iv)
        {
            //string word = "A7BA53AAA7D67CA8CC54913DA398E189";
            //byte[] wordBytes = cipher;//StringToByteArray(word);
            byte[] wordBytes = StringToByteArray(word);
            byte[] byteBuffer = new byte[wordBytes.Length];
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = Encoding.UTF8.GetBytes(key);
                    AES.IV = Encoding.UTF8.GetBytes(iv);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(wordBytes, 0, wordBytes.Length);
                        cs.Close();
                    }
                    byte[] decryptedBytes = ms.ToArray();
                    return System.Text.Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
