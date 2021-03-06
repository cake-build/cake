// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Core.Diagnostics;
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

        public ToolResolutionStrategyFixture(FakeEnvironment environment = null)
        {
            Environment = environment ?? FakeEnvironment.CreateUnixEnvironment();
            FileSystem = new FakeFileSystem(Environment);
            Globber = new Globber(FileSystem, Environment);
            Configuration = new FakeConfiguration();
            Repository = new ToolRepository(Environment);
        }

        public FilePath Resolve(string name)
        {
            var strategy = new ToolResolutionStrategy(FileSystem, Environment, Globber, Configuration, new NullLog());
            return strategy.Resolve(Repository, name);
        }

        public FilePath Resolve(IEnumerable<string> toolExeNames)
        {
            var strategy = new ToolResolutionStrategy(FileSystem, Environment, Globber, Configuration, new NullLog());
            return strategy.Resolve(Repository, toolExeNames);
        }
    }
}