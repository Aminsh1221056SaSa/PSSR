using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSSR.Common
{
    public static class ExtensionHelper
    {
        public static T ParseEnum<T>(this string value) where T: struct
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static List<T> ConvetEnumToList<T>(this T valieable) where T : struct
        {
            return Enum.GetValues(typeof(T))
                        .Cast<T>().ToList();
        }
    }
}
