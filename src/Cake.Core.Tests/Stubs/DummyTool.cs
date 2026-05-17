// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Core.Tests.Stubs
{
    public sealed class DummyTool : Tool<DummySettings>
    {
        private readonly Action<int> _exitCodeValidation;

        public DummyTool(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            Action<int> exitCodeValidation) : base(fileSystem, environment, processRunner, tools)
        {
            _exitCodeValidation = exitCodeValidation;
        }

        public void Run(DummySettings settings)
        {
            Run(settings, new ProcessArgumentBuilder().Append("--foo"), new ProcessSettings(), null);
        }

        protected override void ProcessExitCode(IProcess process)
        {
            if (_exitCodeValidation == null)
            {
                base.ProcessExitCode(process);
                return;
            }
            _exitCodeValidation(process.GetExitCode());
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