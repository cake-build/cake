// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tools.DotNet.Package.Search;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.Package.Search
{
    internal class DotNetPackageSearcherFixture : DotNetFixture<DotNetPackageSearchSettings>
    {
        public string SearchTerm { get; set; }

        public IEnumerable<DotNetPackageSearchItem> Result { get; private set; }

        protected override void RunTool()
        {
            var tool = new DotNetPackageSearcher(FileSystem, Environment, ProcessRunner, Tools);
            Result = tool.Search(SearchTerm, Settings);
        }

        internal void GivenNormalPackageResult()
        {
            ProcessRunner.Process.SetStandardOutput(new string[]
            {
                "{",
                "  \"version\": 2,",
                "  \"problems\": [],",
                "  \"searchResult\": [",
                "    {",
                "      \"sourceName\": \"nuget.org\",",
                "      \"packages\": [",
                "        {",
                "          \"id\": \"Cake\",",
                "          \"latestVersion\": \"0.22.2\"",
                "        },",
                "        {",
                "          \"id\": \"Cake.Core\",",
                "          \"latestVersion\": \"0.22.2\"",
                "        },",
                "        {",
                "          \"id\": \"Cake.CoreCLR\",",
                "          \"latestVersion\": \"0.22.2\"",
                "        }",
                "      ]",
                "    }",
                "  ]",
                "}",
            });
        }
    }
}
