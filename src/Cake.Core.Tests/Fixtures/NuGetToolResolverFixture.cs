// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.IO;
using Cake.Core.IO.NuGet;
using Cake.Core.Tooling;
using Cake.Testing;

namespace Cake.Core.Tests.Fixtures
{
    internal sealed class NuGetToolResolverFixture
    {
        public FakeFileSystem FileSystem { get; set; }
        public FakeEnvironment Environment { get; set; }
        public ToolLocator Tools { get; set; }

        public NuGetToolResolverFixture(FakeEnvironment environment = null)
        {
            Environment = environment ?? FakeEnvironment.CreateUnixEnvironment();
            FileSystem = new FakeFileSystem(Environment);
            Tools = new ToolLocator(
                Environment,
                new ToolRepository(Environment),
                new ToolResolutionStrategy(FileSystem, Environment, new Globber(FileSystem, Environment), new FakeConfiguration()));
        }

        public FilePath Resolve()
        {
            var resolver = new NuGetToolResolver(FileSystem, Environment, Tools);
            return resolver.ResolvePath();
        }
    }
}
