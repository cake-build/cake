// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cake.Core;
using Cake.Core.Diagnostics;

namespace Cake.Frosting.Internal
{
    internal sealed class ReportPrinter : ICakeReportPrinter
    {
        private readonly IConsole _console;
        private readonly ICakeLog _log;

        public ReportPrinter(IConsole console, ICakeLog log)
        {
            _console = console;
            _log = log;
        }

        public void Write(CakeReport report)
        {
            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
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
                    if (ShouldWriteTask(item))
                    {
                        _console.ForegroundColor = GetItemForegroundColor(item);
                        _console.WriteLine(lineFormat, item.TaskName, FormatDuration(item));
                    }
                }

                // Write footer.
                _console.ForegroundColor = ConsoleColor.Green;
                _console.WriteLine(new string('-', 20 + maxTaskNameLength));
                _console.WriteLine(lineFormat, "Total:", FormatTime(GetTotalTime(report)));
            }
            finally
            {
                _console.ResetColor();
            }
        }

        private bool ShouldWriteTask(CakeReportEntry item)
        {
            if (item.ExecutionStatus == CakeTaskExecutionStatus.Delegated)
            {
                return _log.Verbosity >= Verbosity.Verbose;
            }
            return true;
        }

        private static string FormatDuration(CakeReportEntry item)
        {
            return item.ExecutionStatus == CakeTaskExecutionStatus.Skipped
                ? "Skipped" : FormatTime(item.Duration);
        }

        private static ConsoleColor GetItemForegroundColor(CakeReportEntry item)
        {
            return item.ExecutionStatus == CakeTaskExecutionStatus.Executed
                ? ConsoleColor.Green : ConsoleColor.Gray;
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