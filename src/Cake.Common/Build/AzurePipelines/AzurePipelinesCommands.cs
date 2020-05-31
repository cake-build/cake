// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cake.Common.Build.AzurePipelines.Data;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.Build.AzurePipelines
{
    /// <summary>
    /// Responsible for issuing Azure Pipelines agent commands (see <see href="https://github.com/microsoft/azure-pipelines-tasks/blob/master/docs/authoring/commands.md"/>).
    /// </summary>
    public sealed class AzurePipelinesCommands : IAzurePipelinesCommands
    {
        private const string MessagePrefix = "##vso[";
        private const string MessagePostfix = "]";

        private readonly ICakeEnvironment _environment;
        private readonly IBuildSystemServiceMessageWriter _writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzurePipelinesCommands"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="writer">The build system service message writer.</param>
        public AzurePipelinesCommands(ICakeEnvironment environment, IBuildSystemServiceMessageWriter writer)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        /// <summary>
        /// Log a warning issue to timeline record of current task.
        /// </summary>
        /// <param name="message">The warning message.</param>
        public void WriteWarning(string message)
        {
            WriteLoggingCommand("task.logissue", new Dictionary<string, string>
            {
                ["type"] = "warning"
            }, message);
        }

        /// <summary>
        /// Log a warning issue with detailed data to timeline record of current task.
        /// </summary>
        /// <param name="message">The warning message.</param>
        /// <param name="data">The message data.</param>
        public void WriteWarning(string message, AzurePipelinesMessageData data)
        {
            var properties = data.GetProperties();
            properties.Add("type", "warning");
            WriteLoggingCommand("task.logissue", properties, message);
        }

        /// <summary>
        /// Log an error to timeline record of current task.
        /// </summary>
        /// <param name="message">The error message.</param>
        public void WriteError(string message)
        {
            WriteLoggingCommand("task.logissue", new Dictionary<string, string>
            {
                ["type"] = "error"
            }, message);
        }

        /// <summary>
        /// Log an error with detailed data to timeline record of current task.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="data">The message data.</param>
        public void WriteError(string message, AzurePipelinesMessageData data)
        {
            var properties = data.GetProperties();
            properties.Add("type", "error");
            WriteLoggingCommand("task.logissue", properties, message);
        }

        /// <summary>
        /// Set progress and current operation for current task.
        /// </summary>
        /// <param name="progress">Current progress as percentage.</param>
        /// <param name="currentOperation">The current operation.</param>
        public void SetProgress(int progress, string currentOperation)
        {
            WriteLoggingCommand("task.setprogress", new Dictionary<string, string>
            {
                ["value"] = progress.ToString()
            }, currentOperation);
        }

        /// <summary>
        /// Finish timeline record for current task and set task result to succeeded.
        /// </summary>
        public void CompleteCurrentTask()
        {
            WriteLoggingCommand("task.complete", "DONE");
        }

        /// <summary>
        /// Finish timeline record for current task and set task result.
        /// </summary>
        /// <param name="result">The task result status.</param>
        public void CompleteCurrentTask(AzurePipelinesTaskResult result)
        {
            WriteLoggingCommand("task.complete", new Dictionary<string, string>
            {
                ["result"] = result.ToString()
            }, "DONE");
        }

        /// <summary>
        /// Create detail timeline record.
        /// </summary>
        /// <param name="name">Name of the new timeline record.</param>
        /// <param name="type">Type of the new timeline record.</param>
        /// <param name="order">Order of the timeline record.</param>
        /// <returns>The timeline record ID.</returns>
        public Guid CreateNewRecord(string name, string type, int order)
        {
            var guid = Guid.NewGuid();
            WriteLoggingCommand("task.logdetail", new Dictionary<string, string>
            {
                ["id"] = guid.ToString(),
                ["name"] = name,
                ["type"] = type,
                ["order"] = order.ToString()
            }, "create new timeline record");
            return guid;
        }

        /// <summary>
        /// Create detail timeline record.
        /// </summary>
        /// <param name="name">Name of the new timeline record.</param>
        /// <param name="type">Type of the new timeline record.</param>
        /// <param name="order">Order of the timeline record.</param>
        /// <param name="data">Additional data for the new timeline record.</param>
        /// <returns>The timeline record ID.</returns>
        public Guid CreateNewRecord(string name, string type, int order, AzurePipelinesRecordData data)
        {
            var guid = Guid.NewGuid();
            var properties = data.GetProperties();
            properties.Add("id", guid.ToString());
            properties.Add("name", name);
            properties.Add("type", type);
            properties.Add("order", order.ToString());
            WriteLoggingCommand("task.logdetail", properties, "create new timeline record");
            return guid;
        }

        /// <summary>
        /// Update an existing detail timeline record.
        /// </summary>
        /// <param name="id">The ID of the existing timeline record.</param>
        /// <param name="data">Additional data for the timeline record.</param>
        public void UpdateRecord(Guid id, AzurePipelinesRecordData data)
        {
            var properties = data.GetProperties();
            properties.Add("id", id.ToString());
            WriteLoggingCommand("task.logdetail", properties, "update");
        }

        /// <summary>
        /// Sets a variable in the variable service of the task context.
        /// </summary>
        /// <remarks>
        /// The variable is exposed to following tasks as an environment variable.
        /// </remarks>
        /// <param name="name">The variable name.</param>
        /// <param name="value">The variable value.</param>
        public void SetVariable(string name, string value)
        {
            WriteLoggingCommand("task.setvariable", new Dictionary<string, string>
            {
                ["variable"] = name
            }, value);
        }

        /// <summary>
        /// Sets a secret variable in the variable service of the task context.
        /// </summary>
        /// <remarks>
        /// The variable is not exposed to following tasks as an environment variable, and must be passed as inputs.
        /// </remarks>
        /// <param name="name">The variable name.</param>
        /// <param name="value">The variable value.</param>
        public void SetSecretVariable(string name, string value)
        {
            WriteLoggingCommand("task.setvariable", new Dictionary<string, string>
            {
                ["variable"] = name,
                ["issecret"] = "true"
            }, value);
        }

        /// <summary>
        /// Upload and attach summary markdown to current timeline record.
        /// </summary>
        /// <remarks>
        /// This summary is added to the build/release summary and is not available for download with logs.
        /// </remarks>
        /// <param name="markdownPath">Path to the summary markdown file.</param>
        public void UploadTaskSummary(FilePath markdownPath)
        {
            WriteLoggingCommand("task.uploadsummary", markdownPath.MakeAbsolute(_environment).FullPath);
        }

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
        public void UploadTaskLogFile(FilePath logFile)
        {
            WriteLoggingCommand("task.uploadfile", logFile.MakeAbsolute(_environment).FullPath);
        }

        /// <summary>
        /// Create an artifact link, such as a file or folder path or a version control path.
        /// </summary>
        /// <param name="name">The artifact name..</param>
        /// <param name="type">The artifact type.</param>
        /// <param name="location">The link path or value.</param>
        public void LinkArtifact(string name, AzurePipelinesArtifactType type, string location)
        {
            WriteLoggingCommand("artifact.associate", new Dictionary<string, string>
            {
                ["artifactname"] = name,
                ["type"] = type.ToString()
            }, location);
        }

        /// <summary>
        /// Upload local file into a file container folder.
        /// </summary>
        /// <param name="folderName">Folder that the file will upload to.</param>
        /// <param name="file">Path to the local file.</param>
        public void UploadArtifact(string folderName, FilePath file)
        {
            WriteLoggingCommand("artifact.upload", new Dictionary<string, string>
            {
                ["containerfolder"] = folderName
            }, file.MakeAbsolute(_environment).FullPath);
        }

        /// <summary>
        /// Upload local file into a file container folder, and create an artifact.
        /// </summary>
        /// <param name="folderName">Folder that the file will upload to.</param>
        /// <param name="file">Path to the local file.</param>
        /// <param name="artifactName">The artifact name.</param>
        public void UploadArtifact(string folderName, FilePath file, string artifactName)
        {
            WriteLoggingCommand("artifact.upload", new Dictionary<string, string>
            {
                ["containerfolder"] = folderName,
                ["artifactname"] = artifactName
            }, file.MakeAbsolute(_environment).FullPath);
        }

        /// <inheritdoc />
        public void UploadArtifactDirectory(DirectoryPath directory)
        {
            if (directory == null)
            {
                throw new ArgumentNullException(nameof(directory));
            }

            UploadArtifactDirectory(directory, directory.GetDirectoryName());
        }

        /// <inheritdoc />
        public void UploadArtifactDirectory(DirectoryPath directory, string artifactName)
        {
            if (directory == null)
            {
                throw new ArgumentNullException(nameof(directory));
            }

            if (artifactName == null)
            {
                throw new ArgumentNullException(nameof(artifactName));
            }

            WriteLoggingCommand("artifact.upload", new Dictionary<string, string>
            {
                ["containerfolder"] = artifactName,
                ["artifactname"] = artifactName
            }, directory.MakeAbsolute(_environment).FullPath);
        }

        /// <summary>
        /// Upload additional log to build container's <c>logs/tool</c> folder.
        /// </summary>
        /// <param name="logFile">The log file.</param>
        public void UploadBuildLogFile(FilePath logFile)
        {
            WriteLoggingCommand("build.uploadlog", logFile.MakeAbsolute(_environment).FullPath);
        }

        /// <summary>
        /// Update build number for current build.
        /// </summary>
        /// <remarks>
        /// Requires agent version 1.88.
        /// </remarks>
        /// <param name="buildNumber">The build number.</param>
        public void UpdateBuildNumber(string buildNumber)
        {
            WriteLoggingCommand("build.updatebuildnumber", buildNumber);
        }

        /// <summary>
        /// Add a tag for current build.
        /// </summary>
        /// <remarks>
        /// Requires agent version 1.95.
        /// </remarks>
        /// <param name="tag">The tag.</param>
        public void AddBuildTag(string tag)
        {
            WriteLoggingCommand("build.addbuildtag", tag);
        }

        /// <summary>
        /// Publishes and uploads tests results.
        /// </summary>
        /// <param name="data">The publish test results data.</param>
        public void PublishTestResults(AzurePipelinesPublishTestResultsData data)
        {
            var properties = data.GetProperties(_environment);
            WriteLoggingCommand("results.publish", properties, string.Empty);
        }

        /// <summary>
        /// Publishes and uploads code coverage results.
        /// </summary>
        /// <param name="data">The code coverage data.</param>
        public void PublishCodeCoverage(AzurePipelinesPublishCodeCoverageData data)
        {
            var properties = data.GetProperties(_environment);
            WriteLoggingCommand("codecoverage.publish", properties, string.Empty);
        }

        /// <summary>
        /// Publishes and uploads code coverage results.
        /// </summary>
        /// <param name="summaryFilePath">The code coverage summary file path.</param>
        /// <param name="data">The code coverage data.</param>
        public void PublishCodeCoverage(FilePath summaryFilePath, AzurePipelinesPublishCodeCoverageData data)
        {
            if (summaryFilePath == null)
            {
                throw new ArgumentNullException(nameof(summaryFilePath));
            }

            var properties = data.GetProperties(_environment, summaryFilePath);
            WriteLoggingCommand("codecoverage.publish", properties, string.Empty);
        }

        /// <summary>
        /// Publishes and uploads code coverage results.
        /// </summary>
        /// <param name="summaryFilePath">The code coverage summary file path.</param>
        /// <param name="action">The configuration action for the code coverage data.</param>
        public void PublishCodeCoverage(FilePath summaryFilePath, Action<AzurePipelinesPublishCodeCoverageData> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var data = new AzurePipelinesPublishCodeCoverageData();
            action(data);

            PublishCodeCoverage(summaryFilePath, data);
        }

        private void WriteLoggingCommand(string actionName, string value)
        {
            WriteLoggingCommand(actionName, new Dictionary<string, string>(), value);
        }

        private void WriteLoggingCommand(string actionName, Dictionary<string, string> properties, string value)
        {
            var props = string.Join(string.Empty, properties.Select(pair =>
            {
                return string.Format(CultureInfo.InvariantCulture, "{0}={1};", pair.Key, pair.Value);
            }));

            _writer.Write("{0}{1} {2}{3}{4}", MessagePrefix, actionName, props, MessagePostfix, value);
        }
    }
}
