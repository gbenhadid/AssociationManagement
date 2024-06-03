using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AssociationManagement.Tools.QueryBuilder
{
    public static class ValueCastUtility
    {
        public static object CastValueToType(object initialValue, Type targetType)
        {
            if (initialValue == null)
                throw new ArgumentNullException(nameof(initialValue));

            if (targetType == StringPresets.StringType)
                return initialValue.ToString();

            if (targetType.GetTypeInfo().IsEnum)
                return Enum.Parse(targetType, initialValue.ToString());

            if (typeof(IConvertible).IsAssignableFrom(targetType))
                return Convert.ChangeType(initialValue, targetType);

            throw new InvalidCastException($"Cannot convert value to type {targetType.Name}.");
        }

        public static object CastJsonStringToArrayOfType(string jsonString, Type targetType)
        {
            var jsonElement = JsonDocument.Parse(jsonString).RootElement;
            var arrayType = targetType.MakeArrayType();
            return JsonSerializer.Deserialize(jsonElement.GetRawText(), arrayType);
        }
    }
}
