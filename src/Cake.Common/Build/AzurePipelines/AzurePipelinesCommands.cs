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

        /// <inheritdoc/>
        public void WriteWarning(string message)
        {
            WriteLoggingCommand("task.logissue", new Dictionary<string, string>
            {
                ["type"] = "warning"
            }, message);
        }

        /// <inheritdoc/>
        public void WriteWarning(string message, AzurePipelinesMessageData data)
        {
            var properties = data.GetProperties();
            properties.Add("type", "warning");
            WriteLoggingCommand("task.logissue", properties, message);
        }

        /// <inheritdoc/>
        public void WriteError(string message)
        {
            WriteLoggingCommand("task.logissue", new Dictionary<string, string>
            {
                ["type"] = "error"
            }, message);
        }

        /// <inheritdoc/>
        public void WriteError(string message, AzurePipelinesMessageData data)
        {
            var properties = data.GetProperties();
            properties.Add("type", "error");
            WriteLoggingCommand("task.logissue", properties, message);
        }

        /// <inheritdoc/>
        public void SetProgress(int progress, string currentOperation)
        {
            WriteLoggingCommand("task.setprogress", new Dictionary<string, string>
            {
                ["value"] = progress.ToString()
            }, currentOperation);
        }

        /// <inheritdoc/>
        public void CompleteCurrentTask()
        {
            WriteLoggingCommand("task.complete", "DONE");
        }

        /// <inheritdoc/>
        public void CompleteCurrentTask(AzurePipelinesTaskResult result)
        {
            WriteLoggingCommand("task.complete", new Dictionary<string, string>
            {
                ["result"] = result.ToString()
            }, "DONE");
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public void UpdateRecord(Guid id, AzurePipelinesRecordData data)
        {
            var properties = data.GetProperties();
            properties.Add("id", id.ToString());
            WriteLoggingCommand("task.logdetail", properties, "update");
        }

        /// <inheritdoc/>
        public void SetVariable(string name, string value)
        {
            WriteLoggingCommand("task.setvariable", new Dictionary<string, string>
            {
                ["variable"] = name
            }, value);
        }

        /// <inheritdoc/>
        public void SetOutputVariable(string name, string value)
        {
            WriteLoggingCommand("task.setvariable", new Dictionary<string, string>
            {
                ["variable"] = name,
                ["isOutput"] = "true"
            }, value);
        }

        /// <inheritdoc/>
        public void SetSecretVariable(string name, string value)
        {
            WriteLoggingCommand("task.setvariable", new Dictionary<string, string>
            {
                ["variable"] = name,
                ["issecret"] = "true"
            }, value);
        }

        /// <inheritdoc/>
        public void UploadTaskSummary(FilePath markdownPath)
        {
            WriteLoggingCommand("task.uploadsummary", markdownPath.MakeAbsolute(_environment).FullPath);
        }

        /// <inheritdoc/>
        public void UploadTaskLogFile(FilePath logFile)
        {
            WriteLoggingCommand("task.uploadfile", logFile.MakeAbsolute(_environment).FullPath);
        }

        /// <inheritdoc/>
        public void LinkArtifact(string name, AzurePipelinesArtifactType type, string location)
        {
            WriteLoggingCommand("artifact.associate", new Dictionary<string, string>
            {
                ["artifactname"] = name,
                ["type"] = type.ToString()
            }, location);
        }

        /// <inheritdoc/>
        public void UploadArtifact(string folderName, FilePath file)
        {
            WriteLoggingCommand("artifact.upload", new Dictionary<string, string>
            {
                ["containerfolder"] = folderName
            }, file.MakeAbsolute(_environment).FullPath);
        }

        /// <inheritdoc/>
        public void UploadArtifact(string folderName, FilePath file, string artifactName)
        {
            WriteLoggingCommand("artifact.upload", new Dictionary<string, string>
            {
                ["containerfolder"] = folderName,
                ["artifactname"] = artifactName
            }, file.MakeAbsolute(_environment).FullPath);
        }

        /// <inheritdoc/>
        public void UploadArtifactDirectory(DirectoryPath directory)
        {
            if (directory == null)
            {
                throw new ArgumentNullException(nameof(directory));
            }

            UploadArtifactDirectory(directory, directory.GetDirectoryName());
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public void UploadBuildLogFile(FilePath logFile)
        {
            WriteLoggingCommand("build.uploadlog", logFile.MakeAbsolute(_environment).FullPath);
        }

        /// <inheritdoc/>
        public void UpdateBuildNumber(string buildNumber)
        {
            WriteLoggingCommand("build.updatebuildnumber", buildNumber);
        }

        /// <inheritdoc/>
        public void AddBuildTag(string tag)
        {
            WriteLoggingCommand("build.addbuildtag", tag);
        }

        /// <inheritdoc/>
        public void PublishTestResults(AzurePipelinesPublishTestResultsData data)
        {
            var properties = data.GetProperties(_environment);
            WriteLoggingCommand("results.publish", properties, string.Empty);
        }

        /// <inheritdoc/>
        public void PublishCodeCoverage(AzurePipelinesPublishCodeCoverageData data)
        {
            var properties = data.GetProperties(_environment);
            WriteLoggingCommand("codecoverage.publish", properties, string.Empty);
        }

        /// <inheritdoc/>
        public void PublishCodeCoverage(FilePath summaryFilePath, AzurePipelinesPublishCodeCoverageData data)
        {
            if (summaryFilePath == null)
            {
                throw new ArgumentNullException(nameof(summaryFilePath));
            }

            var properties = data.GetProperties(_environment, summaryFilePath);
            WriteLoggingCommand("codecoverage.publish", properties, string.Empty);
        }

        /// <inheritdoc/>
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
