using Hx.Sdk.ImageSharp;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace Hx.Sdk.NetCore.Test
{
    static class Program
    {
        const string LongText = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec aliquet lorem at magna mollis, non semper erat aliquet. In leo tellus, sollicitudin non eleifend et, luctus vel magna. Proin at lacinia tortor, malesuada molestie nisl. Quisque mattis dui quis eros ultricies, quis faucibus turpis dapibus. Donec urna ipsum, dignissim eget condimentum at, condimentum non magna. Donec non urna sit amet lectus tincidunt interdum vitae vitae leo. Aliquam in nisl accumsan, feugiat ipsum condimentum, scelerisque diam. Vivamus quam diam, rhoncus ut semper eget, gravida in metus.
Nullam quis malesuada metus. In hac habitasse platea dictumst. Aliquam faucibus eget eros nec vulputate. Quisque sed dolor lacus. Proin non dolor vitae massa rhoncus vestibulum non a arcu. Morbi mollis, arcu id pretium dictum, augue dui cursus eros, eu pharetra arcu ante non lectus. Integer quis tellus ipsum. Integer feugiat augue id tempus rutrum. Ut eget interdum leo, id fermentum lacus. Morbi euismod, mi at tempus finibus, ante risus ornare eros, eu ultrices ipsum dolor vitae risus. Mauris molestie pretium massa vitae maximus. Fusce ut egestas ex, vitae semper nulla. Proin pretium elit libero, et interdum enim molestie ac.
Pellentesque fermentum vitae lacus non aliquet. Sed nulla ipsum, hendrerit sit amet vulputate varius, volutpat eget est. Pellentesque eget ante erat. Vestibulum venenatis ex quis pretium sagittis. Etiam vel nibh sit amet leo gravida efficitur. In hac habitasse platea dictumst. Nullam lobortis euismod sem dapibus aliquam. Proin accumsan velit a magna gravida condimentum. Nam non massa ac nibh viverra rutrum. Phasellus elit tortor, malesuada et purus nec, placerat mattis neque. Proin auctor risus vel libero ultrices, id fringilla erat facilisis. Donec rutrum, enim sit amet faucibus viverra, velit tellus aliquam tellus, et tempus tellus diam sed dui. Integer fringilla convallis nisl venenatis elementum. Sed volutpat massa ut mauris accumsan, mollis finibus tortor pretium.";
        static void Main(string[] args)
        {
            //System.IO.Directory.CreateDirectory("output");
            //string letter = "A short piece of text";
            //var fontOptions = new ImageSharp.Fonts.FontOptions(LongText);
            //fontOptions.Wordwrap = true;
            //fontOptions.WaterLocation = WaterLocation.RightBottom;
            //ImageManager.MarkLetterWater("fb.jpg", fontOptions);
            //Console.WriteLine("结束");

            var valueType = typeof(ValueConverter);
            var stringType = typeof(EnumToStringConverter<Blog_Enum>);
            ConverterMappingHints mappingHints = null;
            Type[] types = stringType.GetGenericArguments();
            //var toIntType = stringType.MakeGenericType(types);
            object toIntInstance = Activator.CreateInstance(stringType, mappingHints);
            Console.WriteLine(valueType.IsAssignableFrom(stringType));
            Console.ReadLine();
        }

        public static bool IsAssignableFromGenericType(this Type genericType, Type givenType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = givenType.BaseType;
            if (baseType == null) return false;

            return genericType.IsAssignableFromGenericType(baseType);
        }
    }

   
}
