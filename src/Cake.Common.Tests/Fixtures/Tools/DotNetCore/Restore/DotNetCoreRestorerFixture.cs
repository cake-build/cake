// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Cake.Common.Tools.DotNetCore.Restore;
using Cake.Core.IO;
using Cake.Testing;

namespace Cake.Common.Tests.Fixtures.Tools.DotNetCore.Restore
{
    internal sealed class DotNetCoreRestorerFixture : DotNetCoreFixture<DotNetCoreRestoreSettings>
    {
        public string Root { get; set; }
        public List<FilePath> ProjectFiles { get; set; }
        public List<DirectoryPath> DirectoryPaths { get; set; }

        public DotNetCoreRestorerFixture()
        {
            ProjectFiles = new List<FilePath>();
            DirectoryPaths = new List<DirectoryPath>();
        }

        protected override void RunTool()
        {
            var tool = new DotNetCoreRestorer(FileSystem, Environment, ProcessRunner, Tools, new FakeLog());

            if (ProjectFiles.Any())
            {
                tool.Restore(ProjectFiles, Settings);
            }
            else if (DirectoryPaths.Any())
            {
                tool.Restore(DirectoryPaths, Settings);
            }
            else
            {
                tool.Restore(Root, Settings);
            }
        }
    }
}