// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tools.DotNet.Reference.List;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.Reference.List
{
    internal sealed class DotNetReferenceListerFixture : DotNetFixture<DotNetReferenceListSettings>
    {
        public string Project { get; set; }

        public IEnumerable<string> References { get; set; }

        public void GivenProjectReferencesResult()
        {
            ProcessRunner.Process.SetStandardOutput(new string[]
            {
                "Project reference(s)",
                "--------------------",
                "..\\..\\Common\\Common.AspNetCore\\Common.AspNetCore.csproj",
                "..\\..\\Common\\Common.Messaging\\Common.Messaging.csproj",
                "..\\..\\Common\\Common.Utilities\\Common.Utilities.csproj"
            });
        }

        public void GivenEmptyProjectReferencesResult()
        {
            ProcessRunner.Process.SetStandardOutput(new string[]
            {
                "There are no Project to Project references in project C:\\Cake\\Cake.Core\\."
            });
        }

        protected override void RunTool()
        {
            var tool = new DotNetReferenceLister(FileSystem, Environment, ProcessRunner, Tools);
            References = tool.List(Project, Settings);
        }
    }
}
