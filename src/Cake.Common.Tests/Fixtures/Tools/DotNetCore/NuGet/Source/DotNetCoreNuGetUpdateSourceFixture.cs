// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNet.NuGet.Source;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.NuGet.Source
{
    internal sealed class DotNetNuGetUpdateSourceFixture : DotNetNuGetSourcerFixture
    {
        protected override void RunTool()
        {
            var tool = new DotNetNuGetSourcer(FileSystem, Environment, ProcessRunner, Tools);
            tool.UpdateSource(Name, Settings);
        }
    }
}