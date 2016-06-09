// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.InspectCode
{
    /// <summary>
    /// Contains settings used by <see cref="InspectCodeRunner" />.
    /// </summary>
    public sealed class InspectCodeSettings : ToolSettings
    {
        /*
        Not (yet) supported options:
        - /toolset                  MsBuild toolset version. Highest available is used by default. Example: /toolset=12.0.
        - /dumpIssuesTypes (/it)    Dump issues types (default: False).
        - /targets-for-references   MSBuild targets. These targets will be executed to get referenced assemblies of projects..
        - /targets-for-items        MSBuild targets. These targets will be executed to get other items (e.g. Compile item) of projects..
         */

        /// <summary>
        /// Gets or sets the location InspectCode should write its output.
        /// </summary>
        /// <value>The location that InspectCode should write its output</value>
        public FilePath OutputFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether enable solution-wide analysis should be forced.
        /// Default value is <c>false</c>.
        /// </summary>
        /// <value>
        ///     <c>true</c> if solution-wide analysis should be enabled by force; otherwise, <c>fault</c>.
        /// </value>
        public bool SolutionWideAnalysis { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether disable solution-wide analysis should be forced.
        /// Default value is <c>false</c>
        /// </summary>
        /// <value>
        ///     <c>true</c> if solution-wide analysis should be disabled by force; otherwise, <c>fault</c>.
        /// </value>
        public bool NoSolutionWideAnalysis { get; set; }

        /// <summary>
        /// Gets or sets a filter to analyze only particular project(s) instead of the whole solution.
        /// Supports wildcards.
        /// </summary>
        /// <value>The filter to analyze only particular projects(s).</value>
        public string ProjectFilter { get; set; }

        /// <summary>
        /// Gets or sets MSBuild properties.
        /// </summary>
        /// <value>The MSBuild properties to override</value>
        public Dictionary<string, string> MsBuildProperties { get; set; }

        /// <summary>
        /// Gets or sets a list of Resharper extensions which will be used.
        /// </summary>
        public string[] Extensions { get; set; }

        /// <summary>
        /// Gets or sets the directory where caches will be stored.
        /// The default is %TEMP%.
        /// </summary>
        /// <value>The directory where caches will be stored.</value>
        public DirectoryPath CachesHome { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the debug output should be enabled.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the debug output should be enabled; otherwise, <c>false</c>.
        /// </value>
        public bool Debug { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether all global, solution and project settings should be ignored.
        /// Alias for disabling the settings layers GlobalAll, GlobalPerProduct, SolutionShared, SolutionPersonal, ProjectShared and ProjectPersonal.
        /// </summary>
        public bool NoBuildinSettings { get; set; }

        /// <summary>
        /// Gets or sets a list of <c>InspectCodeSettings</c> which will be disabled.
        /// </summary>
        public SettingsLayer[] DisabledSettingsLayers { get; set; }

        /// <summary>
        /// Gets or sets the path to the file to use custom settings from.
        /// </summary>
        public FilePath Profile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to throw an exception on finding violations
        /// </summary>
        public bool ThrowExceptionOnFindingViolations { get; set; }
    }
}
