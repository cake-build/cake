// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.Reflection;
using Cake.Core.Scripting;

namespace Cake.Infrastructure.Scripting
{
    /// <summary>
    /// Represents a Roslyn-based script engine for Cake.
    /// </summary>
    public sealed class RoslynScriptEngine : IScriptEngine
    {
        private readonly IAssemblyLoader _loader;
        private readonly IScriptHostSettings _settings;
        private readonly ICakeConfiguration _configuration;
        private readonly ICakeLog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoslynScriptEngine"/> class.
        /// </summary>
        /// <param name="loader">The assembly loader.</param>
        /// <param name="settings">The script host settings.</param>
        /// <param name="configuration">The Cake configuration.</param>
        /// <param name="log">The log.</param>
        public RoslynScriptEngine(IAssemblyLoader loader, IScriptHostSettings settings, ICakeConfiguration configuration, ICakeLog log)
        {
            _loader = loader;
            _settings = settings;
            _configuration = configuration;
            _log = log;
        }

        /// <summary>
        /// Creates a script session for the specified host.
        /// </summary>
        /// <param name="host">The script host.</param>
        /// <returns>A script session.</returns>
        public IScriptSession CreateSession(IScriptHost host)
        {
            return new RoslynScriptSession(host, _loader, _configuration, _log, _settings);
        }
    }
}