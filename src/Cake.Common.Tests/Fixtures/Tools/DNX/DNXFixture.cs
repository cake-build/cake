using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core.IO;
using Cake.Core.Tooling;
using NSubstitute;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools.DNX
{
    internal abstract class DNXFixture<TSettings> : DNXFixture<TSettings, ToolFixtureResult>
        where TSettings : ToolSettings, new()
    {
        protected override ToolFixtureResult CreateResult(FilePath toolPath, ProcessSettings process)
        {
            return new ToolFixtureResult(toolPath, process);
        }
    }

    internal abstract class DNXFixture<TSettings, TFixtureResult> : ToolFixture<TSettings, TFixtureResult>
        where TSettings : ToolSettings, new()
        where TFixtureResult : ToolFixtureResult
    {
        protected DNXFixture()
            : base("dnvm.cmd")
        {
            ProcessRunner.Process.SetStandardOutput(new string[] { });
        }
    }
}
