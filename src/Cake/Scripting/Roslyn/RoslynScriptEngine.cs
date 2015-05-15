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

        public IScriptCodeGenerator GetCodeGenerator()
        {
            return new RoslynCodeGenerator();
        }

        public IScriptSession CreateSession(IScriptHost host, IDictionary<string, string> arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }

            // Are we using the experimental bits?
            var experimental = arguments.ContainsKey("experimental");
            if (!experimental)
            {
                // Are we running Windows 10, build 10041?
                var operativeSystemVersion = Environment.OSVersion.Version;
                if (operativeSystemVersion.Major == 10 && operativeSystemVersion.Build >= 10041)
                {
                    // Since .NET 4.6.1 is default here and since Roslyn
                    // isn't compatible with that framework version,
                    // we default to the experimental bits.
                    experimental = true;
                }
            }

            // Create the script session.
            _log.Debug("Creating script session...");
            if (experimental)
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
