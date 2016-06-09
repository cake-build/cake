// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Common.Tools.OpenCover;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Testing.Fixtures;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class OpenCoverFixture : ToolFixture<OpenCoverSettings>
    {
        public ICakeContext Context { get; set; }
        public Action<ICakeContext> Action { get; set; }
        public FilePath OutputPath { get; set; }

        public OpenCoverFixture()
            : base("OpenCover.Console.exe")
        {
            // Set the output file.
            OutputPath = new FilePath("./result.xml");

            // Setup the Cake Context.
            Context = Substitute.For<ICakeContext>();
            Context.FileSystem.Returns(FileSystem);
            Context.Arguments.Returns(Substitute.For<ICakeArguments>());
            Context.Environment.Returns(Environment);
            Context.Globber.Returns(Globber);
            Context.Log.Returns(Substitute.For<ICakeLog>());
            Context.Registry.Returns(Substitute.For<IRegistry>());
            Context.ProcessRunner.Returns((IProcessRunner)null);
            Context.Tools.Returns(Tools);

            // Set up the default action that intercepts.
            Action = context =>
            {
                context.ProcessRunner.Start(
                    new FilePath("/Working/tools/Test.exe"),
                    new ProcessSettings()
                    {
                        Arguments = "-argument"
                    });
            };
        }

        protected override void RunTool()
        {
            var tool = new OpenCoverRunner(FileSystem, Environment, ProcessRunner, Tools);
            tool.Run(Context, Action, OutputPath, Settings);
        }
    }
}
