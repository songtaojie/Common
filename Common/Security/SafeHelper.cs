using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Security
{
    public class SafeHelper
    {
        private static string EncryptKey = "60WE4U(7";
        /// <summary>
        /// 使用内置的秘钥进行加密
        /// </summary>
        /// <param name="pToEncrypt">要加密的文本</param>
        /// <returns></returns>
        public static string EncryptDES(string pToEncrypt)
        {
            return EncryptDES(pToEncrypt, EncryptKey);
        }
        public static string DecryptDES(string pToEncrypt)
        {
            return DecryptDES(pToEncrypt, EncryptKey);
        }
        /// <summary>
        /// 进行DES加密
        /// </summary>
        /// <param name="pToEncrypt">要加密的字符串</param>
        /// <param name="key">密钥，必须为8位</param>
        /// <returns>以Base64格式返回的加密字符串</returns>
        public static string EncryptDES(string pToEncrypt, string sKey)
        {
            sKey = GetKey(sKey, 8);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return str;
            }
        }
        /// <summary>
        /// 进行DES解密
        /// </summary>
        /// <param name="pToDecrypt">要解密的字符串</param>
        /// <param name="sKey">密钥，必须为8位</param>
        /// <returns>返回解密后的字符串</returns>
        public static string DecryptDES(string pToDecrypt, string sKey)
        {
            sKey = GetKey(sKey, 8);
            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                MemoryStream ms = new MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }

        private static string GetKey(string sKey, int length)
        {
            if (sKey.Length > length)
                sKey = sKey.Substring(0, length);
            else if (sKey.Length < length)
                sKey = sKey.PadRight(length, ' ');
            return sKey;
        }
    }
}
