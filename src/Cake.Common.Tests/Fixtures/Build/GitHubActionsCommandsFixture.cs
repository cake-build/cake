using System;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Cake.Common.Build.GitHubActions.Commands;
using Cake.Common.Build.GitHubActions.Commands.Artifact;
using Cake.Common.Tests.Fakes;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class GitHubActionsCommandsFixture : HttpMessageHandler
    {
        private const string AcceptHeaderResults = "application/json";
        private const string ArtifactUrl = GitHubActionsInfoFixture.ActionResultsUrl + "twirp/github.actions.results.api.v1.ArtifactService/";
        private const string CreateArtifactUrl = ArtifactUrl + "CreateArtifact";
        private const string FinalizeArtifactUrl = ArtifactUrl + "FinalizeArtifact";
        private const string GetSignedArtifactURLUrl = ArtifactUrl + "GetSignedArtifactURL";
        private const string ListArtifactsUrl = ArtifactUrl + "ListArtifacts";
        private const string UploadFileUrl = "https://cake.build.net/actions-results/a9d82106-d5d5-4310-8f60-0bfac035cf02/workflow-job-run-1d849a45-2f30-5fbb-3226-b730a17a93af/artifacts/91e64594182918fa8012cdbf7d1a4f801fa0c35f485c3277268aad8e3f45377c.zip?sig=upload";
        private const string DownloadFileUrl = "https://cake.build.net/actions-results/a9d82106-d5d5-4310-8f60-0bfac035cf02/workflow-job-run-1d849a45-2f30-5fbb-3226-b730a17a93af/artifacts/91e64594182918fa8012cdbf7d1a4f801fa0c35f485c3277268aad8e3f45377c.zip?sig=download";
        private const string CreateArtifactResponse =
            $$"""
            {
                "ok": true,
                "signed_upload_url": "{{UploadFileUrl}}"
            }
            """;
        private const string FinalizeArtifactResponse =
            """
            {
                "ok": true,
                "artifact_id": "1991105334"
            }
            """;
        private const string GetSignedArtifactURLResponse =
            $$"""
            { 
                "name": "artifact",
                "signed_url": "{{DownloadFileUrl}}"
            }
            """;
        private const string ListArtifactsResponse =
            $$"""
            {
              "artifacts": [
                {
                  "workflow_run_backend_id": "b9e28153-ca20-4b86-91dd-09e8f644efdf",
                  "workflow_job_run_backend_id": "1d849a45-2f30-5fbb-3226-b730a17a93af",
                  "database_id": "1",
                  "name": "artifact",
                  "created_at": "2024-11-09T21:53:00.7110204+00:00"
                }
              ]
            }
            """;

        private GitHubActionsInfoFixture GitHubActionsInfoFixture { get; }
        private ICakeEnvironment Environment { get; }
        public FakeFileSystem FileSystem { get; }
        public FakeBuildSystemServiceMessageWriter Writer { get; }

        public GitHubActionsCommandsFixture()
        {
            GitHubActionsInfoFixture = new GitHubActionsInfoFixture();
            FileSystem = new FakeFileSystem(GitHubActionsInfoFixture.Environment);
            FileSystem.CreateDirectory("/opt");
            Environment = GitHubActionsInfoFixture.Environment;
            Writer = new FakeBuildSystemServiceMessageWriter();
        }

        public GitHubActionsCommands CreateGitHubActionsCommands()
        {
            return new GitHubActionsCommands(Environment, FileSystem, Writer, GitHubActionsInfoFixture.CreateEnvironmentInfo(), CreateClient);
        }

        public GitHubActionsCommandsFixture WithWorkingDirectory(DirectoryPath workingDirectory)
        {
            Environment.WorkingDirectory = workingDirectory;
            return this;
        }

        public GitHubActionsCommandsFixture WithNoGitHubEnv()
        {
            Environment.GetEnvironmentVariable("GITHUB_ENV").Returns(null as string);
            return this;
        }

        public GitHubActionsCommandsFixture WithNoGitHubOutput()
        {
            Environment.GetEnvironmentVariable("GITHUB_OUTPUT").Returns(null as string);
            return this;
        }

        public GitHubActionsCommandsFixture WithNoGitHubStepSummary()
        {
            Environment.GetEnvironmentVariable("GITHUB_STEP_SUMMARY").Returns(null as string);
            return this;
        }

        public GitHubActionsCommandsFixture WithNoGitHubPath()
        {
            Environment.GetEnvironmentVariable("GITHUB_PATH").Returns(null as string);
            return this;
        }

        private HttpClient CreateClient(string name) => new HttpClient(this);

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri.AbsoluteUri == DownloadFileUrl)
            {
            }
            else if (request.RequestUri.AbsoluteUri == UploadFileUrl)
            {
                if (
                    !request.Content.Headers.TryGetValues("x-ms-blob-content-type", out var contentTypes)
                    || !contentTypes.Contains("application/zip")
                    || !request.Content.Headers.TryGetValues("x-ms-blob-type", out var blobTypes)
                    || !blobTypes.Contains("BlockBlob"))
                {
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.Unauthorized
                    };
                }
            }
            else if (request.Headers.Authorization is null || request.Headers.Authorization.Scheme != "Bearer" || request.Headers.Authorization.Parameter != GitHubActionsInfoFixture.ActionRuntimeToken)
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized
                };
            }

            switch (request)
            {
#pragma warning disable SA1013
                // Get Signed Artifact Url
                case
                {
                    RequestUri: { AbsoluteUri: GetSignedArtifactURLUrl },
                    Method: { Method: "POST" },
                }:
                    {
                        using var getSignedArtifactURLRequestStream = await request.Content.ReadAsStreamAsync(cancellationToken);
                        var getSignedArtifactURLRequest = await System.Text.Json.JsonSerializer.DeserializeAsync<GetSignedArtifactURLRequest>(getSignedArtifactURLRequestStream, cancellationToken: cancellationToken);
                        return getSignedArtifactURLRequest switch
                        {
                            { Name: { Length: >0}, WorkflowJobRunBackendId: { Length: >0}, WorkflowRunBackendId: { Length: >0 } } => Ok(new StringContent(GetSignedArtifactURLResponse)),
                            _ => new HttpResponseMessage
                            {
                                StatusCode = HttpStatusCode.BadRequest
                            }
                        };
                    }

                // Create Artifact
                case
                {
                    RequestUri: { AbsoluteUri: CreateArtifactUrl },
                    Method: { Method: "POST" },
                }:
                    {
                        using var createArtifactRequestStream = await request.Content.ReadAsStreamAsync(cancellationToken);
                        var createArtifactRequest = await System.Text.Json.JsonSerializer.DeserializeAsync<CreateArtifactRequest>(createArtifactRequestStream, cancellationToken: cancellationToken);

                        return createArtifactRequest switch
                        {
                            { Version: 4, Name: "artifact", } => Ok(new StringContent(CreateArtifactResponse)),
                            { Version: 4, Name: "artifacts", } => Ok(new StringContent(CreateArtifactResponse)),
                            _ => new HttpResponseMessage
                                {
                                    StatusCode = HttpStatusCode.BadRequest
                                }
                        };
                    }
                // Finalize Artifact
                case
                {
                    RequestUri: { AbsoluteUri: FinalizeArtifactUrl },
                    Method: { Method: "POST" },
                }:
                    {
                        using var createArtifactRequestStream = await request.Content.ReadAsStreamAsync(cancellationToken);
                        var finalizeArtifactRequest = await System.Text.Json.JsonSerializer.DeserializeAsync<FinalizeArtifactRequest>(createArtifactRequestStream, cancellationToken: cancellationToken);

                        return finalizeArtifactRequest switch
                        {
                            { Hash: { Length: > 0},  Size: >0, Name: "artifact", } => Ok(new StringContent(FinalizeArtifactResponse)),
                            { Hash: { Length: > 0 }, Size: > 0, Name: "artifacts", } => Ok(new StringContent(FinalizeArtifactResponse)),
                            _ => new HttpResponseMessage
                            {
                                StatusCode = HttpStatusCode.BadRequest
                            }
                        };
                    }

                // Upload File
                case
                {
                    RequestUri: { AbsoluteUri: UploadFileUrl },
                    Method: { Method: "PUT" }
                }:
                    {
                        return Ok();
                    }

                // List Artifacts
                case
                {
                    RequestUri: { AbsoluteUri: ListArtifactsUrl },
                    Method: { Method: "POST" }
                }:
                    {
                        return Ok(new StringContent(ListArtifactsResponse));
                    }

                // Download File
                case
                {
                    RequestUri: { AbsoluteUri: DownloadFileUrl },
                    Method: { Method: "GET" }
                }:
                    {
                        await using var stream = new System.IO.MemoryStream();
                        using (var zip = new ZipArchive(stream, ZipArchiveMode.Create, true))
                        {
                            var entry = zip.CreateEntry("test.txt");
                            using var entryStream = entry.Open();
                            using var writer = new System.IO.StreamWriter(entryStream);
                            writer.Write("Cake");
                        }
                        return Ok(new ByteArrayContent(stream.ToArray()));
                    }
#pragma warning restore SA1013

                default:
                    {
                        await Task.Delay(1, cancellationToken);
                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.NotFound
                        };
                    }
            }
        }

        private static HttpResponseMessage Ok(HttpContent content = null)
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                ReasonPhrase = "OK",
                Content = content
            };
        }
    }
}