// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.MSBuild
{
    /// <summary>
    /// The MSBuild runner.
    /// </summary>
    public sealed class MSBuildRunner : Tool<MSBuildSettings>
    {
        private const string MSBuildExecutableName = "msbuild.exe";
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

            // Set the maximum number of processors?
            if (settings.MaxCpuCount != null)
            {
                builder.Append(settings.MaxCpuCount > 0 ? string.Concat("/m:", settings.MaxCpuCount) : "/m");
            }

            // Set the detailed summary flag.
            if (settings.DetailedSummary.GetValueOrDefault())
            {
                builder.Append("/ds");
            }

            // Set the no console logger flag.
            if (settings.NoConsoleLogger.GetValueOrDefault())
            {
                builder.Append("/noconlog");
            }

            // Set the verbosity.
            builder.Append(string.Format(CultureInfo.InvariantCulture, "/v:{0}", settings.Verbosity.GetMSBuildVerbosityName()));

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

            if (settings.Loggers.Count > 0)
            {
                foreach (var logger in settings.Loggers)
                {
                    var argument = GetLoggerArgument(logger);
                    builder.Append(argument);
                }
            }

            // Got any file loggers?
            if (settings.FileLoggers.Count > 0)
            {
                var arguments = settings.FileLoggers.Select((logger, index) => GetLoggerArgument(index, logger));

                foreach (var argument in arguments)
                {
                    builder.Append(argument);
                }
            }

            // Use binary logging?
            if (settings.BinaryLogger != null && settings.BinaryLogger.Enabled)
            {
                string binaryOptions = null;
                if (!string.IsNullOrEmpty(settings.BinaryLogger.FileName))
                {
                    binaryOptions = settings.BinaryLogger.FileName;
                }

                if (settings.BinaryLogger.Imports != MSBuildBinaryLogImports.Unspecified)
                {
                    if (!string.IsNullOrEmpty(binaryOptions))
                    {
                        binaryOptions = binaryOptions + ";";
                    }

                    binaryOptions = binaryOptions + "ProjectImports=" + settings.BinaryLogger.Imports;
                }

                if (string.IsNullOrEmpty(binaryOptions))
                {
                    builder.Append("/bl");
                }
                else
                {
                    builder.Append("/bl:" + binaryOptions);
                }
            }

            // Treat errors as warníngs?
            if (settings.WarningsAsErrorCodes.Any())
            {
                var codes = string.Join(";", settings.WarningsAsErrorCodes);
                builder.Append($"/warnaserror:{codes.Quote()}");
            }
            else if (settings.WarningsAsError)
            {
                builder.Append("/warnaserror");
            }

            // Any warnings to NOT treat as errors?
            if (settings.WarningsAsMessageCodes.Any())
            {
                var codes = string.Join(";", settings.WarningsAsMessageCodes);
                builder.Append($"/warnasmessage:{codes.Quote()}");
            }

            // Invoke restore target before any other target?
            if (settings.Restore)
            {
                builder.Append("/restore");
            }

            // Got any console logger parameters?
            if (settings.ConsoleLoggerParameters.Count > 0)
            {
                var argument = "/clp:" + string.Join(";", settings.ConsoleLoggerParameters);
                builder.Append(argument);
            }

            // Add the solution as the last parameter.
            builder.AppendQuoted(solution.MakeAbsolute(_environment).FullPath);

            return builder;
        }

        private string GetLoggerArgument(int index, MSBuildFileLogger logger)
        {
            if (index >= 10)
            {
                throw new InvalidOperationException("Too Many FileLoggers");
            }

            var counter = index == 0 ? string.Empty : index.ToString();
            var argument = $"/fl{counter}";

            var parameters = logger.GetParameters(_environment);
            if (!string.IsNullOrWhiteSpace(parameters))
            {
                argument = $"{argument} /flp{counter}:{parameters}";
            }
            return argument;
        }

        private static string GetLoggerArgument(MSBuildLogger logger)
        {
            var argumentBuilder = new StringBuilder("/logger:");
            if (!string.IsNullOrWhiteSpace(logger.Class))
            {
                argumentBuilder.Append(logger.Class);
                argumentBuilder.Append(",");
            }

            argumentBuilder.Append(logger.Assembly.Quote());

            if (!string.IsNullOrWhiteSpace(logger.Parameters))
            {
                argumentBuilder.Append(";");
                argumentBuilder.Append(logger.Parameters);
            }
            return argumentBuilder.ToString();
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
                    throw new ArgumentOutOfRangeException(nameof(platform), platform, "Invalid platform");
            }
        }

        private static IEnumerable<string> GetPropertyArguments(IDictionary<string, IList<string>> properties)
        {
            foreach (var propertyKey in properties.Keys)
            {
                foreach (var propertyValue in properties[propertyKey])
                {
                    yield return string.Concat("/p:", propertyKey, "=", propertyValue.EscapeMSBuildPropertySpecialCharacters());
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
        protected override IEnumerable<string> GetToolExecutableNames() => new[] { MSBuildExecutableName };

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
    }
}