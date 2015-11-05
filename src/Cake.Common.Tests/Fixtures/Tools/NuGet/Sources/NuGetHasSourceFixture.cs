using Cake.Common.Tools.NuGet.Sources;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet.Sources
{
    internal sealed class NuGetHasSourceFixture : NuGetSourcesFixture
    {
        protected override void RunTool()
        {
            var tool = new NuGetSources(FileSystem, Environment, ProcessRunner, Globber, Resolver);
            tool.HasSource(Source, Settings);
        }
    }
}