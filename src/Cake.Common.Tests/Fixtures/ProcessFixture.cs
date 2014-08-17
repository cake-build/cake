using System.Diagnostics;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures
{
    public sealed class ProcessFixture
    {
        public ICakeContext Context { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public IProcessRunner ProcessRunner { get; set; }
        public IProcess Process { get; set; }

        public ProcessFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("/Working");

            Process = Substitute.For<IProcess>();

            ProcessRunner = Substitute.For<IProcessRunner>();
            ProcessRunner.Start(Arg.Any<ProcessStartInfo>()).Returns(p => Process);

            Context = Substitute.For<ICakeContext>();
            Context.ProcessRunner.Returns(ProcessRunner);
            Context.Environment.Returns(Environment);
        }

        public int Start(string filename)
        {
            return ProcessExtensions.StartProcess(Context, filename);
        }

        public int Start(string filename, ProcessSettings settings)
        {
            return ProcessExtensions.StartProcess(Context, filename, settings);
        }
    }
}
