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
        private readonly RoslynNightlyScriptSessionFactory _nightlyFactory;
        private readonly ICakeLog _log;

        public RoslynScriptEngine(
            RoslynScriptSessionFactory stableFactory,
            RoslynNightlyScriptSessionFactory nightlyFactory,
            ICakeLog log)
        {
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
            if (arguments.ContainsKey("experimental"))
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
