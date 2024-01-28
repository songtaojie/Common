// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sqlsugar;
/// <summary>
/// sqlsugar日志记录
/// </summary>
public interface ISqlsugarLogger
{
    /// <summary>
    /// 日志记录
    /// </summary>
    /// <param name="sugarClient"></param>
    /// <param name="sql"></param>
    /// <param name="pars"></param>
    Task OnLogExecuting(ISqlSugarClient sugarClient, string sql, SugarParameter pars);

    /// <summary>
    /// 异常日志记录
    /// </summary>
    /// <returns></returns>
    Task OnError(ISqlSugarClient sugarClient, SqlSugarException sugarException);
}
