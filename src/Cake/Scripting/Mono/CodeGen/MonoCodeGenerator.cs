using System.Collections.Generic;
using System.Text;
using Cake.Core.Scripting;
using Cake.Scripting.Mono.CodeGen.Parsing;

namespace Cake.Scripting.Mono.CodeGen
{
    internal sealed class MonoCodeGenerator
    {
        public static string Generate(Script script)
        {
            // Process the script.
            IReadOnlyList<ScriptBlock> blocks;
            script = MonoScriptProcessor.Process(script, out blocks);

            var code = new StringBuilder();

            if (script.UsingAliasDirectives.Count > 0)
            {
                foreach (var usingAliasDirective in script.UsingAliasDirectives)
                {
                    code.AppendLine(usingAliasDirective);
                }
                code.AppendLine();
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

            foreach (var block in blocks)
            {
                code.AppendLine(block.Content);
                code.AppendLine();
            }

            code.AppendLine("    public void Execute ()");
            code.AppendLine("    {");

            code.AppendLine(string.Join("\n", script.Lines));
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
                    ? MonoMethodAliasGenerator.Generate(alias.Method)
                    : MonoPropertyAliasGenerator.Generate(alias.Method));
            }
            return string.Join("\r\n", result);
        }

        private static string GetScriptHostProxy()
        {
            // TODO: Generate this from interface.
            var rules = new[]
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