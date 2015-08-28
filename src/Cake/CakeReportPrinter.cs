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
                var maxTaskNameLength = 1;
                foreach (var item in report)
                {
                    if (item.TaskName.Length > maxTaskNameLength)
                    {
                        maxTaskNameLength = item.TaskName.Length;
                    }
                }

                var totalTime = GetTotalTime(report);
                string timeFormat = GetTimeFormat(totalTime);
                int timeLength = Math.Max("Duration".Length, timeFormat.Replace("\\", string.Empty).Length) + 1;

                string lineFormat = "{0," + maxTaskNameLength + "}{1," + timeLength + "}";
                _console.ForegroundColor = ConsoleColor.Green;

                // Write header.
                _console.WriteLine();
                _console.WriteLine(lineFormat, "Task", "Duration");
                _console.WriteLine(new string('-', timeLength + maxTaskNameLength));

                // Write task status.
                foreach (var item in report)
                {
                    _console.WriteLine(lineFormat, item.TaskName, FormatTime(item.Duration));
                }

                // Write footer.
                _console.WriteLine(new string('-', timeLength + maxTaskNameLength));
                _console.WriteLine(lineFormat, "Total:", FormatTime(totalTime));
            }
            finally
            {
                _console.ResetColor();
            }
        }

        private static string FormatTime(TimeSpan time)
        {
            return time.ToString(GetTimeFormat(time));
        }

        private static string GetTimeFormat(TimeSpan time)
        {
          string format = "s\\.ff";
          if (time.TotalSeconds > 9)
          {
            format = "s" + format;
            if (time.TotalMinutes > 1)
            {
              format = "m\\:" + format;
              if (time.TotalMinutes > 9)
              {
                format = "m" + format;
                if (time.TotalHours > 1)
                {
                  format = "h\\:" + format;
                  if (time.TotalHours > 9)
                  {
                    format = "h" + format;
                    if (time.TotalDays > 1)
                    {
                      format = "d\\." + format;
                    }
                  }
                }
              }
            }
          }

          return format;
        }

        private static TimeSpan GetTotalTime(IEnumerable<CakeReportEntry> entries)
        {
            return entries.Select(i => i.Duration)
                .Aggregate(TimeSpan.Zero, (t1, t2) => t1 + t2);
        }
    }
}
