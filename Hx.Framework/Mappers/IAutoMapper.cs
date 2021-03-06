﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Framework.Mappers
{
    /// <summary>
    /// 自动映射接口，只要实体类继承该接口，即可使用Mapper进行映射属性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAutoMapper<T>where T:class,new()
    {
    }
}
