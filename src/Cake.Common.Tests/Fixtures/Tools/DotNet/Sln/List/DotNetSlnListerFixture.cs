// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tools.DotNet.Sln.List;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.Sln.List
{
    internal sealed class DotNetSlnListerFixture : DotNetFixture<DotNetSlnListSettings>
    {
        public string Solution { get; set; }

        public string StandardError { get; set; }

        public IEnumerable<string> Projects { get; private set; }

        public void GivenProjectsResult()
        {
            ProcessRunner.Process.SetStandardOutput(new string[]
            {
                "Project(s)",
                "--------------------",
                "Common\\Common.AspNetCore\\Common.AspNetCore.csproj",
                "Common\\Common.Messaging\\Common.Messaging.csproj",
                "Common\\Common.Utilities\\Common.Utilities.csproj"
            });
        }

        public void GivenErrorResult()
        {
            ProcessRunner.Process.SetStandardError(new string[]
            {
                StandardError
            });
        }

        protected override void RunTool()
        {
            var tool = new DotNetSlnLister(FileSystem, Environment, ProcessRunner, Tools);
            Projects = tool.List(Solution, Settings);
        }
    }
}
