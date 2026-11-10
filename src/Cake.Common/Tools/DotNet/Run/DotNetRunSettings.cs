// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tools.DotNet.MSBuild;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNet.Run;

/// <summary>
/// Contains settings used by <see cref="DotNetRunner" />.
/// </summary>
public class DotNetRunSettings : DotNetSettings
{
    /// <summary>
    /// Gets or sets a specific framework to compile.
    /// </summary>
    public string Framework { get; set; }

    /// <summary>
    /// Gets or sets the configuration under which to build.
    /// </summary>
    public string Configuration { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to not do implicit NuGet package restore.
    /// This makes run faster, but requires restore to be done before run is executed.
    /// </summary>
    public bool NoRestore { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to not do implicit build.
    /// This makes run faster, but requires build to be done before run is executed.
    /// </summary>
    public bool NoBuild { get; set; }

    /// <summary>
    /// Gets or sets the specified NuGet package sources to use during the run is executed.
    /// </summary>
    /// <remarks>
    /// Requires .NET Core 2.x or newer.
    /// </remarks>
    public ICollection<string> Sources { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the target runtime.
    /// </summary>
    public string Runtime { get; set; }

    /// <summary>
    /// Gets or sets additional arguments to be passed to MSBuild.
    /// </summary>
    public DotNetMSBuildSettings MSBuildSettings { get; set; }

    /// <summary>
    /// Gets or sets application environment variables passed as <c>--environment KEY=VALUE</c>.
    /// </summary>
    /// <remarks>
    /// These are not process environment variables. Use <c>ToolSettings.EnvironmentVariables</c> for that.
    /// Requires .NET 11 SDK or newer.
    /// </remarks>
    public IDictionary<string, string> ApplicationEnvironment { get; set; } = new Dictionary<string, string>();

    /// <summary>
    /// Gets or sets the file to run.
    /// </summary>
    /// <remarks>
    /// Requires .NET 11 SDK or newer.
    /// </remarks>
    public FilePath File { get; set; }

    /// <summary>
    /// Gets or sets the target device.
    /// </summary>
    /// <remarks>
    /// Requires .NET 11 SDK or newer.
    /// </remarks>
    public string Device { get; set; }

    /// <summary>
    /// Gets or sets the artifacts path.
    /// </summary>
    /// <remarks>
    /// Requires .NET 11 SDK or newer.
    /// </remarks>
    public DirectoryPath ArtifactsPath { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to skip the local cache.
    /// </summary>
    /// <remarks>
    /// Requires .NET 11 SDK or newer.
    /// </remarks>
    public bool NoCache { get; set; }
}
