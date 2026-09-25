// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.Jenkins.Data;

namespace Cake.Common.Build.Jenkins;

/// <summary>
/// Represents a Jenkins Provider.
/// </summary>
public interface IJenkinsProvider
{
    /// <summary>
    /// Gets a value indicating whether this instance is running on jenkins.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is running on jenkins; otherwise, <c>false</c>.
    /// </value>
    /// <para>Via BuildSystem.</para>
    /// <example>
    /// <code>
    /// if (BuildSystem.Jenkins.IsRunningOnJenkins)
    /// {
    ///     Information("Running on Jenkins");
    /// }
    /// else
    /// {
    ///     Information("Not running on Jenkins");
    /// }
    /// </code>
    /// </example>
    /// <para>Via Jenkins.</para>
    /// <example>
    /// <code>
    /// if (Jenkins.IsRunningOnJenkins)
    /// {
    ///     Information("Running on Jenkins");
    /// }
    /// else
    /// {
    ///     Information("Not running on Jenkins");
    /// }
    /// </code>
    /// </example>
    bool IsRunningOnJenkins { get; }

    /// <summary>
    /// Gets the Jenkins environment.
    /// </summary>
    /// <value>
    /// The Jenkins environment.
    /// </value>
    /// <para>Via BuildSystem.</para>
    /// <example>
    /// <code>
    /// if (BuildSystem.Jenkins.IsRunningOnJenkins)
    /// {
    ///     var jobName = BuildSystem.Jenkins.Environment.Job.JobName;
    /// }
    /// </code>
    /// </example>
    /// <para>Via Jenkins.</para>
    /// <example>
    /// <code>
    /// if (Jenkins.IsRunningOnJenkins)
    /// {
    ///     var jobName = Jenkins.Environment.Job.JobName;
    /// }
    /// </code>
    /// </example>
    JenkinsEnvironmentInfo Environment { get; }
}
