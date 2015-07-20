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
            var parsedBlocks = ParseBlocks(GetCode(script));
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

        private static string GetCode(Script script)
        {
            // TODO: Kind of optimistic but will work for now.
            var result = new List<string>();
            foreach (var line in script.Lines)
            {
                if (line.StartsWith("#"))
                {
                    continue;
                }
                if (line.StartsWith("//"))
                {
                    continue;
                }
                if (line.StartsWith("/*"))
                {
                    continue;
                }
                if (line.EndsWith("*/"))
                {
                    continue;
                }
                result.Add(line);
            }
            return string.Join(Environment.NewLine, result);
        }
    }
}
