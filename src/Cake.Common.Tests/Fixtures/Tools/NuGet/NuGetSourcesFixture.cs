using Cake.Common.Tools.NuGet.Sources;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet
{
    public sealed class NuGetSourcesFixture : NuGetFixture
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public NuGetSourcesSettings Settings { get; set; }

        public NuGetSourcesFixture()
        {
            Name = "name";
            Source = "source";
            Settings = new NuGetSourcesSettings();
        }

        public void GivenExistingSource()
        {
            Process.GetStandardOutput()
                .Returns(new[] { 
                        "  1.  https://www.nuget.org/api/v2/ [Enabled]",
                        "      https://www.nuget.org/api/v2/",
                        string.Format("  2.  {0} [Enabled]", Name),
                        string.Format("      {0}", Source)
                    });
        }

        public void GivenSourceAlreadyHasBeenAdded()
        {
            Process.GetStandardOutput().Returns(new[] { Source });
        }

        public void AddSources()
        {
            var tool = new NuGetSources(FileSystem, Environment, ProcessRunner, Globber, NuGetToolResolver);
            tool.AddSource(Name, Source, Settings);
        }

        public void RemoveSource()
        {
            var tool = new NuGetSources(FileSystem, Environment, ProcessRunner, Globber, NuGetToolResolver);
            tool.RemoveSource(Name, Source, Settings);
        }

        public void HasSource()
        {
            var tool = new NuGetSources(FileSystem, Environment, ProcessRunner, Globber, NuGetToolResolver);
            tool.HasSource(Source, Settings);
        }
    }
}
