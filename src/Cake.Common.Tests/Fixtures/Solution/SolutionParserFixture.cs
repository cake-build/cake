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
            Environment = FakeEnvironment.CreateWindowsEnvironment();
            FileSystem = new FakeFileSystem(Environment);
        }

        public FilePath WithSolutionFile(string slnContent)
        {
            var file = FileSystem.CreateFile("/Working/dummySolution.sln").SetContent(slnContent);
            return file.Path;
        }
    }
}