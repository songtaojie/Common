using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Hx.Sdk.DatabaseAccessor
{
    /// <summary>
    /// 数据库执行命令拦截
    /// </summary>
    internal sealed class SqlCommandProfilerInterceptor : DbCommandInterceptor
    {
    }
}