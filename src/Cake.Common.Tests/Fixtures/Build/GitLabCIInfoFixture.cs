// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.GitLabCI.Data;
using Cake.Core;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class GitLabCIInfoFixture
    {
        public ICakeEnvironment Environment { get; set; }

        public GitLabCIInfoFixture(bool versionNineOrNewer = false)
        {
            Environment = Substitute.For<ICakeEnvironment>();

            // Example values taken from https://docs.gitlab.com/ce/ci/variables/README.html
            Environment.GetEnvironmentVariable("CI_SERVER").Returns("yes");
            if (versionNineOrNewer)
            {
                Environment.GetEnvironmentVariable("CI_JOB_ID").Returns("50");
                Environment.GetEnvironmentVariable("CI_COMMIT_SHA").Returns("1ecfd275763eff1d6b4844ea3168962458c9f27a");
                Environment.GetEnvironmentVariable("CI_COMMIT_REF_NAME").Returns("master");
                Environment.GetEnvironmentVariable("CI_REPOSITORY_URL").Returns("https://gitab-ci-token:abcde-1234ABCD5678ef@gitlab.com/gitlab-org/gitlab-ce.git");
                Environment.GetEnvironmentVariable("CI_COMMIT_TAG").Returns("1.0.0");
                Environment.GetEnvironmentVariable("CI_JOB_NAME").Returns("spec:other");
                Environment.GetEnvironmentVariable("CI_JOB_STAGE").Returns("test");
                Environment.GetEnvironmentVariable("CI_JOB_MANUAL").Returns("true");
                Environment.GetEnvironmentVariable("CI_PIPELINE_TRIGGERED").Returns("true");
                Environment.GetEnvironmentVariable("CI_JOB_TOKEN").Returns("abcde-1234ABCD5678ef");
            }
            else
            {
                Environment.GetEnvironmentVariable("CI_BUILD_ID").Returns("50");
                Environment.GetEnvironmentVariable("CI_BUILD_REF").Returns("1ecfd275763eff1d6b4844ea3168962458c9f27a");
                Environment.GetEnvironmentVariable("CI_BUILD_REF_NAME").Returns("master");
                Environment.GetEnvironmentVariable("CI_BUILD_REPO").Returns("https://gitab-ci-token:abcde-1234ABCD5678ef@gitlab.com/gitlab-org/gitlab-ce.git");
                Environment.GetEnvironmentVariable("CI_BUILD_TAG").Returns("1.0.0");
                Environment.GetEnvironmentVariable("CI_BUILD_NAME").Returns("spec:other");
                Environment.GetEnvironmentVariable("CI_BUILD_STAGE").Returns("test");
                Environment.GetEnvironmentVariable("CI_BUILD_MANUAL").Returns("true");
                Environment.GetEnvironmentVariable("CI_BUILD_TRIGGERED").Returns("true");
                Environment.GetEnvironmentVariable("CI_BUILD_TOKEN").Returns("abcde-1234ABCD5678ef");
            }
            Environment.GetEnvironmentVariable("CI_MERGE_REQUEST_ID").Returns("10");
            Environment.GetEnvironmentVariable("CI_MERGE_REQUEST_IID").Returns("1");
            Environment.GetEnvironmentVariable("CI_PIPELINE_ID").Returns("1000");
            Environment.GetEnvironmentVariable("CI_PIPELINE_IID").Returns("100");
            Environment.GetEnvironmentVariable("CI_PROJECT_ID").Returns("34");
            Environment.GetEnvironmentVariable("CI_PROJECT_DIR").Returns("/builds/gitlab-org/gitlab-ce");
            Environment.GetEnvironmentVariable("CI_PROJECT_NAME").Returns("gitlab-ce");
            Environment.GetEnvironmentVariable("CI_PROJECT_NAMESPACE").Returns("gitlab-org");
            Environment.GetEnvironmentVariable("CI_PROJECT_PATH").Returns("gitlab-org/gitlab-ce");
            Environment.GetEnvironmentVariable("CI_PROJECT_URL").Returns("https://gitlab.com/gitlab-org/gitlab-ce");
            Environment.GetEnvironmentVariable("CI_REGISTRY").Returns("registry.gitlab.com");
            Environment.GetEnvironmentVariable("CI_REGISTRY_IMAGE").Returns("registry.gitlab.com/gitlab-org/gitlab-ce");
            Environment.GetEnvironmentVariable("CI_RUNNER_ID").Returns("10");
            Environment.GetEnvironmentVariable("CI_RUNNER_DESCRIPTION").Returns("my runner");
            Environment.GetEnvironmentVariable("CI_RUNNER_TAGS").Returns("docker, linux");
            Environment.GetEnvironmentVariable("CI_SERVER").Returns("yes");
            Environment.GetEnvironmentVariable("CI_SERVER_NAME").Returns("GitLab");
            Environment.GetEnvironmentVariable("CI_SERVER_REVISION").Returns("70606bf");
            Environment.GetEnvironmentVariable("CI_SERVER_VERSION").Returns("8.9.0");
            Environment.GetEnvironmentVariable("GITLAB_USER_ID").Returns("42");
            Environment.GetEnvironmentVariable("GITLAB_USER_EMAIL").Returns("anthony@warwickcontrol.com");
        }

        public GitLabCIBuildInfo CreateBuildInfo()
        {
            return new GitLabCIBuildInfo(Environment);
        }

        public GitLabCIPullRequestInfo CreatePullRequestInfo()
        {
            return new GitLabCIPullRequestInfo(Environment);
        }

        public GitLabCIProjectInfo CreateProjectInfo()
        {
            return new GitLabCIProjectInfo(Environment);
        }

        public GitLabCIRunnerInfo CreateRunnerInfo()
        {
            return new GitLabCIRunnerInfo(Environment);
        }

        public GitLabCIServerInfo CreateServerInfo()
        {
            return new GitLabCIServerInfo(Environment);
        }

        public GitLabCIEnvironmentInfo CreateEnvironmentInfo()
        {
            return new GitLabCIEnvironmentInfo(Environment);
        }
    }
}
