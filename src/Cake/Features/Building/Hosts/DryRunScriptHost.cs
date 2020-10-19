// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.Scripting;

namespace Cake.Features.Building.Hosts
{
    /// <summary>
    /// The script host used to dry run Cake scripts.
    /// </summary>
    public sealed class DryRunScriptHost : ScriptHost
    {
        private readonly ICakeLog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="DryRunScriptHost"/> class.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="context">The context.</param>
        /// <param name="log">The log.</param>
        public DryRunScriptHost(ICakeEngine engine, ICakeContext context, ICakeLog log)
            : base(engine, context)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        /// <inheritdoc/>
        public override async Task<CakeReport> RunTargetAsync(string target)
        {
            _log.Information("Performing dry run...");
            _log.Information("Target is: {0}", target);
            _log.Information(string.Empty);

            Settings.SetTarget(target);

            var strategy = new DryRunExecutionStrategy(_log);
            var result = await Engine.RunTargetAsync(Context, strategy, Settings).ConfigureAwait(false);

            _log.Information(string.Empty);
            _log.Information("This was a dry run.");
            _log.Information("No tasks were actually executed.");

            return result;
        }
    }
}