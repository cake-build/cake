// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Cake.Core;

namespace Cake.Frosting.Internal.Commands
{
    internal sealed class RunCommand : Command
    {
        private readonly IFrostingContext _context;
        private readonly IExecutionStrategy _strategy;
        private readonly ICakeReportPrinter _printer;
        private readonly ExecutionSettings _executionSettings;

        public RunCommand(
            IFrostingContext context,
            IExecutionStrategy strategy,
            ICakeReportPrinter printer)
        {
            _context = context;
            _strategy = strategy;
            _printer = printer;
            _executionSettings = new ExecutionSettings();
        }

        public override async Task<bool> ExecuteAsync(ICakeEngine engine, CakeHostOptions options)
        {
            _executionSettings.SetTarget(options.Target);
            var report = await engine.RunTargetAsync(_context, _strategy, _executionSettings).ConfigureAwait(false);
            if (report != null && !report.IsEmpty)
            {
                _printer.Write(report);
            }

            return true;
        }
    }
}
