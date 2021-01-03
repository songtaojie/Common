using Hx.Sdk.EntityFrameworkCore.Attributes;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Sdk.NetCore.Test.EFCore
{
    public class Blog
    {
        public string Id { get; set; }

        [DecimalPrecision(18,3)]
        public decimal Active { get; set; }

        [ValueConverter(typeof(EnumToStringConverter<Blog_Enum>))]
        public Blog_Enum Deleted { get; set; }

        [ValueConverter(typeof(BoolToStringConverter), "N", "Y")]
        public bool IsDeleted { get; set; } = true;
    }

    public enum Blog_Enum
    { 
        Y,
        N
    }
}
