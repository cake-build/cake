// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.Diagnostics;
using Cake.Frosting.Internal.Diagnostics;

namespace Cake.Frosting.Internal
{
    internal sealed class ErrorCakeHost : ICakeHost
    {
        private readonly ICakeLog _log;
        private readonly Exception _exception;

        public ErrorCakeHost(Exception exception)
            : this(null, exception)
        {
        }

        public ErrorCakeHost(ICakeLog log, Exception exception)
        {
            _log = log ?? new CakeLog(new DefaultConsole(), Verbosity.Verbose);
            _exception = exception;
        }

        public int Run()
        {
            ErrorHandler.OutputError(_log, _exception);
            return ErrorHandler.GetExitCode(_exception);
        }
    }
}
