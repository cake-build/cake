// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNetCore.NuGet.Delete;

namespace Cake.Common.Tests.Fixtures.Tools.DotNetCore.NuGet.Delete
{
    internal sealed class DotNetCoreDeleterFixture : DotNetCoreFixture<DotNetCoreNuGetDeleteSettings>
    {
        public string PackageName { get; set; }
        public string PackageVersion { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetCoreNuGetDeleter(FileSystem, Environment, ProcessRunner, Tools);
            tool.Delete(PackageName, PackageVersion, Settings);
        }
    }
}