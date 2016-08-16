// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NETCORE
using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.Reflection;
using Cake.Core.Scripting;

namespace Cake.Scripting.XPlat
{
    internal sealed class XPlatScriptEngine : IScriptEngine
    {
        private readonly CakeOptions _options;
        private readonly IAssemblyLoader _loader;
        private readonly ICakeLog _log;

        public XPlatScriptEngine(CakeOptions options, IAssemblyLoader loader, ICakeLog log)
        {
            _options = options;
            _loader = loader;
            _log = log;
        }

        public IScriptSession CreateSession(IScriptHost host, IDictionary<string, string> arguments)
        {
            if (_options.PerformDebug)
            {
                return new DebugXPlatScriptSession(host, _loader, _log);
            }
            return new DefaultXPlatScriptSession(host, _loader, _log);
        }
    }
}
#endif