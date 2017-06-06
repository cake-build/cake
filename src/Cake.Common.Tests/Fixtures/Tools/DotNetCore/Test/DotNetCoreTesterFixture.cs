// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Cake.Common.Tools.DotNetCore.Test;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.DotNetCore.Test
{
    internal sealed class DotNetCoreTesterFixture : DotNetCoreFixture<DotNetCoreTestSettings>
    {
        public string Project { get; set; }

        public List<FilePath> ProjectFiles { get; set; }

        public List<DirectoryPath> DirectoryPaths { get; set; }

        public DotNetCoreTesterFixture()
        {
            ProjectFiles = new List<FilePath>();
            DirectoryPaths = new List<DirectoryPath>();
        }

        protected override void RunTool()
        {
            var tool = new DotNetCoreTester(FileSystem, Environment, ProcessRunner, Tools);

            if (ProjectFiles.Any())
            {
                tool.Test(ProjectFiles, Settings);
            }
            else if (DirectoryPaths.Any())
            {
                tool.Test(DirectoryPaths, Settings);
            }
            else
            {
                tool.Test(Project, Settings);
            }
        }
    }
}