using System.Collections.Generic;
using System.Diagnostics;
using Cake.Common.Tools.ILMerge;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures
{
    public sealed class ILMergeRunnerFixture
    {
        public IProcess Process { get; set; }
        public IProcessRunner ProcessRunner { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public IGlobber Globber { get; set; }

        public FilePath OutputAssemblyPath { get; set; }
        public FilePath PrimaryAssemblyPath { get; set; }
        public List<FilePath> AssemblyPaths { get; set; }

        public ILMergeSettings Settings { get; set; }

        public ILMergeRunnerFixture()
        {
            OutputAssemblyPath = "output.exe";
            PrimaryAssemblyPath = "input.exe";
            AssemblyPaths = new List<FilePath>();

            Process = Substitute.For<IProcess>();
            Process.GetExitCode().Returns(0);

            ProcessRunner = Substitute.For<IProcessRunner>();
            ProcessRunner.Start(Arg.Any<ProcessStartInfo>()).Returns(Process);

            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory = "/Working";

            Globber = Substitute.For<IGlobber>();
            Globber.Match("./tools/**/ILMerge.exe").Returns(new[] { (FilePath)"/Working/tools/ILMerge.exe" });

            Settings = new ILMergeSettings();
        }

        public void Run()
        {
            var runner = new ILMergeRunner(Environment, Globber, ProcessRunner);
            runner.Merge(OutputAssemblyPath, PrimaryAssemblyPath, AssemblyPaths, Settings);
        }
    }
}
