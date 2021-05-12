using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Mvc.Filters
{
    /// <summary>
    /// IOrderedFilter属性枚举值
    /// </summary>
    public class FilterOrder
    {
        /// <summary>
        ///  规范化结构（请求成功）过滤器排序号
        /// </summary>
        public const int SucceededUnifyResultFilterOrder = 8888;

        /// <summary>
        /// 工作单元拦截器排序号
        /// </summary>
        public const int UnitOfWorkFilterOrder = 9999;
    }
}
