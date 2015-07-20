using System;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
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
            if (msg == null)
            {
                return;
            }

            var path = GetSourcePath(msg);
            var row = msg.Location.Row;
            var column = msg.Location.Column;
            var message = string.Format("{0} ({1},{2}): {3}", path.FullPath, row, column, msg.Text);

            var isError = msg.MessageType != null && msg.MessageType.Equals("error", StringComparison.OrdinalIgnoreCase);

            if (msg.IsWarning)
            {
                _log.Warning(message);
            }
            else if (isError)
            {
                _log.Error(message);
            }
            else
            {
                _log.Verbose(message);
            }
        }

        private static FilePath GetSourcePath(AbstractMessage msg)
        {
            // Get the source name.
            var sourceName = "unknown.cake";
            if (msg.Location.SourceFile != null)
            {
                // Get the source name.
                sourceName = msg.Location.SourceFile.Name ?? "unknown";
            }

            var path = new FilePath(sourceName).GetFilename();
            return path;
        }
    }
}
