// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Core.Tests.Stubs
{
    public sealed class DummyTool : Tool<DummySettings>
    {
        public DummyTool(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
        }

        public void Run(DummySettings settings)
        {
            Run(settings, new ProcessArgumentBuilder().Append("--foo"));
        }

        protected override string GetToolName()
        {
            return "dummy";
        }

        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "dummy.exe" };
        }
    }
}
