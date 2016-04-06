using System;
using System.Globalization;

namespace FluentTc.Engine
{
    public static class UniversalTypeConverter
    {
        public static T StringToType<T>(string value)
        {
            return (T) StringToType(value, typeof(T));
        }

        private static object StringToType(string value, Type propertyType)
        {
            var underlyingType = Nullable.GetUnderlyingType(propertyType);
            if (underlyingType == null)
                return Convert.ChangeType(value, propertyType, CultureInfo.InvariantCulture);
            return String.IsNullOrEmpty(value)
              ? null
              : Convert.ChangeType(value, underlyingType, CultureInfo.InvariantCulture);
        } 
    }
}