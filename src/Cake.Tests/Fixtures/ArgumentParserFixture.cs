using Cake.Core.IO;
using Cake.Testing.Fakes;
using NSubstitute;

namespace Cake.Tests.Fixtures
{
    internal sealed class ArgumentParserFixture
    {
        public FakeLog Log { get; set; }
        public IFileSystem FileSystem { get; set; }

        public ArgumentParserFixture()
        {
            Log = new FakeLog();
            FileSystem = Substitute.For<IFileSystem>();
        }
    }
}
