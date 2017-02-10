// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.Chocolatey.New;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey.New
{
    internal sealed class ChocolateyNewFixture : ChocolateyFixture<ChocolateyNewSettings>
    {
        public string PackageId { get; set; }

        public ChocolateyNewFixture()
        {
            PackageId = "MyPackage";
        }

        protected override void RunTool()
        {
            var tool = new ChocolateyScaffolder(FileSystem, Environment, ProcessRunner, Tools, Resolver);
            tool.CreatePackage(PackageId, Settings);
        }
    }
}