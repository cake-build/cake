using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.MSBuild;
using NSubstitute;

namespace Cake.Tests.Fixtures
{
    public sealed class MSBuildRunnerFixture
    {
        public IFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public ICakeContext Context { get; set; }
        public IProcess Process { get; set; }
        public IProcessRunner ProcessRunner { get; set; }

        public MSBuildRunnerFixture(bool is64BitOperativeSystem = false)
        {
            Process = Substitute.For<IProcess>();

            ProcessRunner = Substitute.For<IProcessRunner>();
            ProcessRunner.Start(Arg.Any<ProcessStartInfo>()).Returns(Process);

            Environment = Substitute.For<ICakeEnvironment>();
            Environment.Is64BitOperativeSystem().Returns(is64BitOperativeSystem);           
            Environment.GetSpecialPath(SpecialPath.ProgramFilesX86).Returns("/Program86");
            Environment.WorkingDirectory.Returns("/Working");

            Context = Substitute.For<ICakeContext>();
            Context.Environment.Returns(Environment);
        }

        public MSBuildRunner CreateRunner()
        {
            return new MSBuildRunner(ProcessRunner);
        }
    }
}
