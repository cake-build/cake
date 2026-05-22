// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Cake.Cli.Infrastructure;
using Cake.Core;
using Cake.Core.Configuration;
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
        /// <param name="configuration">The Cake Configuration.</param>
        /// <param name="log">The log.</param>
        public BuildScriptHost(
            ICakeEngine engine,
            IExecutionStrategy executionStrategy,
            ICakeContext context,
            ICakeReportPrinter reportPrinter,
            ICakeConfiguration configuration,
            ICakeLog log) : base(engine, executionStrategy, context, reportPrinter, configuration, log)
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
        private readonly ICakeConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildScriptHost{TContext}"/> class.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="executionStrategy">The execution strategy.</param>
        /// <param name="context">The context.</param>
        /// <param name="reportPrinter">The report printer.</param>
        /// <param name="configuration">The Cake Configuration.</param>
        /// <param name="log">The log.</param>
        public BuildScriptHost(
            ICakeEngine engine,
            IExecutionStrategy executionStrategy,
            TContext context,
            ICakeReportPrinter reportPrinter,
            ICakeConfiguration configuration,
            ICakeLog log) : base(engine, context)
        {
            _executionStrategy = executionStrategy;
            _context = context;
            _reportPrinter = reportPrinter;
            _configuration = configuration;
            _log = log;
        }

        /// <inheritdoc/>
        public override async Task<CakeReport> RunTargetAsync(string target)
        {
            Settings.SetTarget(target);

            return await internalRunTargetAsync();
        }

        /// <inheritdoc/>
        public override async Task<CakeReport> RunTargetsAsync(IEnumerable<string> targets)
        {
            Settings.SetTargets(targets);

            return await internalRunTargetAsync();
        }

        private async Task<CakeReport> internalRunTargetAsync()
        {
            try
            {
                if (_configuration.GetBoolValue(Constants.Settings.UnifiedDependencyGraphForMultipleTargets))
                {
                    Settings.UseUnifiedDependencyGraphForMultipleTargets(true);
                }

                var report = await Engine.RunTargetAsync(_context, _executionStrategy, Settings).ConfigureAwait(false);

                if (report != null && !report.IsEmpty && !_configuration.GetBoolValue(Constants.Settings.NoReport))
                {
                    _reportPrinter.Write(report);
                }

                return report;
            }
            catch (CakeReportException cre)
            {
                if (cre.Report != null && !cre.Report.IsEmpty)
                {
                    _reportPrinter.Write(cre.Report);
                }

                throw;
            }
        }
    }
}