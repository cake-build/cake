using System;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet.Packer
{
    internal sealed class NuGetPackerFixtureResult : ToolFixtureResult
    {
        private readonly string _nuspecContent;

        public string NuspecContent
        {
            get { return _nuspecContent; }
        }

        public NuGetPackerFixtureResult(FakeFileSystem fileSystem, FilePath path, ProcessSettings process)
            : base(path, process)
        {
            _nuspecContent = GetNuSpecContent(fileSystem, process);
        }

        private static string GetNuSpecContent(FakeFileSystem fileSystem, ProcessSettings process)
        {
            var args = process.Arguments.Render();
            var parts = args.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var last = parts.Last();
            var file = fileSystem.GetFile(last.UnQuote());
            return file.Exists ? file.GetTextContent() : null;
        }
    }
}