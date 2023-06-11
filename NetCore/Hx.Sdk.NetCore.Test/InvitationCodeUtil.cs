using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.NetCore.Test
{
    /// <summary>
    /// 生成邀请码的工具类
    /// </summary>
    public class InvitationCodeUtil
    {

        /// <summary>
        /// 随机字符串
        /// </summary>
        private static readonly char[] CHARS = new char[] { 'F', 'L', 'G', 'W', '5', 'X', 'C', '3', '9', 'Z', 'M', '6', '7', 'Y', 'R', 'T', '2', 'H', 'S', '8', 'D', 'V', 'E', 'J', '4', 'K', 'Q', 'P', 'U', 'A', 'N', 'B' };

        private const int CHARS_LENGTH = 32;
        /// <summary>
        /// 默认的邀请码长度
        /// </summary>
        private const int CODE_LENGTH = 8;

        /// <summary>
        /// 随机数据
        /// </summary>
        private const long SLAT = 1234561L;

        /// <summary>
        /// PRIME1 与 CHARS 的长度 L互质，可保证 ( id * PRIME1) % L 在 [0,L)上均匀分布
        /// </summary>
        private const int PRIME1 = 3;

        /// <summary>
        /// PRIME2 与 CODE_LENGTH 互质，可保证 ( index * PRIME2) % CODE_LENGTH  在 [0，CODE_LENGTH）上均匀分布
        /// </summary>
        private const int PRIME2 = 11;

        /// <summary>
        /// 生成邀请码, 默认为 8位邀请码
        /// </summary>
        /// <param name="id"> 唯一的id. 支持的最大值为: (32^7 - <seealso cref="SLAT"/>)/<seealso cref="PRIME1"/> = 11452834602
        /// @return </param>
        public static string Gen(long? id)
        {
            return Gen(id, CODE_LENGTH);
        }

        /// <summary>
        /// 生成邀请码
        /// </summary>
        /// <param name="id"> 唯一的id主键. 支持的最大值为: (32^7 - <seealso cref="SLAT"/>)/<seealso cref="PRIME1"/> </param>
        /// <param name="length">邀请码长度</param>
        /// <returns> code </returns>
        public static string Gen(long? id, int length)
        {

            // 补位
            id = id * PRIME1 + SLAT;
            //将 id 转换成32进制的值
            long[] b = new long[length];
            // 32进制数
            b[0] = id.Value;
            for (int i = 0; i < length - 1; i++)
            {
                b[i + 1] = b[i] / CHARS_LENGTH;
                // 按位扩散
                b[i] = (b[i] + i * b[0]) % CHARS_LENGTH;
            }

            long tmp = 0;
            for (int i = 0; i < length - 2; i++)
            {
                tmp += b[i];
            }
            b[length - 1] = tmp * PRIME1 % CHARS_LENGTH;

            // 进行混淆
            long[] codeIndexArray = new long[length];
            for (int i = 0; i < length; i++)
            {
                codeIndexArray[i] = b[i * PRIME2 % length];
            }

            StringBuilder buffer = new StringBuilder();
            //Arrays.stream(codeIndexArray).boxed().map(long?.intValue).map(t => CHARS[t]).forEach(buffer.append);

            foreach (var t in codeIndexArray)
            {
                buffer.Append(CHARS[t]);
            }
            return buffer.ToString();
        }
    }
}
