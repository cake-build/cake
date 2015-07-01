using System.Collections.Generic;
using Cake.Core.Scripting;

namespace Cake.Scripting.Mono
{
    internal sealed class MonoCodeGenerator
    {
        public string Generate(Script script)
        {
            var alteredLines = new List<string> ();

            foreach (var l in script.Lines) {
                if (l.StartsWith ("#line 1"))
                    alteredLines.Add ("// " + l);
                else
                    alteredLines.Add (l);               
            }

            var aliases = GetAliasCode(script);
            var code = string.Join("\r\n", alteredLines);
            return string.Join("\r\n", aliases, code);
        }

        private static string GetAliasCode(Script context)
        {
            var result = new List<string>();
            foreach (var alias in context.Aliases)
            {
                result.Add(alias.Type == ScriptAliasType.Method
                    ? MethodAliasGenerator.Generate(alias.Method)
                    : PropertyAliasGenerator.Generate(alias.Method));
            }
            return string.Join("\r\n", result);
        }
    }
}
