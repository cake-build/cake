// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using Cake.Common.Build.GitHubActions.Data;
using Cake.Core;
using Cake.Core.IO;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Cake.Common.Build.GitHubActions.Commands.Artifact
{
    internal record GitHubActionsArtifactService(
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
        ICakeEnvironment Environment,
        IFileSystem FileSystem,
        GitHubActionsEnvironmentInfo ActionsEnvironment,
        Func<string, HttpClient> CreateHttpClient)
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
    {
        private const string JsonContentType = "application/json";
        private const string ZipContentType = "application/zip";
        private static readonly Uri CreateArtifactUrl = new Uri("CreateArtifact", UriKind.Relative);
        private static readonly Uri FinalizeArtifactUrl = new Uri("FinalizeArtifact", UriKind.Relative);
        private static readonly Uri GetSignedArtifactURLUrl = new Uri("GetSignedArtifactURL", UriKind.Relative);
        private static readonly Uri ListArtifactsUrl = new Uri("ListArtifacts", UriKind.Relative);

        internal async Task DownloadArtifactFiles(
            string artifactName,
            DirectoryPath directoryPath)
        {
            var listArtifactsResponse = await ListArtifacts(
                artifactName);

            if (listArtifactsResponse.Artifacts.FirstOrDefault(artifact => artifact.Name == artifactName)
                is { WorkflowRunBackendId.Length: > 0 } and { WorkflowJobRunBackendId.Length: > 0 } artifact)
            {
                var signedArtifactURLResponse = await GetSignedArtifactURL(artifact.WorkflowRunBackendId, artifact.WorkflowJobRunBackendId, artifactName);

                await DownloadArtifact(signedArtifactURLResponse.SignedUrl, directoryPath);
            }
            else
            {
                throw new CakeException($"Artifact {artifactName} not found.");
            }
        }

        private async Task<ListArtifactsResponse> ListArtifacts(
            string nameFilter = null,
            long? idFilter = null)
        {
            GetWorkflowBackendIds(out var workflowRunBackendId, out var workflowJobRunBackendId);

            var listArtifactsRequest = new ListArtifactsRequest(
                workflowRunBackendId,
                workflowJobRunBackendId,
                nameFilter,
                idFilter);

            return await PostArtifactService<ListArtifactsRequest, ListArtifactsResponse>(
                ListArtifactsUrl,
                listArtifactsRequest);
        }

        private async Task DownloadArtifact(string signedUrl, DirectoryPath directoryPath)
        {
            if (string.IsNullOrWhiteSpace(signedUrl))
            {
                throw new ArgumentNullException(nameof(signedUrl));
            }

            using var downloadClient = GetStorageHttpClient();
            using var downloadResponse = await downloadClient.GetAsync(signedUrl);

            if (!downloadResponse.IsSuccessStatusCode)
            {
                throw new CakeException($"Artifact download failed {downloadResponse.StatusCode:F} ({downloadResponse.StatusCode:D}).");
            }

            await using var downloadStream = await downloadResponse.Content.ReadAsStreamAsync();
            using var archive = new ZipArchive(downloadStream, ZipArchiveMode.Read);
            foreach (var entry in archive.Entries)
            {
                var entryPath = directoryPath.CombineWithFilePath(entry.FullName);
                if (FileSystem.GetFile(entryPath).Exists)
                {
                    FileSystem.GetFile(entryPath).Delete();
                }
                else if (FileSystem.GetDirectory(entryPath.GetDirectory()) is { Exists: false } entryDirectory)
                {
                    entryDirectory.Create();
                }
                using var entryStream = entry.Open();
                using var fileStream = FileSystem.GetFile(entryPath).OpenWrite();
                await entryStream.CopyToAsync(fileStream);
            }
        }

        private async Task<GetSignedArtifactURLResponse> GetSignedArtifactURL(
            string workflowRunBackendId,
            string workflowJobRunBackendId,
            string artifactName)
        {
            var getSignedArtifactURLRequest = new GetSignedArtifactURLRequest(
                workflowRunBackendId,
                workflowJobRunBackendId,
                artifactName);

            return await PostArtifactService<GetSignedArtifactURLRequest, GetSignedArtifactURLResponse>(
                GetSignedArtifactURLUrl,
                getSignedArtifactURLRequest);
        }

        internal async Task<string> CreateAndUploadArtifactFiles(
           string artifactName,
           DirectoryPath rootPath,
           params IFile[] files)
        {
            var tempArchivePath = Environment
                                .GetSpecialPath(SpecialPath.LocalTemp)
                                .CombineWithFilePath($"{Guid.NewGuid():n}.zip");

            try
            {
                GetWorkflowBackendIds(out var workflowRunBackendId, out var workflowJobRunBackendId);

                await CreateArtifactArchive(rootPath, files, tempArchivePath);

                (long size, string hash) = GetArtifactArchiveSizeAndHash(tempArchivePath);

                var signedUploadUrl = await CreateArtifact(artifactName, workflowRunBackendId, workflowJobRunBackendId);

                await UploadArtifact(tempArchivePath, size, signedUploadUrl);

                var artifactId = await FinalizeArtifact(artifactName, hash, size, workflowRunBackendId, workflowJobRunBackendId);

                return artifactId;
            }
            finally
            {
                if (FileSystem.GetFile(tempArchivePath).Exists)
                {
                    FileSystem.GetFile(tempArchivePath).Delete();
                }
            }
        }

        private async Task<TResult> PostArtifactService<TParam, TResult>(
            Uri uri,
            TParam param,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = null)
        {
            using var httpClient = GetArtifactsHttpClient();

            var jsonData = JsonSerializer.SerializeToUtf8Bytes(param);

            using var response = await httpClient.PostAsync(
                uri,
                new ByteArrayContent(jsonData)
                {
                    Headers = { ContentType = MediaTypeHeaderValue.Parse(JsonContentType) }
                });

            if (!response.IsSuccessStatusCode)
            {
                throw new CakeException($"Artifact service call {memberName} failed {response.StatusCode:F} ({response.StatusCode:D}).");
            }

            await using var responseStream = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<TResult>(responseStream);
        }

        private async Task<string> CreateArtifact(string artifactName, string workflowRunBackendId, string workflowJobRunBackendId)
        {
            var createArtifactRequest = new CreateArtifactRequest(
                                4,
                                artifactName,
                                workflowRunBackendId,
                                workflowJobRunBackendId);

            var (ok, signedUploadUrl) = await PostArtifactService<CreateArtifactRequest, CreateArtifactResponse>(
                CreateArtifactUrl,
                createArtifactRequest);

            if (!ok)
            {
                throw new CakeException("Artifact creation failed.");
            }

            if (string.IsNullOrWhiteSpace(signedUploadUrl))
            {
                throw new CakeException("Artifact upload url missing.");
            }

            return signedUploadUrl;
        }

        private async Task UploadArtifact(FilePath contentPath, long contentLength, string signedUploadUrl)
        {
            using var uploadClient = GetStorageHttpClient();
            await using var uploadStream = FileSystem.GetFile(contentPath).OpenRead();
            using var uploadContent = new StreamContent(uploadStream)
            {
                Headers =
                        {
                            ContentType = MediaTypeHeaderValue.Parse(ZipContentType),
                            ContentLength = contentLength
                        }
            };
            uploadContent.Headers.TryAddWithoutValidation("x-ms-blob-content-type", ZipContentType);
            uploadContent.Headers.TryAddWithoutValidation("x-ms-blob-type", "BlockBlob");

            using var response = await uploadClient.PutAsync(
                                signedUploadUrl,
                                uploadContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new CakeException($"Artifact upload failed {response.StatusCode:F} ({response.StatusCode:D}).");
            }
        }

        private async Task<string> FinalizeArtifact(
            string artifactName,
            string hash,
            long contentLength,
            string workflowRunBackendId,
            string workflowJobRunBackendId)
        {
            var finalizeArtifactRequest = new FinalizeArtifactRequest(
                                artifactName,
                                hash,
                                contentLength,
                                workflowRunBackendId,
                                workflowJobRunBackendId);

            var (ok, artifactId) = await PostArtifactService<FinalizeArtifactRequest, FinalizeArtifactResponse>(
                FinalizeArtifactUrl,
                finalizeArtifactRequest);

            if (!ok)
            {
                throw new CakeException("Artifact finalization failed.");
            }

            if (string.IsNullOrWhiteSpace(artifactId))
            {
                throw new CakeException("Artifact id missing.");
            }

            return artifactId;
        }

        private (long size, string hash) GetArtifactArchiveSizeAndHash(FilePath tempArchivePath)
        {
            var size = FileSystem.GetFile(tempArchivePath).Length;
            using var stream = FileSystem.GetFile(tempArchivePath).OpenRead();
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(stream);
            var hash = Convert.ToHexString(hashBytes).ToLowerInvariant();
            return (size, hash);
        }

        private void GetWorkflowBackendIds(out string workflowRunBackendId, out string workflowJobRunBackendId)
        {
            try
            {
                var jwt = new JsonWebToken(ActionsEnvironment.Runtime.Token);
                (workflowRunBackendId, workflowJobRunBackendId) = jwt.TryGetClaim("scp", out var scope)
                    ? scope.Value.Split(' ').FirstOrDefault(s => s.StartsWith("Actions.Results:"))?.Split(':') is { Length: 3 } workflowRunBackendParts
                                                    ? (workflowRunBackendParts[1],
                                                        workflowRunBackendParts[2])
                                                    : default
                    : default;

                if (string.IsNullOrWhiteSpace(workflowRunBackendId))
                {
                    throw new CakeException("GitHub Actions Workflow Token workflowRunBackendId missing.");
                }

                if (string.IsNullOrWhiteSpace(workflowJobRunBackendId))
                {
                    throw new CakeException("GitHub Actions Workflow Token workflowJobRunBackendId missing.");
                }
            }
            catch (Exception ex)
            {
                throw new CakeException("GitHub Actions Workflow Token invalid.", ex);
            }
        }

        private async Task CreateArtifactArchive(DirectoryPath rootPath, IFile[] files, FilePath tempArchivePath)
        {
            if (FileSystem.GetDirectory(tempArchivePath.GetDirectory()) is { Exists: false } tempArchiveDirectory)
            {
                tempArchiveDirectory.Create();
            }

            using var archiveStream = FileSystem.GetFile(tempArchivePath).OpenWrite();
            using var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create);
            foreach (var file in files)
            {
                var relativePath = rootPath.GetRelativePath(file.Path.GetDirectory());
                var entry = archive.CreateEntry(relativePath.CombineWithFilePath(file.Path.GetFilename()).FullPath, CompressionLevel.SmallestSize);
                using var entryStream = entry.Open();
                using var fileStream = file.OpenRead();
                await fileStream.CopyToAsync(entryStream);
            }
        }

        private HttpClient GetArtifactsHttpClient([System.Runtime.CompilerServices.CallerMemberName] string memberName = null)
        {
            var client = CreateHttpClient(memberName);
            client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(JsonContentType));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ActionsEnvironment.Runtime.Token);
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Cake", Environment.Runtime.CakeVersion.ToString()));
            client.BaseAddress = new Uri(string.Concat(
                                    ActionsEnvironment.Runtime.ResultsUrl,
                                    "twirp/github.actions.results.api.v1.ArtifactService/"));
            return client;
        }

        private HttpClient GetStorageHttpClient([System.Runtime.CompilerServices.CallerMemberName] string memberName = null)
        {
            var client = CreateHttpClient(memberName);
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Cake", Environment.Runtime.CakeVersion.ToString()));
            return client;
        }
    }
}