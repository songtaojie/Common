using System;

namespace Hx.Sdk.Core
{
    /// <summary>
    /// 控制台帮助类(内部使用)
    /// </summary>
    public static class ConsoleHelper
    {
        static void WriteColorLine(string str, string prefix, ConsoleColor color)
        {
            ConsoleColor currentForeColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(prefix);
            Console.ForegroundColor = currentForeColor;
            Console.WriteLine(str);
        }

        /// <summary>
        /// 打印错误信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="newLine">是否添加一行空行</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteErrorLine(string str, bool newLine = false, ConsoleColor color = ConsoleColor.Red)
        {
            WriteColorLine(str, "error：", color);
            if (newLine) Console.WriteLine();
        }

        /// <summary>
        /// 打印警告信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="newLine">是否添加一行空行</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteWarningLine(string str, bool newLine = false, ConsoleColor color = ConsoleColor.Yellow)
        {
            WriteColorLine(str, "warn：", color);
            if (newLine) Console.WriteLine();
        }
        /// <summary>
        /// 打印正常信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="newLine">是否添加一行空行</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteInfoLine(string str, bool newLine = false, ConsoleColor color = ConsoleColor.White)
        {
            WriteColorLine(str, "info：", color);
            if (newLine) Console.WriteLine();
        }
        /// <summary>
        /// 打印成功的信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="newLine">是否添加一行空行</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteSuccessLine(string str, bool newLine = false, ConsoleColor color = ConsoleColor.DarkGreen)
        {
            WriteColorLine(str, "info：", color);
            if (newLine) Console.WriteLine();
        }
    }
}
