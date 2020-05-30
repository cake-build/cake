// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNetCore.NuGet.Source;

namespace Cake.Common.Tests.Fixtures.Tools.DotNetCore.NuGet.Source
{
    internal sealed class DotNetCoreNuGetHasSourceFixture : DotNetCoreNuGetSourcerFixture
    {
        public bool HasSource { get; set; }

        public void GivenProcessOutput(string[] output)
        {
            ProcessRunner.Process.SetStandardOutput(output);
        }

        protected override void RunTool()
        {
            var tool = new DotNetCoreNuGetSourcer(FileSystem, Environment, ProcessRunner, Tools);
            HasSource = tool.HasSource(Name, Settings);
        }
    }
}