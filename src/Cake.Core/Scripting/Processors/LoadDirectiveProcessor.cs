// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core.IO;
using Cake.Core.Scripting.Analysis;

namespace Cake.Core.Scripting.Processors
{
    internal sealed class LoadDirectiveProcessor : LineProcessor
    {
        private readonly NugetScriptDirectiveProcessor _nugetScriptDirectiveProcessor;

        public LoadDirectiveProcessor(ICakeEnvironment environment)
            : base(environment)
        {
            _nugetScriptDirectiveProcessor = new NugetScriptDirectiveProcessor(environment);
        }

        public override bool Process(IScriptAnalyzerContext context, string line, out string replacement)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            replacement = null;

            var tokens = Split(line);
            if (tokens.Length <= 0)
            {
                return false;
            }

            if (!tokens[0].Equals("#l", StringComparison.Ordinal) &&
                !tokens[0].Equals("#load", StringComparison.Ordinal))
            {
                return false;
            }

            var value = tokens[1];
            
            // Check if this is a nuget reference
            if (value.Contains("nuget:"))
            {
                // How to Support both "#l" and "#load", replace to #nuscript and process it with the NugetScriptDirectiveProcessor.
                line = line.Replace(tokens[0], NugetScriptDirectiveProcessor.DirectiveName);

                // Set the replacement line to the modified line.
                replacement = string.Concat("// ", line);

                // We need a out value to call the processor.
                string outValue;

                // This is a nuget reference use the NugetScript processor.
                return _nugetScriptDirectiveProcessor.Process(context, line, out outValue);
            }

            var directoryPath = GetAbsoluteDirectory(context.Script.Path);
            var scriptPath = new FilePath(value.UnQuote()).MakeAbsolute(directoryPath);

            context.Analyze(scriptPath);

            return true;
        }
    }
}
