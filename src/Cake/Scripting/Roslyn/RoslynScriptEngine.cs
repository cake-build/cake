// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.Scripting;
using Cake.Scripting.Roslyn.Nightly;
using Cake.Scripting.Roslyn.Stable;

namespace Cake.Scripting.Roslyn
{
    internal sealed class RoslynScriptEngine : IScriptEngine
    {
        private readonly RoslynScriptSessionFactory _stableFactory;
        private readonly CakeOptions _options;
        private readonly RoslynNightlyScriptSessionFactory _nightlyFactory;
        private readonly ICakeLog _log;

        public RoslynScriptEngine(
            CakeOptions options,
            RoslynScriptSessionFactory stableFactory,
            RoslynNightlyScriptSessionFactory nightlyFactory,
            ICakeLog log)
        {
            _options = options;
            _nightlyFactory = nightlyFactory;
            _stableFactory = stableFactory;
            _log = log;
        }

        public IScriptSession CreateSession(IScriptHost host, IDictionary<string, string> arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }

            // Create the script session.
            _log.Debug("Creating script session...");

            // Are we using the experimental bits?
            if (_options.Experimental)
            {
                // Use the nightly build.
                _log.Debug("Using prerelease build of Roslyn.");
                return _nightlyFactory.CreateSession(host);
            }

            // Use the stable build.
            return _stableFactory.CreateSession(host);
        }
    }
}
