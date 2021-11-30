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
        public override bool Process(IScriptAnalyzerContext context, string line, out string replacement)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
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

            // Using disposable block?
            var @namespace = tokens[1].TrimEnd(';');
            if (@namespace.StartsWith("("))
            {
                return false;
            }

            // Using disposable statement?
            const int usingLength = 5;
            int openParentheses = line.IndexOf('(', usingLength),
                closeParentheses = openParentheses < usingLength ? -1 : line.IndexOf(')', openParentheses);

            if (closeParentheses > openParentheses)
            {
                return false;
            }

            // Using alias directive?
            if (tokens.Any(t => t == "="))
            {
                context.Current.UsingAliases.Add(string.Join(" ", tokens));
                return true;
            }

            // Using static directive?
            if (tokens.Length == 3 && tokens[1].Equals("static", StringComparison.Ordinal))
            {
                context.Current.UsingStaticDirectives.Add(string.Join(" ", tokens));
                return true;
            }

            // Namespace
            context.Current.Namespaces.Add(@namespace);
            return true;
        }
    }
}