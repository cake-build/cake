// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.Scripting.Analysis;

namespace Cake.Core.Scripting.Processors;

internal sealed class UsingStatementProcessor : LineProcessor
{
    public override bool Process(IScriptAnalyzerContext context, string line, out string replacement)
    {
        ArgumentNullException.ThrowIfNull(context);

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

        // Using declaration with implicit type (i.e. using var x = ...)?
        if (tokens[1].Equals("var", StringComparison.Ordinal))
        {
            return false;
        }

        // Using alias directive (using Identifier = Name;)?
        if (line.Contains('='))
        {
            if (tokens.Length >= 4
                && tokens[2].Equals("=", StringComparison.Ordinal)
                && StripComment(line).EndsWith(';'))
            {
                context.Current.UsingAliases.Add(string.Join(" ", tokens));
                return true;
            }

            // Using declaration with explicit type (i.e. using Foo x = ...)
            // or spanning multiple lines.
            return false;
        }

        // Using static directive?
        if (tokens.Length == 3 && tokens[1].Equals("static", StringComparison.Ordinal))
        {
            context.Current.UsingStaticDirectives.Add(string.Join(" ", tokens));
            return true;
        }

        // Using namespace directive (using Name;)?
        if (tokens.Length == 2
            || tokens[1].EndsWith(';')
            || tokens[2].StartsWith(';')
            || tokens[2].StartsWith("//", StringComparison.Ordinal))
        {
            context.Current.Namespaces.Add(@namespace);
            return true;
        }

        // Something else, i.e. a using declaration spanning multiple lines.
        return false;
    }

    private static string StripComment(string line)
    {
        var index = line.IndexOf("//", StringComparison.Ordinal);
        return (index < 0 ? line : line[..index]).TrimEnd();
    }
}
