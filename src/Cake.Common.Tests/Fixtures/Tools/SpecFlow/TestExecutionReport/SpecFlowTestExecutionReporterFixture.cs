// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Common.Tools.SpecFlow.TestExecutionReport;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools.SpecFlow.TestExecutionReport
{
    internal sealed class SpecFlowTestExecutionReporterFixture : SpecFlowFixture<SpecFlowTestExecutionReportSettings>
    {
        public ICakeContext Context { get; set; }
        public Action<ICakeContext> Action { get; set; }
        public FilePath ProjectFile { get; set; }

        public SpecFlowTestExecutionReporterFixture()
        {
            // Set the project file.
            ProjectFile = new FilePath("./Tests.csproj");

            // Setup the Cake Context.
            Context = Substitute.For<ICakeContext>();
            Context.FileSystem.Returns(FileSystem);
            Context.Arguments.Returns(Substitute.For<ICakeArguments>());
            Context.Environment.Returns(Environment);
            Context.Globber.Returns(Globber);
            Context.Log.Returns(Substitute.For<ICakeLog>());
            Context.Registry.Returns(Substitute.For<IRegistry>());
            Context.ProcessRunner.Returns(Substitute.For<IProcessRunner>());
            Context.Tools.Returns(Tools);

            // Set up the default action that intercepts.
            Action = context =>
            {
                context.ProcessRunner.Start(
                    new FilePath("/Working/tools/MSTest.exe"),
                    new ProcessSettings()
                    {
                        Arguments = "/resultsfile:\"/Working/TestResult.trx\""
                    });
            };
        }

        protected override void RunTool()
        {
            var tool = new SpecFlowTestExecutionReporter(FileSystem, Environment, ProcessRunner, Tools);
            tool.Run(Context, Action, ProjectFile, Settings);
        }
    }
}
