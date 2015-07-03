using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Cake.Core.Scripting;

namespace Cake.Scripting.Mono
{
    internal sealed class MonoCodeGenerator
    {
        public string Generate(Script script, out int codeLineOffset)
        {
            codeLineOffset = 0;

            var code = new StringBuilder();

            var scriptLines = new StringBuilder();
            var extrasLines = new StringBuilder();

            var isInExtras = false;

            foreach (var l in script.Lines) 
            {
                var line = l;

                if (line.StartsWith("#line", StringComparison.InvariantCultureIgnoreCase)) 
                {
                    line = "// " + line;
                }

                if (line.StartsWith("#region \"extras\"")) 
                {
                    isInExtras = true;
                    continue;
                }

                if (isInExtras && line.StartsWith("#endregion", StringComparison.InvariantCultureIgnoreCase)) 
                {
                    isInExtras = false;
                    continue;
                }

                if (isInExtras) 
                {
                    extrasLines.AppendLine(line);
                } 
                else 
                {
                    scriptLines.AppendLine(line);                
                }
            }

            code.AppendLine("public class CakeBuildScriptImpl");
            code.AppendLine("{");
            code.AppendLine("    public CakeBuildScriptImpl (IScriptHost scriptHost)");
            code.AppendLine("    {");
            code.AppendLine("        ScriptHost = scriptHost;");
            code.AppendLine("    }");
            code.AppendLine();
            code.AppendLine(GetAliasCode(script));
            code.AppendLine();
            code.AppendLine(GetScriptHostProxy());
            code.AppendLine();
            code.AppendLine(extrasLines.ToString());
            code.AppendLine();
            code.AppendLine("    public void Execute ()");
            code.AppendLine("    {");

            // Pass back where the 'user' code starts so we can calculate offsets of errors/warnings
            codeLineOffset = Regex.Matches(code.ToString(), Environment.NewLine).Count;

            code.AppendLine(scriptLines.ToString());
            code.AppendLine();
            code.AppendLine("    }");
            code.AppendLine("}");

            return code.ToString();
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

        private static string GetScriptHostProxy()
        {
            var rules = new string[] 
            {
                "IScriptHost ScriptHost { get; set; }",
                "ICakeContext Context { get { return ScriptHost.Context; } }",
                "IReadOnlyList<CakeTask> Tasks { get { return ScriptHost.Tasks; } }",
                "CakeTaskBuilder<ActionTask> Task(string name) { return ScriptHost.Task (name); }",
                "void Setup (Action action) { ScriptHost.Setup (action); }",
                "void Teardown(Action action) { ScriptHost.Teardown (action); }",
                "CakeReport RunTarget(string target) { return ScriptHost.RunTarget (target); }" 
            };

            return "    " + string.Join("\r\n    ", rules);
        }
    }
}
