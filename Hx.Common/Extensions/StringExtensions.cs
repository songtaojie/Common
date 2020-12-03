using Hx.Common.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Common.Extensions
{
    /// <summary>
    /// 字符串扩展类
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 字符串转为bool值
        /// </summary>
        /// <param name="value">要转换的值</param>
        /// <returns>如果转换成功，返回转换后的值，否则返回null</returns>
        public static bool? ToBool(this string value)
        {
            bool? result = null;
            bool.TryParse(value, out bool r);
            result = r;
            return result;
        }

        #region 加密解密
        /// <summary>
        /// 使用内置的秘钥进行加密
        /// </summary>
        /// <param name="pToEncrypt">要加密的文本</param>
        /// <returns></returns>
        public static string DESEncrypt(this string pToEncrypt)
        {
            return SafeHelper.DESEncrypt(pToEncrypt);
        }
        /// <summary>
        /// 使用内置的秘钥进行DES解密
        /// </summary>
        /// <param name="pToEncrypt">要解密的字符串</param>
        /// <returns>返回解密后的字符串</returns>
        public static string DESDecrypt(this string pToEncrypt)
        {
            return SafeHelper.DESDecrypt(pToEncrypt);
        }
        #endregion
        /// <summary>
        /// 使用MD5对字符串进行加密
        /// </summary>
        /// <param name="pToEncrypt">要加密的字符串</param>
        /// <returns>已经加密的字符串</returns>
        public static string MD5Encrypt(this string pToEncrypt)
        {
            return SafeHelper.Md5Encrypt(pToEncrypt);
        }
        /// <summary>
        /// 使用MD5对字符串进行两次加密
        /// </summary>
        /// <param name="pToEncrypt">要加密的字符串</param>
        /// <returns>已经加密的字符串</returns>
        public static string MD5TwoEncrypt(this string pToEncrypt)
        {
            return pToEncrypt.MD5Encrypt().MD5Encrypt();
        }
    }
}
