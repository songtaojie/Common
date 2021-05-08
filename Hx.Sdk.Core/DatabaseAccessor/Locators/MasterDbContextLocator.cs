using Hx.Sdk.DependencyInjection;

namespace Hx.Sdk.DatabaseAccessor
{
    /// <summary>
    /// 默认数据库上下文定位器
    /// </summary>
    [SkipScan]
    public sealed class MasterDbContextLocator : IDbContextLocator
    {
    }
}