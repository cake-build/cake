// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.Reflection;
using Cake.Core.Scripting;

namespace Cake.Infrastructure.Scripting
{
    public sealed class RoslynScriptEngine : IScriptEngine
    {
        private readonly IAssemblyLoader _loader;
        private readonly IScriptHostSettings _settings;
        private readonly ICakeConfiguration _configuration;
        private readonly ICakeLog _log;

        public RoslynScriptEngine(IAssemblyLoader loader, IScriptHostSettings settings, ICakeConfiguration configuration, ICakeLog log)
        {
            _loader = loader;
            _settings = settings;
            _configuration = configuration;
            _log = log;
        }

        public IScriptSession CreateSession(IScriptHost host)
        {
            return new RoslynScriptSession(host, _loader, _configuration, _log, _settings);
        }
    }
}