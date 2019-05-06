using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Common.Redis
{
    /// <summary>
    /// Redis的前缀
    /// </summary>
    public enum RedisTypePrefix
    {
        /// <summary>
        /// 以S:为前缀
        /// </summary>
        [Description("S:")]
        String
    }
}
