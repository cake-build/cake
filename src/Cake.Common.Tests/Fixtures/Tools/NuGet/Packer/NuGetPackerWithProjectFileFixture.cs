// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tests.Properties;
using Cake.Common.Tools.NuGet.Pack;
using Cake.Core.IO;
using Cake.Testing;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet.Packer
{
    internal sealed class NuGetPackerWithProjectFileFixture : NuGetPackerFixture
    {
        public FilePath ProjectFilePath { get; set; }

        public NuGetPackerWithProjectFileFixture()
        {
            ProjectFilePath = "./existing.csproj";
            FileSystem.CreateFile("/Working/existing.csproj").SetContent(Resources.Nuspec_ProjectFile);
        }

        protected override void RunTool()
        {
            var tool = new NuGetPacker(FileSystem, Environment, ProcessRunner, Log, Tools, Resolver);
            tool.Pack(ProjectFilePath, Settings);
        }
    }
}
