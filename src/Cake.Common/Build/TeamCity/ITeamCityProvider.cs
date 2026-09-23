// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.TeamCity.Data;
using Cake.Core.IO;

namespace Cake.Common.Build.TeamCity
{
    /// <summary>
    /// Represents a TeamCity provider.
    /// </summary>
    public interface ITeamCityProvider
    {
        /// <summary>
        /// Gets a value indicating whether the current build is running on TeamCity.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on TeamCity; otherwise, <c>false</c>.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TeamCity.IsRunningOnTeamCity)
        /// {
        ///     Information("Running on TeamCity");
        /// }
        /// else
        /// {
        ///     Information("Not running on TeamCity");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TeamCity.</para>
        /// <example>
        /// <code>
        /// if (TeamCity.IsRunningOnTeamCity)
        /// {
        ///     Information("Running on TeamCity");
        /// }
        /// else
        /// {
        ///     Information("Not running on TeamCity");
        /// }
        /// </code>
        /// </example>
        bool IsRunningOnTeamCity { get; }

        /// <summary>
        /// Gets the TeamCity environment.
        /// </summary>
        /// <value>
        /// The TeamCity environment.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TeamCity.IsRunningOnTeamCity)
        /// {
        ///     Information(
        ///         @"Environment:
        ///         PullRequest: {0}
        ///         Build Configuration Name: {1}
        ///         TeamCity Project Name: {2}",
        ///         BuildSystem.TeamCity.Environment.PullRequest.IsPullRequest,
        ///         BuildSystem.TeamCity.Environment.Build.BuildConfName,
        ///         BuildSystem.TeamCity.Environment.Project.Name
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TeamCity");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TeamCity.</para>
        /// <example>
        /// <code>
        /// if (TeamCity.IsRunningOnTeamCity)
        /// {
        ///     Information(
        ///         @"Environment:
        ///         PullRequest: {0}
        ///         Build Configuration Name: {1}
        ///         TeamCity Project Name: {2}",
        ///         BuildSystem.TeamCity.Environment.PullRequest.IsPullRequest,
        ///         BuildSystem.TeamCity.Environment.Build.BuildConfName,
        ///         BuildSystem.TeamCity.Environment.Project.Name
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TeamCity");
        /// }
        /// </code>
        /// </example>
        TeamCityEnvironmentInfo Environment { get; }

        /// <summary>
        /// Report a build problem to TeamCity.
        /// </summary>
        /// <param name="description">A human-readable plain text describing the build problem. By default, the description appears in the build status text and in the list of build's problems. The text is limited to 4000 symbols, and will be truncated if the limit is exceeded.</param>
        /// <param name="identity">A unique problem ID (optional). Different problems must have different identity, same problems - same identity, which should not change throughout builds if the same problem, for example, the same compilation error occurs. It must be a valid Java ID up to 60 characters. If omitted, the identity is calculated based on the description text.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TeamCity.IsRunningOnTeamCity)
        /// {
        ///     BuildSystem.TeamCity.BuildProblem("Compilation failed", "CompileError");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TeamCity.</para>
        /// <example>
        /// <code>
        /// if (TeamCity.IsRunningOnTeamCity)
        /// {
        ///     TeamCity.BuildProblem("Compilation failed", "CompileError");
        /// }
        /// </code>
        /// </example>
        void BuildProblem(string description, string identity = null);

        /// <summary>
        /// Tell TeamCity to import data of a given type.
        /// </summary>
        /// <param name="type">Date type.</param>
        /// <param name="path">Data file path.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TeamCity.IsRunningOnTeamCity)
        /// {
        ///     BuildSystem.TeamCity.ImportData("junit", "./test-results.xml");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TeamCity.</para>
        /// <example>
        /// <code>
        /// if (TeamCity.IsRunningOnTeamCity)
        /// {
        ///     TeamCity.ImportData("junit", "./test-results.xml");
        /// }
        /// </code>
        /// </example>
        void ImportData(string type, FilePath path);

        /// <summary>
        /// Tell TeamCity to import coverage from dotCover snapshot file.
        /// </summary>
        /// <param name="snapshotFile">Snapshot file path.</param>
        /// <param name="dotCoverHome">The full path to the dotCover home folder to override the bundled dotCover.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TeamCity.IsRunningOnTeamCity)
        /// {
        ///     BuildSystem.TeamCity.ImportDotCoverCoverage("./coverage.dcvr");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TeamCity.</para>
        /// <example>
        /// <code>
        /// if (TeamCity.IsRunningOnTeamCity)
        /// {
        ///     TeamCity.ImportDotCoverCoverage("./coverage.dcvr");
        /// }
        /// </code>
        /// </example>
        void ImportDotCoverCoverage(FilePath snapshotFile, DirectoryPath dotCoverHome = null);

        /// <summary>
        /// Tells TeamCity to publish artifacts in the given directory.
        /// </summary>
        /// <param name="path">Path to artifacts.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TeamCity.IsRunningOnTeamCity)
        /// {
        ///     BuildSystem.TeamCity.PublishArtifacts("./artifacts");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TeamCity.</para>
        /// <example>
        /// <code>
        /// if (TeamCity.IsRunningOnTeamCity)
        /// {
        ///     TeamCity.PublishArtifacts("./artifacts");
        /// }
        /// </code>
        /// </example>
        void PublishArtifacts(string path);

        /// <summary>
        /// Tells TeamCity to change the current build number.
        /// </summary>
        /// <param name="buildNumber">The required build number.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TeamCity.IsRunningOnTeamCity)
        /// {
        ///     BuildSystem.TeamCity.SetBuildNumber("1.2.3.4");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TeamCity.</para>
        /// <example>
        /// <code>
        /// if (TeamCity.IsRunningOnTeamCity)
        /// {
        ///     TeamCity.SetBuildNumber("1.2.3.4");
        /// }
        /// </code>
        /// </example>
        void SetBuildNumber(string buildNumber);

        /// <summary>
        /// Tells TeamCity to set a named parameter with a given value.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to set.</param>
        /// <param name="parameterValue">The value to set for the named parameter.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TeamCity.IsRunningOnTeamCity)
        /// {
        ///     BuildSystem.TeamCity.SetParameter("env.MyVar", "value");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TeamCity.</para>
        /// <example>
        /// <code>
        /// if (TeamCity.IsRunningOnTeamCity)
        /// {
        ///     TeamCity.SetParameter("env.MyVar", "value");
        /// }
        /// </code>
        /// </example>
        void SetParameter(string parameterName, string parameterValue);

        /// <summary>
        /// Write the end of a message block to the TeamCity build log.
        /// </summary>
        /// <param name="blockName">Block name.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TeamCity.IsRunningOnTeamCity)
        /// {
        ///     BuildSystem.TeamCity.WriteEndBlock("Restore");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TeamCity.</para>
        /// <example>
        /// <code>
        /// if (TeamCity.IsRunningOnTeamCity)
        /// {
        ///     TeamCity.WriteEndBlock("Restore");
        /// }
        /// </code>
        /// </example>
        void WriteEndBlock(string blockName);

        /// <summary>
        /// Write the end of a build block to the TeamCity build log.
        /// </summary>
        /// <param name="compilerName">Build compiler name.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TeamCity.IsRunningOnTeamCity)
        /// {
        ///     BuildSystem.TeamCity.WriteEndBuildBlock("dotnet");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TeamCity.</para>
        /// <example>
        /// <code>
        /// if (TeamCity.IsRunningOnTeamCity)
        /// {
        ///     TeamCity.WriteEndBuildBlock("dotnet");
        /// }
        /// </code>
        /// </example>
        void WriteEndBuildBlock(string compilerName);

        /// <summary>
        /// Write a progressFinish message to the TeamCity build log.
        /// </summary>
        /// <param name="message">Build log message.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TeamCity.IsRunningOnTeamCity)
        /// {
        ///     BuildSystem.TeamCity.WriteEndProgress("Restore finished");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TeamCity.</para>
        /// <example>
        /// <code>
        /// if (TeamCity.IsRunningOnTeamCity)
        /// {
        ///     TeamCity.WriteEndProgress("Restore finished");
        /// }
        /// </code>
        /// </example>
        void WriteEndProgress(string message);

        /// <summary>
        /// Write a progress message to the TeamCity build log.
        /// </summary>
        /// <param name="message">Build log message.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TeamCity.IsRunningOnTeamCity)
        /// {
        ///     BuildSystem.TeamCity.WriteProgressMessage("Doing an action...");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TeamCity.</para>
        /// <example>
        /// <code>
        /// if (TeamCity.IsRunningOnTeamCity)
        /// {
        ///     TeamCity.WriteProgressMessage("Doing an action...");
        /// }
        /// </code>
        /// </example>
        void WriteProgressMessage(string message);

        /// <summary>
        /// Write the start of a message block to the TeamCity build log.
        /// </summary>
        /// <param name="blockName">Block name.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TeamCity.IsRunningOnTeamCity)
        /// {
        ///     BuildSystem.TeamCity.WriteStartBlock("Restore");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TeamCity.</para>
        /// <example>
        /// <code>
        /// if (TeamCity.IsRunningOnTeamCity)
        /// {
        ///     TeamCity.WriteStartBlock("Restore");
        /// }
        /// </code>
        /// </example>
        void WriteStartBlock(string blockName);

        /// <summary>
        /// Write the start of a build block to the TeamCity build log.
        /// </summary>
        /// <param name="compilerName">Build compiler name.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TeamCity.IsRunningOnTeamCity)
        /// {
        ///     BuildSystem.TeamCity.WriteStartBuildBlock("dotnet");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TeamCity.</para>
        /// <example>
        /// <code>
        /// if (TeamCity.IsRunningOnTeamCity)
        /// {
        ///     TeamCity.WriteStartBuildBlock("dotnet");
        /// }
        /// </code>
        /// </example>
        void WriteStartBuildBlock(string compilerName);

        /// <summary>
        /// Write a progressStart message to the TeamCity build log.
        /// </summary>
        /// <param name="message">Build log message.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TeamCity.IsRunningOnTeamCity)
        /// {
        ///     BuildSystem.TeamCity.WriteStartProgress("Restore started");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TeamCity.</para>
        /// <example>
        /// <code>
        /// if (TeamCity.IsRunningOnTeamCity)
        /// {
        ///     TeamCity.WriteStartProgress("Restore started");
        /// }
        /// </code>
        /// </example>
        void WriteStartProgress(string message);

        /// <summary>
        /// Write a message to the TeamCity build log. - Messages not added to the build status.
        /// </summary>
        /// <param name="message">Message contents.</param>
        /// <param name="status">Build status.</param>
        /// <param name="errorDetails">Error details if status is error.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TeamCity.IsRunningOnTeamCity)
        /// {
        ///     BuildSystem.TeamCity.WriteStatus("Tests failed", "ERROR", "Assertion failed");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TeamCity.</para>
        /// <example>
        /// <code>
        /// if (TeamCity.IsRunningOnTeamCity)
        /// {
        ///     TeamCity.WriteStatus("Tests failed", "ERROR", "Assertion failed");
        /// }
        /// </code>
        /// </example>
        void WriteStatus(string message, string status = "NORMAL", string errorDetails = null);

        /// <summary>
        /// Write a status message to the TeamCity build log. - Prepend message to build status.
        /// </summary>
        /// <param name="message">Message contents.</param>
        /// <param name="status">Build status.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TeamCity.IsRunningOnTeamCity)
        /// {
        ///     BuildSystem.TeamCity.WritePrependBuildStatus("Tests running");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TeamCity.</para>
        /// <example>
        /// <code>
        /// if (TeamCity.IsRunningOnTeamCity)
        /// {
        ///     TeamCity.WritePrependBuildStatus("Tests running");
        /// }
        /// </code>
        /// </example>
        void WritePrependBuildStatus(string message, string status = null);

        /// <summary>
        /// Write a status message to the TeamCity build log. - append message to build status.
        /// </summary>
        /// <param name="message">Message contents.</param>
        /// <param name="status">Build status.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TeamCity.IsRunningOnTeamCity)
        /// {
        ///     BuildSystem.TeamCity.WriteAppendBuildStatus("Packaging");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TeamCity.</para>
        /// <example>
        /// <code>
        /// if (TeamCity.IsRunningOnTeamCity)
        /// {
        ///     TeamCity.WriteAppendBuildStatus("Packaging");
        /// }
        /// </code>
        /// </example>
        void WriteAppendBuildStatus(string message, string status = null);

        /// <summary>
        /// Write a status message to the TeamCity build log. - replace existing build status.
        /// </summary>
        /// <param name="message">Message contents.</param>
        /// <param name="status">Build status.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TeamCity.IsRunningOnTeamCity)
        /// {
        ///     BuildSystem.TeamCity.WriteReplacementBuildStatus("Build succeeded");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TeamCity.</para>
        /// <example>
        /// <code>
        /// if (TeamCity.IsRunningOnTeamCity)
        /// {
        ///     TeamCity.WriteReplacementBuildStatus("Build succeeded");
        /// }
        /// </code>
        /// </example>
        void WriteReplacementBuildStatus(string message, string status = null);

        /// <summary>
        /// Write a statistic message to the TeamCity build log.
        /// </summary>
        /// <param name="key">The statistic key.</param>
        /// <param name="value">The statistic value.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TeamCity.IsRunningOnTeamCity)
        /// {
        ///     BuildSystem.TeamCity.WriteStatistic("ArtifactsSize", "12345");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TeamCity.</para>
        /// <example>
        /// <code>
        /// if (TeamCity.IsRunningOnTeamCity)
        /// {
        ///     TeamCity.WriteStatistic("ArtifactsSize", "12345");
        /// }
        /// </code>
        /// </example>
        void WriteStatistic(string key, string value);
    }
}