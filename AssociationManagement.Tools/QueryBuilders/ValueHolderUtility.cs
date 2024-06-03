using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AssociationManagement.Tools.QueryBuilder
{
    public static class ValueHolderUtility
    {
        public static object GetValueHolderWithValue(object value)
        {
            var valueHolderType = ValueHolderPresets.ValueHolderType;

            Type[] typeArgs = { value.GetType() };

            var genericType = valueHolderType.MakeGenericType(typeArgs);

            object instance = Activator.CreateInstance(genericType);

            var valueProperty = instance.GetType().GetProperty(nameof(ValueHolder<object>.Value),
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            valueProperty.SetValue(instance, value, null);

            return instance;
        }
    }

}
