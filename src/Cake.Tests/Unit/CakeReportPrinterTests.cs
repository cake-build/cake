// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Testing;
using NSubstitute;
using Xunit;

namespace Cake.Tests.Unit
{
    public sealed class CakeReportPrinterTests
    {
        public sealed class TheWriteMethod
        {
            [Fact]
            public void Should_Throw_If_Report_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var console = Substitute.For<IConsole>();
                var printer = new CakeReportPrinter(console, context);

                // When
                var result = Record.Exception(() => printer.Write(null));

                // Then
                AssertEx.IsArgumentNullException(result, "report");
            }

            [Fact]
            public void Should_Default_To_30_Width()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var console = new FakeConsole();
                var report = new CakeReport();
                var taskName = "TaskName";
                var duration = TimeSpan.FromSeconds(10);

                report.Add(taskName, duration);
                var printer = new CakeReportPrinter(console, context);

                // When
                printer.Write(report);

                // Then
                string expected = $"{taskName, -30}{duration, -20}";
                Assert.Contains(console.Messages, s => s == expected);
            }

            [Fact]
            public void Should_Increase_Width_For_Long_Task_Names()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var console = new FakeConsole();
                var report = new CakeReport();
                var taskName = "TaskName";
                var taskName2 = "Task-Name-That-Has-A-Length-Of-44-Characters";
                var duration = TimeSpan.FromSeconds(10);

                report.Add(taskName, duration);
                report.Add(taskName2, duration);
                var printer = new CakeReportPrinter(console, context);

                // When
                printer.Write(report);

                // Then
                string expected = $"{taskName, -45}{duration, -20}";
                Assert.Contains(console.Messages, s => s == expected);
            }

            [Fact]
            public void Should_Print_No_Duration_For_Skipped_Tasks()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var console = new FakeConsole();
                var report = new CakeReport();
                var taskName = "TaskName";
                var taskNameThatWasSkipped = "TaskName-That-Was-Skipped";
                var duration = TimeSpan.FromSeconds(10);

                report.Add(taskName, duration);
                report.AddSkipped(taskNameThatWasSkipped);

                var printer = new CakeReportPrinter(console, context);

                // When
                printer.Write(report);

                // Then
                string expected = $"{taskNameThatWasSkipped, -30}{"Skipped", -20}";
                Assert.Contains(console.Messages, s => s == expected);
            }

            [Theory]
            [InlineData(Verbosity.Verbose)]
            [InlineData(Verbosity.Diagnostic)]
            public void Should_Print_Delegated_Tasks_When_Verbosity_Suffices(Verbosity verbosity)
            {
                // Given
                var log = Substitute.For<ICakeLog>();
                log.Verbosity.Returns(verbosity);
                var context = Substitute.For<ICakeContext>();
                context.Log.Returns(log);
                var console = new FakeConsole();
                var report = new CakeReport();
                var taskName = "TaskName";
                var tasknameThatWasDelegated = "TaskName-That-Was-Delegated";
                var duration = TimeSpan.FromSeconds(10);
                var durationDelegatedTask = TimeSpan.FromSeconds(5);

                report.Add(taskName, duration);
                report.AddDelegated(tasknameThatWasDelegated, durationDelegatedTask);

                var printer = new CakeReportPrinter(console, context);

                // When
                printer.Write(report);

                // Then
                var expected = $"{tasknameThatWasDelegated, -30}{durationDelegatedTask, -20}";
                Assert.Contains(console.Messages, s => s == expected);
            }

            [Theory]
            [InlineData(Verbosity.Quiet)]
            [InlineData(Verbosity.Minimal)]
            [InlineData(Verbosity.Normal)]
            public void Should_Not_Print_Delegated_Tasks_When_Verbosity_Does_Not_Suffice(Verbosity verbosity)
            {
                // Given
                var log = Substitute.For<ICakeLog>();
                log.Verbosity.Returns(verbosity);
                var context = Substitute.For<ICakeContext>();
                context.Log.Returns(log);
                var console = new FakeConsole();
                var report = new CakeReport();
                var taskName = "TaskName";
                var tasknameThatWasDelegated = "TaskName-That-Was-Delegated";
                var duration = TimeSpan.FromSeconds(10);
                var durationDelegatedTask = TimeSpan.FromSeconds(5);

                report.Add(taskName, duration);
                report.AddDelegated(tasknameThatWasDelegated, durationDelegatedTask);

                var printer = new CakeReportPrinter(console, context);

                // When
                printer.Write(report);

                // Then
                var expected = $"{tasknameThatWasDelegated, -30}{durationDelegatedTask, -20}";
                Assert.DoesNotContain(console.Messages, s => s == expected);
            }
        }
    }
}