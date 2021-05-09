﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Sdk.Extensions
{
    /// <summary>
    /// 控制台扩展类
    /// </summary>
    public static class ConsoleExtensions
    {
        static void WriteColorLine(string str, ConsoleColor color)
        {
            ConsoleColor currentForeColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(str);
            Console.ForegroundColor = currentForeColor;
        }

        /// <summary>
        /// 打印错误信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="newLine">是否添加一行空行</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteErrorLine(this string str, bool newLine = false, ConsoleColor color = ConsoleColor.Red)
        {
            WriteColorLine(str, color);
            if (newLine) Console.WriteLine();
        }

        /// <summary>
        /// 打印警告信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="newLine">是否添加一行空行</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteWarningLine(this string str, bool newLine = false, ConsoleColor color = ConsoleColor.Yellow)
        {
            WriteColorLine(str, color);
            if (newLine) Console.WriteLine();
        }
        /// <summary>
        /// 打印正常信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="newLine">是否添加一行空行</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteInfoLine(this string str, bool newLine = false, ConsoleColor color = ConsoleColor.White)
        {
            WriteColorLine(str, color);
            if (newLine) Console.WriteLine();
        }
        /// <summary>
        /// 打印成功的信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="newLine">是否添加一行空行</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteSuccessLine(this string str, bool newLine = false, ConsoleColor color = ConsoleColor.Green)
        {
            WriteColorLine(str, color);
            if (newLine) Console.WriteLine();
        }
    }
}