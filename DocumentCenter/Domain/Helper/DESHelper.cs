using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace DocumentCenter.Domain.Helper
{
    public class DESHelper
    {
        //密匙
        private static string key = "jaf-2019";
        public static string Key
        {
            get
            {
                return key;
            }
        }

        //偏移量
        private static string iv = "00000000";
        public static string IV
        {
            get
            {
                return iv;
            }
        }


        public static string DesEncrypt(string encryptStr)
        {
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            provider.Padding = PaddingMode.PKCS7;
            provider.Mode = CipherMode.CBC;

            byte[] keyBytes = GetKeyBytes();
            byte[] iv = Encoding.UTF8.GetBytes(IV);
            byte[] strArray = Encoding.UTF8.GetBytes(encryptStr);
            MemoryStream ms = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream(ms, provider.CreateEncryptor(keyBytes, iv), CryptoStreamMode.Write);
            cryptoStream.Write(strArray, 0, strArray.Length);
            cryptoStream.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }

        public static string DesDecrypt(string decryptStr)
        {
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            provider.Padding = PaddingMode.PKCS7;
            provider.Mode = CipherMode.CBC;

            byte[] keyBytes = GetKeyBytes();
            byte[] iv = Encoding.UTF8.GetBytes(IV);
            byte[] strArray = Convert.FromBase64String(decryptStr);
            MemoryStream ms = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream(ms, provider.CreateDecryptor(keyBytes, iv), CryptoStreamMode.Write);
            cryptoStream.Write(strArray, 0, strArray.Length);
            cryptoStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(ms.ToArray());
        }

        private static byte[] GetKeyBytes()
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(Key);
            byte[] bytes = new byte[8];
            for (int i = 0; i < keyBytes.Length; i++)
            {
                bytes[i] = keyBytes[i];
            }

            return bytes;
        }
    }
}