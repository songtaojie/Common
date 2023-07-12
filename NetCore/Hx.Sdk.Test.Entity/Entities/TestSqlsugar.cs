using Hx.Sdk.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.Test.Entity.Entities
{
    [SugarTable(null,"测试表")]
    public class TestSqlsugar:EntityBase
    {
        [SugarColumn(IsPrimaryKey =true,ColumnDescription ="主键id")]
        public override long Id { get => base.Id; set => base.Id = value; }
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(IsNullable =true,Length =200,ColumnDescription ="名称")]
        public string Name { get; set; }
    }
}
