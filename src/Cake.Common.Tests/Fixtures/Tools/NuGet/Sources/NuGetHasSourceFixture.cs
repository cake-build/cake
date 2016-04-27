using Cake.Common.Tools.NuGet.Sources;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet.Sources
{
    internal sealed class NuGetHasSourceFixture : NuGetSourcesFixture
    {
        protected override void RunTool()
        {
            var tool = new NuGetSources(FileSystem, Environment, ProcessRunner, Tools, Resolver);
            tool.HasSource(Source, Settings);
        }
    }
}