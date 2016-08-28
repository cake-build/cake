// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Frosting.Internal.Commands
{
    internal sealed class RunCommand : Command
    {
        private readonly IFrostingContext _context;
        private readonly IExecutionStrategy _strategy;
        private readonly ICakeReportPrinter _printer;

        public RunCommand(
            IFrostingContext context,
            IExecutionStrategy strategy,
            ICakeReportPrinter printer)
        {
            _context = context;
            _strategy = strategy;
            _printer = printer;
        }

        public override bool Execute(ICakeEngine engine, CakeHostOptions options)
        {
            var report = engine.RunTarget(_context, _strategy, options.Target);
            if (report != null && !report.IsEmpty)
            {
                _printer.Write(report);
            }

            return true;
        }
    }
}
