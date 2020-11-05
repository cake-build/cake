// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core.Diagnostics;

namespace Cake.Frosting.Internal.Arguments
{
    internal static class VerbosityParser
    {
        private static readonly Dictionary<string, Verbosity> Lookup;

        static VerbosityParser()
        {
            Lookup = new Dictionary<string, Verbosity>(StringComparer.OrdinalIgnoreCase)
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
        }

        public static bool TryParse(string value, out Verbosity verbosity)
        {
            return Lookup.TryGetValue(value, out verbosity);
        }
    }
}
