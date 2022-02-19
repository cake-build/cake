// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.GitHubActions.Data;
using Cake.Core;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class GitHubActionsInfoFixture
    {
        public const string ActionRuntimeToken = "zht1j5NeW2T5ZsOxncX4CUEiWYhD4ZRwoDghkARk";
        public const string ActionRuntimeUrl = "https://pipelines.actions.githubusercontent.com/ip0FyYnZXxdEOcOwPHkRsZJd2x6G5XoT486UsAb0/";
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
            Environment.GetEnvironmentVariable("GITHUB_ENV").Returns("/opt/github.env");
            Environment.GetEnvironmentVariable("GITHUB_PATH").Returns("/opt/github.path");
            Environment.WorkingDirectory.Returns("/home/runner/work/cake/cake");
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
