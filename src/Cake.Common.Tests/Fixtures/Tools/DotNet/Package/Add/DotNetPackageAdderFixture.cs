// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNet.Package.Add;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.Package.Add
{
    internal sealed class DotNetPackageAdderFixture : DotNetFixture<DotNetPackageAddSettings>
    {
        public string PackageName { get; set; }

        public string Project { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetPackageAdder(FileSystem, Environment, ProcessRunner, Tools);
            tool.Add(PackageName, Project, Settings);
        }
    }
}
