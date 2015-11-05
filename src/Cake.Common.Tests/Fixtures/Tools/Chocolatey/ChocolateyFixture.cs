using Cake.Common.Tools.Chocolatey;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey
{
    internal abstract class ChocolateyFixture<TSettings> : ChocolateyFixture<TSettings, ToolFixtureResult>
        where TSettings : ToolSettings, new()
    {
        protected override ToolFixtureResult CreateResult(FilePath toolPath, ProcessSettings process)
        {
            return new ToolFixtureResult(toolPath, process);
        }
    }

    internal abstract class ChocolateyFixture<TSettings, TFixtureResult> : ToolFixture<TSettings, TFixtureResult>
        where TSettings : ToolSettings, new()
        where TFixtureResult : ToolFixtureResult
    {
        public IChocolateyToolResolver Resolver { get; set; }
        public ICakeLog Log { get; set; }

        protected ChocolateyFixture()
            : base("choco.exe")
        {
            Resolver = Substitute.For<IChocolateyToolResolver>();
            Log = Substitute.For<ICakeLog>();
        }
    }
}
