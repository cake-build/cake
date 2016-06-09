// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.Scripting;

namespace Cake.Scripting
{
    /// <summary>
    /// The script host used to execute Cake scripts.
    /// </summary>
    public sealed class BuildScriptHost : ScriptHost
    {
        private readonly ICakeReportPrinter _reportPrinter;
        private readonly ICakeLog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildScriptHost"/> class.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="context">The context.</param>
        /// <param name="reportPrinter">The report printer.</param>
        /// <param name="log">The log.</param>
        public BuildScriptHost(
            ICakeEngine engine,
            ICakeContext context,
            ICakeReportPrinter reportPrinter,
            ICakeLog log) : base(engine, context)
        {
            _reportPrinter = reportPrinter;
            _log = log;
        }

        /// <summary>
        /// Runs the specified target.
        /// </summary>
        /// <param name="target">The target to run.</param>
        /// <returns>The resulting report.</returns>
        public override CakeReport RunTarget(string target)
        {
            var strategy = new DefaultExecutionStrategy(_log);
            var report = Engine.RunTarget(Context, strategy, target);
            if (report != null && !report.IsEmpty)
            {
                _reportPrinter.Write(report);
            }
            return report;
        }
    }
}
