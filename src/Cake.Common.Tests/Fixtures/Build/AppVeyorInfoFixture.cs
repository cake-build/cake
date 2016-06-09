// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Build.AppVeyor.Data;
using Cake.Core;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class AppVeyorInfoFixture
    {
        public ICakeEnvironment Environment { get; set; }

        public AppVeyorInfoFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();

            Environment.GetEnvironmentVariable("APPVEYOR_API_URL").Returns("http://localhost:1029/");

            Environment.GetEnvironmentVariable("APPVEYOR_PROJECT_ID").Returns("85364");
            Environment.GetEnvironmentVariable("APPVEYOR_PROJECT_NAME").Returns("Cake");
            Environment.GetEnvironmentVariable("APPVEYOR_PROJECT_SLUG").Returns("cake");

            Environment.GetEnvironmentVariable("APPVEYOR_BUILD_FOLDER").Returns(@"C:\projects\cake");
            Environment.GetEnvironmentVariable("APPVEYOR_BUILD_ID").Returns("378354");
            Environment.GetEnvironmentVariable("APPVEYOR_BUILD_NUMBER").Returns("2");
            Environment.GetEnvironmentVariable("APPVEYOR_BUILD_VERSION").Returns("1.0.2");

            Environment.GetEnvironmentVariable("APPVEYOR_PULL_REQUEST_NUMBER").Returns("1");
            Environment.GetEnvironmentVariable("APPVEYOR_PULL_REQUEST_TITLE").Returns("Changes stuff.");

            Environment.GetEnvironmentVariable("APPVEYOR_JOB_ID").Returns("d6qpdshbol69ucbq");
            Environment.GetEnvironmentVariable("APPVEYOR_JOB_NAME").Returns("Job1");

            Environment.GetEnvironmentVariable("APPVEYOR_REPO_PROVIDER").Returns("github");
            Environment.GetEnvironmentVariable("APPVEYOR_REPO_SCM").Returns("git");
            Environment.GetEnvironmentVariable("APPVEYOR_REPO_NAME").Returns("cake-build/cake");
            Environment.GetEnvironmentVariable("APPVEYOR_REPO_BRANCH").Returns("develop");

            Environment.GetEnvironmentVariable("APPVEYOR_REPO_TAG").Returns("True");
            Environment.GetEnvironmentVariable("APPVEYOR_REPO_TAG_NAME").Returns("v1.0.25");

            Environment.GetEnvironmentVariable("APPVEYOR_REPO_COMMIT").Returns("01c08e7b0f3434b1c6c30c880be33ed7331e8639");
            Environment.GetEnvironmentVariable("APPVEYOR_REPO_COMMIT_AUTHOR").Returns("Patrik Svensson");
            Environment.GetEnvironmentVariable("APPVEYOR_REPO_COMMIT_AUTHOR_EMAIL").Returns("author@mail.com");
            Environment.GetEnvironmentVariable("APPVEYOR_REPO_COMMIT_TIMESTAMP").Returns("1/5/2015 3:13:01 AM");
            Environment.GetEnvironmentVariable("APPVEYOR_REPO_COMMIT_MESSAGE").Returns("A test commit.");
            Environment.GetEnvironmentVariable("APPVEYOR_REPO_COMMIT_MESSAGE_EXTENDED").Returns("Testing stuff.");

            Environment.GetEnvironmentVariable("APPVEYOR_SCHEDULED_BUILD").Returns("True");
            Environment.GetEnvironmentVariable("PLATFORM").Returns("Debug");
            Environment.GetEnvironmentVariable("CONFIGURATION").Returns("x86");
        }

        public AppVeyorBuildInfo CreateBuildInfo()
        {
            return new AppVeyorBuildInfo(Environment);
        }

        public AppVeyorProjectInfo CreateProjectInfo()
        {
            return new AppVeyorProjectInfo(Environment);
        }

        public AppVeyorCommitInfo CreateCommitInfo()
        {
            return new AppVeyorCommitInfo(Environment);
        }

        public AppVeyorRepositoryInfo CreateRepositoryInfo()
        {
            return new AppVeyorRepositoryInfo(Environment);
        }

        public AppVeyorPullRequestInfo CreatePullRequestInfo()
        {
            return new AppVeyorPullRequestInfo(Environment);
        }

        public AppVeyorTagInfo CreateTagInfo()
        {
            return new AppVeyorTagInfo(Environment);
        }

        public AppVeyorEnvironmentInfo CreateEnvironmentInfo()
        {
            return new AppVeyorEnvironmentInfo(Environment);
        }
    }
}
