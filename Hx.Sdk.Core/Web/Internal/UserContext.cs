using Hx.Sdk.ConfigureOptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Hx.Sdk.Core.Internal
{
    /// <summary>
    /// 用户上下文操作类
    /// </summary>
    internal class UserContext : IUserContext
    {
        /// <summary>
        /// 是否使用IdentityServer4
        /// </summary>
        private static bool _isUseIds4 = false;
        /// <summary>
        /// HttpContext访问器
        /// </summary>
        private IHttpContextAccessor _contextAccessor;

        public HttpContext HttpContext
        {
            get
            {
                return _contextAccessor.HttpContext;
            }
        }

        public UserContext(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            _isUseIds4 = App.Settings.UseIdentityServer4 == true;
        }
        /// <summary>
        /// 用户的名字
        /// </summary>
        public string UserName
        {
            get
            { 
                string name = HttpContext.User.Identity.Name;
                if (!string.IsNullOrEmpty(name)) return name;
                string getNameType = _isUseIds4 ? HxClaimTypes.Ids4Name : ClaimTypes.Name;
               return GetClaimValueByType(getNameType).FirstOrDefault();
            }
        }

        /// <summary>
        /// 是否是超级管理员
        /// </summary>
        public bool IsSuperAdmin
        {
            get
            {
                var claims = GetClaimsIdentity();
                var isAdmin = claims.Any(c => c.Type == ClaimTypes.Role && c.Value == HxClaimValues.SuperAdmin);
                return IsAuthenticated && isAdmin;
            }
        }

        /// <summary>
        /// 是否是管理员
        /// </summary>
        public bool IsAdmin
        {
            get
            {
                var claims = GetClaimsIdentity();
                var isAdmin = claims.Any(c => c.Type == ClaimTypes.Role && c.Value == HxClaimValues.Admin);
                return IsAuthenticated && isAdmin;
            }
        }
        /// <summary>
        /// Jwt的id
        /// </summary>
        public string JwtId => GetClaimValueByType(HxClaimTypes.Jti).FirstOrDefault();

        /// <summary>
        /// 用户的id
        /// </summary>
        public string UserId
        {
            get
            {
                return GetClaimValueByType(ClaimTypes.NameIdentifier).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取cookie的值
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public string GetCookieValue(string cookieName)
        {
            return HttpContext.Request.Cookies[cookieName];
        }

        /// <summary>
        /// 设置cookie的值
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="value"></param>
        /// <param name="expires">过期时间</param>
        public void SetCookieValue(string cookieName, string value, DateTime? expires = null)
        {
            string cookieValue = GetCookieValue(cookieName);
            if (!string.IsNullOrEmpty(cookieValue)) HttpContext.Response.Cookies.Delete(cookieName);
            if (expires.HasValue)
            {
                HttpContext.Response.Cookies.Append(cookieName, value, new CookieOptions
                {
                    Expires = new DateTimeOffset(expires.Value)
                });
            }
            else
            {
                HttpContext.Response.Cookies.Append(cookieName, value);
            }
        }
        /// <summary>
        /// 是否已经验证，即是否一登录
        /// </summary>
        /// <returns></returns>
        public bool IsAuthenticated => HttpContext.User.Identity.IsAuthenticated;
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public string GetToken()
        {
            var auth = HttpContext.Request.Headers["Authorization"];
            if (Microsoft.Extensions.Primitives.StringValues.IsNullOrEmpty(auth)) return string.Empty;
            return auth.ToString().Replace("Bearer ", "");
        }
       
        /// <summary>
        /// 获取claims集合
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return HttpContext.User.Claims;
        }
        /// <summary>
        /// 根据claim获取相应的值
        /// </summary>
        /// <param name="ClaimType"></param>
        /// <returns></returns>
        public List<string> GetClaimValueByType(string ClaimType)
        {

            return (from item in GetClaimsIdentity()
                    where item.Type == ClaimType
                    select item.Value).ToList();

        }

        public T GetClaimValueByType<T>(string ClaimType)
        {
            var claim = GetClaimValueByType(ClaimType).FirstOrDefault();
            if (claim == null) return default;
            var type = typeof(T);
            if (type == typeof(int) || type == typeof(int?))
            {
                int.TryParse(claim, out int result);
                return (T)(object)result;
            }
            else if (type == typeof(bool) || type == typeof(bool?))
            {
                bool.TryParse(claim, out bool result);
                return (T)(object)result;
            }
            else if (type == typeof(decimal) || type == typeof(decimal?))
            {
                decimal.TryParse(claim, out decimal result);
                return (T)(object)result;
            }
            else if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                DateTime.TryParse(claim, out DateTime result);
                return (T)(object)result;
            }
            return default;
        }
    }
}
