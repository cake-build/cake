// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.IO;

namespace Cake.Common.Build.TeamCity
{
    /// <summary>
    /// Represents a TeamCity provider.
    /// </summary>
    public interface ITeamCityProvider
    {
        /// <summary>
        /// Report a build problem to TeamCity.
        /// </summary>
        /// <param name="description">Description of build problem.</param>
        /// <param name="identity">Build identity.</param>
        void BuildProblem(string description, string identity);

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
        /// Gets a value indicating whether the current build is running on TeamCity.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on TeamCity; otherwise, <c>false</c>.
        /// </value>
        bool IsRunningOnTeamCity { get; }

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
