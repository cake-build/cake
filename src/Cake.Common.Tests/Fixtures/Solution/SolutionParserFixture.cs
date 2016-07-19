using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Solution
{
    internal sealed class SolutionParserFixture
    {
        public FakeEnvironment Environment { get; set; }

        public FakeFileSystem FileSystem { get; set; }

        public SolutionParserFixture()
        {
            var environment = FakeEnvironment.CreateUnixEnvironment();
            Environment = environment;
            var fileSystem = new FakeFileSystem(environment);
            FileSystem = fileSystem;
        }

        public FilePath WithSolutionFile(string slnContent)
        {
            var file = FileSystem.CreateFile("/Working/dummySolution.sln").SetContent(slnContent);
            return file.Path;
        }
    }
}