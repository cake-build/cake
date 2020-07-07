// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.Reflection;
using Cake.Core.Scripting;

namespace Cake.Scripting.Roslyn
{
    internal sealed class RoslynScriptEngine : IScriptEngine
    {
        private readonly CakeOptions _options;
        private readonly ICakeConfiguration _configuration;
        private readonly IAssemblyLoader _loader;
        private readonly ICakeLog _log;

        public RoslynScriptEngine(CakeOptions options, ICakeConfiguration configuration, IAssemblyLoader loader, ICakeLog log)
        {
            _options = options;
            _configuration = configuration;
            _loader = loader;
            _log = log;
        }

        public IScriptSession CreateSession(IScriptHost host)
        {
            return new RoslynScriptSession(host, _loader, _log, _options, _configuration);
        }
    }
}