using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.IO.NuGet;
using Cake.Core.Tooling;
using Cake.Testing.Shared;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet
{
    internal abstract class NuGetFixture<TSettings> : NuGetFixture<TSettings, ToolFixtureResult>
        where TSettings : ToolSettings, new()
    {
        protected override ToolFixtureResult CreateResult(FilePath toolPath, ProcessSettings process)
        {
            return new ToolFixtureResult(toolPath, process);
        }
    }

    internal abstract class NuGetFixture<TSettings, TFixtureResult> : ToolFixture<TSettings, TFixtureResult>
        where TSettings : ToolSettings, new()
        where TFixtureResult : ToolFixtureResult
    {
        public ICakeLog Log { get; set; }
        public INuGetToolResolver Resolver { get; set; }

        protected NuGetFixture()
            : base("NuGet.exe")
        {
            Process.GetStandardOutput().Returns(new string[] { });

            Resolver = Substitute.For<INuGetToolResolver>();
            Log = Substitute.For<ICakeLog>();
        }
    }
}