// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.Reflection;
using Cake.Core.Scripting;

namespace Cake.Scripting.Roslyn
{
    internal sealed class RoslynScriptEngine : IScriptEngine
    {
        private readonly CakeOptions _options;
        private readonly IAssemblyLoader _loader;
        private readonly ICakeLog _log;

        public RoslynScriptEngine(CakeOptions options, IAssemblyLoader loader, ICakeLog log)
        {
            _options = options;
            _loader = loader;
            _log = log;
        }

        public IScriptSession CreateSession(IScriptHost host, IDictionary<string, string> arguments)
        {
            if (_options.PerformDebug)
            {
                return new RoslynDebugScriptSession(host, _loader, _log);
            }
            return new RoslynScriptSession(host, _loader, _log);
        }
    }
}