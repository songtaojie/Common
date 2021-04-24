using Hx.Sdk.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Sdk.Entity.Page
{
    /// <summary>
    /// 分页参数
    /// </summary>
    [SkipScan]
    public class BasePageParam
    {
        /// <summary>
        /// 每页多少条数据
        /// </summary>
        public int PageSize { get; set; } = 10;
        /// <summary>
        /// 当前页码
        /// 默认从第一页开始
        /// </summary>
        public int PageIndex { get; set; } = 1;
        /// <summary>
        /// 排序的键
        /// </summary>
        public string SortKey { get; set; } = string.Empty;
        /// <summary>
        /// 0 正序 1倒序
        /// </summary>
        public SortTypeEnum SortType { get; set; }
    }
    /// <summary>
    /// 排序类型
    /// </summary>
    [SkipScan]
    public enum SortTypeEnum
    {
        /// <summary>
        /// 正序
        /// </summary>
        ASC = 0,
        /// <summary>
        /// 倒序
        /// </summary>
        DESC = 1
    }
}
