// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.Scripting;

namespace Cake.Scripting
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
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }
            _log = log;
        }

        /// <summary>
        /// Runs the specified target.
        /// </summary>
        /// <param name="target">The target to run.</param>
        /// <returns>The resulting report.</returns>
        public override CakeReport RunTarget(string target)
        {
            _log.Information("Performing dry run...");
            _log.Information("Target is: {0}", target);
            _log.Information(string.Empty);

            var strategy = new DryRunExecutionStrategy(_log);
            var result = Engine.RunTarget(Context, strategy, target);

            _log.Information(string.Empty);
            _log.Information("This was a dry run.");
            _log.Information("No tasks were actually executed.");

            return result;
        }
    }
}
