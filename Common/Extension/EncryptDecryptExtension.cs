using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extension
{
    /// <summary>
    /// 字符串数据的加密解密主要用于密码等隐私类的文本
    /// </summary>
    public static class EncryptDecryptExtension
    {
        private static string EncryptKey = "60WE4U(7";
        // DES全称为Data Encryption Standard，即数据加密标准。
        // 1997年数据加密标准DES正式公布，其分组长度为64比特，密钥长度为64比特，
        // 其中8比特为奇偶校验位，所以实际长度为56比特。现在DES已经被AES所取代。
        /// <summary>
        /// string的扩展方法，使用DES进行加密(使用内置的秘钥)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string DES3Encrypt(this string data)
        {
            return DES3Encrypt(data, EncryptKey);
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key">必须16位的秘钥</param>
        /// <returns></returns>
        public static string DES3Encrypt(this string data, string key)
        {
            byte[] inputArray = Encoding.UTF8.GetBytes(data);
            var tripleDES = TripleDES.Create();
            var byteKey = Encoding.UTF8.GetBytes(key);
            byte[] allKey = new byte[24];
            Buffer.BlockCopy(byteKey, 0, allKey, 0, 16);
            Buffer.BlockCopy(byteKey, 0, allKey, 16, 8);
            tripleDES.Key = allKey;
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        /// <summary>
        /// string的扩展方法，使用DES进行解密(使用内置的秘钥)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string DES3Decrypt(this string data)
        {
            return DES3Decrypt(data,EncryptKey);
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string DES3Decrypt(this string data, string key)
        {
            byte[] inputArray = Convert.FromBase64String(data);
            var tripleDES = TripleDES.Create();
            var byteKey = Encoding.UTF8.GetBytes(key);
            byte[] allKey = new byte[24];
            Buffer.BlockCopy(byteKey, 0, allKey, 0, 16);
            Buffer.BlockCopy(byteKey, 0, allKey, 16, 8);
            tripleDES.Key = allKey;
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            return Encoding.UTF8.GetString(resultArray);
        }
        /// <summary>
        /// 16位MD5加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string MD5Encrypt16(this string value)
        {
            var md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(value)), 4, 8);
            t2 = t2.Replace("-", "");
            return t2;
        }
        /// <summary>
        /// 32位的MD5加密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Md5Encrypt32(this string value)
        {
            byte[] bytes;
            using (var md5 = MD5.Create())
            {
                bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
            var result = new StringBuilder();
            foreach (byte t in bytes)
            {
                result.Append(t.ToString("X2"));
            }
            return result.ToString();
        }
        /// <summary>
        /// 64位的MD5加密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string MD5Encrypt64(this string value)
        {
            string str = value;
            //string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像
                                    // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            return Convert.ToBase64String(s);
        }
    }
}
