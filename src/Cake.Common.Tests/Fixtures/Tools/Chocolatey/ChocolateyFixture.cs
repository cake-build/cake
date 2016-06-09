// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.Chocolatey;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Testing.Fixtures;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey
{
    internal abstract class ChocolateyFixture<TSettings> : ChocolateyFixture<TSettings, ToolFixtureResult>
        where TSettings : ToolSettings, new()
    {
        protected override ToolFixtureResult CreateResult(FilePath path, ProcessSettings process)
        {
            return new ToolFixtureResult(path, process);
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
