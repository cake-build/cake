using System;
using System.IO;
using System.Linq;
using Cake.Common.Tests.Properties;
using Cake.Common.Tools.NuGet.Pack;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing.Fakes;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet
{
    public sealed class NuGetPackerFixture : NuGetFixture
    {
        public FilePath NuSpecFilePath { get; set; }
        public NuGetPackSettings Settings { get; set; }

        public NuGetPackerFixture()
        {
            NuSpecFilePath = "./existing.nuspec";
            Settings = new NuGetPackSettings();

            FileSystem.CreateFile("/Working/existing.nuspec").SetContent(Resources.Nuspec_NoMetadataValues);
        }

        public void WithNuSpecXml(string xml)
        {
            FileSystem.CreateFile("/Working/existing.nuspec").SetContent(xml);
        }

        public void GivenTemporaryNuSpecAlreadyExist()
        {
            FileSystem.CreateFile("/Working/existing.temp.nuspec");
        }

        public string Pack()
        {
            string content = null;
            ProcessRunner.When(p => p.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()))
                .Do(info => {
                    content = GetContent(info[1] as ProcessSettings);
                });

            var tool = new NuGetPacker(FileSystem, Environment, ProcessRunner, Log, Globber, NuGetToolResolver);
            tool.Pack(NuSpecFilePath, Settings);

            // Return the content.
            return content;
        }

        private string GetContent(ProcessSettings settings)
        {
            var args = settings.Arguments.Render();
            var parts = args.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var last = parts.Last();

            var file = FileSystem.GetFile(last.UnQuote());
            if (file.Exists)
            {
                using (var stream = file.OpenRead())
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }

            return null;
        }
    }
}
