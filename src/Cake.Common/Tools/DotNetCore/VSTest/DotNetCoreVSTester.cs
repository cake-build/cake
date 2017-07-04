// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Common.Tools.VSTest;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNetCore.VSTest
{
    /// <summary>
    /// .NET Core VSTest tester.
    /// </summary>
    public sealed class DotNetCoreVSTester : DotNetCoreTool<DotNetCoreVSTestSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCoreVSTester" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetCoreVSTester(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Tests the project using the specified path with arguments and settings.
        /// </summary>
        /// <param name="testFiles">A list of test files to run.</param>
        /// <param name="settings">The settings.</param>
        public void Test(IEnumerable<FilePath> testFiles, DotNetCoreVSTestSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (testFiles == null || !testFiles.Any())
            {
                throw new ArgumentNullException(nameof(testFiles));
            }

            RunCommand(settings, GetArguments(testFiles, settings));
        }

        private ProcessArgumentBuilder GetArguments(IEnumerable<FilePath> testFiles, DotNetCoreVSTestSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("vstest");

            // Specific path?
            foreach (var testFile in testFiles)
            {
                builder.AppendQuoted(testFile.MakeAbsolute(_environment).FullPath);
            }

            // Settings
            if (settings.Settings != null)
            {
                builder.AppendSwitchQuoted("--Settings", ":", settings.Settings.MakeAbsolute(_environment).FullPath);
            }

            // Tests to run
            if (settings.TestsToRun.Any())
            {
                builder.AppendSwitch("--Tests", ":", string.Join(",", settings.TestsToRun));
            }

            // Path to custom test adapter
            if (settings.TestAdapterPath != null)
            {
                builder.AppendSwitchQuoted("--TestAdapterPath", ":", settings.TestAdapterPath.MakeAbsolute(_environment).FullPath);
            }

            // Platform architecture to execute tests on
            if (settings.Platform != VSTestPlatform.Default)
            {
                builder.AppendSwitch("--Platform", ":", settings.Platform.ToString());
            }

            // Target Framework
            if (!string.IsNullOrWhiteSpace(settings.Framework))
            {
                builder.AppendSwitch("--Framework", ":", settings.Framework);
            }

            // Run tests in parallel?
            if (settings.Parallel)
            {
                builder.Append("--Parallel");
            }

            // Test Case Filter
            if (!string.IsNullOrWhiteSpace(settings.TestCaseFilter))
            {
                builder.AppendSwitchQuoted("--TestCaseFilter", ":", settings.TestCaseFilter);
            }

            // Logger
            if (!string.IsNullOrWhiteSpace(settings.Logger))
            {
                builder.AppendSwitchQuoted("--logger", ":", settings.Logger);
            }

            // Parent Process Id?
            if (!string.IsNullOrWhiteSpace(settings.ParentProcessId))
            {
                builder.AppendSwitch("--ParentProcessId", ":", settings.ParentProcessId);
            }

            // Port?
            if (settings.Port.HasValue)
            {
                builder.AppendSwitch("--Port", ":", settings.Port.Value.ToString());
            }

            // Write to Diagnostic file?
            if (settings.DiagnosticFile != null)
            {
                builder.AppendSwitchQuoted("--Diag", ":", settings.DiagnosticFile.MakeAbsolute(_environment).FullPath);
            }

            // Extra arguments
            foreach (var argument in settings.Arguments)
            {
                builder.AppendSwitchQuoted(argument.Key, "=", argument.Value);
            }

            return builder;
        }
    }
}
