// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.MSBuild
{
    /// <summary>
    /// The MSBuild runner.
    /// </summary>
    public sealed class MSBuildRunner : Tool<MSBuildSettings>
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

            // Set the maximum number of processors.
            builder.Append(settings.MaxCpuCount > 0 ? string.Concat("/m:", settings.MaxCpuCount) : "/m");

            // Set the verbosity.
            builder.Append(string.Format(CultureInfo.InvariantCulture, "/v:{0}", GetVerbosityName(settings.Verbosity)));

            if (settings.NodeReuse != null)
            {
                builder.Append(string.Concat("/nr:", settings.NodeReuse.Value ? "true" : "false"));
            }

            // Got a specific configuration in mind?
            if (!string.IsNullOrWhiteSpace(settings.Configuration))
            {
                // Add the configuration as a property.
                builder.AppendSwitchQuoted("/p:Configuration", "=", settings.Configuration);
            }

            // Build for a specific platform?
            if (settings.PlatformTarget.HasValue)
            {
                var platform = settings.PlatformTarget.Value;
                bool isSolution = string.Equals(solution.GetExtension(), ".sln", StringComparison.OrdinalIgnoreCase);
                builder.Append(string.Concat("/p:Platform=", GetPlatformName(platform, isSolution)));
            }

            // Got any properties?
            if (settings.Properties.Count > 0)
            {
                foreach (var property in GetPropertyArguments(settings.Properties))
                {
                    builder.Append(property);
                }
            }

            // Got any targets?
            if (settings.Targets.Count > 0)
            {
                var targets = string.Join(";", settings.Targets);
                builder.Append(string.Concat("/target:", targets));
            }
            else
            {
                // Use default target.
                builder.Append("/target:Build");
            }

            // Add the solution as the last parameter.
            builder.AppendQuoted(solution.MakeAbsolute(_environment).FullPath);

            return builder;
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
                case PlatformTarget.Win32:
                    return "Win32";
                default:
                    throw new ArgumentOutOfRangeException("platform", platform, "Invalid platform");
            }
        }

        private static string GetVerbosityName(Verbosity verbosity)
        {
            switch (verbosity)
            {
                case Verbosity.Quiet:
                    return "quiet";
                case Verbosity.Minimal:
                    return "minimal";
                case Verbosity.Normal:
                    return "normal";
                case Verbosity.Verbose:
                    return "detailed";
                case Verbosity.Diagnostic:
                    return "diagnostic";
            }
            throw new CakeException("Encountered unknown MSBuild build log verbosity.");
        }

        private static IEnumerable<string> GetPropertyArguments(IDictionary<string, IList<string>> properties)
        {
            foreach (var propertyKey in properties.Keys)
            {
                foreach (var propertyValue in properties[propertyKey])
                {
                    yield return string.Concat("/p:", propertyKey, "=", propertyValue);
                }
            }
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
                throw new ArgumentNullException("settings");
            }

            MSBuildPlatform buildPlatform = settings.MSBuildPlatform;

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
    }
}
