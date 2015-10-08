using Cake.Common.Tools.Chocolatey.Sources;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey
{
    public sealed class ChocolateySourcesFixture : ChocolateyFixture
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public ChocolateySourcesSettings Settings { get; set; }

        public ChocolateySourcesFixture()
        {
            Name = "name";
            Source = "source";
            Settings = new ChocolateySourcesSettings();
        }

        public void AddSource()
        {
            var tool = new ChocolateySources(FileSystem, Environment, ProcessRunner, Globber, ChocolateyToolResolver);
            tool.AddSource(Name, Source, Settings);
        }

        public void RemoveSource()
        {
            var tool = new ChocolateySources(FileSystem, Environment, ProcessRunner, Globber, ChocolateyToolResolver);
            tool.RemoveSource(Name, Settings);
        }

        public void EnableSource()
        {
            var tool = new ChocolateySources(FileSystem, Environment, ProcessRunner, Globber, ChocolateyToolResolver);
            tool.EnableSource(Name, Settings);
        }

        public void DisableSource()
        {
            var tool = new ChocolateySources(FileSystem, Environment, ProcessRunner, Globber, ChocolateyToolResolver);
            tool.DisableSource(Name, Settings);
        }
    }
}