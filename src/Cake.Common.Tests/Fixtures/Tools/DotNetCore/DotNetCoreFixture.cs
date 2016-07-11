// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.DotNetCore;
using Cake.Core.IO;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools.DotNetCore
{
    internal abstract class DotNetCoreFixture<TSettings> : ToolFixture<TSettings, ToolFixtureResult>
        where TSettings : DotNetCoreSettings, new()
    {
        protected DotNetCoreFixture()
            : base("dotnet.exe")
        {
            ProcessRunner.Process.SetStandardOutput(new string[] { });
        }

        protected override ToolFixtureResult CreateResult(FilePath path, ProcessSettings process)
        {
            return new ToolFixtureResult(path, process);
        }
    }
}
