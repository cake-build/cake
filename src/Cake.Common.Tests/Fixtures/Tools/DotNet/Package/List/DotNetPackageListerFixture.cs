// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNet.Package.List;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.Package.List
{
    internal sealed class DotNetPackageListerFixture : DotNetFixture<DotNetPackageListSettings>
    {
        public string Project { get; set; }
        public DotNetPackageList Result { get; set; }

        public void GivenPackgeListResult()
        {
            ProcessRunner.Process.SetStandardOutput(new string[]
            {
                "{",
                "  \"version\": 1,",
                "  \"parameters\": \"\",",
                "  \"projects\": [",
                "    {",
                "      \"path\": \"src/lib/MyProject.csproj\",",
                "      \"frameworks\": [",
                "        {",
                "          \"framework\": \"netstandard2.0\",",
                "          \"topLevelPackages\": [",
                "            {",
                "              \"id\": \"NETStandard.Library\",",
                "              \"requestedVersion\": \"[2.0.3, )\",",
                "              \"resolvedVersion\": \"2.0.3\",",
                "              \"autoReferenced\": \"true\"",
                "            }",
                "          ]",
                "        }",
                "      ]",
                "    }",
                "  ]",
                "}"
            });
        }

        protected override void RunTool()
        {
            var tool = new DotNetPackageLister(FileSystem, Environment, ProcessRunner, Tools);
            Result = tool.List(Project, Settings);
        }
    }
}
