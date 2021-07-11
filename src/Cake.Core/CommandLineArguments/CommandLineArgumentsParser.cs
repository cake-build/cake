using System;
using System.Collections.Generic;

namespace Cake.Core.CommandLineArguments
{
    internal static class CommandLineArgumentsParser
    {
        public static IDictionary<string, string> Parse(string[] arguments)
        {
            var length = arguments.Length - 1;
            var dict = new Dictionary<string, string>(length);

            if (length == 0)
            {
                return dict;
            }

            string key = null;
            for (var i = 1; i < arguments.Length; i++)
            {
                var arg = arguments[i];

                if (string.IsNullOrWhiteSpace(key))
                {
                    if (IsSingleWordArgumentPair(arg))
                    {
                        var pair = SeparateSingleWordArgumentPair(arg);
                        dict[pair[0]] = pair[1];
                        continue;
                    }

                    if (IsTwoWordsArgumentPair(arg))
                    {
                        key = arg.TrimStart('-');
                        continue;
                    }

                    dict[arg] = string.Empty;
                }
                else
                {
                    dict[key] = arg;
                    key = null;
                }
            }

            return dict;
        }

        private static bool IsTwoWordsArgumentPair(string str) =>
            str.StartsWith("-");

        private static bool IsSingleWordArgumentPair(string str) =>
            str.IndexOf("=", StringComparison.InvariantCulture) > 0;

        private static string[] SeparateSingleWordArgumentPair(string str)
        {
            var index = str.IndexOf("=", StringComparison.InvariantCulture);
            return new[] { str.Substring(0, index), str.Substring(index + 1, str.Length - index - 1) };
        }
    }
}