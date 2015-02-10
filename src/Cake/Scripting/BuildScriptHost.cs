using Cake.Core;
using Cake.Core.Scripting;

namespace Cake.Scripting
{
    /// <summary>
    /// The script host used to execute Cake scripts.
    /// </summary>
    public sealed class BuildScriptHost : ScriptHost
    {
        private readonly ICakeReportPrinter _reportPrinter;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildScriptHost"/> class.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="reportPrinter">The report printer.</param>
        public BuildScriptHost(ICakeEngine engine, ICakeReportPrinter reportPrinter) 
            : base(engine)
        {
            _reportPrinter = reportPrinter;
        }

        /// <summary>
        /// Runs the specified target.
        /// </summary>
        /// <param name="target">The target to run.</param>
        /// <returns>The resulting report.</returns>
        public override CakeReport RunTarget(string target)
        {
            var report = Engine.RunTarget(target);
            if (report != null && !report.IsEmpty)
            {
                _reportPrinter.Write(report);
            }
            return report;
        }
    }
}
