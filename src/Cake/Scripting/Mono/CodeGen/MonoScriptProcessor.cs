using System;
using System.Collections.Generic;
using Cake.Core.Scripting;
using Cake.Scripting.Mono.CodeGen.Parsing;

namespace Cake.Scripting.Mono.CodeGen
{
    internal static class MonoScriptProcessor
    {
        public static Script Process(Script script, out IReadOnlyList<CodeBlock> blocks)
        {
            var code = string.Join(Environment.NewLine, script.Lines);
            var parsedBlocks = ParseBlocks(code);
            var lines = new List<string>();
            var result = new List<CodeBlock>();
            foreach (var codeBlock in parsedBlocks)
            {
                if (codeBlock.HasScope)
                {
                    result.Add(codeBlock);
                }
                else
                {
                    lines.Add(codeBlock.Content);
                }
            }
            blocks = result; // Assign the parsed blocks.
            return new Script(script.Namespaces, lines, script.Aliases);
        }

        private static IEnumerable<CodeBlock> ParseBlocks(string code)
        {
            var result = new List<CodeBlock>();
            using (var scanner = new CodeBlockParser(code))
            {
                while (true)
                {
                    var block = scanner.GetBlock();
                    if (block == null)
                    {
                        break;
                    }
                    result.Add(block);
                }
            }
            return result;
        }
    }
}
