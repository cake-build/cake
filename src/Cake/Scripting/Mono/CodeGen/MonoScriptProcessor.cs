// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Scripting;
using Cake.Scripting.Mono.CodeGen.Parsing;

namespace Cake.Scripting.Mono.CodeGen
{
    internal static class MonoScriptProcessor
    {
        public static Script Process(Script script, out IReadOnlyList<ScriptBlock> blocks)
        {
            var lines = new List<string>();
            var result = new List<ScriptBlock>();
            foreach (var codeBlock in ParseBlocks(script))
            {
                if (codeBlock.HasScope)
                {
                    result.Add(codeBlock);
                }
                else
                {
                    lines.AddRange(codeBlock.Content.SplitLines());
                }
            }
            blocks = result; // Assign the parsed blocks.
            return new Script(script.Namespaces, lines, script.Aliases, script.UsingAliasDirectives);
        }

        private static IEnumerable<ScriptBlock> ParseBlocks(Script script)
        {
            var code = string.Join(Environment.NewLine, script.Lines);
            using (var parser = new ScriptParser(code))
            {
                var result = new List<ScriptBlock>();
                while (true)
                {
                    var block = parser.ParseNext();
                    if (block == null)
                    {
                        break;
                    }
                    result.Add(block);
                }
                return result;
            }
        }
    }
}
