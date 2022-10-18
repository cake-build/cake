// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNet.NuGet.Delete;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.NuGet.Delete
{
    internal sealed class DotNetNuGetDeleterFixture : DotNetFixture<DotNetNuGetDeleteSettings>
    {
        public string PackageName { get; set; }
        public string PackageVersion { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetNuGetDeleter(FileSystem, Environment, ProcessRunner, Tools);
            tool.Delete(PackageName, PackageVersion, Settings);
        }
    }
}