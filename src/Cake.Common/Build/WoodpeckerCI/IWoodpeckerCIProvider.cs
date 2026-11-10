// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.WoodpeckerCI.Commands;
using Cake.Common.Build.WoodpeckerCI.Data;

namespace Cake.Common.Build.WoodpeckerCI;

/// <summary>
/// Represents a WoodpeckerCI provider.
/// </summary>
public interface IWoodpeckerCIProvider
{
    /// <summary>
    /// Gets a value indicating whether the current build is running on WoodpeckerCI.
    /// </summary>
    /// <value>
    /// <c>true</c> if the current build is running on WoodpeckerCI; otherwise, <c>false</c>.
    /// </value>
    /// <para>Via BuildSystem.</para>
    /// <example>
    /// <code>
    /// if (BuildSystem.WoodpeckerCI.IsRunningOnWoodpeckerCI)
    /// {
    ///     Information("Running on WoodpeckerCI");
    /// }
    /// else
    /// {
    ///     Information("Not running on WoodpeckerCI");
    /// }
    /// </code>
    /// </example>
    /// <para>Via WoodpeckerCI.</para>
    /// <example>
    /// <code>
    /// if (WoodpeckerCI.IsRunningOnWoodpeckerCI)
    /// {
    ///     Information("Running on WoodpeckerCI");
    /// }
    /// else
    /// {
    ///     Information("Not running on WoodpeckerCI");
    /// }
    /// </code>
    /// </example>
    bool IsRunningOnWoodpeckerCI { get; }

    /// <summary>
    /// Gets the WoodpeckerCI environment.
    /// </summary>
    /// <value>
    /// The WoodpeckerCI environment.
    /// </value>
    /// <para>Via BuildSystem.</para>
    /// <example>
    /// <code>
    /// if (BuildSystem.WoodpeckerCI.IsRunningOnWoodpeckerCI)
    /// {
    ///     var commitSha = BuildSystem.WoodpeckerCI.Environment.Commit.Sha;
    /// }
    /// </code>
    /// </example>
    /// <para>Via WoodpeckerCI.</para>
    /// <example>
    /// <code>
    /// if (WoodpeckerCI.IsRunningOnWoodpeckerCI)
    /// {
    ///     var commitSha = WoodpeckerCI.Environment.Commit.Sha;
    /// }
    /// </code>
    /// </example>
    WoodpeckerCIEnvironmentInfo Environment { get; }

    /// <summary>
    /// Gets the WoodpeckerCI commands.
    /// </summary>
    /// <value>
    /// The WoodpeckerCI commands.
    /// </value>
    /// <para>Via BuildSystem.</para>
    /// <example>
    /// <code>
    /// if (BuildSystem.WoodpeckerCI.IsRunningOnWoodpeckerCI)
    /// {
    ///     BuildSystem.WoodpeckerCI.Commands.SetEnvironmentVariable("MY_VAR", "value");
    /// }
    /// </code>
    /// </example>
    /// <para>Via WoodpeckerCI.</para>
    /// <example>
    /// <code>
    /// if (WoodpeckerCI.IsRunningOnWoodpeckerCI)
    /// {
    ///     WoodpeckerCI.Commands.SetEnvironmentVariable("MY_VAR", "value");
    /// }
    /// </code>
    /// </example>
    WoodpeckerCICommands Commands { get; }
}
