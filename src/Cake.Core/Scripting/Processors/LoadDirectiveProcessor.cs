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
        public LoadDirectiveProcessor(ICakeEnvironment environment)
            : base(environment)
        {
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

            var directoryPath = GetAbsoluteDirectory(context.Script.Path);
            var scriptPath = new FilePath(tokens[1].UnQuote()).MakeAbsolute(directoryPath);

            context.Analyze(scriptPath);

            return true;
        }
    }
}
