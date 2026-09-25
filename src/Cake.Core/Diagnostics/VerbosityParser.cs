// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Cake.Core.Diagnostics;

/// <summary>
/// Parses <see cref="Verbosity"/> values from strings, including CLI aliases.
/// </summary>
public static class VerbosityParser
{
    private static readonly Dictionary<string, Verbosity> Lookup =
        new Dictionary<string, Verbosity>(StringComparer.OrdinalIgnoreCase)
        {
            { "q", Verbosity.Quiet },
            { "quiet", Verbosity.Quiet },
            { "m", Verbosity.Minimal },
            { "minimal", Verbosity.Minimal },
            { "n", Verbosity.Normal },
            { "normal", Verbosity.Normal },
            { "v", Verbosity.Verbose },
            { "verbose", Verbosity.Verbose },
            { "d", Verbosity.Diagnostic },
            { "diagnostic", Verbosity.Diagnostic }
        };

    /// <summary>
    /// Tries to parse a verbosity value from a string.
    /// </summary>
    /// <param name="value">The value to parse.</param>
    /// <param name="verbosity">The parsed verbosity when parsing succeeds.</param>
    /// <returns><c>true</c> when <paramref name="value"/> is a recognized verbosity; otherwise <c>false</c>.</returns>
    public static bool TryParse(string value, out Verbosity verbosity)
    {
        if (!string.IsNullOrWhiteSpace(value) && Lookup.TryGetValue(value.Trim(), out verbosity))
        {
            return true;
        }

        verbosity = default;
        return false;
    }
}
