// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using System.Text;
using Cake.Core.Scripting;
using Cake.Core.Scripting.CodeGen;
using Cake.Scripting.Mono.CodeGen.Parsing;

namespace Cake.Scripting.Mono.CodeGen
{
    internal static class MonoCodeGenerator
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

            code.AppendLine("public class CakeBuildScriptImpl : Cake.Scripting.Mono.CodeGen.CakeBuildScriptImplBase");
            code.AppendLine("{");
            code.AppendLine("    public CakeBuildScriptImpl (IScriptHost scriptHost):base(scriptHost) { }");
            code.AppendLine();
            code.AppendLine(GetAliasCode(script));
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
                    ? MethodAliasGenerator.Generate(alias.Method)
                    : PropertyAliasGenerator.Generate(alias.Method));
            }
            return string.Join("\r\n", result);
        }
    }
}
