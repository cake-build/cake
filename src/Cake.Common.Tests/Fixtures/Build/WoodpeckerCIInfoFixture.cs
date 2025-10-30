// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.WoodpeckerCI.Data;
using Cake.Core;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    public class WoodpeckerCIInfoFixture
    {
        public ICakeEnvironment Environment { get; set; }

        public WoodpeckerCIInfoFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();

            // WoodpeckerCI Environment
            Environment.GetEnvironmentVariable("CI").Returns("woodpecker");
            Environment.GetEnvironmentVariable("CI_WORKSPACE").Returns("/woodpecker/src/git.example.com/john-doe/my-repo");

            // WoodpeckerCI RepositoryInfo
            Environment.GetEnvironmentVariable("CI_REPO").Returns("john-doe/my-repo");
            Environment.GetEnvironmentVariable("CI_REPO_OWNER").Returns("john-doe");
            Environment.GetEnvironmentVariable("CI_REPO_NAME").Returns("my-repo");
            Environment.GetEnvironmentVariable("CI_REPO_REMOTE_ID").Returns("82");
            Environment.GetEnvironmentVariable("CI_REPO_URL").Returns("https://git.example.com/john-doe/my-repo");
            Environment.GetEnvironmentVariable("CI_REPO_CLONE_URL").Returns("https://git.example.com/john-doe/my-repo.git");
            Environment.GetEnvironmentVariable("CI_REPO_CLONE_SSH_URL").Returns("git@git.example.com:john-doe/my-repo.git");
            Environment.GetEnvironmentVariable("CI_REPO_DEFAULT_BRANCH").Returns("main");
            Environment.GetEnvironmentVariable("CI_REPO_PRIVATE").Returns("true");
            Environment.GetEnvironmentVariable("CI_REPO_TRUSTED_NETWORK").Returns("false");
            Environment.GetEnvironmentVariable("CI_REPO_TRUSTED_VOLUMES").Returns("false");
            Environment.GetEnvironmentVariable("CI_REPO_TRUSTED_SECURITY").Returns("false");

            // WoodpeckerCI CommitInfo
            Environment.GetEnvironmentVariable("CI_COMMIT_SHA").Returns("eba09b46064473a1d345da7abf28b477468e8dbd");
            Environment.GetEnvironmentVariable("CI_COMMIT_REF").Returns("refs/heads/main");
            Environment.GetEnvironmentVariable("CI_COMMIT_REFSPEC").Returns("issue-branch:main");
            Environment.GetEnvironmentVariable("CI_COMMIT_BRANCH").Returns("main");
            Environment.GetEnvironmentVariable("CI_COMMIT_SOURCE_BRANCH").Returns("issue-branch");
            Environment.GetEnvironmentVariable("CI_COMMIT_TARGET_BRANCH").Returns("main");
            Environment.GetEnvironmentVariable("CI_COMMIT_TAG").Returns("v1.10.3");
            Environment.GetEnvironmentVariable("CI_COMMIT_PULL_REQUEST").Returns("1");
            Environment.GetEnvironmentVariable("CI_COMMIT_PULL_REQUEST_LABELS").Returns("server");
            Environment.GetEnvironmentVariable("CI_COMMIT_MESSAGE").Returns("Initial commit");
            Environment.GetEnvironmentVariable("CI_COMMIT_AUTHOR").Returns("john-doe");
            Environment.GetEnvironmentVariable("CI_COMMIT_AUTHOR_EMAIL").Returns("john-doe@example.com");
            Environment.GetEnvironmentVariable("CI_COMMIT_PRERELEASE").Returns("false");

            // WoodpeckerCI PipelineInfo
            Environment.GetEnvironmentVariable("CI_PIPELINE_NUMBER").Returns("8");
            Environment.GetEnvironmentVariable("CI_PIPELINE_PARENT").Returns("0");
            Environment.GetEnvironmentVariable("CI_PIPELINE_EVENT").Returns("push");
            Environment.GetEnvironmentVariable("CI_PIPELINE_URL").Returns("https://ci.example.com/repos/john-doe/my-repo/pipeline/123");
            Environment.GetEnvironmentVariable("CI_PIPELINE_FORGE_URL").Returns("https://git.example.com/john-doe/my-repo/commit/abc123");
            Environment.GetEnvironmentVariable("CI_PIPELINE_DEPLOY_TARGET").Returns("production");
            Environment.GetEnvironmentVariable("CI_PIPELINE_DEPLOY_TASK").Returns("migration");
            Environment.GetEnvironmentVariable("CI_PIPELINE_CREATED").Returns("1722617519");
            Environment.GetEnvironmentVariable("CI_PIPELINE_STARTED").Returns("1722617519");
            Environment.GetEnvironmentVariable("CI_PIPELINE_FILES").Returns("[\".woodpecker.yml\",\"README.md\"]");
            Environment.GetEnvironmentVariable("CI_PIPELINE_AUTHOR").Returns("octocat");
            Environment.GetEnvironmentVariable("CI_PIPELINE_AVATAR").Returns("https://git.example.com/avatars/john-doe");

            // WoodpeckerCI WorkflowInfo
            Environment.GetEnvironmentVariable("CI_WORKFLOW_NAME").Returns("release");

            // WoodpeckerCI StepInfo
            Environment.GetEnvironmentVariable("CI_STEP_NAME").Returns("build package");
            Environment.GetEnvironmentVariable("CI_STEP_NUMBER").Returns("0");
            Environment.GetEnvironmentVariable("CI_STEP_STARTED").Returns("1722617519");
            Environment.GetEnvironmentVariable("CI_STEP_URL").Returns("https://ci.example.com/repos/7/pipeline/8");

            // WoodpeckerCI SystemInfo
            Environment.GetEnvironmentVariable("CI_SYSTEM_NAME").Returns("woodpecker");
            Environment.GetEnvironmentVariable("CI_SYSTEM_URL").Returns("https://ci.example.com");
            Environment.GetEnvironmentVariable("CI_SYSTEM_HOST").Returns("ci.example.com");
            Environment.GetEnvironmentVariable("CI_SYSTEM_VERSION").Returns("2.7.0");

            // WoodpeckerCI ForgeInfo
            Environment.GetEnvironmentVariable("CI_FORGE_TYPE").Returns("github");
            Environment.GetEnvironmentVariable("CI_FORGE_URL").Returns("https://git.example.com");
        }

        public WoodpeckerCIEnvironmentInfo CreateEnvironmentInfo()
        {
            return new WoodpeckerCIEnvironmentInfo(Environment);
        }

        public WoodpeckerCIRepositoryInfo CreateRepositoryInfo()
        {
            return new WoodpeckerCIRepositoryInfo(Environment);
        }

        public WoodpeckerCICommitInfo CreateCommitInfo()
        {
            return new WoodpeckerCICommitInfo(Environment);
        }

        public WoodpeckerCIPipelineInfo CreatePipelineInfo()
        {
            return new WoodpeckerCIPipelineInfo(Environment);
        }

        public WoodpeckerCIWorkflowInfo CreateWorkflowInfo()
        {
            return new WoodpeckerCIWorkflowInfo(Environment);
        }

        public WoodpeckerCIStepInfo CreateStepInfo()
        {
            return new WoodpeckerCIStepInfo(Environment);
        }

        public WoodpeckerCISystemInfo CreateSystemInfo()
        {
            return new WoodpeckerCISystemInfo(Environment);
        }

        public WoodpeckerCIForgeInfo CreateForgeInfo()
        {
            return new WoodpeckerCIForgeInfo(Environment);
        }

        public WoodpeckerCIInfoFixture SetEnvironmentVariable(string name, string value)
        {
            Environment.GetEnvironmentVariable(name).Returns(value);
            return this;
        }
    }
}
