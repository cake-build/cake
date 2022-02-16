// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.Scripting;

namespace Cake.Cli
{
    /// <summary>
    /// The script host used to execute Cake scripts.
    /// </summary>
    public sealed class BuildScriptHost : BuildScriptHost<ICakeContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildScriptHost"/> class.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="executionStrategy">The execution strategy.</param>
        /// <param name="context">The context.</param>
        /// <param name="reportPrinter">The report printer.</param>
        /// <param name="log">The log.</param>
        public BuildScriptHost(
            ICakeEngine engine,
            IExecutionStrategy executionStrategy,
            ICakeContext context,
            ICakeReportPrinter reportPrinter,
            ICakeLog log) : base(engine, executionStrategy, context, reportPrinter, log)
        {
        }
    }

    /// <summary>
    /// The script host used to execute Cake scripts.
    /// </summary>
    /// <typeparam name="TContext">The context type.</typeparam>
    public class BuildScriptHost<TContext> : ScriptHost
        where TContext : ICakeContext
    {
        private readonly ICakeReportPrinter _reportPrinter;
        private readonly ICakeLog _log;
        private readonly IExecutionStrategy _executionStrategy;
        private readonly TContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildScriptHost{TContext}"/> class.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="executionStrategy">The execution strategy.</param>
        /// <param name="context">The context.</param>
        /// <param name="reportPrinter">The report printer.</param>
        /// <param name="log">The log.</param>
        public BuildScriptHost(
            ICakeEngine engine,
            IExecutionStrategy executionStrategy,
            TContext context,
            ICakeReportPrinter reportPrinter,
            ICakeLog log) : base(engine, context)
        {
            _executionStrategy = executionStrategy;
            _context = context;
            _reportPrinter = reportPrinter;
            _log = log;
        }

        /// <inheritdoc/>
        public override async Task<CakeReport> RunTargetAsync(string target)
        {
            Settings.SetTarget(target);

            var report = await Engine.RunTargetAsync(_context, _executionStrategy, Settings).ConfigureAwait(false);
            if (report != null && !report.IsEmpty)
            {
                _reportPrinter.Write(report);
            }

            return report;
        }
    }
}