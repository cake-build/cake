using Cake.Core.IO;
using Cake.Core.Tests.Fakes;
using NSubstitute;

namespace Cake.Core.Tests.Fixtures
{
    public sealed class CakeEngineFixture
    {
        public IFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public FakeLog Log { get; set; }
        public IGlobber Globber { get; set; }
        public ICakeArguments Arguments { get; set; }
        public IProcessRunner ProcessRunner { get; set; }

        public CakeEngineFixture()
        {
            FileSystem = Substitute.For<IFileSystem>();
            Environment = Substitute.For<ICakeEnvironment>();
            Log = new FakeLog();
            Globber = Substitute.For<IGlobber>();
            Arguments = Substitute.For<ICakeArguments>();
            ProcessRunner = Substitute.For<IProcessRunner>();
        }

        public CakeEngine CreateEngine()
        {
            return new CakeEngine(FileSystem, Environment, Log, Arguments, Globber, ProcessRunner);
        }
    }
}
