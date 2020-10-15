// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.Diagnostics;

namespace Cake.Frosting.Internal
{
    internal static class ErrorHandler
    {
        public static int OutputError(ICakeLog log, Exception exception)
        {
            if (log.Verbosity == Verbosity.Diagnostic)
            {
                log.Error("Error: {0}", exception);
            }
            else
            {
                log.Error("Error: {0}", exception.Message);
            }
            return 1;
        }

        public static int GetExitCode(Exception exception)
        {
            return 1;
        }
    }
}
