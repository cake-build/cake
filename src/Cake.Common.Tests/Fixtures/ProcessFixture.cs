using Cake.Core;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures
{
    internal sealed class ProcessFixture
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
            ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns(p => Process);

            Context = Substitute.For<ICakeContext>();
            Context.ProcessRunner.Returns(ProcessRunner);
            Context.Environment.Returns(Environment);
        }

        public int Start(string filename)
        {
            return Context.StartProcess(filename);
        }

        public int Start(string filename, ProcessSettings settings)
        {
            return Context.StartProcess(filename, settings);
        }
    }
}
