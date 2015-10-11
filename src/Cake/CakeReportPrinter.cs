using System;
using System.Collections.Generic;
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
                var maxTaskNameLength = 29;
                foreach (var item in report)
                {
                    if (item.TaskName.Length > maxTaskNameLength)
                    {
                        maxTaskNameLength = item.TaskName.Length;
                    }
                }

                maxTaskNameLength++;
                string lineFormat = "{0,-" + maxTaskNameLength + "}{1,-20}";
                _console.ForegroundColor = ConsoleColor.Green;

                // Write header.
                _console.WriteLine();
                _console.WriteLine(lineFormat, "Task", "Duration");
                _console.WriteLine(new string('-', 20 + maxTaskNameLength));

                // Write task status.
                foreach (var item in report)
                {
                    _console.WriteLine(lineFormat, item.TaskName, FormatTime(item.Duration));
                }

                // Write footer.
                _console.WriteLine(new string('-', 20 + maxTaskNameLength));
                _console.WriteLine(lineFormat, "Total:", FormatTime(GetTotalTime(report)));
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

        private static TimeSpan GetTotalTime(IEnumerable<CakeReportEntry> entries)
        {
            return entries.Select(i => i.Duration)
                .Aggregate(TimeSpan.Zero, (t1, t2) => t1 + t2);
        }
    }
}