using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Hx.Sdk.Entity.Base
{
    /// <summary>
    ///  该基类封装了删除，禁用等状态字段
    /// </summary>
    /// <typeparam name="TKeyType">主键的类型</typeparam>
    public abstract class BaseStatusEntity<TKeyType>:BaseEntity<TKeyType>
    {
        private const string No = "N";
        private const string Yes = "Y";
        #region 删除

        /// <summary>
        /// 是否删除,使用char字段存储，这样查询时不用进行取非的判断，所有都用等于判断
        /// Y:代表删除，N代表没删除，默认值为N
        /// </summary>
        [Column(TypeName = "char(1)")]
        public virtual string Delete
        {
            get; set;
        } = No;

        /// <summary>
        /// 是否禁用,使用char字段存储，这样查询时不用进行取非的判断，所有都用等于判断
        /// Y:代表禁用，N代表没禁用，默认值为N
        /// </summary>
        public virtual string Disable { get; set; } = No;
        #endregion

        #region 实体操作

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="modifierId">删除者的id</param>
        /// <param name="modifier">删除者的姓名</param>
        public virtual void SetDelete(string modifierId, string modifier)
        {
            Delete = Yes;
            SetModifier(modifierId, modifier);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="deleted">删除状态</param>
        /// <param name="modifierId">删除者的id</param>
        /// <param name="modifier">删除者的姓名</param>
        public virtual void SetDelete(StatusEntityEnum deleted, string modifierId, string modifier)
        {
            Delete = deleted == StatusEntityEnum.Yes?Yes:No;
            SetModifier(modifierId, modifier);
        }

        /// <summary>
        /// 禁用
        /// </summary>
        /// <param name="modifierId">禁用者的id</param>
        /// <param name="modifier">禁用者的姓名</param>
        public virtual void SetDisable(string modifierId, string modifier)
        {
            Disable = Yes;
            SetModifier(modifierId, modifier);
        }

        /// <summary>
        /// 禁用
        /// </summary>
        /// <param name="disabled">禁用状态</param>
        /// <param name="modifierId">禁用者的id</param>
        /// <param name="modifier">禁用者的姓名</param>
        public virtual void SetDisable(StatusEntityEnum disabled, string modifierId, string modifier)
        {
            Disable = disabled == StatusEntityEnum.Yes ? Yes : No;
            SetModifier(modifierId, modifier);
        }
        #endregion
    }

    /// <summary>
    /// 状态枚举
    /// </summary>
    public enum StatusEntityEnum
    { 
        /// <summary>
        /// 是
        /// </summary>
        Yes,
        /// <summary>
        /// 否
        /// </summary>
        No
    }
}
