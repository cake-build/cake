using Cake.Core.Diagnostics;
using Mono.CSharp;

namespace Cake.Scripting.Mono
{
    internal sealed class MonoConsoleReportPrinter : ReportPrinter
    {
        private readonly ICakeLog _log;

        public MonoConsoleReportPrinter(ICakeLog log)
        {
            _log = log;
        }

        public override void Print(AbstractMessage msg, bool showFullPath)
        {
            if (msg.IsWarning)
            {
                _log.Warning("Warning: {0}", msg.Text);
            }
            _log.Verbose("{0}: {1}", msg.MessageType, msg.Text);
        }
    }
}
