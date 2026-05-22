// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.Rwx.Data;
using Cake.Core;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class RwxInfoFixture
    {
        public ICakeEnvironment Environment { get; set; }

        public RwxInfoFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();

            // RwxEnvironmentInfo
            Environment.GetEnvironmentVariable("CI").Returns("true");
            Environment.GetEnvironmentVariable("RWX").Returns("true");

            // RwxRunInfo
            Environment.GetEnvironmentVariable("RWX_RUN_ID").Returns("01J9ABCDEFG");
            Environment.GetEnvironmentVariable("RWX_RUN_TITLE").Returns("Build and test");
            Environment.GetEnvironmentVariable("RWX_RUN_URL").Returns("https://cloud.rwx.com/runs/01J9ABCDEFG");

            // RwxTaskInfo
            Environment.GetEnvironmentVariable("RWX_TASK_ID").Returns("01J9TASKABC");
            Environment.GetEnvironmentVariable("RWX_TASK_URL").Returns("https://cloud.rwx.com/tasks/01J9TASKABC");
            Environment.GetEnvironmentVariable("RWX_TASK_ATTEMPT_NUMBER").Returns("2");

            // RwxActorInfo
            Environment.GetEnvironmentVariable("RWX_ACTOR_ID").Returns("usr_12345");
            Environment.GetEnvironmentVariable("RWX_ACTOR").Returns("octocat");

            // RwxGitInfo
            Environment.GetEnvironmentVariable("RWX_GIT_REPOSITORY_URL").Returns("https://github.com/cake-build/cake.git");
            Environment.GetEnvironmentVariable("RWX_GIT_REPOSITORY_NAME").Returns("cake-build/cake");
            Environment.GetEnvironmentVariable("RWX_GIT_COMMIT_SHA").Returns("0123456789abcdef0123456789abcdef01234567");
            Environment.GetEnvironmentVariable("RWX_GIT_COMMIT_MESSAGE").Returns("Add RWX build provider\n\nMore details here.");
            Environment.GetEnvironmentVariable("RWX_GIT_COMMIT_SUMMARY").Returns("Add RWX build provider");
            Environment.GetEnvironmentVariable("RWX_GIT_COMMITTER_NAME").Returns("Octo Cat");
            Environment.GetEnvironmentVariable("RWX_GIT_COMMITTER_EMAIL").Returns("octocat@example.com");
            Environment.GetEnvironmentVariable("RWX_GIT_REF").Returns("refs/heads/main");
            Environment.GetEnvironmentVariable("RWX_GIT_REF_NAME").Returns("main");

            // RwxRuntimeInfo
            Environment.GetEnvironmentVariable("RWX_VALUES").Returns("/rwx/values");
            Environment.GetEnvironmentVariable("RWX_ARTIFACTS").Returns("/rwx/artifacts");
            Environment.GetEnvironmentVariable("RWX_ENV").Returns("/rwx/env");
        }

        public RwxRunInfo CreateRunInfo()
        {
            return new RwxRunInfo(Environment);
        }

        public RwxTaskInfo CreateTaskInfo()
        {
            return new RwxTaskInfo(Environment);
        }

        public RwxActorInfo CreateActorInfo()
        {
            return new RwxActorInfo(Environment);
        }

        public RwxGitInfo CreateGitInfo()
        {
            return new RwxGitInfo(Environment);
        }

        public RwxRuntimeInfo CreateRuntimeInfo()
        {
            return new RwxRuntimeInfo(Environment);
        }

        public RwxEnvironmentInfo CreateEnvironmentInfo()
        {
            return new RwxEnvironmentInfo(Environment);
        }
    }
}
