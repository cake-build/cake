using System;

// ReSharper disable once CheckNamespace
namespace Cake.Core
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

        public static string[] SplitLines(this string content)
        {
            content = NormalizeLineEndings(content);
            return content.Split(new[] { "\r\n" }, StringSplitOptions.None);
        }

        public static string NormalizeLineEndings(this string value)
        {
            value = value.Replace("\r\n", "\n");
            value = value.Replace("\r", string.Empty);
            return value.Replace("\n", "\r\n");
        }
    }
}
