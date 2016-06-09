// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.NuGet.Restore;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet.Restorer
{
    internal sealed class NuGetRestorerFixture : NuGetFixture<NuGetRestoreSettings>
    {
        public FilePath TargetFilePath { get; set; }

        public NuGetRestorerFixture()
        {
            TargetFilePath = "./project.sln";
        }

        protected override void RunTool()
        {
            var tool = new NuGetRestorer(FileSystem, Environment, ProcessRunner, Tools, Resolver);
            tool.Restore(TargetFilePath, Settings);
        }
    }
}
