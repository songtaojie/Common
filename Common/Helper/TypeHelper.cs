using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper
{
    public class TypeHelper
    {
        /// <summary>
        /// 最小的时间对象
        /// </summary>
        public readonly static DateTime MIN_DATETIME = new DateTime(1900, 1, 1);
        /// <summary>
        /// 设置最大的时间对象
        /// </summary>
        public readonly static DateTime MAX_DATETIME = new DateTime(2079, 1, 1);
        /// <summary>
        /// 最小的十进制数值
        /// </summary>
        public readonly static decimal MIN_DECIMAL = -9999999999.999999999m;
        /// <summary>
        /// 最大的十进制数值
        /// </summary>
        public readonly static decimal MAX_DECIMAL = 9999999999.999999999m;
        /// <summary>
        /// 判断给定的值是否为空
        /// </summary>
        /// <param name="value"></param>
        /// <returns>true代表为空，false代表不为空</returns>
        public static bool IsNull(object value)
        {
            return (value == null || value == DBNull.Value);
        }
        /// <summary>
        /// 判断给定的值是否不为空
        /// </summary>
        /// <param name="value"></param>
        /// <returns>true代表不为空;false代表为空</returns>
        public static bool IsNotNull(object value)
        {
            return !IsNull(value);
        }
        /// <summary>
        /// 将指定值表示形式转换为等效的 64位带符号整数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long GetInt64(object value)
        {
            return IsNull(value) ? default(long) : Convert.ToInt64(value);
        }
        /// <summary>
        /// 将指定值表示形式转换为等效的 64位带符号整数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Nullable<long> GetNInt64(object value)
        {
            if (IsNull(value)) return null;
            return (long)value;
        }

        public static int GetInt32(object value)
        {
            return IsNull(value) ? default(int) : Convert.ToInt32(value);
        }
        public static Nullable<int> GetNInt32(object value)
        {
            if (IsNull(value)) return null;
            return (int)value;
        }

        public static Int16 GetInt16(object value)
        {
            return IsNull(value) ? default(Int16) : Convert.ToInt16(value);
        }
        public static Nullable<Int16> GetNInt16(object value)
        {
            if (IsNull(value)) return null;
            return (Int16)value;
        }

        public static decimal GetDecimal(object value)
        {
            return IsNull(value) ? default(decimal) : (decimal)value;
        }
        public static Nullable<decimal> GetNDecimal(object value)
        {
            if (IsNull(value)) return null;
            return (decimal)value;
        }
        public static bool IsOverDecimal(decimal value)
        {
            return Helper.Compare(value, MAX_DECIMAL) > 0 || Helper.Compare(value, MIN_DECIMAL) < 0;
        }

        public static DateTime GetDateTime(object value)
        {
            return IsNull(value) ? DateTime.Today : (DateTime)value;
        }
        public static Nullable<DateTime> GetNDateTime(object value)
        {
            if (IsNull(value)) return null;
            return (DateTime)value;
        }

        public static string GetString(object value)
        {
            return IsNull(value) ? default(string) : (string)value;
        }

        public static bool GetBoolean(object value)
        {
            return TypeHelper.GetBoolean(value, "Y");
        }
        public static bool GetBoolean(object value, string compare)
        {
            return Helper.AreEqual(TypeHelper.GetString(value), compare);
        }

        /// <summary>
        /// 取绝对值
        /// </summary>
        public static decimal Abs(decimal dec)
        {
            return (Helper.Compare(dec, 0m) == -1 ? -dec : dec);
        }
        /// <summary>
        /// 取绝对值, 且等于0时,使用1替换
        /// </summary>
        public static decimal Abs(decimal dec, decimal replace)
        {
            int compare = Helper.Compare(dec, 0m);
            return (compare == -1 ? -dec : (compare == 0 ? replace : dec));
        }
    }
}
