// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Polyfill
{
    internal static class EnvironmentHelper
    {
        public static string GetCommandLine()
        {
#if NETCORE
            // Extremely naive solution.
            return string.Join(" ", Environment.GetCommandLineArgs());
#else
            return Environment.CommandLine;
#endif
        }
    }
}
