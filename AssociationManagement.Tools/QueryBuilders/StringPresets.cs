
using System.Reflection;

namespace AssociationManagement.Tools.QueryBuilder
{
    internal static class StringPresets
    {
        internal static readonly Type StringType = typeof(string);

        internal static readonly MethodInfo ContainsMethod = StringType.GetRuntimeMethod("Contains", new[] { StringType });
    }
}
