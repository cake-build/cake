// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Build.GitHubActions.Data;
using Cake.Core;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class GitHubActionsInfoFixture
    {
        public const string ActionRuntimeToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6Ikh5cTROQVRBanNucUM3bWRydEFoaHJDUjJfUSJ9.eyJuYW1laWQiOiJkZGRkZGRkZC1kZGRkLWRkZGQtZGRkZC1kZGRkZGRkZGRkZGQiLCJzY3AiOiJBY3Rpb25zLkdlbmVyaWNSZWFkOjAwMDAwMDAwLTAwMDAtMDAwMC0wMDAwLTAwMDAwMDAwMDAwMCBBY3Rpb25zLlJlc3VsdHM6YjllMjgxNTMtY2EyMC00Yjg2LTkxZGQtMDllOGY2NDRlZmRmOjFkODQ5YTQ1LTJmMzAtNWZiYi0zMjI2LWI3MzBhMTdhOTNhZiBBY3Rpb25zLlVwbG9hZEFydGlmYWN0czowMDAwMDAwMC0wMDAwLTAwMDAtMDAwMC0wMDAwMDAwMDAwMDAvMTpCdWlsZC9CdWlsZC8xNiBMb2NhdGlvblNlcnZpY2UuQ29ubmVjdCBSZWFkQW5kVXBkYXRlQnVpbGRCeVVyaTowMDAwMDAwMC0wMDAwLTAwMDAtMDAwMC0wMDAwMDAwMDAwMDAvMTpCdWlsZC9CdWlsZC8xNiIsIklkZW50aXR5VHlwZUNsYWltIjoiU3lzdGVtOlNlcnZpY2VJZGVudGl0eSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL3NpZCI6IkRERERERERELUREREQtRERERC1ERERELURERERERERERERERCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcHJpbWFyeXNpZCI6ImRkZGRkZGRkLWRkZGQtZGRkZC1kZGRkLWRkZGRkZGRkZGRkZCIsImF1aSI6ImUyMTI4OTY1LThlY2EtNDgxYy1hODhkLWJmOTFlZDg3Y2RiNSIsInNpZCI6ImMwNmVjY2E0LWY3ZjUtNGY4Mi1iM2IxLTJhYjM0M2Y4Mjg3NCIsImFjIjoiW3tcIlNjb3BlXCI6XCJyZWZzL2hlYWRzL21haW5cIixcIlBlcm1pc3Npb25cIjozfV0iLCJhY3NsIjoiMTAiLCJvcmNoaWQiOiJiOWUyODE1My1jYTIwLTRiODYtOTFkZC0wOWU4ZjY0NGVmZGYuYnVpbGQudWJ1bnR1LWxhdGVzdCIsImlzcyI6InZzdG9rZW4uYWN0aW9ucy5naXRodWJ1c2VyY29udGVudC5jb20iLCJhdWQiOiJ2c3Rva2VuLmFjdGlvbnMuZ2l0aHVidXNlcmNvbnRlbnQuY29tfHZzbzo0M2YwNTdkMC0wODAzLTRkOTEtOTRhMS1mOGViMTAzZGYxMWYiLCJuYmYiOjE3Mjc1NDQzOTIsImV4cCI6MTcyNzU2NzE5Mn0.sUTvwxD-NlbAhQJB7cIInovd9qDkFHWcwOiiQAlHCsjpRBCEUWb3tWfOmCEpn8It4FWkaSszjMd8oecBEMlyEUtk6Cm6l1AqCUnIT13B48c_2sjhjWz-UDNMt94nzYH2ulC8mBcV_kSEIHJUvOnFKrFMKEdg6axAjLCx4la9MOklVq2ehx6DC12qbUNpTELJGeWz_JvKHWexyfN1qJgUw3y4ritZDJF3HLTpb5IJS7sQmFZVB7F2P6DF-1iaCBX5hgA9KfiwWXw6oTkKd6aOEyJpcBe0b87V_-fVTivOUS-ABE5XN6TCLZSmt7X6qwTPeSoLKgQGx1h_tHwubGDjtQ";
        public const string ActionRuntimeUrl = "https://pipelines.actions.githubusercontent.com/ip0FyYnZXxdEOcOwPHkRsZJd2x6G5XoT486UsAb0/";
        public const string ActionResultsUrl = "https://results-receiver.actions.githubusercontent.com/";
        private readonly Version CakeTestVersion = new Version(1, 2, 3, 4);

        public ICakeEnvironment Environment { get; }

        public GitHubActionsInfoFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();

            Environment.GetEnvironmentVariable("GITHUB_ACTIONS").Returns("true");
            Environment.GetEnvironmentVariable("HOME").Returns("/home/runner");

            Environment.GetEnvironmentVariable("RUNNER_NAME").Returns("RunnerName");
            Environment.GetEnvironmentVariable("RUNNER_OS").Returns("Linux");
            Environment.GetEnvironmentVariable("RUNNER_TEMP").Returns("/home/runner/work/_temp");
            Environment.GetEnvironmentVariable("RUNNER_TOOL_CACHE").Returns("/opt/hostedtoolcache");
            Environment.GetEnvironmentVariable("RUNNER_WORKSPACE").Returns("/home/runner/work/cake");
            Environment.GetEnvironmentVariable("ImageOS").Returns("ubuntu20");
            Environment.GetEnvironmentVariable("ImageVersion").Returns("20211209.3");
            Environment.GetEnvironmentVariable("RUNNER_USER").Returns("runner");

            Environment.GetEnvironmentVariable("GITHUB_ACTION").Returns("run1");
            Environment.GetEnvironmentVariable("GITHUB_ACTION_PATH").Returns("/path/to/action");
            Environment.GetEnvironmentVariable("GITHUB_ACTOR").Returns("dependabot");
            Environment.GetEnvironmentVariable("GITHUB_API_URL").Returns("https://api.github.com");
            Environment.GetEnvironmentVariable("GITHUB_BASE_REF").Returns("master");
            Environment.GetEnvironmentVariable("GITHUB_EVENT_NAME").Returns("pull_request");
            Environment.GetEnvironmentVariable("GITHUB_EVENT_PATH").Returns("/home/runner/work/_temp/_github_workflow/event.json");
            Environment.GetEnvironmentVariable("GITHUB_GRAPHQL_URL").Returns("https://api.github.com/graphql");
            Environment.GetEnvironmentVariable("GITHUB_HEAD_REF").Returns("dependabot/nuget/Microsoft.SourceLink.GitHub-1.0.0");
            Environment.GetEnvironmentVariable("GITHUB_JOB").Returns("job");
            Environment.GetEnvironmentVariable("GITHUB_REF").Returns("refs/pull/1/merge");
            Environment.GetEnvironmentVariable("GITHUB_REPOSITORY").Returns("cake-build/cake");
            Environment.GetEnvironmentVariable("GITHUB_REPOSITORY_OWNER").Returns("cake-build");
            Environment.GetEnvironmentVariable("GITHUB_RUN_ID").Returns("34058136");
            Environment.GetEnvironmentVariable("GITHUB_RUN_NUMBER").Returns("60");
            Environment.GetEnvironmentVariable("GITHUB_SERVER_URL").Returns("https://github.com");
            Environment.GetEnvironmentVariable("GITHUB_SHA").Returns("d1e4f990f57349334368c8253382abc63be02d73");
            Environment.GetEnvironmentVariable("GITHUB_WORKFLOW").Returns("Build");
            Environment.GetEnvironmentVariable("GITHUB_WORKSPACE").Returns("/home/runner/work/cake/cake");
            Environment.GetEnvironmentVariable("GITHUB_RUN_ATTEMPT").Returns("2");
            Environment.GetEnvironmentVariable("GITHUB_REF_PROTECTED").Returns("true");
            Environment.GetEnvironmentVariable("GITHUB_REF_NAME").Returns("main");

            Environment.GetEnvironmentVariable("ACTIONS_RUNTIME_TOKEN").Returns(ActionRuntimeToken);
            Environment.GetEnvironmentVariable("ACTIONS_RUNTIME_URL").Returns(ActionRuntimeUrl);
            Environment.GetEnvironmentVariable("ACTIONS_RESULTS_URL").Returns(ActionResultsUrl);
            Environment.GetEnvironmentVariable("GITHUB_ENV").Returns("/opt/github.env");
            Environment.GetEnvironmentVariable("GITHUB_OUTPUT").Returns("/opt/github.output");
            Environment.GetEnvironmentVariable("GITHUB_STEP_SUMMARY").Returns("/opt/github.stepsummary");
            Environment.GetEnvironmentVariable("GITHUB_PATH").Returns("/opt/github.path");
            Environment.WorkingDirectory.Returns("/home/runner/work/cake/cake");

            Environment.GetSpecialPath(Core.IO.SpecialPath.LocalTemp).Returns("/tmp");
            Environment.Runtime.CakeVersion.Returns(CakeTestVersion);
        }

        public GitHubActionsRunnerInfo CreateRunnerInfo(string architecture = null)
        {
            Environment.GetEnvironmentVariable("RUNNER_ARCH").Returns(architecture);
            return new GitHubActionsRunnerInfo(Environment);
        }

        public GitHubActionsWorkflowInfo CreateWorkflowInfo(string refType = null)
        {
            Environment.GetEnvironmentVariable("GITHUB_REF_TYPE").Returns(refType);
            return new GitHubActionsWorkflowInfo(Environment);
        }

        public GitHubActionsPullRequestInfo CreatePullRequestInfo()
        {
            return new GitHubActionsPullRequestInfo(Environment);
        }

        public GitHubActionsEnvironmentInfo CreateEnvironmentInfo()
        {
            return new GitHubActionsEnvironmentInfo(Environment);
        }

        public GitHubActionsRuntimeInfo CreateRuntimeInfo()
        {
            return new GitHubActionsRuntimeInfo(Environment);
        }
    }
}
