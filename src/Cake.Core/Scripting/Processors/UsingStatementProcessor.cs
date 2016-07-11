// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Linq;
using Cake.Core.Scripting.Analysis;

namespace Cake.Core.Scripting.Processors
{
    internal sealed class UsingStatementProcessor : LineProcessor
    {
        public UsingStatementProcessor(ICakeEnvironment environment)
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
            if (tokens.Length <= 1)
            {
                return false;
            }

            if (!tokens[0].Equals("using", StringComparison.Ordinal))
            {
                return false;
            }

            // Using block?
            var @namespace = tokens[1].TrimEnd(';');
            if (@namespace.StartsWith("("))
            {
                return false;
            }

            // Using alias directive?
            if (tokens.Any(t => t == "="))
            {
                context.Script.UsingAliases.Add(string.Join(" ", tokens));
                return true;
            }

            // Namespace
            context.Script.Namespaces.Add(@namespace);
            return true;
        }
    }
}
