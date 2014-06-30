using System;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Scripting;

namespace Cake.Scripting
{
    public sealed class DescriptionScriptHost : ScriptHost
    {
        public DescriptionScriptHost(ICakeEngine engine) : base(engine)
        {
        }

        public new CakeReport RunTarget(string target)
        {
            
            /*var report = _engine.RunTarget(target);
            if (!report.IsEmpty)
            {
                CakeReportPrinter.Write(report);
            }            
            return report;*/
            return new CakeReport();
        }
    }
}