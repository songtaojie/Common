using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Hx.Sdk.Entity
{
    /// <summary>
    ///  基础的实体类，封装了创建，编辑的公共字段,
    /// </summary>
    /// <typeparam name="TKeyType">主键的类型</typeparam>
    public abstract class BaseEntity<TKeyType> : BaseModel, IEntity<TKeyType>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [MaxLength(36)]
        public virtual TKeyType Id
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
        /// 创建人
        /// </summary>
        [MaxLength(36)]
        public string CreaterId { get; set; } = string.Empty;

        /// <summary>
        /// 创建人姓名
        /// </summary>
        [MaxLength(36)]
        public string Creater { get; set; } = string.Empty;
        #endregion

        /// <summary>
        /// 最后更新时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public virtual DateTime? LastModifyTime { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        [MaxLength(36)]
        public string LastModifier { get; set; } = string.Empty;

        /// <summary>
        /// 最后修改人的id
        /// </summary>
        [MaxLength(36)]
        public string LastModifierId { get; set; } = string.Empty;

        #region 实体操作

        /// <summary>
        /// 添加创建人信息
        /// </summary>
        /// <param name="createrId">创建人id</param>
        /// <param name="creater">创建人姓名</param>
        public virtual BaseEntity<TKeyType> SetCreater(string createrId, string creater)
        {
            CreaterId = createrId;
            Creater = creater;
            CreateTime = DateTime.Now;
            SetModifier(createrId, creater);
            return this;
        }

        /// <summary>
        /// 添加修改人信息
        /// </summary>
        /// <param name="modifierId">修改者id</param>
        /// <param name="modifier">修改者姓名</param>
        public virtual BaseEntity<TKeyType> SetModifier(string modifierId, string modifier)
        {
            LastModifierId = modifierId;
            LastModifier = modifier;
            LastModifyTime = DateTime.Now;
            return this;
        }

        #endregion
    }
}
