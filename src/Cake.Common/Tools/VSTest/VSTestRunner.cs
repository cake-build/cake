// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="VSTestRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="toolLocator">The tool servce.</param>
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
                throw new ArgumentNullException("assemblyPaths");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            base.Run(settings, GetArguments(assemblyPaths, settings));
        }

        private ProcessArgumentBuilder GetArguments(IEnumerable<FilePath> assemblyPaths, VSTestSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            // Add the assembly to build.
            foreach (var assemblyPath in assemblyPaths)
            {
                builder.Append(assemblyPath.MakeAbsolute(_environment).FullPath.Quote());
            }

            if (settings.SettingsFile != null)
            {
                builder.Append(string.Format(CultureInfo.InvariantCulture, "/Settings:{0}", settings.SettingsFile));
            }

            if (settings.InIsolation)
            {
                builder.Append("/InIsolation");
            }

            if (settings.PlatformArchitecture != VSTestPlatform.Default)
            {
                builder.Append(string.Format(CultureInfo.InvariantCulture, "/Platform:{0}", settings.PlatformArchitecture));
            }

            if (settings.FrameworkVersion != VSTestFrameworkVersion.Default)
            {
                builder.Append(string.Format(CultureInfo.InvariantCulture, "/Framework:{0}", settings.FrameworkVersion.ToString().Replace("NET", "Framework")));
            }

            if (settings.Logger == VSTestLogger.Trx)
            {
                builder.Append("/Logger:trx");
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
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return Enumerable.Empty<string>();
        }

        /// <summary>
        /// Gets alternative file paths which the tool may exist in
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected override IEnumerable<FilePath> GetAlternativeToolPaths(VSTestSettings settings)
        {
            foreach (var version in new[] { "15.0", "14.0", "12.0", "11.0" })
            {
                var path = GetToolPath(version);
                if (_fileSystem.Exist(path))
                {
                    yield return path;
                }
            }
        }

        private FilePath GetToolPath(string version)
        {
            var programFiles = _environment.GetSpecialPath(SpecialPath.ProgramFilesX86);
            var root = programFiles.Combine(string.Concat("Microsoft Visual Studio ", version, "/Common7/IDE/CommonExtensions/Microsoft/TestWindow"));
            return root.CombineWithFilePath("vstest.console.exe");
        }
    }
}
