// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Cake.Common.Build.GitHubActions.Commands.Artifact;
using Cake.Common.Build.GitHubActions.Data;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Build.GitHubActions.Commands
{
    /// <summary>
    /// Provides GitHub Actions commands for a current build.
    /// </summary>
    public sealed class GitHubActionsCommands
    {
        private readonly ICakeEnvironment _environment;
        private readonly IFileSystem _fileSystem;
        private readonly IBuildSystemServiceMessageWriter _writer;
        private readonly GitHubActionsEnvironmentInfo _actionsEnvironment;
        private readonly GitHubActionsArtifactService _artifactsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubActionsCommands"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="writer">The build system service message writer.</param>
        /// <param name="actionsEnvironment">The actions environment.</param>
        /// <param name="createHttpClient">The http client factory.</param>
        public GitHubActionsCommands(
            ICakeEnvironment environment,
            IFileSystem fileSystem,
            IBuildSystemServiceMessageWriter writer,
            GitHubActionsEnvironmentInfo actionsEnvironment,
            Func<string, HttpClient> createHttpClient)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
            _actionsEnvironment = actionsEnvironment ?? throw new ArgumentNullException(nameof(actionsEnvironment));
            _artifactsService = new GitHubActionsArtifactService(environment, fileSystem, actionsEnvironment, createHttpClient ?? throw new ArgumentNullException(nameof(createHttpClient)));
        }

        /// <summary>
        /// Write debug message to the build log.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Debug(string message)
        {
            WriteCommand("debug", message);
        }

        /// <summary>
        /// Write notice message to the build log.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="annotation">The annotation.</param>
        public void Notice(string message, GitHubActionsAnnotation annotation = null)
        {
            WriteCommand("notice", annotation?.GetParameters(), message);
        }

        /// <summary>
        /// Write warning message to the build log.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="annotation">The annotation.</param>
        public void Warning(string message, GitHubActionsAnnotation annotation = null)
        {
            WriteCommand("warning", annotation?.GetParameters(), message);
        }

        /// <summary>
        /// Write error message to the build log.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="annotation">The annotation.</param>
        public void Error(string message, GitHubActionsAnnotation annotation = null)
        {
            WriteCommand("error", annotation?.GetParameters(), message);
        }

        /// <summary>
        /// Start a group in the build log.
        /// </summary>
        /// <param name="title">The title.</param>
        public void StartGroup(string title)
        {
            WriteCommand("group", title);
        }

        /// <summary>
        /// End a group in the build log.
        /// </summary>
        public void EndGroup()
        {
            WriteCommand("endgroup");
        }

        /// <summary>
        /// Registers a secret which will get masked in the build log.
        /// </summary>
        /// <param name="secret">The secret.</param>
        public void SetSecret(string secret)
        {
            WriteCommand("add-mask", secret);
        }

        /// <summary>
        /// Prepends a directory to the system PATH variable and automatically makes it available to all subsequent actions in the current job.
        /// </summary>
        /// <param name="path">The directory path.</param>
        public void AddPath(DirectoryPath path)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (_actionsEnvironment.Runtime.SystemPath == null)
            {
                throw new CakeException("GitHub Actions Runtime SystemPath missing.");
            }

            var file = _fileSystem.GetFile(_actionsEnvironment.Runtime.SystemPath);
            using var stream = file.Open(FileMode.Append, FileAccess.Write, FileShare.None);
            using var writer = new StreamWriter(stream);
            writer.WriteLine(path.MakeAbsolute(_environment).FullPath);
        }

        /// <summary>
        /// Creates or updates an environment variable for any steps running next in a job.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The Value.</param>
        public void SetEnvironmentVariable(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (_actionsEnvironment.Runtime.EnvPath == null)
            {
                throw new CakeException("GitHub Actions Runtime EnvPath missing.");
            }

            var file = _fileSystem.GetFile(_actionsEnvironment.Runtime.EnvPath);
            using var stream = file.Open(FileMode.Append, FileAccess.Write, FileShare.None);
            using var writer = new StreamWriter(stream);
            writer.Write(key);
            writer.WriteLine("<<CAKEEOF");
            writer.WriteLine(value);
            writer.WriteLine("CAKEEOF");
        }

        /// <summary>
        /// Creates or updates an output parameter for any steps running next in a job.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The Value.</param>
        public void SetOutputParameter(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (_actionsEnvironment.Runtime.OutputPath == null)
            {
                throw new CakeException("GitHub Actions Runtime OutputPath missing.");
            }

            var file = _fileSystem.GetFile(_actionsEnvironment.Runtime.OutputPath);
            using var stream = file.Open(FileMode.Append, FileAccess.Write, FileShare.None);
            using var writer = new StreamWriter(stream);
            writer.Write(key);
            writer.Write("=");
            writer.WriteLine(value);
        }

        /// <summary>
        /// Creates or updates the step summary for a GitHub workflow.
        /// </summary>
        /// <param name="summary">The step summary.</param>
        public void SetStepSummary(string summary)
        {
            if (string.IsNullOrEmpty(summary))
            {
                throw new ArgumentNullException(nameof(summary));
            }

            if (_actionsEnvironment.Runtime.StepSummary == null)
            {
                throw new CakeException("GitHub Actions Runtime StepSummary missing.");
            }

            var file = _fileSystem.GetFile(_actionsEnvironment.Runtime.StepSummary);
            using var stream = file.Open(FileMode.Append, FileAccess.Write, FileShare.None);
            using var writer = new StreamWriter(stream);
            writer.WriteLine(summary);
        }

        /// <summary>
        /// Upload local file into a file container folder, and create an artifact.
        /// </summary>
        /// <param name="path">Path to the local file.</param>
        /// <param name="artifactName">The artifact name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task UploadArtifact(FilePath path, string artifactName)
        {
            var file = _fileSystem.GetFile(ValidateArtifactParameters(path, artifactName));

            if (!file.Exists)
            {
                throw new FileNotFoundException("Artifact file not found.", file.Path.FullPath);
            }

            await _artifactsService.CreateAndUploadArtifactFiles(artifactName, file.Path.GetDirectory(), file);
        }

        /// <summary>
        /// Upload local directory files into a file container folder, and create an artifact.
        /// </summary>
        /// <param name="path">Path to the local directory.</param>
        /// <param name="artifactName">The artifact name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task UploadArtifact(DirectoryPath path, string artifactName)
        {
            var directory = _fileSystem.GetDirectory(ValidateArtifactParameters(path, artifactName));

            if (!directory.Exists)
            {
                throw new DirectoryNotFoundException(FormattableString.Invariant($"Artifact directory {directory.Path.FullPath} not found."));
            }

            var files = directory
                            .GetFiles("*", SearchScope.Recursive)
                            .ToArray();

            await _artifactsService.CreateAndUploadArtifactFiles(artifactName, directory.Path, files);
        }

        /// <summary>
        /// Download remote artifact container into local directory.
        /// </summary>
        /// <param name="artifactName">The artifact name.</param>
        /// <param name="path">Path to the local directory.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task DownloadArtifact(string artifactName, DirectoryPath path)
        {
            var directory = _fileSystem.GetDirectory(ValidateArtifactParameters(path, artifactName));

            if (!directory.Exists)
            {
                throw new DirectoryNotFoundException(FormattableString.Invariant($"Local directory {directory.Path.FullPath} not found."));
            }

            await _artifactsService.DownloadArtifactFiles(artifactName, directory.Path);
        }

        internal void WriteCommand(string command, string message = null)
        {
            WriteCommand(command, new Dictionary<string, string>(), message);
        }

        internal void WriteCommand(string command, Dictionary<string, string> parameters, string message)
        {
            var parameterString = parameters?.Count > 0 ? string.Concat(" ", string.Join(",", parameters.Select(pair => $"{pair.Key}={EscapeCommandParameter(pair.Value)}"))) : string.Empty;

            _writer.Write("::{0}{1}::{2}", command, parameterString, EscapeCommandMessage(message));
        }

        private static string EscapeCommandMessage(string value) => (value ?? string.Empty).Replace("%", "%25").Replace("\r", "%0D").Replace("\n", "%0A");

        private static string EscapeCommandParameter(string value) => (value ?? string.Empty).Replace("%", "%25").Replace("\r", "%0D").Replace("\n", "%0A").Replace(":", "%3A").Replace(",", "%2C");

        private T ValidateArtifactParameters<T>(T path, string artifactName) where T : IPath<T>
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (string.IsNullOrWhiteSpace(artifactName))
            {
                throw new ArgumentNullException(nameof(artifactName));
            }

            if (string.IsNullOrWhiteSpace(_actionsEnvironment.Runtime.Token))
            {
                throw new CakeException("GitHub Actions Runtime Token missing.");
            }

            if (string.IsNullOrWhiteSpace(_actionsEnvironment.Runtime.Url))
            {
                throw new CakeException("GitHub Actions Runtime Url missing.");
            }

            if (string.IsNullOrWhiteSpace(_actionsEnvironment.Workflow.RunId))
            {
                throw new CakeException("GitHub Actions Workflow RunId missing.");
            }

            return path.MakeAbsolute(_environment);
        }
    }
}
