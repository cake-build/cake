using System;
using System.Globalization;
using System.Linq;
using Cake.Core;

namespace Cake
{
    internal sealed class CakeReportPrinter : ICakeReportPrinter
    {
        private readonly IConsole _console;

        public CakeReportPrinter(IConsole console)
        {
            _console = console;
        }

        public void Write(CakeReport report)
        {
            if (report == null)
            {
                throw new ArgumentNullException("report");
            }

            try
            {
                _console.ForegroundColor = ConsoleColor.Green;

                // Write header.
                _console.WriteLine();
                _console.WriteLine("{0,-30}{1,-20}", "Task", "Duration");
                _console.WriteLine(new string('-', 50));

                // Write task status.
                foreach (var item in report)
                {
                    _console.WriteLine("{0,-30}{1,-20}", item.TaskName, FormatTime(item.Duration));
                }

                // Write footer.
                _console.WriteLine(new string('-', 50));
                _console.WriteLine("{0,-30}{1,-20}", "Total:", FormatTime(GetTotalTime(report)));
            }
            finally
            {
                _console.ResetColor();
            }
        }

        private static string FormatTime(TimeSpan time)
        {
            return time.ToString("c", CultureInfo.InvariantCulture);
        }

        private static TimeSpan GetTotalTime(CakeReport report)
        {
            return report.Select(i => i.Duration)
                .Aggregate(TimeSpan.Zero, (t1, t2) => t1 + t2);
        }
    }
}
