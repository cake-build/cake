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
        private const string AcceptGzip = "application/octet-stream; api-version=" + ApiVersion;
        private const string AcceptEncodingGzip = "gzip";
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

        private const string GetArtifactResourceUrl = GitHubActionsInfoFixture.ActionRuntimeUrl +
            "_apis/pipelines/workflows/34058136/artifacts?api-version=6.0-preview&artifactName=artifact";
        private const string FileContainerResourceUrl = GitHubActionsInfoFixture.ActionRuntimeUrl + @"_apis/resources/Containers/4794789";
        private const string GetArtifactResourceResponse = @"{
    ""count"": 1,
    ""value"": [
        {
            ""containerId"": 4794789,
            ""size"": 4,
            ""signedContent"": null,
            ""fileContainerResourceUrl"": """ + FileContainerResourceUrl + @""",
            ""type"": ""actions_storage"",
            ""name"": ""artifact"",
            ""url"": """ + GitHubActionsInfoFixture.ActionRuntimeUrl + @"_apis/pipelines/1/runs/7/artifacts?artifactName=artifact"",
            ""expiresOn"": ""2022-03-16T08:22:01.5699067Z"",
            ""items"": null
        }
    ]
}";

        private const string GetContainerItemResourcesUrl = FileContainerResourceUrl + "?itemPath=artifact";
        private const string GetContainerItemResourcesResponse = @"{
    ""count"": 1,
    ""value"": [
        {
            ""containerId"": 4794789,
            ""scopeIdentifier"": ""00000000-0000-0000-0000-000000000000"",
            ""path"": ""artifact/test.txt"",
            ""itemType"": ""file"",
            ""status"": ""created"",
            ""fileLength"": 4,
            ""fileEncoding"": 1,
            ""fileType"": 1,
            ""dateCreated"": ""2021-12-16T09:05:18.803Z"",
            ""dateLastModified"": ""2021-12-16T09:05:18.907Z"",
            ""createdBy"": ""2daeb16b-86ae-4e46-ba89-92a8aa076e52"",
            ""lastModifiedBy"": ""2daeb16b-86ae-4e46-ba89-92a8aa076e52"",
            ""itemLocation"": """ + GetContainerItemResourcesUrl + @"%2Ftest.txt&metadata=True"",
            ""contentLocation"": """ + GetContainerItemResourcesUrl + @"%2Ftest.txt"",
            ""fileId"": 1407,
            ""contentId"": """"
        }
    ]
}";

        private const string DownloadItemResourceUrl = GetContainerItemResourcesUrl + "%2Ftest.txt";
        private const string DownloadItemResourceResponse = "Cake";

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

            if (
                !request.Headers.TryGetValues("Accept", out var values)
                || !values.Contains(AcceptHeader))
            {
                if (request.RequestUri.AbsoluteUri != DownloadItemResourceUrl
                    || !values.Contains(AcceptGzip)
                    || !request.Headers.TryGetValues("Accept-Encoding", out var encodingValues)
                    || !encodingValues.Contains(AcceptEncodingGzip))
                {
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
            }

            switch (request)
            {
#pragma warning disable SA1013
                // Create Artifact FilePath
                case
                {
                    RequestUri: { AbsoluteUri: CreateArtifactUrl },
                    Method: { Method: "POST" },
                }:
                    {
                        return Ok(new StringContent(CreateArtifactResponse));
                    }

                // Create Artifact DirectoryPath
                case
                {
                    RequestUri: { AbsoluteUri: CreateArtifactsUrl },
                    Method: { Method: "POST" },
                }:
                    {
                        return Ok(new StringContent(CreateArtifactsResponse));
                    }

                // Download Artifact - Get Artifact Container Resource
                case
                {
                    RequestUri: { AbsoluteUri: GetArtifactResourceUrl },
                    Method: { Method: "GET" }
                }:
                    {
                        return Ok(new StringContent(GetArtifactResourceResponse));
                    }

                // Download Artifact - Get Artifact Container Item Resource
                case
                {
                    RequestUri: { AbsoluteUri: GetContainerItemResourcesUrl },
                    Method: { Method: "GET" }
                }:
                    {
                        return Ok(new StringContent(GetContainerItemResourcesResponse));
                    }

                // Download Artifact - DownloadItemResource
                case
                {
                    RequestUri: { AbsoluteUri: DownloadItemResourceUrl },
                    Method: { Method: "GET" }
                }:
                    {
                        return Ok(new StringContent(DownloadItemResourceResponse));
                    }

                // Put FilePath
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

                // Put DirectoryPath
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