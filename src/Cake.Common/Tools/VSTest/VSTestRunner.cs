// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.VSTest
{
    /// <summary>
    /// The VSTest unit test runner.
    /// Used by Visual Studio 2012 and newer.
    /// </summary>
    public sealed class VSTestRunner : Tool<VSTestSettings>
    {
        private const string VSTestConsoleExecutableName = "vstest.console.exe";
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="VSTestRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="toolLocator">The tool locator.</param>
        public VSTestRunner(IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator toolLocator)
            : base(fileSystem, environment, processRunner, toolLocator)
        {
            _fileSystem = fileSystem;
            _environment = environment;
        }

        /// <summary>
        /// Runs the tests in the specified assembly.
        /// </summary>
        /// <param name="assemblyPaths">The assembly path.</param>
        /// <param name="settings">The settings.</param>
        public void Run(IEnumerable<FilePath> assemblyPaths, VSTestSettings settings)
        {
            if (assemblyPaths == null)
            {
                throw new ArgumentNullException(nameof(assemblyPaths));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            Run(settings, GetArguments(assemblyPaths, settings));
        }

        private ProcessArgumentBuilder GetArguments(IEnumerable<FilePath> assemblyPaths, VSTestSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            // Add the assembly to build.
            foreach (var assemblyPath in assemblyPaths)
            {
                builder.AppendQuoted(assemblyPath.MakeAbsolute(_environment).FullPath);
            }

            if (settings.SettingsFile != null)
            {
                builder.AppendSwitchQuoted("/Settings", ":", settings.SettingsFile.MakeAbsolute(_environment).FullPath);
            }

            if (settings.Parallel)
            {
                builder.Append("/Parallel");
            }

            if (settings.EnableCodeCoverage)
            {
                builder.Append("/EnableCodeCoverage");
            }

            if (settings.InIsolation)
            {
                builder.Append("/InIsolation");
            }

            if (settings.UseVsixExtensions != null)
            {
                builder.AppendSwitch("/UseVsixExtensions", ":", settings.UseVsixExtensions.Value ? "true" : "false");
            }

            if (settings.TestAdapterPath != null)
            {
                builder.AppendSwitchQuoted("/TestAdapterPath", ":", settings.TestAdapterPath.MakeAbsolute(_environment).FullPath);
            }

            if (settings.PlatformArchitecture != VSTestPlatform.Default)
            {
                builder.AppendSwitch("/Platform", ":", settings.PlatformArchitecture.ToString());
            }

            if (settings.FrameworkVersion != VSTestFrameworkVersion.Default)
            {
                builder.AppendSwitch("/Framework", ":", settings.FrameworkVersion.ToString().Replace("NET", "Framework"));
            }

            if (settings.TestCaseFilter != null)
            {
                builder.AppendSwitchQuoted("/TestCaseFilter", ":", settings.TestCaseFilter);
            }

            if (settings.Diag != null)
            {
                builder.AppendSwitchQuoted("/Diag", ":", settings.Diag.MakeAbsolute(_environment).FullPath);
            }

            if (settings.ResultsDirectory != null)
            {
                builder.AppendSwitchQuoted("/ResultsDirectory", ":", settings.ResultsDirectory.MakeAbsolute(_environment).FullPath);
            }

            if (!string.IsNullOrEmpty(settings.Logger))
            {
                builder.Append("/Logger:{0}", settings.Logger.Trim());
            }

            return builder;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The tool name.</returns>
        protected override string GetToolName()
        {
            return "VSTest";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames() => new[] { VSTestConsoleExecutableName };

        /// <summary>
        /// Gets alternative file paths which the tool may exist in.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected override IEnumerable<FilePath> GetAlternativeToolPaths(VSTestSettings settings)
        {
            var vsRootRelativeToolPath = FilePath.FromString($"Common7/IDE/CommonExtensions/Microsoft/TestWindow/{VSTestConsoleExecutableName}");
            foreach (var year in VisualStudio.Versions.TwentySeventeenAndLater)
            {
                foreach (var edition in settings.AllowPreviewVersion
                             ? VisualStudio.Editions.All
                             : VisualStudio.Editions.Stable)
                {
                    var path = VisualStudio.GetYearAndEditionToolPath(_environment, year, edition, vsRootRelativeToolPath);
                    if (_fileSystem.Exist(path))
                    {
                        yield return path;
                    }
                }
            }

            foreach (var version in VisualStudio.Versions.TenToFourteen)
            {
                var path = VisualStudio.GetVersionNumberToolPath(_environment, version, vsRootRelativeToolPath);
                if (_fileSystem.Exist(path))
                {
                    yield return path;
                }
            }
        }
    }
}