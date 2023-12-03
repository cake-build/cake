// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNet.Package.Remove;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.Package.Remove
{
    internal sealed class DotNetPackageRemoverFixture : DotNetFixture<DotNetPackageRemoveSettings>
    {
        public string PackageName { get; set; }

        public string Project { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetPackageRemover(FileSystem, Environment, ProcessRunner, Tools);
            tool.Remove(PackageName, Project);
        }
    }
}
