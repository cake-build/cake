// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using Cake.Core;
using Cake.Core.Scripting;
using Xunit;
using Cake.Scripting.Mono.CodeGen;
using Cake.Scripting.Mono.CodeGen.Parsing;

namespace Cake.Tests.Fixtures
{
    internal sealed class MonoScriptProcessorFixture
    {
        private readonly string _resourcePath;
        private static readonly Assembly _assembly = typeof(MonoScriptProcessorFixture).Assembly;

        public MonoScriptProcessorFixture(string resourcePath)
        {
            _resourcePath = resourcePath;
        }

        public string Process()
        {
            // Read all lines from the resource.
            var lines = ReadLinesFromResource(_assembly, _resourcePath, true);
            if (lines == null)
            {
                throw new InvalidOperationException("Could not find resource.");
            }

            // Create the script.
            var script = new Script(
                Enumerable.Empty<string>(),
                lines,
                Enumerable.Empty<ScriptAlias>(),
                Enumerable.Empty<string>());

            // Process the script.
            IReadOnlyList<ScriptBlock> blocks;
            var processedScript = MonoScriptProcessor.Process(script, out blocks);

            // Reassemble the script to something we can compare.
            return Reassemble(processedScript, blocks);
        }

        public string GetExpectedOutput()
        {
            // Get the expected output.
            var outputLines = ReadLinesFromResource(_assembly, _resourcePath, false);
            return outputLines.Reassemble();
        }

        private static IEnumerable<string> ReadLinesFromResource(Assembly assembly, string path, bool input)
        {
            Assert.NotNull(assembly);
            using (var stream = assembly.GetManifestResourceStream(string.Format("{0}.{1}", path, input ? "input" : "output")))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadAllLines();
                    }
                }
            }
            return null;
        }

        private static string Reassemble(Script result, IReadOnlyList<ScriptBlock> blocks)
        {
            var builder = new StringBuilder();

            if (blocks.Count > 0)
            {
                builder.AppendLine("// BLOCKS");
                foreach (var block in blocks)
                {
                    builder.AppendLine(block.Content);
                }
            }

            if (result.Lines.Count > 0)
            {
                if (blocks.Count > 0)
                {
                    builder.AppendLine();
                }
                builder.AppendLine("// LINES");
                foreach (var line in result.Lines)
                {
                    builder.AppendLine(line);
                }
            }

            return builder.ToString().NormalizeLineEndings();
        }
    }
}
