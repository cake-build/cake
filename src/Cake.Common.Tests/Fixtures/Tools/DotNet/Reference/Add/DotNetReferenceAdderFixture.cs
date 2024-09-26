// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tools.DotNet.Reference.Add;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.Reference.Add
{
    internal sealed class DotNetReferenceAdderFixture : DotNetFixture<DotNetReferenceAddSettings>
    {
        public string Project { get; set; }

        public IEnumerable<FilePath> ProjectReferences { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetReferenceAdder(FileSystem, Environment, ProcessRunner, Tools);
            tool.Add(Project, ProjectReferences, Settings);
        }
    }
}
