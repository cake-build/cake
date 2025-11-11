// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Cake.Core.Diagnostics;
using Cake.Testing;
using NSubstitute;
using Xunit;
using static VerifyXunit.Verifier;

namespace Cake.Core.Tests.Unit;

public class CakeReportPrinterTests
{
    public sealed class TheConstructor
    {
        [Fact]
        public void Should_Throw_If_Console_Is_Null()
        {
            // Given
            var context = Substitute.For<ICakeContext>();
            // When
            var result = Record.Exception(() => new CakeReportPrinter(null, context));
            // Then
            AssertEx.IsArgumentNullException(result, "console");
        }

        [Fact]
        public void Should_Throw_If_Context_Is_Null()
        {
            // Given
            var console = new FakeConsole();
            // When
            var result = Record.Exception(() => new CakeReportPrinter(console, null));
            // Then
            AssertEx.IsArgumentNullException(result, "context");
        }
    }

    [Theory]
    [InlineData(Verbosity.Quiet)]
    [InlineData(Verbosity.Minimal)]
    [InlineData(Verbosity.Normal)]
    [InlineData(Verbosity.Verbose)]
    [InlineData(Verbosity.Diagnostic)]
    public async Task Write(Verbosity verbosity)
    {
        // Given
        var log = Substitute.For<ICakeLog>();
        log.Verbosity.Returns(verbosity);

        var context = Substitute.For<ICakeContext>();
        context.Log.Returns(log);

        var console = new FakeConsole();

        var reportPrinter = new CakeReportPrinter(console, context);

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
                                            status == CakeTaskExecutionStatus.Skipped ? $"{status:F} reason" : null,
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
        await Verify(console.Messages);
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
        var log = Substitute.For<ICakeLog>();
        log.Verbosity.Returns(verbosity);

        var context = Substitute.For<ICakeContext>();
        context.Log.Returns(log);

        var console = new FakeConsole();
        var reportPrinter = new CakeReportPrinter(console, context);

        // When
        reportPrinter.WriteStep("Test Step", verbosity);

        // Then
        await Verify(console.Messages);
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
        var log = Substitute.For<ICakeLog>();
        log.Verbosity.Returns(verbosity);

        var context = Substitute.For<ICakeContext>();
        context.Log.Returns(log);

        var console = new FakeConsole();
        var reportPrinter = new CakeReportPrinter(console, context);

        // When
        reportPrinter.WriteLifeCycleStep("Setup", verbosity);

        // Then
        await Verify(console.Messages);
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
        var log = Substitute.For<ICakeLog>();
        log.Verbosity.Returns(verbosity);

        var context = Substitute.For<ICakeContext>();
        context.Log.Returns(log);

        var console = new FakeConsole();
        var reportPrinter = new CakeReportPrinter(console, context);

        // When
        reportPrinter.WriteSkippedStep("Skipped Step", verbosity);

        // Then
        await Verify(console.Messages);
    }
}
