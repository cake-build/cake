using Cake.Common.Tools.NuGet.Sources;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet.Sources
{
    internal sealed class NuGetRemoveSourceFixture : NuGetSourcesFixture
    {
        protected override void RunTool()
        {
            var tool = new NuGetSources(FileSystem, Environment, ProcessRunner, Tools, Resolver);
            tool.RemoveSource(Name, Source, Settings);
        }
    }
}