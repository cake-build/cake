using Cake.Core;
using Cake.Core.Scripting;

namespace Cake.Scripting
{
    public sealed class DefaultScriptHost : ScriptHost
    {
        public DefaultScriptHost(ICakeEngine engine)
            : base(engine)
        {
        }

        public override CakeReport RunTarget(string target)
        {
            var report = Engine.RunTarget(target);
            if (report != null && !report.IsEmpty)
            {
                CakeReportPrinter.Write(report);
            }
            return report;
        }
    }
}
