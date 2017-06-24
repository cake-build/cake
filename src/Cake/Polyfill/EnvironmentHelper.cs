// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.Text;

namespace Cake.Polyfill
{
    public static IEnumerable<string> GetCommandLineArgs(bool skipExecutable = true)
    {
#if NETCORE
        return Environment.GetCommandLineArgs()
            .Skip(skipExecutable ? 1 : 0);
#else
        return QuoteAwareStringSplitter
            .Split(Environment.CommandLine)
            .Skip(skipExecutable ? 1 : 0);
#endif
    }
}
