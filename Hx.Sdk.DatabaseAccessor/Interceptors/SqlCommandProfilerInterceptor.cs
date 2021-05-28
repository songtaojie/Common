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
        private ILogger<SqlCommandProfilerInterceptor> _logger;
        public SqlCommandProfilerInterceptor(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SqlCommandProfilerInterceptor>();
        }

        public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
        {
            _logger.LogInformation(eventData.EventId.Name+ command.CommandText);
            return base.ReaderExecuted(command, eventData, result);
        }

        public override int NonQueryExecuted(DbCommand command, CommandExecutedEventData eventData, int result)
        {
            _logger.LogInformation(command.CommandText);
            return base.NonQueryExecuted(command, eventData, result);
        }

        public override Task<int> NonQueryExecutedAsync(DbCommand command, CommandExecutedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(command.CommandText);
            return base.NonQueryExecutedAsync(command, eventData, result, cancellationToken);
        }
    }
}