using Cake.Core.IO;
using Cake.Core.Tooling;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools.DNU
{
    internal abstract class DNUFixture<TSettings> : DNUFixture<TSettings, ToolFixtureResult>
        where TSettings : ToolSettings, new()
    {
        protected override ToolFixtureResult CreateResult(FilePath toolPath, ProcessSettings process)
        {
            return new ToolFixtureResult(toolPath, process);
        }
    }

    internal abstract class DNUFixture<TSettings, TFixtureResult> : ToolFixture<TSettings, TFixtureResult>
        where TSettings : ToolSettings, new()
        where TFixtureResult : ToolFixtureResult
    {
        protected DNUFixture()
            : base("dnu.cmd")
        {
            Process.GetStandardOutput().Returns(new string[] { });
        }
    }
}