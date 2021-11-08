﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Cake.Common.Build.GitHubActions.Data;
using Cake.Core;
using Cake.Core.IO;
using Path = Cake.Core.IO.Path;

namespace Cake.Common.Build.GitHubActions.Commands
{
    /// <summary>
    /// Provides GitHub Actions commands for a current build.
    /// </summary>
    public sealed class GitHubActionsCommands
    {
        private const string ApiVersion = "6.0-preview";
        private const string AcceptHeader = "application/json;api-version=" + ApiVersion;
        private const string ContentTypeHeader = "application/json";

        private readonly ICakeEnvironment _environment;
        private readonly IFileSystem _fileSystem;
        private readonly GitHubActionsEnvironmentInfo _actionsEnvironment;
        private readonly Func<string, HttpClient> _createHttpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubActionsCommands"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="actionsEnvironment">The actions environment.</param>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="createHttpClient">The http client factory.</param>
        public GitHubActionsCommands(
            ICakeEnvironment environment,
            IFileSystem fileSystem,
            GitHubActionsEnvironmentInfo actionsEnvironment,
            Func<string, HttpClient> createHttpClient)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _actionsEnvironment = actionsEnvironment ?? throw new ArgumentNullException(nameof(actionsEnvironment));
            _createHttpClient = createHttpClient ?? throw new ArgumentNullException(nameof(createHttpClient));
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
        /// Upload local file into a file container folder, and create an artifact.
        /// </summary>
        /// <param name="path">Path to the local file.</param>
        /// <param name="artifactName">The artifact name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task UploadArtifact(FilePath path, string artifactName)
        {
            ValidateUploadArtifactParameters(path, artifactName);

            var file = _fileSystem.GetFile(path);

            if (!file.Exists)
            {
                throw new FileNotFoundException("Artifact file not found.", path.FullPath);
            }

            await CreateAndUploadArtifactFiles(artifactName, path.GetDirectory(), file);
        }

        /// <summary>
        /// Upload local directory files into a file container folder, and create an artifact.
        /// </summary>
        /// <param name="path">Path to the local directory.</param>
        /// <param name="artifactName">The artifact name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task UploadArtifact(DirectoryPath path, string artifactName)
        {
            ValidateUploadArtifactParameters(path, artifactName);

            var directory = _fileSystem.GetDirectory(path);

            if (!directory.Exists)
            {
                throw new DirectoryNotFoundException(FormattableString.Invariant($"Artifact directory {path.FullPath} not found."));
            }

            var files = directory
                            .GetFiles("*", SearchScope.Recursive)
                            .ToArray();

            await CreateAndUploadArtifactFiles(artifactName, path, files);
        }

        private void ValidateUploadArtifactParameters(Path path, string artifactName)
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
        }

        private async Task CreateAndUploadArtifactFiles(
            string artifactName,
            DirectoryPath rootPath,
            params IFile[] files)
        {
            var artifactUrl = string.Concat(
                _actionsEnvironment.Runtime.Url,
                "_apis/pipelines/workflows/",
                _actionsEnvironment.Workflow.RunId,
                "/artifacts?api-version=",
                ApiVersion,
                "&artifactName=",
                Uri.EscapeDataString(artifactName));

            var client = GetRuntimeHttpClient();

            var artifactResponse = await CreateArtifact(artifactName, client, artifactUrl);

            long totalFileSize = 0L;
            foreach (var file in files)
            {
                using var artifactStream = file.OpenRead();
                await UploadFile(rootPath, artifactName, artifactResponse, client, artifactStream, file);
                totalFileSize += file.Length;
            }

            await FinalizeArtifact(client, artifactUrl, totalFileSize);
        }

        private HttpClient GetRuntimeHttpClient([System.Runtime.CompilerServices.CallerMemberName] string memberName = null)
        {
            var client = _createHttpClient(memberName);
            client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(AcceptHeader));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _actionsEnvironment.Runtime.Token);
            return client;
        }

        private static async Task FinalizeArtifact(HttpClient client, string artifactUrl, long totalSize)
        {
            var jsonData = JsonSerializer.SerializeToUtf8Bytes(new PatchArtifactSize(totalSize));

            var patchResponse = await client.SendAsync(
                new HttpRequestMessage(
                    new HttpMethod("PATCH"),
                    artifactUrl)
                {
                    Content = new ByteArrayContent(jsonData)
                    {
                        Headers = { ContentType = MediaTypeHeaderValue.Parse(ContentTypeHeader) }
                    }
                });

            patchResponse.EnsureSuccessStatusCode();
        }

        private static async Task UploadFile(DirectoryPath rootPath, string artifactName, ArtifactResponse artifactResponse, HttpClient client, Stream artifactStream, IFile file)
        {
            var itemPath = string.Concat(
                artifactName,
                "/",
                rootPath.GetRelativePath(file.Path).FullPath);

            var putFileUrl = string.Concat(
                artifactResponse?.FileContainerResourceUrl ?? throw new ArgumentNullException("FileContainerResourceUrl"),
                $"?itemPath={Uri.EscapeDataString(itemPath)}");

            var putResponse = await client.PutAsync(
                putFileUrl,
                new StreamContent(artifactStream)
                {
                    Headers =
                    {
                        ContentType = MediaTypeHeaderValue.Parse("application/octet-stream"),
                        ContentLength = file.Length,
                        ContentRange = new ContentRangeHeaderValue(0, file.Length - 1L, file.Length)
                    }
                });

            if (!putResponse.IsSuccessStatusCode)
            {
                throw new CakeException(
                    FormattableString.Invariant($"Put artifact file {itemPath} failed."),
                    new HttpRequestException(
                        FormattableString.Invariant($"Response status code does not indicate success: {putResponse.StatusCode:d} ({putResponse.ReasonPhrase}).")));
            }
        }

        private static async Task<ArtifactResponse> CreateArtifact(string artifactName, HttpClient client, string artifactUrl)
        {
            var jsonData = JsonSerializer.SerializeToUtf8Bytes(new CreateArtifactParameters(artifactName));
            var response = await client.PostAsync(
                artifactUrl,
                new ByteArrayContent(jsonData)
                {
                    Headers = { ContentType = MediaTypeHeaderValue.Parse(ContentTypeHeader) }
                });

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var artifactResponse = await JsonSerializer.DeserializeAsync<ArtifactResponse>(responseStream)
                                    ?? throw new CakeException("Failed to parse ArtifactResponse");

            return artifactResponse;
        }
    }
}
