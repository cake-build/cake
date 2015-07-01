using System;
using Cake.Core.Scripting;
using Cake.Core.Diagnostics;
using System.Collections.Generic;

namespace Cake.Scripting.Mono
{
    internal sealed class MonoScriptEngine : IScriptEngine
    {
        private readonly ICakeLog _log;

        public MonoScriptEngine(ICakeLog log)
        {
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

            return new MonoScriptSession (host, _log);
        }
    }
}

