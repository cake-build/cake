using System;
using Cake.Core;
using Cake.Testing.Fakes;
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
                var console = Substitute.For<IConsole>();
                var printer = new CakeReportPrinter(console);

                // When
                var result = Record.Exception(() => printer.Write(null));

                // Then
                Assert.IsArgumentNullException(result, "report");
            }

            [Fact]
            public void Should_Default_To_30_Width()
            {
                // Given
                var console = new FakeConsole();
                var report = new CakeReport();
                string taskName = "TaskName";
                TimeSpan duration = TimeSpan.FromSeconds(10);

                report.Add(taskName, duration);
                var printer = new CakeReportPrinter(console);

                // When
                printer.Write(report);

                // Then
                string expected = String.Format("{0,-30}{1,-20}", taskName, duration);
                Assert.Contains(console.Messages, s => s == expected);
            }

            [Fact]
            public void Should_Increase_Width_For_Long_Task_Names()
            {
                // Given
                var console = new FakeConsole();
                var report = new CakeReport();
                string taskName = "TaskName";
                string taskName2 = "Task-Name-That-Has-A-Length-Of-44-Characters";
                TimeSpan duration = TimeSpan.FromSeconds(10);

                report.Add(taskName, duration);
                report.Add(taskName2, duration);
                var printer = new CakeReportPrinter(console);

                // When
                printer.Write(report);

                // Then
                string expected = String.Format("{0,-45}{1,-20}", taskName, duration);
                Assert.Contains(console.Messages, s => s == expected);
            }
        }
    }
}
