using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cake.Arguments
{
    internal sealed class ArgumentTokenizer
    {
        public static string[] Tokenize(string args)
        {
            return TokenizeCore(args).ToArray();
        }

        private static IEnumerable<string> TokenizeCore(string commandLine)
        {
            if (string.IsNullOrWhiteSpace(commandLine))
            {
                yield break;
            }
            var index = commandLine.IndexOf(' ');
            if (index == -1)
            {
                yield break;
            }
            newvalue:
            var sb = new StringBuilder();
            var inQuote = false;
            for (; ++index < commandLine.Length;)
            {
                var c = commandLine[index];
                switch (c)
                {
                    case '"':
                    {
                        inQuote = !inQuote;
                        break;
                    }
                    case ' ':
                    {
                        if (inQuote)
                        {
                            break;
                        }
                        if (sb.Length > 1)
                        {
                            yield return sb.ToString();
                        }
                        goto newvalue;
                    }
                }
                sb.Append(c);
            }
            if (sb.Length > 0)
            {
                yield return sb.ToString();
            }
        }
    }
}
