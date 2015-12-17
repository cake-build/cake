using System;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Shared;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey.Packer
{
    internal sealed class ChocolateyPackerFixtureResult : ToolFixtureResult
    {
        private readonly string _nuspecContent;

        public string NuspecContent
        {
            get { return _nuspecContent; }
        }

        public ChocolateyPackerFixtureResult(FakeFileSystem fileSystem, FilePath toolPath, ProcessSettings process)
            : base(toolPath, process)
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