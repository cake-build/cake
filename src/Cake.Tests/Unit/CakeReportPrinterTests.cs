using System;
using System.Globalization;
using System.Threading;
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
            public void Should_Default_To_Width_Of_Longest_Task()
            {
                // Given
                var console = new FakeConsole();
                var report = new CakeReport();
                string taskName = "TaskName";
                string taskName2 = "Task2";
                TimeSpan duration = TimeSpan.FromSeconds(10);

                report.Add(taskName, duration);
                report.Add(taskName2, duration);
                var printer = new CakeReportPrinter(console);


                // When
                printer.Write(report);

                // Then
                string expected = "TaskName    10.00";
                Assert.Equal(expected, console.Messages[2]);
                expected =        "   Task2    10.00";
                Assert.Equal(expected, console.Messages[3]);
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
                string expected = "                                    TaskName    10.00";
                Assert.Equal(expected, console.Messages[2]);
                expected =        "Task-Name-That-Has-A-Length-Of-44-Characters    10.00";
                Assert.Equal(expected, console.Messages[3]);
            }

            [Theory]
            [InlineData("00:00:10", "   10.00")]
            [InlineData("00:00:10.56789", "   10.56")]
            [InlineData("00:01:10", " 1:10.00")]
            [InlineData("00:11:10", "11:10.00")]
            [InlineData("01:11:10", "1:11:10.00")]
            [InlineData("11:11:10", "11:11:10.00")]
            [InlineData("1.11:11:10", "1.11:11:10.00")]
            public void Should_Format_Time_Based_On_Significance(string durationText, string expectedFormat)
            {
              // Given
              var console = new FakeConsole();
              var report = new CakeReport();
              string taskName = "TaskName";
              TimeSpan duration = TimeSpan.Parse(durationText);

              report.Add(taskName, duration);
              var printer = new CakeReportPrinter(console);

              // When
              printer.Write(report);

              // Then
              string expected = "TaskName " + expectedFormat;
              Assert.Equal(expected, console.Messages[2]);
            }

            [Fact]
            public void Should_Format_Line_Time_Based_On_Significance()
            {
              // Given
              var console = new FakeConsole();
              var report = new CakeReport();
              string taskName = "TaskName";
              string taskName2 = "TaskName2";

              report.Add(taskName, new TimeSpan(0, 0, 0, 5, 232));
              report.Add(taskName2, new TimeSpan(1, 1, 1, 5, 0));

              var printer = new CakeReportPrinter(console);

              // When
              printer.Write(report);

              // Then
              string expected = " TaskName          5.23";
              Assert.Equal(expected, console.Messages[2]);
              expected =        "TaskName2 1.01:01:05.00";
              Assert.Equal(expected, console.Messages[3]);
            }

            [Fact]
            public void Should_Format_Decimal_For_Culture()
            {
              // Given
              using (CultureHelper.SetCulture("de-DE"))
              {
                var console = new FakeConsole();
                var report = new CakeReport();
                string taskName = "TaskName";
                string taskName2 = "Task2";
                TimeSpan duration = TimeSpan.FromSeconds(10);

                report.Add(taskName, duration);
                report.Add(taskName2, duration);
                var printer = new CakeReportPrinter(console);


                // When
                printer.Write(report);

                // Then
                Assert.Equal(",", Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                string expected = "TaskName    10,00";
                Assert.Equal(expected, console.Messages[2]);
                expected = "   Task2    10,00";
                Assert.Equal(expected, console.Messages[3]);
              }
            }

       
          [Fact]
            public void Should_Format_Time_Separator_For_Culture()
            {
              // Given
              using (CultureHelper.SetCulture("ml-IN"))
              {
                var console = new FakeConsole();
                var report = new CakeReport();
                string taskName = "TaskName";
                string taskName2 = "Task2";
                TimeSpan duration = TimeSpan.FromSeconds(70);

                report.Add(taskName, duration);
                report.Add(taskName2, duration);
                var printer = new CakeReportPrinter(console);


                // When
                printer.Write(report);

                // Then
                Assert.Equal(".", Thread.CurrentThread.CurrentCulture.DateTimeFormat.TimeSeparator);
                string expected = "TaskName  1.10.00";
                Assert.Equal(expected, console.Messages[2]);
                expected = "   Task2  1.10.00";
                Assert.Equal(expected, console.Messages[3]);
              }
            }

          private class CultureHelper : IDisposable
          {
            private readonly CultureInfo _currentCulture;

            private CultureHelper(string culture)
            {
              _currentCulture = Thread.CurrentThread.CurrentCulture;
              Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(culture);
              Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
            }

            public static CultureHelper SetCulture(string culture)
            {
              var helper = new CultureHelper(culture);
              return helper;
            }

            public void Dispose()
            {
              Thread.CurrentThread.CurrentUICulture = _currentCulture;
              Thread.CurrentThread.CurrentCulture = _currentCulture;
            }
          }

        }
    }
}
