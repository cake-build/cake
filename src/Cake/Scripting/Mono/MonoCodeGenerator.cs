using System.Collections.Generic;
using Cake.Core.Scripting;
using System.Text;

namespace Cake.Scripting.Mono
{
    internal sealed class MonoCodeGenerator
    {
        public string Generate(Script script)
        {
            var code = new StringBuilder ();

            code.AppendLine ("public class CakeBuildScriptImpl");
            code.AppendLine ("{");
            code.AppendLine ("    public CakeBuildScriptImpl (IScriptHost scriptHost)");
            code.AppendLine ("    {");
            code.AppendLine ("        ScriptHost = scriptHost;");
            code.AppendLine ("    }");
            code.AppendLine ();

            code.Append (GetAliasCode (script));

            code.AppendLine ();
            code.AppendLine (GetScriptHostProxy ());
            code.AppendLine ();

            code.AppendLine ("    public void Execute ()");
            code.AppendLine ("    {");

            foreach (var l in script.Lines) {
                if (l.StartsWith ("#line 1", System.StringComparison.InvariantCulture))
                    code.AppendLine ("        // " + l);
                else
                    code.AppendLine ("        " + l);               
            }

            code.AppendLine ("    }");
            code.AppendLine ("}");

            return code.ToString ();
        }

        static string GetAliasCode(Script context)
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

        static string GetScriptHostProxy ()
        {
            return "    " + string.Join ("\r\n    ", new string[] {
            "IScriptHost ScriptHost { get; set; }",
            "ICakeContext Context { get { return ScriptHost.Context; } }",
            "IReadOnlyList<CakeTask> Tasks { get { return ScriptHost.Tasks; } }",
            "CakeTaskBuilder<ActionTask> Task(string name) { return ScriptHost.Task (name); }",
            "void Setup (Action action) { ScriptHost.Setup (action); }",
            "void Teardown(Action action) { ScriptHost.Teardown (action); }",
            "CakeReport RunTarget(string target) { return ScriptHost.RunTarget (target); }" });
        }
    }
}
