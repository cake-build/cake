using Cake.Core;
using Cake.Core.Scripting;

namespace Cake.Scripting
{
    public sealed class BuildScriptHost : ScriptHost
    {
        private readonly ICakeReportPrinter _reportPrinter;

        public BuildScriptHost(ICakeEngine engine, ICakeReportPrinter reportPrinter) 
            : base(engine)
        {
            _reportPrinter = reportPrinter;
        }

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
