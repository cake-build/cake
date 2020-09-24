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
        void BuildProblem(string description, string identity = null);

        /// <summary>
        /// Tell TeamCity to import data of a given type.
        /// </summary>
        /// <param name="type">Date type.</param>
        /// <param name="path">Data file path.</param>
        void ImportData(string type, FilePath path);

        /// <summary>
        /// Tell TeamCity to import coverage from dotCover snapshot file.
        /// </summary>
        /// <param name="snapshotFile">Snapshot file path.</param>
        /// <param name="dotCoverHome">The full path to the dotCover home folder to override the bundled dotCover.</param>
        void ImportDotCoverCoverage(FilePath snapshotFile, DirectoryPath dotCoverHome = null);

        /// <summary>
        /// Tells TeamCity to publish artifacts in the given directory.
        /// </summary>
        /// <param name="path">Path to artifacts.</param>
        void PublishArtifacts(string path);

        /// <summary>
        /// Tells TeamCity to change the current build number.
        /// </summary>
        /// <param name="buildNumber">The required build number.</param>
        void SetBuildNumber(string buildNumber);

        /// <summary>
        /// Tells TeamCity to set a named parameter with a given value.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to set.</param>
        /// <param name="parameterValue">The value to set for the named parameter.</param>
        void SetParameter(string parameterName, string parameterValue);

        /// <summary>
        /// Write the end of a message block to the TeamCity build log.
        /// </summary>
        /// <param name="blockName">Block name.</param>
        void WriteEndBlock(string blockName);

        /// <summary>
        /// Write the end of a build block to the TeamCity build log.
        /// </summary>
        /// <param name="compilerName">Build compiler name.</param>
        void WriteEndBuildBlock(string compilerName);

        /// <summary>
        /// Write a progressFinish message to the TeamCity build log.
        /// </summary>
        /// <param name="message">Build log message.</param>
        void WriteEndProgress(string message);

        /// <summary>
        /// Write a progress message to the TeamCity build log.
        /// </summary>
        /// <param name="message">Build log message.</param>
        void WriteProgressMessage(string message);

        /// <summary>
        /// Write the start of a message block to the TeamCity build log.
        /// </summary>
        /// <param name="blockName">Block name.</param>
        void WriteStartBlock(string blockName);

        /// <summary>
        /// Write the start of a build block to the TeamCity build log.
        /// </summary>
        /// <param name="compilerName">Build compiler name.</param>
        void WriteStartBuildBlock(string compilerName);

        /// <summary>
        /// Write a progressStart message to the TeamCity build log.
        /// </summary>
        /// <param name="message">Build log message.</param>
        void WriteStartProgress(string message);

        /// <summary>
        /// Write a status message to the TeamCity build log.
        /// </summary>
        /// <param name="message">Message contents.</param>
        /// <param name="status">Build status.</param>
        /// <param name="errorDetails">Error details if status is error.</param>
        void WriteStatus(string message, string status = "NORMAL", string errorDetails = null);
    }
}