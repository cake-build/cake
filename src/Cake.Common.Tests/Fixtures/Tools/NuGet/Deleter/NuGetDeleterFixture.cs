// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.NuGet.Delete;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet.Deleter
{
    internal sealed class NuGetDeleterFixture : NuGetFixture<NuGetDeleteSettings>
    {
        public string PackageID { get; set; }

        public string PackageVersion { get; set; }

        public NuGetDeleterFixture()
        {
            PackageID = "existing";
            PackageVersion = "0.1.0";
        }

        protected override void RunTool()
        {
            var tool = new NuGetDeleter(FileSystem, Environment, ProcessRunner, Tools, Resolver, Log);
            tool.Delete(PackageID, PackageVersion, Settings);
        }
    }
}