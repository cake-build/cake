// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;
using Cake.Testing;

namespace Cake.Common.Tests.Fixtures.Solution
{
    internal sealed class SolutionParserFixture
    {
        public FakeEnvironment Environment { get; set; }

        public FakeFileSystem FileSystem { get; set; }

        public SolutionParserFixture()
        {
            var environment = FakeEnvironment.CreateUnixEnvironment();
            Environment = environment;
            var fileSystem = new FakeFileSystem(environment);
            FileSystem = fileSystem;
        }

        public FilePath WithSolutionFile(string slnContent)
        {
            var file = FileSystem.CreateFile("/Working/dummySolution.sln").SetContent(slnContent);
            return file.Path;
        }
    }
}