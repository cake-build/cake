using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Common.Tools.DNU.Build;
using Cake.Common.Tools.DotCover.Analyse;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools.DotCover.Analyse
{
    internal sealed class DotCoverAnalyserFixture : DotCoverFixture<DotCoverAnalyseSettings>
    {
        public ICakeContext Context { get; set; }
        public Action<ICakeContext> Action { get; set; }
        public FilePath OutputPath { get; set; }

        public DotCoverAnalyserFixture()
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
            var tool = new DotCoverAnalyser(FileSystem, Environment, ProcessRunner, Globber);
            tool.Analyse(Context, Action, OutputPath, Settings);
        }
    }
}
