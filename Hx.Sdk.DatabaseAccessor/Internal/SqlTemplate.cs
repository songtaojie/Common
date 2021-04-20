using Hx.Sdk.DatabaseAccessor.Models;
using Hx.Sdk.DependencyInjection;

namespace Hx.Sdk.DatabaseAccessor
{
    /// <summary>
    /// Sql 模板
    /// </summary>
    [SkipScan]
    internal sealed class SqlTemplate
    {
        /// <summary>
        /// Sql 语句
        /// </summary>
        public string Sql { get; set; }

        /// <summary>
        /// Sql 参数
        /// </summary>
        public SqlTemplateParameter[] Params { get; set; }
    }
}