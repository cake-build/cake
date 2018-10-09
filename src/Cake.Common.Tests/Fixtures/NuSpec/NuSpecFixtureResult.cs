using Cake.Core.IO;
using Cake.Testing;

namespace Cake.Common.Tests.Fixtures.NuSpec
{
    internal sealed class NuSpecFixtureResult
    {
        public string NuspecContent { get; }

        public NuSpecFixtureResult(FakeFileSystem fileSystem, FilePath path)
        {
            NuspecContent = GetNuSpecContent(fileSystem, path);
        }

        private static string GetNuSpecContent(FakeFileSystem fileSystem, FilePath path)
        {
            var file = fileSystem.GetFile(path);
            return file.Exists ? file.GetTextContent() : null;
        }
    }
}
