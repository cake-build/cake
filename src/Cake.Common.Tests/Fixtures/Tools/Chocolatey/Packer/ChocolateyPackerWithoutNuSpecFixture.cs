// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.Chocolatey.Pack;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey.Packer
{
    internal sealed class ChocolateyPackerWithoutNuSpecFixture : ChocolateyPackerFixture
    {
        protected override void RunTool()
        {
            var tool = new ChocolateyPacker(FileSystem, Environment, ProcessRunner, Log, Tools, Resolver);
            tool.Pack(Settings);
        }
    }
}
