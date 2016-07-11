// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.MSTest
{
    /// <summary>
    /// The MSTest unit test runner.
    /// </summary>
    public sealed class MSTestRunner : Tool<MSTestSettings>
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="MSTestRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public MSTestRunner(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _fileSystem = fileSystem;
            _environment = environment;
        }

        /// <summary>
        /// Runs the tests in the specified assembly.
        /// </summary>
        /// <param name="assemblyPaths">The assembly path.</param>
        /// <param name="settings">The settings.</param>
        public void Run(IEnumerable<FilePath> assemblyPaths, MSTestSettings settings)
        {
            if (assemblyPaths == null)
            {
                throw new ArgumentNullException("assemblyPaths");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Run(settings, GetArguments(assemblyPaths, settings));
        }

        private ProcessArgumentBuilder GetArguments(IEnumerable<FilePath> assemblyPaths, MSTestSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            // Add the assembly to build.
            foreach (var assemblyPath in assemblyPaths)
            {
                builder.Append(string.Concat("/testcontainer:", assemblyPath.MakeAbsolute(_environment).FullPath).Quote());
            }

            if (!string.IsNullOrEmpty(settings.Category))
            {
                builder.Append(string.Concat("/category:", settings.Category.Quote()));
            }

            if (settings.NoIsolation)
            {
                builder.Append("/noisolation");
            }

            if (settings.TestSettings != null)
            {
                builder.Append(
                    string.Concat("/testsettings:", settings.TestSettings.MakeAbsolute(_environment).FullPath.Quote()));
            }

            return builder;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The tool name.</returns>
        protected override string GetToolName()
        {
            return "MSTest";
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
        protected override IEnumerable<FilePath> GetAlternativeToolPaths(MSTestSettings settings)
        {
            foreach (var version in new[] { "14.0", "12.0", "11.0", "10.0" })
            {
                var path = GetToolPath(version);
                if (_fileSystem.Exist(path))
                {
                    yield return path;
                }
            }

            foreach (var environmentVariable in new[] { "VS140COMNTOOLS", "VS130COMNTOOLS", "VS120COMNTOOLS", "VS110COMNTOOLS", "VS100COMNTOOLS" })
            {
                var path = GetCommonToolPath(environmentVariable);
                if (path != null && _fileSystem.Exist(path))
                {
                    yield return path;
                }
            }
        }

        private FilePath GetToolPath(string version)
        {
            var programFiles = _environment.GetSpecialPath(SpecialPath.ProgramFilesX86);
            var root = programFiles.Combine(string.Concat("Microsoft Visual Studio ", version, "/Common7/IDE"));
            return root.CombineWithFilePath("mstest.exe");
        }

        private FilePath GetCommonToolPath(string environmentVariable)
        {
            var visualStudioCommonToolsPath = _environment.GetEnvironmentVariable(environmentVariable);

            if (string.IsNullOrWhiteSpace(visualStudioCommonToolsPath))
            {
                return null;
            }

            var root = new DirectoryPath(visualStudioCommonToolsPath).Combine("../IDE").Collapse();
            return root.CombineWithFilePath("mstest.exe");
        }
    }
}
