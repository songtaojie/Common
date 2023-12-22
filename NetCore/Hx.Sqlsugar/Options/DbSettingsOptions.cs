using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sqlsugar
{
    /// <summary>
    /// 数据库连接配置
    /// </summary>
    public class DbSettingsOptions
    {
        /// <summary>
        /// 数据库连接配置
        /// </summary>
        public IEnumerable<DbConnectionConfig>? ConnectionConfigs { get; set; }
        
        /// <summary>
        /// 设置默认 Redis 配置
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        internal static DbSettingsOptions GetDefaultSettings()
        {
            return new DbSettingsOptions
            {
                ConnectionConfigs = new List<DbConnectionConfig>()
            };
        }
    }
}
