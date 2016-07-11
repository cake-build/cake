// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Testing;

namespace Cake.Core.Tests.Fixtures
{
    internal sealed class ToolResolutionStrategyFixture
    {
        public FakeFileSystem FileSystem { get; set; }
        public FakeEnvironment Environment { get; set; }
        public IGlobber Globber { get; set; }
        public FakeConfiguration Configuration { get; set; }
        public IToolRepository Repository { get; set; }

        public ToolResolutionStrategyFixture()
        {
            Environment = FakeEnvironment.CreateUnixEnvironment();
            FileSystem = new FakeFileSystem(Environment);
            Globber = new Globber(FileSystem, Environment);
            Configuration = new FakeConfiguration();
            Repository = new ToolRepository(Environment);
        }

        public FilePath Resolve(string name)
        {
            var strategy = new ToolResolutionStrategy(FileSystem, Environment, Globber, Configuration);
            return strategy.Resolve(Repository, name);
        }
    }
}
