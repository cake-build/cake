using System;
using System.Collections.Generic;
using System.Text;

namespace Cake.Core.Extensions
{
    internal static class TypeExtensions
    {
        public static bool IsStatic(this Type type)
        {
            return type.IsAbstract && type.IsSealed;
        }

        public static string GetFullName(this Type type, bool includeNamespace = true)
        {
            if (type.IsGenericParameter)
            {
                return type.Name;
            }
            return type.IsGenericType
                ? GetGenericTypeName(type, includeNamespace)
                : includeNamespace ? type.FullName : type.Name;
        }

        private static string GetGenericTypeName(this Type type, bool includeNamespace)
        {
            var builder = new StringBuilder();
            if (includeNamespace)
            {
                builder.Append(type.Namespace);
                builder.Append(".");
            }
            builder.Append(type.Name.Substring(0, type.Name.IndexOf('`')));
            builder.Append("<");
            builder.Append(GetGenericTypeArguments(type, includeNamespace));
            builder.Append(">");
            return builder.ToString();
        }

        private static string GetGenericTypeArguments(this Type type, bool includeNamespace)
        {
            var genericArguments = new List<string>();
            foreach (var argument in type.GetGenericArguments())
            {
                genericArguments.Add(GetFullName(argument, includeNamespace));
            }
            return string.Join(", ", genericArguments);
        }
    }
}
