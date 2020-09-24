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
        public ICakeEnvironment Environment { get; set; }

        public GitHubActionsInfoFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();

            Environment.GetEnvironmentVariable("GITHUB_ACTIONS").Returns("true");
            Environment.GetEnvironmentVariable("HOME").Returns("/home/runner");

            Environment.GetEnvironmentVariable("RUNNER_OS").Returns("Linux");
            Environment.GetEnvironmentVariable("RUNNER_TEMP").Returns("/home/runner/work/_temp");
            Environment.GetEnvironmentVariable("RUNNER_TOOL_CACHE").Returns("/opt/hostedtoolcache");
            Environment.GetEnvironmentVariable("RUNNER_WORKSPACE").Returns("/home/runner/work/cake");

            Environment.GetEnvironmentVariable("GITHUB_ACTION").Returns("run1");
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
        }

        public GitHubActionsRunnerInfo CreateRunnerInfo()
        {
            return new GitHubActionsRunnerInfo(Environment);
        }

        public GitHubActionsWorkflowInfo CreateWorkflowInfo()
        {
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
    }
}
