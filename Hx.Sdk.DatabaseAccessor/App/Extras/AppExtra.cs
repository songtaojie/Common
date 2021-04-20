using Hx.Sdk.DependencyInjection;

namespace Hx.Sdk
{
    /// <summary>
    /// 官方包定义
    /// </summary>
    [SkipScan]
    internal static class AppExtra
    {
        /// <summary>
        /// Jwt 验证包
        /// </summary>
        internal const string AUTHENTICATION_JWTBEARER = "Hx.Sdk.Extras.Authentication.JwtBearer";

        /// <summary>
        /// Mapster 映射包
        /// </summary>
        internal const string OBJECTMAPPER_MAPSTER = "Hx.Sdk.Extras.ObjectMapper.Mapster";
    }
}