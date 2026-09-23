// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
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
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.WriteWarning("Watch this");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.WriteWarning("Watch this");
        /// }
        /// </code>
        /// </example>
        void WriteWarning(string message);

        /// <summary>
        /// Log a warning issue with detailed data to timeline record of current task.
        /// </summary>
        /// <param name="message">The warning message.</param>
        /// <param name="data">The message data.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.WriteWarning(
        ///         "Unused variable",
        ///         new AzurePipelinesMessageData { SourcePath = "./src/Program.cs", LineNumber = 10 });
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.WriteWarning(
        ///         "Unused variable",
        ///         new AzurePipelinesMessageData { SourcePath = "./src/Program.cs", LineNumber = 10 });
        /// }
        /// </code>
        /// </example>
        void WriteWarning(string message, AzurePipelinesMessageData data);

        /// <summary>
        /// Log an error to timeline record of current task.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.WriteError("Build failed");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.WriteError("Build failed");
        /// }
        /// </code>
        /// </example>
        void WriteError(string message);

        /// <summary>
        /// Log an error with detailed data to timeline record of current task.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="data">The message data.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.WriteError(
        ///         "Missing semicolon",
        ///         new AzurePipelinesMessageData { SourcePath = "./src/Program.cs", LineNumber = 42 });
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.WriteError(
        ///         "Missing semicolon",
        ///         new AzurePipelinesMessageData { SourcePath = "./src/Program.cs", LineNumber = 42 });
        /// }
        /// </code>
        /// </example>
        void WriteError(string message, AzurePipelinesMessageData data);

        /// <summary>
        /// Begin a collapsible group.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.BeginGroup("Restore");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.BeginGroup("Restore");
        /// }
        /// </code>
        /// </example>
        public void BeginGroup(string name);

        /// <summary>
        /// End a collapsible group.
        /// </summary>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.EndGroup();
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.EndGroup();
        /// }
        /// </code>
        /// </example>
        public void EndGroup();

        /// <summary>
        /// Log command.
        /// </summary>
        /// <param name="commandLine">The command-line being run.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.Command("dotnet test");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.Command("dotnet test");
        /// }
        /// </code>
        /// </example>
        public void Command(string commandLine);

        /// <summary>
        /// Log section.
        /// </summary>
        /// <param name="name">The name of the section.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.Section("Tests");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.Section("Tests");
        /// }
        /// </code>
        /// </example>
        public void Section(string name);

        /// <summary>
        /// Set progress and current operation for current task.
        /// </summary>
        /// <param name="progress">Current progress as percentage.</param>
        /// <param name="currentOperation">The current operation.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.SetProgress(50, "Running tests");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.SetProgress(50, "Running tests");
        /// }
        /// </code>
        /// </example>
        void SetProgress(int progress, string currentOperation);

        /// <summary>
        /// Finish timeline record for current task and set task result to succeeded.
        /// </summary>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.CompleteCurrentTask();
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.CompleteCurrentTask();
        /// }
        /// </code>
        /// </example>
        void CompleteCurrentTask();

        /// <summary>
        /// Finish timeline record for current task and set task result.
        /// </summary>
        /// <param name="result">The task result status.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.CompleteCurrentTask(AzurePipelinesTaskResult.Succeeded);
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.CompleteCurrentTask(AzurePipelinesTaskResult.Succeeded);
        /// }
        /// </code>
        /// </example>
        void CompleteCurrentTask(AzurePipelinesTaskResult result);

        /// <summary>
        /// Create detail timeline record.
        /// </summary>
        /// <param name="name">Name of the new timeline record.</param>
        /// <param name="type">Type of the new timeline record.</param>
        /// <param name="order">Order of the timeline record.</param>
        /// <returns>The timeline record ID.</returns>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     var recordId = BuildSystem.AzurePipelines.Commands.CreateNewRecord("Compile", "build", 1);
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     var recordId = AzurePipelines.Commands.CreateNewRecord("Compile", "build", 1);
        /// }
        /// </code>
        /// </example>
        Guid CreateNewRecord(string name, string type, int order);

        /// <summary>
        /// Create detail timeline record.
        /// </summary>
        /// <param name="name">Name of the new timeline record.</param>
        /// <param name="type">Type of the new timeline record.</param>
        /// <param name="order">Order of the timeline record.</param>
        /// <param name="data">Additional data for the new timeline record.</param>
        /// <returns>The timeline record ID.</returns>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     var recordId = BuildSystem.AzurePipelines.Commands.CreateNewRecord(
        ///         "Compile",
        ///         "build",
        ///         1,
        ///         new AzurePipelinesRecordData { Progress = 0 });
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     var recordId = AzurePipelines.Commands.CreateNewRecord(
        ///         "Compile",
        ///         "build",
        ///         1,
        ///         new AzurePipelinesRecordData { Progress = 0 });
        /// }
        /// </code>
        /// </example>
        Guid CreateNewRecord(string name, string type, int order, AzurePipelinesRecordData data);

        /// <summary>
        /// Update an existing detail timeline record.
        /// </summary>
        /// <param name="id">The ID of the existing timeline record.</param>
        /// <param name="data">Additional data for the timeline record.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.UpdateRecord(recordId, new AzurePipelinesRecordData { Progress = 100 });
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.UpdateRecord(recordId, new AzurePipelinesRecordData { Progress = 100 });
        /// }
        /// </code>
        /// </example>
        void UpdateRecord(Guid id, AzurePipelinesRecordData data);

        /// <summary>
        /// Sets a variable in the variable service of the task context.
        /// </summary>
        /// <remarks>
        /// The variable is exposed to following tasks as an environment variable.
        /// </remarks>
        /// <param name="name">The variable name.</param>
        /// <param name="value">The variable value.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.SetVariable("MyVar", "value");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.SetVariable("MyVar", "value");
        /// }
        /// </code>
        /// </example>
        void SetVariable(string name, string value);

        /// <summary>
        /// Sets a output variable in the variable service of the task context.
        /// </summary>
        /// <remarks>
        /// The variable is exposed to following tasks as an environment variable.
        /// </remarks>
        /// <param name="name">The variable name.</param>
        /// <param name="value">The variable value.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.SetOutputVariable("CakeVersion", "1.0.0");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.SetOutputVariable("CakeVersion", "1.0.0");
        /// }
        /// </code>
        /// </example>
        void SetOutputVariable(string name, string value);

        /// <summary>
        /// Sets a secret variable in the variable service of the task context.
        /// </summary>
        /// <remarks>
        /// The variable is not exposed to following tasks as an environment variable, and must be passed as inputs.
        /// </remarks>
        /// <param name="name">The variable name.</param>
        /// <param name="value">The variable value.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.SetSecretVariable("ApiKey", apiKey);
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.SetSecretVariable("ApiKey", apiKey);
        /// }
        /// </code>
        /// </example>
        void SetSecretVariable(string name, string value);

        /// <summary>
        /// Upload and attach summary markdown to current timeline record.
        /// </summary>
        /// <remarks>
        /// This summary is added to the build/release summary and is not available for download with logs.
        /// </remarks>
        /// <param name="markdownPath">Path to the summary markdown file.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.UploadTaskSummary("./summary.md");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.UploadTaskSummary("./summary.md");
        /// }
        /// </code>
        /// </example>
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
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.UploadTaskLogFile("./logs/task.log");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.UploadTaskLogFile("./logs/task.log");
        /// }
        /// </code>
        /// </example>
        void UploadTaskLogFile(FilePath logFile);

        /// <summary>
        /// Create an artifact link, such as a file or folder path or a version control path.
        /// </summary>
        /// <param name="name">The artifact name.</param>
        /// <param name="type">The artifact type.</param>
        /// <param name="location">The link path or value.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.LinkArtifact("drop", AzurePipelinesArtifactType.FilePath, "./artifacts");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.LinkArtifact("drop", AzurePipelinesArtifactType.FilePath, "./artifacts");
        /// }
        /// </code>
        /// </example>
        void LinkArtifact(string name, AzurePipelinesArtifactType type, string location);

        /// <summary>
        /// Upload local file into a file container folder.
        /// </summary>
        /// <param name="folderName">Folder that the file will upload to.</param>
        /// <param name="file">Path to the local file.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.UploadArtifact("drop", "./artifacts/package.nupkg");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.UploadArtifact("drop", "./artifacts/package.nupkg");
        /// }
        /// </code>
        /// </example>
        void UploadArtifact(string folderName, FilePath file);

        /// <summary>
        /// Upload local file into a file container folder, and create an artifact.
        /// </summary>
        /// <param name="folderName">Folder that the file will upload to.</param>
        /// <param name="file">Path to the local file.</param>
        /// <param name="artifactName">The artifact name.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.UploadArtifact("drop", "./artifacts/package.nupkg", "nuget");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.UploadArtifact("drop", "./artifacts/package.nupkg", "nuget");
        /// }
        /// </code>
        /// </example>
        void UploadArtifact(string folderName, FilePath file, string artifactName);

        /// <summary>
        /// Upload local directory as a container folder, and create an artifact.
        /// </summary>
        /// <param name="directory">Path to the local directory.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.UploadArtifactDirectory("./artifacts");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.UploadArtifactDirectory("./artifacts");
        /// }
        /// </code>
        /// </example>
        void UploadArtifactDirectory(DirectoryPath directory);

        /// <summary>
        /// Upload local directory as a container folder, and create an artifact with the specified name.
        /// </summary>
        /// <param name="directory">Path to the local directory.</param>
        /// <param name="artifactName">The artifact name.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.UploadArtifactDirectory("./artifacts", "drop");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.UploadArtifactDirectory("./artifacts", "drop");
        /// }
        /// </code>
        /// </example>
        void UploadArtifactDirectory(DirectoryPath directory, string artifactName);

        /// <summary>
        /// Upload additional log to build container's <c>logs/tool</c> folder.
        /// </summary>
        /// <param name="logFile">The log file.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.UploadBuildLogFile("./logs/build.log");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.UploadBuildLogFile("./logs/build.log");
        /// }
        /// </code>
        /// </example>
        void UploadBuildLogFile(FilePath logFile);

        /// <summary>
        /// Update build number for current build.
        /// </summary>
        /// <remarks>
        /// Requires agent version 1.88.
        /// </remarks>
        /// <param name="buildNumber">The build number.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.UpdateBuildNumber("1.2.3.4");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.UpdateBuildNumber("1.2.3.4");
        /// }
        /// </code>
        /// </example>
        void UpdateBuildNumber(string buildNumber);

        /// <summary>
        /// Add a tag for current build.
        /// </summary>
        /// <remarks>
        /// Requires agent version 1.95.
        /// </remarks>
        /// <param name="tag">The tag.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.AddBuildTag("release");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.AddBuildTag("release");
        /// }
        /// </code>
        /// </example>
        void AddBuildTag(string tag);

        /// <summary>
        /// Publishes and uploads tests results.
        /// </summary>
        /// <param name="data">The publish test results data.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.PublishTestResults(new AzurePipelinesPublishTestResultsData
        ///     {
        ///         TestRunner = AzurePipelinesTestRunnerType.XUnit,
        ///         TestResultsFiles = new[] { (FilePath)"./test-results.xml" }
        ///     });
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.PublishTestResults(new AzurePipelinesPublishTestResultsData
        ///     {
        ///         TestRunner = AzurePipelinesTestRunnerType.XUnit,
        ///         TestResultsFiles = new[] { (FilePath)"./test-results.xml" }
        ///     });
        /// }
        /// </code>
        /// </example>
        void PublishTestResults(AzurePipelinesPublishTestResultsData data);

        /// <summary>
        /// Publishes and uploads tests results.
        /// </summary>
        /// <param name="filePath">The test result file path.</param>
        /// <param name="data">The publish test results data.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.PublishTestResults(
        ///         "./test-results.xml",
        ///         new AzurePipelinesPublishTestResultsData { TestRunner = AzurePipelinesTestRunnerType.XUnit });
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.PublishTestResults(
        ///         "./test-results.xml",
        ///         new AzurePipelinesPublishTestResultsData { TestRunner = AzurePipelinesTestRunnerType.XUnit });
        /// }
        /// </code>
        /// </example>
        void PublishTestResults(FilePath filePath, AzurePipelinesPublishTestResultsData data);

        /// <summary>
        /// Publishes and uploads tests results.
        /// </summary>
        /// <param name="filePath">The test result file path.</param>
        /// <param name="action">The configuration action for the publish test results data.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.PublishTestResults("./test-results.xml", data =>
        ///     {
        ///         data.TestRunner = AzurePipelinesTestRunnerType.XUnit;
        ///     });
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.PublishTestResults("./test-results.xml", data =>
        ///     {
        ///         data.TestRunner = AzurePipelinesTestRunnerType.XUnit;
        ///     });
        /// }
        /// </code>
        /// </example>
        void PublishTestResults(FilePath filePath, Action<AzurePipelinesPublishTestResultsData> action);

        /// <summary>
        /// Publishes and uploads tests results.
        /// </summary>
        /// <param name="filePaths">The test result file paths.</param>
        /// <param name="data">The publish test results data.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.PublishTestResults(
        ///         new FilePath[] { "./test-results.xml" },
        ///         new AzurePipelinesPublishTestResultsData { TestRunner = AzurePipelinesTestRunnerType.XUnit });
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.PublishTestResults(
        ///         new FilePath[] { "./test-results.xml" },
        ///         new AzurePipelinesPublishTestResultsData { TestRunner = AzurePipelinesTestRunnerType.XUnit });
        /// }
        /// </code>
        /// </example>
        void PublishTestResults(IEnumerable<FilePath> filePaths, AzurePipelinesPublishTestResultsData data);

        /// <summary>
        /// Publishes and uploads tests results.
        /// </summary>
        /// <param name="filePaths">The test result file paths.</param>
        /// <param name="action">The configuration action for the publish test results data.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.PublishTestResults(
        ///         new FilePath[] { "./test-results.xml" },
        ///         data => { data.TestRunner = AzurePipelinesTestRunnerType.XUnit; });
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.PublishTestResults(
        ///         new FilePath[] { "./test-results.xml" },
        ///         data => { data.TestRunner = AzurePipelinesTestRunnerType.XUnit; });
        /// }
        /// </code>
        /// </example>
        void PublishTestResults(IEnumerable<FilePath> filePaths, Action<AzurePipelinesPublishTestResultsData> action);

        /// <summary>
        /// Publishes and uploads code coverage results.
        /// </summary>
        /// <param name="data">The code coverage data.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.PublishCodeCoverage(new AzurePipelinesPublishCodeCoverageData
        ///     {
        ///         CodeCoverageTool = AzurePipelinesCodeCoverageToolType.Cobertura,
        ///         SummaryFileLocation = "./coverage.xml"
        ///     });
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.PublishCodeCoverage(new AzurePipelinesPublishCodeCoverageData
        ///     {
        ///         CodeCoverageTool = AzurePipelinesCodeCoverageToolType.Cobertura,
        ///         SummaryFileLocation = "./coverage.xml"
        ///     });
        /// }
        /// </code>
        /// </example>
        void PublishCodeCoverage(AzurePipelinesPublishCodeCoverageData data);

        /// <summary>
        /// Publishes and uploads code coverage results.
        /// </summary>
        /// <param name="summaryFilePath">The code coverage summary file path.</param>
        /// <param name="data">The code coverage data.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.PublishCodeCoverage(
        ///         "./coverage.xml",
        ///         new AzurePipelinesPublishCodeCoverageData { CodeCoverageTool = AzurePipelinesCodeCoverageToolType.Cobertura });
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.PublishCodeCoverage(
        ///         "./coverage.xml",
        ///         new AzurePipelinesPublishCodeCoverageData { CodeCoverageTool = AzurePipelinesCodeCoverageToolType.Cobertura });
        /// }
        /// </code>
        /// </example>
        void PublishCodeCoverage(FilePath summaryFilePath, AzurePipelinesPublishCodeCoverageData data);

        /// <summary>
        /// Publishes and uploads code coverage results.
        /// </summary>
        /// <param name="summaryFilePath">The code coverage summary file path.</param>
        /// <param name="action">The configuration action for the code coverage data.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     BuildSystem.AzurePipelines.Commands.PublishCodeCoverage("./coverage.xml", data =>
        ///     {
        ///         data.CodeCoverageTool = AzurePipelinesCodeCoverageToolType.Cobertura;
        ///     });
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     AzurePipelines.Commands.PublishCodeCoverage("./coverage.xml", data =>
        ///     {
        ///         data.CodeCoverageTool = AzurePipelinesCodeCoverageToolType.Cobertura;
        ///     });
        /// }
        /// </code>
        /// </example>
        void PublishCodeCoverage(FilePath summaryFilePath, Action<AzurePipelinesPublishCodeCoverageData> action);
    }
}
