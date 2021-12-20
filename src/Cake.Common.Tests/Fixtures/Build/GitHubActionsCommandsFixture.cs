using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Cake.Common.Build.GitHubActions.Commands;
using Cake.Common.Build.GitHubActions.Data;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class GitHubActionsCommandsFixture : HttpMessageHandler
    {
        private const string ApiVersion = "6.0-preview";
        private const string AcceptHeader = "application/json; api-version=" + ApiVersion;
        private const string CreateArtifactUrl = GitHubActionsInfoFixture.ActionRuntimeUrl +
                                                 "_apis/pipelines/workflows/34058136/artifacts?api-version=" + ApiVersion + "&artifactName=artifact";
        private const string CreateArtifactsUrl = GitHubActionsInfoFixture.ActionRuntimeUrl +
                                                 "_apis/pipelines/workflows/34058136/artifacts?api-version=" + ApiVersion + "&artifactName=artifacts";
        private const string PutFileUrl = GitHubActionsInfoFixture.ActionRuntimeUrl +
                                          "_apis/resources/Containers/942031?itemPath=artifact%2Fartifact.txt";
        private const string CreateArtifactResponse = @"{
    ""containerId"": 942031,
    ""size"": -1,
    ""signedContent"": null,
    ""fileContainerResourceUrl"": """ + GitHubActionsInfoFixture.ActionRuntimeUrl + @"_apis/resources/Containers/942031"",
    ""type"": ""actions_storage"",
    ""name"": ""artifact"",
    ""url"": """ + GitHubActionsInfoFixture.ActionRuntimeUrl + @"_apis/pipelines/1/runs/7/artifacts?artifactName=artifact"",
    ""expiresOn"": ""2021-12-14T18:43:29.7431144Z"",
    ""items"": null
}";
        private const string CreateArtifactsResponse = @"{
    ""containerId"": 942031,
    ""size"": -1,
    ""signedContent"": null,
    ""fileContainerResourceUrl"": """ + GitHubActionsInfoFixture.ActionRuntimeUrl + @"_apis/resources/Containers/942031"",
    ""type"": ""actions_storage"",
    ""name"": ""artifact"",
    ""url"": """ + GitHubActionsInfoFixture.ActionRuntimeUrl + @"_apis/pipelines/1/runs/7/artifacts?artifactName=artifacts"",
    ""expiresOn"": ""2021-12-14T18:43:29.7431144Z"",
    ""items"": null
}";

        private const string PutDirectoryRootUrl = GitHubActionsInfoFixture.ActionRuntimeUrl +
                                  "_apis/resources/Containers/942031?itemPath=artifacts%2Fartifact.txt";
        private const string PutDirectoryFolderAUrl = GitHubActionsInfoFixture.ActionRuntimeUrl +
                                          "_apis/resources/Containers/942031?itemPath=artifacts%2Ffolder_a%2Fartifact.txt";
        private const string PutDirectoryFolderBUrl = GitHubActionsInfoFixture.ActionRuntimeUrl +
                                          "_apis/resources/Containers/942031?itemPath=artifacts%2Ffolder_b%2Fartifact.txt";
        private const string PutDirectoryFolderBFolderCUrl = GitHubActionsInfoFixture.ActionRuntimeUrl +
                                          "_apis/resources/Containers/942031?itemPath=artifacts%2Ffolder_b%2Ffolder_c%2Fartifact.txt";

        private GitHubActionsInfoFixture GitHubActionsInfoFixture { get; }
        private ICakeEnvironment Environment { get; }
        public FakeFileSystem FileSystem { get; }

        public GitHubActionsCommandsFixture()
        {
            GitHubActionsInfoFixture = new GitHubActionsInfoFixture();
            FileSystem = new FakeFileSystem(GitHubActionsInfoFixture.Environment);
            FileSystem.CreateDirectory("/opt");
            Environment = GitHubActionsInfoFixture.Environment;
        }

        public GitHubActionsCommands CreateGitHubActionsCommands()
        {
            return new GitHubActionsCommands(Environment, FileSystem, GitHubActionsInfoFixture.CreateEnvironmentInfo(), CreateClient);
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

        public GitHubActionsCommandsFixture WithNoGitHubPath()
        {
            Environment.GetEnvironmentVariable("GITHUB_PATH").Returns(null as string);
            return this;
        }

        private HttpClient CreateClient(string name) => new HttpClient(this);

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization is null || request.Headers.Authorization.Scheme != "Bearer" || request.Headers.Authorization.Parameter != GitHubActionsInfoFixture.ActionRuntimeToken)
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized
                };
            }

            if (!request.Headers.TryGetValues("Accept", out var values) || !values.Contains(AcceptHeader))
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            switch (request)
            {
#pragma warning disable SA1013
                // FilePath
                case
                {
                    RequestUri: { AbsoluteUri: CreateArtifactUrl },
                    Method: { Method: "POST" },
                }:
                    {
                        return Ok(new StringContent(CreateArtifactResponse));
                    }

                // DirectoryPath
                case
                {
                    RequestUri: { AbsoluteUri: CreateArtifactsUrl },
                    Method: { Method: "POST" },
                }:
                    {
                        return Ok(new StringContent(CreateArtifactsResponse));
                    }

                // FilePath
                case
                {
                    RequestUri: { AbsoluteUri: PutFileUrl },
                    Method: { Method: "PUT" }
                }:
                case
                {
                    RequestUri: { AbsoluteUri: CreateArtifactUrl },
                    Method: { Method: "PATCH" },
                }:

                // DirectoryPath
                case
                {
                    RequestUri: { AbsoluteUri: PutDirectoryRootUrl },
                    Method: { Method: "PUT" }
                }:
                case
                {
                    RequestUri: { AbsoluteUri: PutDirectoryFolderAUrl },
                    Method: { Method: "PUT" }
                }:
                case
                {
                    RequestUri: { AbsoluteUri: PutDirectoryFolderBUrl },
                    Method: { Method: "PUT" }
                }:
                case
                {
                    RequestUri: { AbsoluteUri: PutDirectoryFolderBFolderCUrl },
                    Method: { Method: "PUT" }
                }:
                case
                {
                    RequestUri: { AbsoluteUri: CreateArtifactsUrl },
                    Method: { Method: "PATCH" },
                }:
                    {
                        return Ok();
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