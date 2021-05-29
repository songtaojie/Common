using Hx.Sdk.DependencyInjection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Hx.Sdk.DatabaseAccessor
{
    /// <summary>
    /// 数据库执行命令拦截
    /// </summary>
    [SkipScan]
    internal sealed class SqlCommandProfilerInterceptor : DbCommandInterceptor
    {
    }
}