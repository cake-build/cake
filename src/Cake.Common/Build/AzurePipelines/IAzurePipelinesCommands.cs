// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Build.AzurePipelines.Data;
using Cake.Core.IO;

namespace Cake.Common.Build.AzurePipelines
{
    /// <summary>
    /// Represents an Azure Pipelines command provider.
    /// </summary>
    public interface IAzurePipelinesCommands
    {
        /// <summary>
        /// Log a warning issue to timeline record of current task.
        /// </summary>
        /// <param name="message">The warning message.</param>
        void WriteWarning(string message);

        /// <summary>
        /// Log a warning issue with detailed data to timeline record of current task.
        /// </summary>
        /// <param name="message">The warning message.</param>
        /// <param name="data">The message data.</param>
        void WriteWarning(string message, AzurePipelinesMessageData data);

        /// <summary>
        /// Log an error to timeline record of current task.
        /// </summary>
        /// <param name="message">The error message.</param>
        void WriteError(string message);

        /// <summary>
        /// Log an error with detailed data to timeline record of current task.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="data">The message data.</param>
        void WriteError(string message, AzurePipelinesMessageData data);

        /// <summary>
        /// Set progress and current operation for current task.
        /// </summary>
        /// <param name="progress">Current progress as percentage.</param>
        /// <param name="currentOperation">The current operation.</param>
        void SetProgress(int progress, string currentOperation);

        /// <summary>
        /// Finish timeline record for current task and set task result to succeeded.
        /// </summary>
        void CompleteCurrentTask();

        /// <summary>
        /// Finish timeline record for current task and set task result.
        /// </summary>
        /// <param name="result">The task result status.</param>
        void CompleteCurrentTask(AzurePipelinesTaskResult result);

        /// <summary>
        /// Create detail timeline record.
        /// </summary>
        /// <param name="name">Name of the new timeline record.</param>
        /// <param name="type">Type of the new timeline record.</param>
        /// <param name="order">Order of the timeline record.</param>
        /// <returns>The timeline record ID.</returns>
        Guid CreateNewRecord(string name, string type, int order);

        /// <summary>
        /// Create detail timeline record.
        /// </summary>
        /// <param name="name">Name of the new timeline record.</param>
        /// <param name="type">Type of the new timeline record.</param>
        /// <param name="order">Order of the timeline record.</param>
        /// <param name="data">Additional data for the new timeline record.</param>
        /// <returns>The timeline record ID.</returns>
        Guid CreateNewRecord(string name, string type, int order, AzurePipelinesRecordData data);

        /// <summary>
        /// Update an existing detail timeline record.
        /// </summary>
        /// <param name="id">The ID of the existing timeline record.</param>
        /// <param name="data">Additional data for the timeline record.</param>
        void UpdateRecord(Guid id, AzurePipelinesRecordData data);

        /// <summary>
        /// Sets a variable in the variable service of the task context.
        /// </summary>
        /// <remarks>
        /// The variable is exposed to following tasks as an environment variable.
        /// </remarks>
        /// <param name="name">The variable name.</param>
        /// <param name="value">The variable value.</param>
        void SetVariable(string name, string value);

        /// <summary>
        /// Sets a output variable in the variable service of the task context.
        /// </summary>
        /// <remarks>
        /// The variable is exposed to following tasks as an environment variable.
        /// </remarks>
        /// <param name="name">The variable name.</param>
        /// <param name="value">The variable value.</param>
        void SetOutputVariable(string name, string value);

        /// <summary>
        /// Sets a secret variable in the variable service of the task context.
        /// </summary>
        /// <remarks>
        /// The variable is not exposed to following tasks as an environment variable, and must be passed as inputs.
        /// </remarks>
        /// <param name="name">The variable name.</param>
        /// <param name="value">The variable value.</param>
        void SetSecretVariable(string name, string value);

        /// <summary>
        /// Upload and attach summary markdown to current timeline record.
        /// </summary>
        /// <remarks>
        /// This summary is added to the build/release summary and is not available for download with logs.
        /// </remarks>
        /// <param name="markdownPath">Path to the summary markdown file.</param>
        void UploadTaskSummary(FilePath markdownPath);

        /// <summary>
        /// Upload file as additional log information to the current timeline record.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The file shall be available for download along with task logs.
        /// </para>
        /// <para>
        /// Requires agent version 1.101.
        /// </para>
        /// </remarks>
        /// <param name="logFile">Path to the additional log file.</param>
        void UploadTaskLogFile(FilePath logFile);

        /// <summary>
        /// Create an artifact link, such as a file or folder path or a version control path.
        /// </summary>
        /// <param name="name">The artifact name.</param>
        /// <param name="type">The artifact type.</param>
        /// <param name="location">The link path or value.</param>
        void LinkArtifact(string name, AzurePipelinesArtifactType type, string location);

        /// <summary>
        /// Upload local file into a file container folder.
        /// </summary>
        /// <param name="folderName">Folder that the file will upload to.</param>
        /// <param name="file">Path to the local file.</param>
        void UploadArtifact(string folderName, FilePath file);

        /// <summary>
        /// Upload local file into a file container folder, and create an artifact.
        /// </summary>
        /// <param name="folderName">Folder that the file will upload to.</param>
        /// <param name="file">Path to the local file.</param>
        /// <param name="artifactName">The artifact name.</param>
        void UploadArtifact(string folderName, FilePath file, string artifactName);

        /// <summary>
        /// Upload local directory as a container folder, and create an artifact.
        /// </summary>
        /// <param name="directory">Path to the local directory.</param>
        void UploadArtifactDirectory(DirectoryPath directory);

        /// <summary>
        /// Upload local directory as a container folder, and create an artifact with the specified name.
        /// </summary>
        /// <param name="directory">Path to the local directory.</param>
        /// <param name="artifactName">The artifact name.</param>
        void UploadArtifactDirectory(DirectoryPath directory, string artifactName);

        /// <summary>
        /// Upload additional log to build container's <c>logs/tool</c> folder.
        /// </summary>
        /// <param name="logFile">The log file.</param>
        void UploadBuildLogFile(FilePath logFile);

        /// <summary>
        /// Update build number for current build.
        /// </summary>
        /// <remarks>
        /// Requires agent version 1.88.
        /// </remarks>
        /// <param name="buildNumber">The build number.</param>
        void UpdateBuildNumber(string buildNumber);

        /// <summary>
        /// Add a tag for current build.
        /// </summary>
        /// <remarks>
        /// Requires agent version 1.95.
        /// </remarks>
        /// <param name="tag">The tag.</param>
        void AddBuildTag(string tag);

        /// <summary>
        /// Publishes and uploads tests results.
        /// </summary>
        /// <param name="data">The publish test results data.</param>
        void PublishTestResults(AzurePipelinesPublishTestResultsData data);

        /// <summary>
        /// Publishes and uploads code coverage results.
        /// </summary>
        /// <param name="data">The code coverage data.</param>
        void PublishCodeCoverage(AzurePipelinesPublishCodeCoverageData data);

        /// <summary>
        /// Publishes and uploads code coverage results.
        /// </summary>
        /// <param name="summaryFilePath">The code coverage summary file path.</param>
        /// <param name="data">The code coverage data.</param>
        void PublishCodeCoverage(FilePath summaryFilePath, AzurePipelinesPublishCodeCoverageData data);

        /// <summary>
        /// Publishes and uploads code coverage results.
        /// </summary>
        /// <param name="summaryFilePath">The code coverage summary file path.</param>
        /// <param name="action">The configuration action for the code coverage data.</param>
        void PublishCodeCoverage(FilePath summaryFilePath, Action<AzurePipelinesPublishCodeCoverageData> action);
    }
}
