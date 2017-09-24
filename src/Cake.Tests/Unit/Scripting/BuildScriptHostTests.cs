// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
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
            public async Task Should_Proxy_Call_To_Engine()
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
                await engine.Received(1).RunTargetAsync(context, Arg.Any<DefaultExecutionStrategy>(), "Target");
            }

            [Fact]
            public async Task Should_Print_Report()
            {
                // Given
                var report = new CakeReport();
                report.Add("Target", TimeSpan.FromSeconds(1));
                var engine = Substitute.For<ICakeEngine>();
                var context = Substitute.For<ICakeContext>();
                engine.RunTargetAsync(context, Arg.Any<DefaultExecutionStrategy>(), "Target").Returns(Task.FromResult(report));
                var printer = Substitute.For<ICakeReportPrinter>();
                var log = Substitute.For<ICakeLog>();
                var host = new BuildScriptHost(engine, context, printer, log);

                // When
                await host.RunTargetAsync("Target");

                // Then
                printer.Received(1).Write(report);
            }

            [Fact]
            public async Task Should_Not_Print_Report_That_Is_Null()
            {
                // Given
                var engine = Substitute.For<ICakeEngine>();
                var context = Substitute.For<ICakeContext>();
                engine.RunTargetAsync(context, Arg.Any<DefaultExecutionStrategy>(), Arg.Any<string>()).Returns(Task.FromResult((CakeReport)null));
                var printer = Substitute.For<ICakeReportPrinter>();
                var log = Substitute.For<ICakeLog>();
                var host = new BuildScriptHost(engine, context, printer, log);

                // When
                await host.RunTargetAsync("Target");

                // Then
                printer.Received(0).Write(Arg.Any<CakeReport>());
            }

            [Fact]
            public async Task Should_Not_Print_Empty_Report()
            {
                // Given
                var engine = Substitute.For<ICakeEngine>();
                var context = Substitute.For<ICakeContext>();
                engine.RunTargetAsync(context, Arg.Any<DefaultExecutionStrategy>(), Arg.Any<string>()).Returns(Task.FromResult(new CakeReport()));
                var printer = Substitute.For<ICakeReportPrinter>();
                var log = Substitute.For<ICakeLog>();
                var host = new BuildScriptHost(engine, context, printer, log);

                // When
                await host.RunTargetAsync("Target");

                // Then
                printer.Received(0).Write(Arg.Any<CakeReport>());
            }
        }
    }
}