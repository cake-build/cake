// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tools.DotNet.Sln.Remove;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.Sln.Remove
{
    internal sealed class DotNetSlnRemoverFixture : DotNetFixture<DotNetSlnRemoveSettings>
    {
        public FilePath Solution { get; set; }

        public IEnumerable<FilePath> ProjectPath { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetSlnRemover(FileSystem, Environment, ProcessRunner, Tools);
            tool.Remove(Solution, ProjectPath, Settings);
        }
    }
}
