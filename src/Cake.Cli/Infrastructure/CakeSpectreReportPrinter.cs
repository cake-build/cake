// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cake.Core;
using Cake.Core.Diagnostics;
using Spectre.Console;

namespace Cake.Cli
{
    /// <summary>
    /// The default report printer.
    /// </summary>
    public sealed class CakeSpectreReportPrinter : ICakeReportPrinter
    {
        private readonly IAnsiConsole _console;

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeSpectreReportPrinter"/> class.
        /// </summary>
        /// <param name="console">The console.</param>
        public CakeSpectreReportPrinter(IAnsiConsole console)
        {
            _console = console;
        }

        /// <inheritdoc/>
        public void Write(CakeReport report, Verbosity verbosity)
        {
            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }

            if (verbosity <= Verbosity.Quiet)
            {
                return;
            }

            // Create a table
            var table = new Table().Border(TableBorder.SimpleHeavy);
            table.Width(100);
            table.BorderStyle(new Style().Foreground(ConsoleColor.Green));

            var includeSkippedReasonColumn = report.Any(r => !string.IsNullOrEmpty(r.SkippedMessage));
            var rowStyle = new Style(ConsoleColor.Green);

            // Add some columns
            table.AddColumn(new TableColumn(new Text("Task", rowStyle)).Footer(new Text("Total:", rowStyle)).PadRight(10));
            table.AddColumn(
                new TableColumn(
                    new Text("Duration", rowStyle)).Footer(
                        new Text(FormatTime(GetTotalTime(report)), rowStyle)));

            if (includeSkippedReasonColumn)
            {
                table.AddColumn(new TableColumn(new Text("Skip Reason", rowStyle)));
            }

            foreach (var item in report)
            {
                var itemStyle = GetItemStyle(item);

                if (includeSkippedReasonColumn)
                {
                    table.AddRow(new Markup(item.TaskName, itemStyle),
                                new Markup(FormatDuration(item), itemStyle),
                                new Markup(item.SkippedMessage, itemStyle));
                }
                else
                {
                    table.AddRow(new Markup(item.TaskName, itemStyle),
                                new Markup(FormatDuration(item), itemStyle));
                }
            }

            // Render the table to the console
            _console.Write(table);
        }

        /// <inheritdoc/>
        public void WriteStep(string name, Verbosity verbosity)
        {
            if (verbosity < Verbosity.Normal)
            {
                return;
            }

            var table = new Table().Border(DoubleBorder.Shared);
            table.Width(100);
            table.AddColumn(name);
            _console.Write(new Padder(table).Padding(0, 1, 0, 0));
        }

        /// <inheritdoc/>
        public void WriteLifeCycleStep(string name, Verbosity verbosity)
        {
            if (verbosity < Verbosity.Normal)
            {
                return;
            }

            _console.WriteLine();

            var table = new Table().Border(SingleBorder.Shared);
            table.Width(100);
            table.AddColumn(name);
            _console.Write(table);
        }

        /// <inheritdoc/>
        public void WriteSkippedStep(string name, Verbosity verbosity)
        {
            if (verbosity < Verbosity.Verbose)
            {
                return;
            }

            _console.WriteLine();

            var table = new Table().Border(DoubleBorder.Shared);
            table.Width(100);
            table.AddColumn(name);
            _console.Write(table);
        }

        private static string FormatDuration(CakeReportEntry item)
        {
            if (item.ExecutionStatus == CakeTaskExecutionStatus.Skipped)
            {
                return "Skipped";
            }

            return FormatTime(item.Duration);
        }

        private static Style GetItemStyle(CakeReportEntry item)
        {
            if (item.Category == CakeReportEntryCategory.Setup || item.Category == CakeReportEntryCategory.Teardown)
            {
                return new Style(ConsoleColor.Cyan);
            }

            if (item.ExecutionStatus == CakeTaskExecutionStatus.Failed)
            {
                return new Style(ConsoleColor.Red);
            }

            if (item.ExecutionStatus == CakeTaskExecutionStatus.Executed)
            {
                return new Style(ConsoleColor.Green);
            }

            return new Style(ConsoleColor.Gray);
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