using Cake.Core.Diagnostics;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.Core.Tests.Fixtures
{
    public sealed class CakeEngineFixture
    {
        public IFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public ILogger Log { get; set; }
        public IGlobber Globber { get; set; }

        public CakeEngineFixture()
        {
            FileSystem = Substitute.For<IFileSystem>();
            Environment = Substitute.For<ICakeEnvironment>();
            Log = Substitute.For<ILogger>();
            Globber = Substitute.For<IGlobber>();
        }

        public CakeEngine CreateEngine()
        {
            return new CakeEngine(FileSystem, Environment, Log, Globber);
        }
    }
}
