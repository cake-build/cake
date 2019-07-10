// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Cake.Common.Tools.DotNetCore.MSBuild;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.MSBuild
{
    /// <summary>
    /// The MSBuild runner.
    /// </summary>
    public sealed class MSBuildRunner : DotNetBuildTool<MSBuildSettings>
    {
        private readonly ICakeEnvironment _environment;
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="MSBuildRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="runner">The runner.</param>
        /// <param name="tools">The tool locator.</param>
        public MSBuildRunner(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner runner,
            IToolLocator tools) : base(fileSystem, environment, runner, tools)
        {
            _fileSystem = fileSystem;
            _environment = environment;
        }

        /// <summary>
        /// Runs MSBuild with the specified settings.
        /// </summary>
        /// <param name="solution">The solution to build.</param>
        /// <param name="settings">The settings.</param>
        public void Run(FilePath solution, MSBuildSettings settings)
        {
            Run(settings, GetArguments(solution, settings));
        }

        private ProcessArgumentBuilder GetArguments(FilePath solution, MSBuildSettings settings)
        {
            var builder = new ProcessArgumentBuilder();
            builder.Append(string.Format(CultureInfo.InvariantCulture, $"/v:{settings.Verbosity}"));

            // Build for a specific platform?
            if (settings.PlatformTarget.HasValue)
            {
                var platform = settings.PlatformTarget.Value;
                bool isSolution = string.Equals(solution.GetExtension(), ".sln", StringComparison.OrdinalIgnoreCase);
                builder.Append(string.Concat("/property:Platform=", GetPlatformName(platform, isSolution)));
            }

            builder.AppendMSBuildSettings(settings, _environment);

            // Got any targets?
            if (settings.Targets.Count <= 0)
            {
                // Should use implicit target?
                if (!settings.NoImplicitTarget.GetValueOrDefault())
                {
                    // Use default target.
                    builder.Append("/target:Build");
                }
            }

            builder.AppendQuoted(solution.MakeAbsolute(_environment).FullPath);

            return builder;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "MSBuild";
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
        protected override IEnumerable<FilePath> GetAlternativeToolPaths(MSBuildSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var buildPlatform = settings.MSBuildPlatform;

            // If we haven't explicitly set an MSBuild target then use the Platform Target
            if (buildPlatform == MSBuildPlatform.Automatic)
            {
                switch (settings.PlatformTarget)
                {
                    case PlatformTarget.x86:
                        buildPlatform = MSBuildPlatform.x86;
                        break;
                    case PlatformTarget.x64:
                        buildPlatform = MSBuildPlatform.x64;
                        break;
                }
            }

            var path = MSBuildResolver.GetMSBuildPath(_fileSystem, _environment, settings.ToolVersion, buildPlatform);
            if (path != null)
            {
                return new[] { path };
            }

            return Enumerable.Empty<FilePath>();
        }

        private static string GetPlatformName(PlatformTarget platform, bool isSolution)
        {
            switch (platform)
            {
                case PlatformTarget.MSIL:
                    // Solutions expect "Any CPU", but projects expect "AnyCPU"
                    return isSolution ? "\"Any CPU\"" : "AnyCPU";
                case PlatformTarget.x86:
                    return "x86";
                case PlatformTarget.x64:
                    return "x64";
                case PlatformTarget.ARM:
                    return "arm";
                case PlatformTarget.ARM64:
                    return "arm64";
                case PlatformTarget.Win32:
                    return "Win32";
                default:
                    throw new ArgumentOutOfRangeException(nameof(platform), platform, "Invalid platform");
            }
        }
    }
}