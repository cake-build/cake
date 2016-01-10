using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Testing.Fixtures;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake.Common.Tests.Fixtures.Tools.DNVM
{
    internal abstract class DNVMFixture<TSettings> : DNVMFixture<TSettings, ToolFixtureResult>
        where TSettings : ToolSettings, new()
    {
        protected override ToolFixtureResult CreateResult(FilePath toolPath, ProcessSettings process)
        {
            return new ToolFixtureResult(toolPath, process);
        }
    }

    internal abstract class DNVMFixture<TSettings, TFixtureResult> : ToolFixture<TSettings, TFixtureResult>
        where TSettings : ToolSettings, new()
        where TFixtureResult : ToolFixtureResult
    {
        protected DNVMFixture()
            : base("dnvmwrapper.sh")
        {
            ProcessRunner.Process.SetStandardOutput(new string[] { });
        }
    }
}
