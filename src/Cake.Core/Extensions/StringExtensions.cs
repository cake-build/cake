using System;

namespace Cake.Core.Extensions
{
    public static class StringExtensions
    {
        public static string Quote(this string value)
        {
            if (!IsQuoted(value))
            {
                value = string.Concat("\"", value, "\"");
            }
            return value;
        }

        public static string UnQuote(this string value)
        {
            if (IsQuoted(value))
            {
                value = value.Trim('"');                
            }
            return value;
        }

        private static bool IsQuoted(this string value)
        {
            return value.StartsWith("\"", StringComparison.OrdinalIgnoreCase)
                   && value.EndsWith("\"", StringComparison.OrdinalIgnoreCase);
        }
    }
}
