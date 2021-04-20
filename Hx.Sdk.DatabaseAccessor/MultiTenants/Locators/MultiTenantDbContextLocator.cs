using Hx.Sdk.DependencyInjection;

namespace Hx.Sdk.DatabaseAccessor
{
    /// <summary>
    /// 多租户数据库上下文定位器
    /// </summary>
    [SkipScan]
    public sealed class MultiTenantDbContextLocator : IDbContextLocator
    {
    }
}