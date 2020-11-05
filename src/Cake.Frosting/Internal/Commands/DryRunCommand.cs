// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.Diagnostics;

namespace Cake.Frosting.Internal.Commands
{
    internal sealed class DryRunCommand : Command
    {
        private readonly IFrostingContext _context;
        private readonly ICakeLog _log;
        private readonly ExecutionSettings _executionSettings;

        public DryRunCommand(IFrostingContext context, ICakeLog log)
        {
            _context = context;
            _log = log;
            _executionSettings = new ExecutionSettings();
        }

        public override async Task<bool> ExecuteAsync(ICakeEngine engine, CakeHostOptions options)
        {
            _executionSettings.SetTarget(options.Target);

            _log.Information("Performing dry run...");
            _log.Information("Target is: {0}", options.Target);
            _log.Information(string.Empty);

            var strategy = new DryRunExecutionStrategy(_log);
            await engine.RunTargetAsync(_context, strategy, _executionSettings).ConfigureAwait(false);

            _log.Information(string.Empty);
            _log.Information("This was a dry run.");
            _log.Information("No tasks were actually executed.");

            return true;
        }
    }
}
