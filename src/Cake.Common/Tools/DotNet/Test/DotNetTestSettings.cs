// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tools.DotNet.MSBuild;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNet.Test;

/// <summary>
/// Contains settings used by <see cref="DotNetTester" />.
/// </summary>
public class DotNetTestSettings : DotNetSettings
{
    /// <summary>
    /// Gets or sets the settings file to use when running tests.
    /// </summary>
    public FilePath Settings { get; set; }

    /// <summary>
    /// Gets or sets the filter expression to filter out tests in the current project.
    /// </summary>
    /// <remarks>
    /// For more information on filtering support, see https://aka.ms/vstest-filtering.
    /// </remarks>
    public string Filter { get; set; }

    /// <summary>
    /// Gets or sets the path to use for the custom test adapter in the test run.
    /// </summary>
    public DirectoryPath TestAdapterPath { get; set; }

    /// <summary>
    /// Gets or sets the loggers for test results.
    /// </summary>
    public ICollection<string> Loggers { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the output directory.
    /// </summary>
    public DirectoryPath OutputDirectory { get; set; }

    /// <summary>
    /// Gets or sets the configuration under which to build.
    /// </summary>
    public string Configuration { get; set; }

    /// <summary>
    /// Gets or sets the data collectors for the test run.
    /// </summary>
    public ICollection<string> Collectors { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets specific framework to compile.
    /// </summary>
    public string Framework { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to not build the project before testing.
    /// </summary>
    public bool NoBuild { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to not do implicit NuGet package restore.
    /// This makes build faster, but requires restore to be done before build is executed.
    /// </summary>
    /// <remarks>
    /// Requires .NET Core 2.x or newer.
    /// </remarks>
    public bool NoRestore { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to run tests without displaying the Microsoft TestPlatform banner.
    /// </summary>
    /// <remarks>
    /// Available since .NET Core 3.0 SDK.
    /// </remarks>
    public bool NoLogo { get; set; }

    /// <summary>
    /// Gets or sets a file to write diagnostic messages to.
    /// </summary>
    public FilePath DiagnosticFile { get; set; }

    /// <summary>
    /// Gets or sets the results directory. This setting is only available from 2.0.0 upward.
    /// </summary>
    public DirectoryPath ResultsDirectory { get; set; }

    /// <summary>
    /// Gets or sets the file path to write VSTest reports to.
    /// </summary>
    public FilePath VSTestReportPath { get; set; }

    /// <summary>
    /// Gets or sets the target runtime to test for. This setting is only available from .NET Core 3.x upward.
    /// </summary>
    public string Runtime { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to run the tests in blame mode. This option is helpful in isolating a problematic test causing the test host to crash.
    /// Outputs a 'Sequence.xml' file in the current directory that captures the order of execution of test before the crash.
    /// </summary>
    public bool Blame { get; set; }

    /// <summary>
    /// Gets or sets the specified NuGet package sources to use during testing.
    /// </summary>
    /// <remarks>
    /// Requires .NET Core 2.x or newer.
    /// </remarks>
    public ICollection<string> Sources { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets additional arguments to be passed to MSBuild.
    /// </summary>
    public DotNetMSBuildSettings MSBuildSettings { get; set; }

    /// <summary>
    /// Gets or sets the complete-run timeout passed as <c>--timeout</c>.
    /// </summary>
    /// <remarks>
    /// Use a positive number and unit, such as <c>500ms</c>, <c>90s</c>, <c>10m</c>, <c>2h</c>, or <c>1d</c>.
    /// A timed-out run exits with code 3.
    /// Requires .NET 11 Preview 7 or newer and MTP 2.4 or later.
    /// </remarks>
    public string Timeout { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of failed tests passed as <c>--maximum-failed-tests</c>.
    /// </summary>
    /// <remarks>
    /// Stops the complete run after it reaches this number of failed, errored, timed-out, or canceled tests.
    /// The run exits with code 13.
    /// Requires .NET 11 Preview 7 or newer and MTP 2.4 or later.
    /// </remarks>
    public int? MaximumFailedTests { get; set; }

    /// <summary>
    /// Gets or sets the artifacts path.
    /// </summary>
    /// <remarks>
    /// Requires .NET 11 SDK or newer.
    /// </remarks>
    public DirectoryPath ArtifactsPath { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to ignore project-to-project references and only build the specified root project.
    /// </summary>
    /// <remarks>
    /// Requires .NET 11 SDK or newer.
    /// </remarks>
    public bool NoDependencies { get; set; }

    /// <summary>
    /// Gets or sets file-globbing patterns passed as <c>--test-modules</c>.
    /// </summary>
    /// <remarks>
    /// Only tests in matching modules run. Prefix a pattern with <c>!</c> to exclude matches.
    /// Multiple patterns are joined with semicolons. Whitespace around each pattern is ignored.
    /// Because this option does not evaluate projects, use it to run already-built test applications
    /// when project restore state is not available.
    /// Requires .NET 11 Preview 6 or newer for exclusion prefixes.
    /// </remarks>
    public ICollection<string> TestModules { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets application environment variables passed as <c>--environment KEY=VALUE</c>.
    /// </summary>
    /// <remarks>
    /// These are not process environment variables. Use <c>ToolSettings.EnvironmentVariables</c> for that.
    /// Requires .NET 11 SDK or newer.
    /// </remarks>
    public IDictionary<string, string> ApplicationEnvironment { get; set; } = new Dictionary<string, string>();

    /// <summary>
    /// Gets or sets the Microsoft Testing Platform configuration file.
    /// </summary>
    /// <remarks>
    /// Maps to <c>--config-file</c>, not <see cref="Settings"/> / <c>--settings</c>.
    /// Requires .NET 11 SDK or newer.
    /// </remarks>
    public FilePath ConfigFile { get; set; }

    /// <summary>
    /// Gets or sets the path type for the test command.
    /// When set to <see cref="DotNetTestPathType.Auto"/>, the path type will be automatically detected based on the file extension.
    /// When set to <see cref="DotNetTestPathType.Project"/>, the path will be treated as a project file.
    /// When set to <see cref="DotNetTestPathType.Solution"/>, the path will be treated as a solution file.
    /// This is particularly useful for .NET 10+ where explicit --project or --solution parameters are required.
    /// </summary>
    public DotNetTestPathType PathType { get; set; } = DotNetTestPathType.None;
}
