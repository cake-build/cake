using System;
using Cake.Core.IO;
using Cake.Testing;

namespace Cake.Common.Tests.Fixtures.Solution
{
    internal sealed class SlnxParserFixture
    {
        public FakeEnvironment Environment { get; set; }

        public FakeFileSystem FileSystem { get; set; }

        public SlnxParserFixture()
        {
            var environment = FakeEnvironment.CreateUnixEnvironment();
            Environment = environment;
            var fileSystem = new FakeFileSystem(environment);
            FileSystem = fileSystem;
        }

        public static FilePath WithSolutionFile(string slnxContent)
        {
            var tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"dummySolution_{Guid.NewGuid()}.slnx");
            System.IO.File.WriteAllText(tempPath, slnxContent);
            return new FilePath(tempPath);
        }
    }
}