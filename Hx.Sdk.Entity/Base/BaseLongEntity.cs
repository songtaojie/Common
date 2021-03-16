﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Hx.Sdk.Entity.Base
{
    /// <summary>
    /// 基础的实体类，封装了公共的字段，主键是Long类型的
    /// </summary>
    public abstract class BaseLongEntity : BaseModel, IEntity<long>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public long Id
        {
            get;
            set;
        }
        #region 创建
        /// <summary>
        /// 创建时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public virtual DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 这条记录属于哪个用户
        /// </summary>
        [StringLength(100)]
        public virtual string UserId { get; set; }
        /// <summary>
        /// 用户的登录名称
        /// </summary>
        [StringLength(50)]
        public virtual string UserName { get; set; }
        #endregion

        #region 删除

        /// <summary>
        /// 是否删除
        /// </summary>
        [Column(TypeName = "char(1)")]
        public virtual string Delete
        {
            get; set;
        } = "N";
        /// <summary>
        /// 删除人ID
        /// </summary>
        [StringLength(100)]
        public virtual string DeletelUserId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public virtual DateTime? DeleteTime { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public virtual DateTime? LastModifyTime { get; set; }

        #endregion
    }
}
