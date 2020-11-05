// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.Diagnostics;
using Cake.Frosting.Internal.Diagnostics;

namespace Cake.Frosting.Internal
{
    internal sealed class ErrorCakeHostBuilder : ICakeHostBuilder
    {
        private readonly ICakeLog _log;
        private readonly Exception _exception;

        public ErrorCakeHostBuilder(Exception exception)
        {
            _log = new CakeLog(new DefaultConsole(), Verbosity.Verbose);
            _exception = exception;
        }

        public ICakeHostBuilder ConfigureServices(Action<ICakeServices> configureServices)
        {
            return this;
        }

        public ICakeHost Build()
        {
            return new ErrorCakeHost(_log, _exception);
        }
    }
}