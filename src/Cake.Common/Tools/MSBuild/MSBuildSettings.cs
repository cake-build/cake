// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.MSBuild
{
    /// <summary>
    /// Contains settings used by <see cref="MSBuildRunner"/>.
    /// </summary>
    public sealed class MSBuildSettings : ToolSettings
    {
        private readonly HashSet<string> _targets;
        private readonly Dictionary<string, IList<string>> _properties;
        private readonly List<MSBuildLogger> _loggers;
        private readonly List<MSBuildFileLogger> _fileLoggers;
        private readonly HashSet<string> _warningsAsErrorCodes;
        private readonly HashSet<string> _warningsAsMessageCodes;
        private readonly HashSet<string> _consoleLoggerParameters;
        internal string CustomVersion { get; set; }

        /// <summary>
        /// Gets the targets.
        /// </summary>
        /// <value>The targets.</value>
        public ISet<string> Targets => _targets;

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>The properties.</value>
        public IDictionary<string, IList<string>> Properties => _properties;

        /// <summary>
        /// Gets or sets the platform target.
        /// </summary>
        /// <value>The platform target.</value>
        public PlatformTarget? PlatformTarget { get; set; }

        /// <summary>
        /// Gets or sets the MSBuild platform.
        /// </summary>
        /// <value>The MSBuild platform.</value>
        public MSBuildPlatform MSBuildPlatform { get; set; }

        /// <summary>
        /// Gets or sets the tool version.
        /// </summary>
        /// <value>The tool version.</value>
        public MSBuildToolVersion ToolVersion { get; set; }

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public string Configuration { get; set; }

        /// <summary>
        /// Gets or sets the maximum CPU count.
        /// If this value is zero, MSBuild will use as many processes as
        /// there are available CPUs to build the project. If not set
        /// MSBuild compile projects in this solution one at a time.
        /// </summary>
        /// <value>The maximum CPU count.</value>
        public int? MaxCpuCount { get; set; }

        /// <summary>
        /// Gets or sets whether or not node reuse is used.
        /// When you’re doing multiple builds in a row, this helps reduce your total build time,
        /// by avoiding the start up costs of each MSBuild child node.
        /// </summary>
        public bool? NodeReuse { get; set; }

        /// <summary>
        /// Gets or sets whether or not detailed summary is created.
        /// Shows detailed information at the end of the build
        /// about the configurations built and how they were
        /// scheduled to nodes.
        /// </summary>
        public bool? DetailedSummary { get; set; }

        /// <summary>
        /// Gets or sets whether or not information is logged to the console.
        /// Disable the default console logger and do not log events
        /// to the console.
        /// </summary>
        public bool? NoConsoleLogger { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show copyright information at the start of the program.
        /// </summary>
        public bool? NoLogo { get; set; }

        /// <summary>
        /// Gets or sets the default value of all the version numbers embedded in the build output.
        /// </summary>
        public string Version
        {
            get => GetPropertyValueOrDefault("Version");
            set => this.WithProperty("Version", value);
        }

        /// <summary>
        /// Gets or sets the base version number embedded in the build output.
        /// </summary>
        public string VersionPrefix
        {
            get => GetPropertyValueOrDefault("VersionPrefix");
            set => this.WithProperty("VersionPrefix", value);
        }

        /// <summary>
        /// Gets or sets the pre-release label of the version number embedded in the build output.
        /// </summary>
        public string VersionSuffix
        {
            get => GetPropertyValueOrDefault("VersionSuffix");
            set => this.WithProperty("VersionSuffix", value);
        }

        /// <summary>
        /// Gets or sets the file version number embedded in the build output.
        /// </summary>
        public string FileVersion
        {
            get => GetPropertyValueOrDefault("FileVersion");
            set => this.WithProperty("FileVersion", value);
        }

        /// <summary>
        /// Gets or sets the assembly version number embedded in the build output.
        /// </summary>
        public string AssemblyVersion
        {
            get => GetPropertyValueOrDefault("AssemblyVersion");
            set => this.WithProperty("AssemblyVersion", value);
        }

        /// <summary>
        /// Gets or sets the assembly informational version number embedded in the build output.
        /// </summary>
        public string InformationalVersion
        {
            get => GetPropertyValueOrDefault("InformationalVersion");
            set => this.WithProperty("InformationalVersion", value);
        }

        /// <summary>
        /// Gets or sets the version number of the NuGet package generated.
        /// </summary>
        public string PackageVersion
        {
            get => GetPropertyValueOrDefault("PackageVersion");
            set => this.WithProperty("PackageVersion", value);
        }

        /// <summary>
        /// Gets or sets the release notes of the NuGet package generated.
        /// </summary>
        public string PackageReleaseNotes
        {
            get => GetPropertyValueOrDefault("PackageReleaseNotes");
            set => this.WithProperty("PackageReleaseNotes", value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to normalize stored file paths used when producing deterministic builds.
        /// </summary>
        /// <remarks>
        /// For more information see https://devblogs.microsoft.com/dotnet/producing-packages-with-source-link/#deterministic-builds.
        /// </remarks>
        public bool? ContinuousIntegrationBuild { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether implicit target should be passed to MSBuild.
        /// If set to true, no targets will be specified.
        /// If set to false, and no targets specified, Build target will be passed by default.
        /// </summary>
        public bool? NoImplicitTarget { get; set; }

        /// <summary>
        /// Gets or sets the amount of information to display in the build log.
        /// Each logger displays events based on the verbosity level that you set for that logger.
        /// </summary>
        /// <value>The build log verbosity.</value>
        public Verbosity Verbosity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a symbol package should be created.
        /// </summary>
        public bool? IncludeSymbols { get; set; }

        /// <summary>
        /// Gets or sets the symbol package format.
        /// </summary>
        /// <value>The symbol package format.</value>
        public string SymbolPackageFormat { get; set; }

        /// <summary>
        /// Gets the loggers.
        /// </summary>
        public ICollection<MSBuildLogger> Loggers => _loggers;

        /// <summary>
        /// Gets the file loggers.
        /// </summary>
        public ICollection<MSBuildFileLogger> FileLoggers => _fileLoggers;

        /// <summary>
        /// Gets or sets the binary logging options.
        /// </summary>
        public MSBuildBinaryLogSettings BinaryLogger { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether warnings should be treated as errors.
        /// Treats all warnings as errors unless <see cref="WarningsAsErrorCodes"/> has specific codes specified.
        /// </summary>
        public bool WarningsAsError { get; set; }

        /// <summary>
        /// Gets the warning codes to treat as errors.
        /// If any specified <seealso cref="WarningsAsError"/> will implicitly be treated as true.
        /// </summary>
        public ISet<string> WarningsAsErrorCodes => _warningsAsErrorCodes;

        /// <summary>
        /// Gets the warning codes to NOT treat as errors.
        /// </summary>
        /// <remarks>Only available MSBuild version 15 (VS2017) and newer.</remarks>
        public ISet<string> WarningsAsMessageCodes => _warningsAsMessageCodes;

        /// <summary>
        /// Gets or sets a value indicating whether the Restore target should be run before any other targets.
        /// This setting will pass the /restore option down to MSBuild.
        /// Use this setting when working with the new csproj format.
        /// </summary>
        public bool Restore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to lock the package dependency graph while
        /// restoring, using the packages.lock.json file.
        /// This setting is available with at least Visual Studio 2017 version 15.9 and above or NET SDK version 2.1.500 and above.
        /// </summary>
        public bool? RestoreLockedMode { get; set; }

        /// <summary>
        /// Gets the console logger parameters.
        /// </summary>
        public ISet<string> ConsoleLoggerParameters => _consoleLoggerParameters;

        /// <summary>
        /// Gets or sets a value indicating whether tools from a preview edition of Visual Studio should be used.
        /// <para>
        /// If set to <c>true</c>, MSBuildTools from a Preview edition
        /// (e.g. Visual Studio 2022 Preview) will be considered to be used.
        /// </para>
        /// </summary>
        public bool AllowPreviewVersion { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MSBuildSettings"/> class.
        /// </summary>
        public MSBuildSettings()
        {
            _targets = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _properties = new Dictionary<string, IList<string>>(StringComparer.OrdinalIgnoreCase);
            _loggers = new List<MSBuildLogger>();
            _fileLoggers = new List<MSBuildFileLogger>();
            _warningsAsErrorCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _warningsAsMessageCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _consoleLoggerParameters = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            ToolVersion = MSBuildToolVersion.Default;
            Configuration = string.Empty;
            Verbosity = Verbosity.Normal;
            MSBuildPlatform = MSBuildPlatform.Automatic;
        }

        private string GetPropertyValueOrDefault(string propertyName, string @default = null)
        {
            if (!Properties.TryGetValue(propertyName, out var propertyValues))
            {
                return @default;
            }

            var propertyValue = string.Join(";", propertyValues);
            return propertyValue;
        }
    }
}