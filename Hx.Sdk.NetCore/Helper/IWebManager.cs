using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Sdk.NetCore
{
    /// <summary>
    /// Web管理类
    /// </summary>
    public interface IWebManager
    {
        /// <summary>
        /// web应用程序根路径
        /// </summary>
        string WebRootPath { get; }

        /// <summary>
        /// web应用程序根路径
        /// </summary>
        string ContentRootPath { get; }

        /// <summary>
        /// 把绝对路径转换成相对路径
        /// </summary>
        /// <param name="absolutePath">绝对路径</param>
        /// <returns></returns>
        string ToRelativePath(string absolutePath);

        /// <summary>
        /// 把相对路径转换成绝对路径
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        string ToAbsolutePath(string relativePath);

        /// <summary>
        /// 获取路由的全路径
        /// </summary>
        /// <param name="host">网站host</param>
        /// <param name="routeUrl"></param>
        /// <returns></returns>
        string GetFullUrl(string host, string routeUrl);
    }
}
