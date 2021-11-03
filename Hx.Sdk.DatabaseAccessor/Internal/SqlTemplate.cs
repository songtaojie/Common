using Hx.Sdk.DatabaseAccessor.Models;
namespace Hx.Sdk.DatabaseAccessor
{
    /// <summary>
    /// Sql 模板
    /// </summary>
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