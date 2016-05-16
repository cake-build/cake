using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools.DNU
{
    internal abstract class DNUFixture<TSettings> : DNUFixture<TSettings, ToolFixtureResult>
        where TSettings : ToolSettings, new()
    {
        protected override ToolFixtureResult CreateResult(FilePath path, ProcessSettings process)
        {
            return new ToolFixtureResult(path, process);
        }
    }

    internal abstract class DNUFixture<TSettings, TFixtureResult> : ToolFixture<TSettings, TFixtureResult>
        where TSettings : ToolSettings, new()
        where TFixtureResult : ToolFixtureResult
    {
        protected DNUFixture()
            : base("dnu.cmd")
        {
            ProcessRunner.Process.SetStandardOutput(new string[] { });
        }
    }
}