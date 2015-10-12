using System;
using System.IO;
using System.Linq;
using Cake.Common.Tests.Properties;
using Cake.Common.Tools.Chocolatey.Pack;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey
{
    public sealed class ChocolateyPackerFixture : ChocolateyFixture
    {
        public FilePath NuSpecFilePath { get; set; }
        public ChocolateyPackSettings Settings { get; set; }

        public ChocolateyPackerFixture()
        {
            NuSpecFilePath = "./existing.nuspec";
            Settings = new ChocolateyPackSettings();

            FileSystem.CreateFile("/Working/existing.nuspec").SetContent(Resources.ChocolateyNuspec_NoMetadataValues);
        }

        public void WithNuSpecXml(string xml)
        {
            FileSystem.CreateFile("/Working/existing.nuspec").SetContent(xml);
        }

        public void GivenTemporaryNuSpecAlreadyExist()
        {
            FileSystem.CreateFile("/Working/existing.temp.nuspec");
        }

        public string Pack(bool nuspec = true)
        {
            string content = null;
            ProcessRunner.When(p => p.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()))
                .Do(info => {
                    content = GetContent(info[1] as ProcessSettings);
                });

            var tool = new ChocolateyPacker(FileSystem, Environment, ProcessRunner, Log, Globber, ChocolateyToolResolver);
            if (nuspec)
            {
                tool.Pack(NuSpecFilePath, Settings);
            }
            else
            {
                tool.Pack(Settings);
            }

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