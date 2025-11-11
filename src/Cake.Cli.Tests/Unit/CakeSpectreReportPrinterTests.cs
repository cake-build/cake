// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.Diagnostics;
using Spectre.Console.Testing;

namespace Cake.Cli.Tests.Unit;

public class CakeSpectreReportPrinterTests
{
    public sealed class TheConstructor
    {
        [Fact]
        public void Should_Throw_If_Console_Is_Null()
        {
            // Given
            // When
            var result = Record.Exception(() => new CakeSpectreReportPrinter(null));
            // Then
            AssertEx.IsArgumentNullException(result, "console");
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Write(bool includeReason)
    {
        // Given
        var console = new TestConsole();
        var reportPrinter = new CakeSpectreReportPrinter(console);

        var executionCategories = Enum
                                    .GetValues<CakeReportEntryCategory>()
                                    .OrderBy(
                                        category => category switch {
                                            CakeReportEntryCategory.Setup => 0,
                                            CakeReportEntryCategory.Task => 1,
                                            CakeReportEntryCategory.Teardown => 9,
                                            _ => 5
                                        })
                                    .ToArray();

        var executionStatuses = Enum
                                    .GetValues<CakeTaskExecutionStatus>()
                                    .OrderBy(
                                        status => status switch {
                                            CakeTaskExecutionStatus.Executed => 0,
                                            CakeTaskExecutionStatus.Skipped => 1,
                                            CakeTaskExecutionStatus.Failed => 2,
                                            CakeTaskExecutionStatus.Delegated => 9,
                                            _ => 5
                                        })
                                    .ToArray();

        var report = executionCategories
                            .Aggregate(
                                new CakeReport(),
                                (report, category) => executionStatuses.Aggregate(
                                    report,
                                    (report, status) =>
                                    {
                                        report.Add(
                                            $"{category:F}{status:F}",
                                            includeReason && status == CakeTaskExecutionStatus.Skipped ? $"{status:F} reason" : null,
                                            category,
                                            TimeSpan.FromMilliseconds(((int)category + 1) * ((int)status + 1) * 111),
                                            status);
                                        return report;
                                    },
                                    report => report),
                                report => report);

        // When
        reportPrinter.Write(report);

        // Then
        await Verify(console.Output);
    }

    [Theory]
    [InlineData(Verbosity.Quiet)]
    [InlineData(Verbosity.Minimal)]
    [InlineData(Verbosity.Normal)]
    [InlineData(Verbosity.Verbose)]
    [InlineData(Verbosity.Diagnostic)]
    public async Task WriteStep(Verbosity verbosity)
    {
        // Given
        var console = new TestConsole();
        var reportPrinter = new CakeSpectreReportPrinter(console);

        // When
        reportPrinter.WriteStep("Test Step", verbosity);

        // Then
        await Verify(console.Output);
    }

    [Theory]
    [InlineData(Verbosity.Quiet)]
    [InlineData(Verbosity.Minimal)]
    [InlineData(Verbosity.Normal)]
    [InlineData(Verbosity.Verbose)]
    [InlineData(Verbosity.Diagnostic)]
    public async Task WriteLifeCycleStep(Verbosity verbosity)
    {
        // Given
        var console = new TestConsole();
        var reportPrinter = new CakeSpectreReportPrinter(console);

        // When
        reportPrinter.WriteLifeCycleStep("Setup", verbosity);

        // Then
        await Verify(console.Output);
    }

    [Theory]
    [InlineData(Verbosity.Quiet)]
    [InlineData(Verbosity.Minimal)]
    [InlineData(Verbosity.Normal)]
    [InlineData(Verbosity.Verbose)]
    [InlineData(Verbosity.Diagnostic)]
    public async Task WriteSkippedStep(Verbosity verbosity)
    {
        // Given
        var console = new TestConsole();
        var reportPrinter = new CakeSpectreReportPrinter(console);

        // When
        reportPrinter.WriteSkippedStep("Skipped Step", verbosity);

        // Then
        await Verify(console.Output);
    }
}