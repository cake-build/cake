// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core;
using Cake.Core.Diagnostics;
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
                var context = Substitute.For<ICakeContext>();
                var printer = Substitute.For<ICakeReportPrinter>();
                var log = Substitute.For<ICakeLog>();
                var host = new BuildScriptHost(engine, context, printer, log);

                // When
                host.RunTarget("Target");

                // Then
                engine.Received(1).RunTarget(context, Arg.Any<DefaultExecutionStrategy>(), "Target");
            }

            [Fact]
            public void Should_Print_Report()
            {
                // Given
                var report = new CakeReport();
                report.Add("Target", TimeSpan.FromSeconds(1));
                var engine = Substitute.For<ICakeEngine>();
                var context = Substitute.For<ICakeContext>();
                engine.RunTarget(context, Arg.Any<DefaultExecutionStrategy>(), "Target").Returns(report);
                var printer = Substitute.For<ICakeReportPrinter>();
                var log = Substitute.For<ICakeLog>();
                var host = new BuildScriptHost(engine, context, printer, log);

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
                var context = Substitute.For<ICakeContext>();
                engine.RunTarget(context, Arg.Any<DefaultExecutionStrategy>(), Arg.Any<string>()).Returns((CakeReport)null);
                var printer = Substitute.For<ICakeReportPrinter>();
                var log = Substitute.For<ICakeLog>();
                var host = new BuildScriptHost(engine, context, printer, log);

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
                var context = Substitute.For<ICakeContext>();
                engine.RunTarget(context, Arg.Any<DefaultExecutionStrategy>(), Arg.Any<string>()).Returns(new CakeReport());
                var printer = Substitute.For<ICakeReportPrinter>();
                var log = Substitute.For<ICakeLog>();
                var host = new BuildScriptHost(engine, context, printer, log);

                // When
                host.RunTarget("Target");

                // Then
                printer.Received(0).Write(Arg.Any<CakeReport>());
            }
        }
    }
}
