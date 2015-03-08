using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Testing.Fakes;
using NSubstitute;

namespace Cake.Core.Tests.Fixtures
{
    internal sealed class CakeEngineFixture
    {
        public IFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public FakeLog Log { get; set; }
        public IGlobber Globber { get; set; }
        public ICakeArguments Arguments { get; set; }
        public IProcessRunner ProcessRunner { get; set; }
        public IEnumerable<IToolResolver> ToolResolvers { get; set; }
        public ICakeContext Context { get; set; }

        public CakeEngineFixture()
        {
            FileSystem = Substitute.For<IFileSystem>();
            Environment = Substitute.For<ICakeEnvironment>();
            Log = new FakeLog();
            Globber = Substitute.For<IGlobber>();
            Arguments = Substitute.For<ICakeArguments>();
            ProcessRunner = Substitute.For<IProcessRunner>();
            ToolResolvers = Substitute.For<IEnumerable<IToolResolver>>();

            Context = Substitute.For<ICakeContext>();
            Context.Arguments.Returns(Arguments);
            Context.Environment.Returns(Environment);
            Context.FileSystem.Returns(FileSystem);
            Context.Globber.Returns(Globber);
            Context.Log.Returns(Log);
            Context.ProcessRunner.Returns(ProcessRunner);
        }

        public CakeEngine CreateEngine()
        {
            return new CakeEngine(Log);
        }
    }
}
