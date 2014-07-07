using System;
using Cake.Core;
using Cake.Scripting;
using NSubstitute;
using Xunit;

namespace Cake.Tests.Unit.Scripting
{
    public sealed class BuildScriptHostTests
    {
        public sealed class TheRunTargetMethod
        {
            [Fact]
            public void Should_Proxy_Call_To_Engine()
            {
                // Given
                var engine = Substitute.For<ICakeEngine>();
                var printer = Substitute.For<ICakeReportPrinter>();
                var host = new BuildScriptHost(engine, printer);

                // When
                host.RunTarget("Target");

                // Then
                engine.Received(1).RunTarget("Target");
            }

            [Fact]
            public void Should_Print_Report()
            {
                // Given
                var report = new CakeReport();
                report.Add("Target", TimeSpan.FromSeconds(1));
                var engine = Substitute.For<ICakeEngine>();
                engine.RunTarget("Target").Returns(report);
                var printer = Substitute.For<ICakeReportPrinter>();
                var host = new BuildScriptHost(engine, printer);

                // When
                host.RunTarget("Target");

                // Then
                printer.Received(1).Write(report);
            }

            [Fact]
            public void Should_Not_Print_Report_That_Is_Null()
            {
                // Given
                var engine = Substitute.For<ICakeEngine>();
                engine.RunTarget(Arg.Any<string>()).Returns((CakeReport)null);
                var printer = Substitute.For<ICakeReportPrinter>();
                var host = new BuildScriptHost(engine, printer);

                // When
                host.RunTarget("Target");

                // Then
                printer.Received(0).Write(Arg.Any<CakeReport>());
            }

            [Fact]
            public void Should_Not_Print_Empty_Report()
            {
                // Given
                var engine = Substitute.For<ICakeEngine>();
                engine.RunTarget(Arg.Any<string>()).Returns(new CakeReport());
                var printer = Substitute.For<ICakeReportPrinter>();
                var host = new BuildScriptHost(engine, printer);

                // When
                host.RunTarget("Target");

                // Then
                printer.Received(0).Write(Arg.Any<CakeReport>());
            }
        }
    }
}
