using System;
using System.ComponentModel;
using System.Globalization;

namespace Core
{
    public class StringToTypeConvertor
    {
        public static object Convert(string sourceString, Type targetType, CultureInfo culture = null)
        {
            object result = null;
            bool isEmpty = string.IsNullOrEmpty(sourceString);
            if (culture == null)
                culture = CultureInfo.CurrentCulture;
            TypeConverter converter = TypeDescriptor.GetConverter(targetType);
            result = converter.ConvertFromString(null, culture, sourceString);
            return result;
        }
    }
}
