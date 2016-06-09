// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.NuGet.Update;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet.Update
{
    internal sealed class NuGetUpdateFixture : NuGetFixture<NuGetUpdateSettings>
    {
        public FilePath TargetFile { get; set; }

        public NuGetUpdateFixture()
        {
            TargetFile = "./packages.config";
        }

        protected override void RunTool()
        {
            var tool = new NuGetUpdater(FileSystem, Environment, ProcessRunner, Tools, Resolver);
            tool.Update(TargetFile, Settings);
        }
    }
}
