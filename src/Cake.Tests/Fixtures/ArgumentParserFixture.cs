using Cake.Arguments;
using Cake.Core.IO;
using Cake.Testing;
using NSubstitute;

namespace Cake.Tests.Fixtures
{
    internal sealed class ArgumentParserFixture
    {
        public FakeLog Log { get; set; }
        public VerbosityParser VerbosityParser { get; set; }
        public IFileSystem FileSystem { get; set; }

        public ArgumentParserFixture()
        {
            Log = new FakeLog();
            FileSystem = Substitute.For<IFileSystem>();
            VerbosityParser = new VerbosityParser(Log);
        }
    }
}