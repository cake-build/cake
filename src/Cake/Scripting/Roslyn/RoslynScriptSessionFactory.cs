using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.Scripting;
using Cake.Scripting.Roslyn.Installation;
using Roslyn.Scripting.CSharp;

namespace Cake.Scripting.Roslyn
{
    internal sealed class RoslynScriptSessionFactory : IScriptSessionFactory
    {
        private readonly ICakeEnvironment _environment;
        private readonly IRoslynInstaller _installer;
        private readonly ICakeLog _log;

        public RoslynScriptSessionFactory(
            ICakeEnvironment environment,
            IRoslynInstaller installer, 
            ICakeLog log)
        {
            _environment = environment;
            _installer = installer;
            _log = log;
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

            // Get the installation instructions.
            var root = _environment.GetApplicationRoot();
            var instructions = RoslynHelpers.GetInstallationInstructions(experimental);

            // Should we install Roslyn?
            if (!_installer.IsInstalled(root, instructions))
            {
                _installer.Install(root, instructions);
            }

            // Create the script session.
            _log.Debug("Creating script session...");
            if (experimental)
            {
                // Use the nightly build.
                _log.Information("Using nightly build of Roslyn.");
                return new RoslynNightlyScriptSession(host, _log);
            }

            // Use the stable build.
            var roslynScriptEngine = new ScriptEngine();
            var session = roslynScriptEngine.CreateSession(host, typeof(IScriptHost));
            return new RoslynScriptSession(session, _log);
        }
    }
}
