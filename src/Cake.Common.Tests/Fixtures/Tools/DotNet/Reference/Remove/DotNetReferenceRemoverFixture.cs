// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tools.DotNet.Reference.Remove;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.Reference.Remove
{
    internal sealed class DotNetReferenceRemoverFixture : DotNetFixture<DotNetReferenceRemoveSettings>
    {
        public string Project { get; set; }

        public IEnumerable<FilePath> ProjectReferences { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetReferenceRemover(FileSystem, Environment, ProcessRunner, Tools);
            tool.Remove(Project, ProjectReferences, Settings);
        }
    }
}
