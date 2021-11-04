using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Options
{
 
    /// <summary>
    /// 监听配置变化
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public interface IListenerOptions<TOptions> where TOptions : class
    {
        /// <summary>
        /// 监听
        /// </summary>
        /// <param name="name"></param>
        /// <param name="options"></param>
        void OnListener(string name,TOptions options);
    }
}
