using Hx.Sdk.NetCore.Config;
using Microsoft.Extensions.Hosting;
using Hx.Sdk.Common.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hx.Sdk.NetCore
{
    /// <summary>
    /// Web管理类
    /// </summary>
    internal class WebManager: IWebManager
    {
        private readonly IHostEnvironment env;
        /// <summary>
        /// web帮助类
        /// </summary>
        /// <param name="env"></param>
        public WebManager(IHostEnvironment env)
        {
            this.env = env;
            var propInfo = env.GetType().GetProperty("WebRootPath");
            if (propInfo == null)
            {
                WebRootPath = Path.Combine(env.ContentRootPath, "wwwroot");
            }
            else
            {
                WebRootPath = propInfo.GetValue(env).ObjToString();
            }
        }
        /// <summary>
        /// web应用程序根路径
        /// </summary>
        public string WebRootPath { get; }

        /// <summary>
        /// web应用程序根路径
        /// </summary>
        public string ContentRootPath => env.ContentRootPath;
        /// <summary>
        /// 把绝对路径转换成相对路径
        /// </summary>
        /// <param name="absolutePath">绝对路径</param>
        /// <returns></returns>
        public string ToRelativePath(string absolutePath)
        {
            if (string.IsNullOrEmpty(absolutePath)) return string.Empty;
            return absolutePath.Replace(WebRootPath, "/").Replace(@"\", @"/"); //转换成相对路径
        }
        /// <summary>
        /// 把相对路径转换成绝对路径
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public string ToAbsolutePath(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return relativePath;
            if (relativePath.First() == '/')
            {
                relativePath = relativePath.Substring(1);
            }
            return Path.Combine(WebRootPath, relativePath);
        }

        /// <summary>
        /// 获取路由的全路径
        /// </summary>
        /// <param name="host">网站host</param>
        /// <param name="routeUrl"></param>
        /// <returns></returns>
        public string GetFullUrl(string host, string routeUrl)
        {
            if (string.IsNullOrEmpty(routeUrl)) return host;

            while (!string.IsNullOrEmpty(host) && host.Last() == '/')
            {
                host = host.Remove(host.Length - 1);
            }
            while (!string.IsNullOrEmpty(routeUrl) && routeUrl.First() == '/')
            {
                routeUrl = routeUrl.Substring(1);
            }
            return host + "/" + routeUrl;
        }
    }
}
