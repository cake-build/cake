// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Build.MyGet;

/// <summary>
/// Represents a MyGet provider.
/// </summary>
public interface IMyGetProvider
{
    /// <summary>
    /// Gets a value indicating whether the current build is running on MyGet.
    /// </summary>
    /// <value>
    /// <c>true</c> if the current build is running on MyGet; otherwise, <c>false</c>.
    /// </value>
    /// <para>Via BuildSystem.</para>
    /// <example>
    /// <code>
    /// if (BuildSystem.MyGet.IsRunningOnMyGet)
    /// {
    ///     Information("Running on MyGet");
    /// }
    /// else
    /// {
    ///     Information("Not running on MyGet");
    /// }
    /// </code>
    /// </example>
    /// <para>Via MyGet.</para>
    /// <example>
    /// <code>
    /// if (MyGet.IsRunningOnMyGet)
    /// {
    ///     Information("Running on MyGet");
    /// }
    /// else
    /// {
    ///     Information("Not running on MyGet");
    /// }
    /// </code>
    /// </example>
    bool IsRunningOnMyGet { get; }

    /// <summary>
    /// Report a build problem to MyGet.
    /// </summary>
    /// <param name="description">Description of build problem.</param>
    /// <para>Via BuildSystem.</para>
    /// <example>
    /// <code>
    /// if (BuildSystem.MyGet.IsRunningOnMyGet)
    /// {
    ///     BuildSystem.MyGet.BuildProblem("Something went wrong...");
    /// }
    /// </code>
    /// </example>
    /// <para>Via MyGet.</para>
    /// <example>
    /// <code>
    /// if (MyGet.IsRunningOnMyGet)
    /// {
    ///     MyGet.BuildProblem("Something went wrong...");
    /// }
    /// </code>
    /// </example>
    void BuildProblem(string description);

    /// <summary>
    /// Allows setting an environment variable that can be used by a future process.
    /// </summary>
    /// <param name="name">Name of the parameter to set.</param>
    /// <param name="value">Value to assign to the parameter.</param>
    /// <para>Via BuildSystem.</para>
    /// <example>
    /// <code>
    /// if (BuildSystem.MyGet.IsRunningOnMyGet)
    /// {
    ///     BuildSystem.MyGet.SetParameter("MyVar", "value");
    /// }
    /// </code>
    /// </example>
    /// <para>Via MyGet.</para>
    /// <example>
    /// <code>
    /// if (MyGet.IsRunningOnMyGet)
    /// {
    ///     MyGet.SetParameter("MyVar", "value");
    /// }
    /// </code>
    /// </example>
    void SetParameter(string name, string value);

    /// <summary>
    /// Write a status message to the MyGet build log.
    /// </summary>
    /// <param name="message">Message contents.</param>
    /// <param name="status">Build status.</param>
    /// <param name="errorDetails">Error details if status is error.</param>
    /// <para>Via BuildSystem.</para>
    /// <example>
    /// <code>
    /// if (BuildSystem.MyGet.IsRunningOnMyGet)
    /// {
    ///     BuildSystem.MyGet.WriteStatus("Build failed", MyGetBuildStatus.Error, "Details");
    /// }
    /// </code>
    /// </example>
    /// <para>Via MyGet.</para>
    /// <example>
    /// <code>
    /// if (MyGet.IsRunningOnMyGet)
    /// {
    ///     MyGet.WriteStatus("Build failed", MyGetBuildStatus.Error, "Details");
    /// }
    /// </code>
    /// </example>
    void WriteStatus(string message, MyGetBuildStatus status, string errorDetails = null);

    /// <summary>
    /// Tells MyGet to change the current build number.
    /// </summary>
    /// <param name="buildNumber">The required build number.</param>
    /// <para>Via BuildSystem.</para>
    /// <example>
    /// <code>
    /// if (BuildSystem.MyGet.IsRunningOnMyGet)
    /// {
    ///     BuildSystem.MyGet.SetBuildNumber("1.2.3.4");
    /// }
    /// </code>
    /// </example>
    /// <para>Via MyGet.</para>
    /// <example>
    /// <code>
    /// if (MyGet.IsRunningOnMyGet)
    /// {
    ///     MyGet.SetBuildNumber("1.2.3.4");
    /// }
    /// </code>
    /// </example>
    void SetBuildNumber(string buildNumber);
}
