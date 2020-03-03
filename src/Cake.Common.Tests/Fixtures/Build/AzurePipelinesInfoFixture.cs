// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.AzurePipelines.Data;
using Cake.Core;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    public sealed class AzurePipelinesInfoFixture
    {
        public ICakeEnvironment Environment { get; set; }

        public AzurePipelinesInfoFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();

            // TFBuild RepositoryInfo
            Environment.GetEnvironmentVariable("BUILD_SOURCEVERSION").Returns("4efbc1ffb993dfbcf024e6a9202865cc0b6d9c50");
            Environment.GetEnvironmentVariable("BUILD_SOURCETFVCSHELVESET").Returns("Shelveset1");
            Environment.GetEnvironmentVariable("BUILD_REPOSITORY_NAME").Returns("cake");
            Environment.GetEnvironmentVariable("BUILD_REPOSITORY_PROVIDER").Returns("GitHub");
            Environment.GetEnvironmentVariable("BUILD_SOURCEBRANCHNAME").Returns("develop");
            Environment.GetEnvironmentVariable("BUILD_SOURCEBRANCH").Returns("refs/heads/develop");
            Environment.GetEnvironmentVariable("BUILD_SOURCEVERSIONMESSAGE").Returns("A commit message");
            Environment.GetEnvironmentVariable("BUILD_REPOSITORY_GIT_SUBMODULECHECKOUT").Returns("A commit message");

            // TFBuild AgentInfo
            Environment.GetEnvironmentVariable("AGENT_BUILDDIRECTORY").Returns(@"c:\agent\_work\1");
            Environment.GetEnvironmentVariable("AGENT_HOMEDIRECTORY").Returns(@"c:\agent");
            Environment.GetEnvironmentVariable("AGENT_WORKFOLDER").Returns(@"c:\agent\_work");
            Environment.GetEnvironmentVariable("AGENT_ID").Returns("71");
            Environment.GetEnvironmentVariable("AGENT_NAME").Returns("Agent-1");
            Environment.GetEnvironmentVariable("AGENT_MACHINENAME").Returns("BuildServer");
            Environment.GetEnvironmentVariable("AGENT_JOBNAME").Returns("Job");
            Environment.GetEnvironmentVariable("AGENT_JOBSTATUS").Returns("SucceededWithIssues");
            Environment.GetEnvironmentVariable("AGENT_TOOLSDIRECTORY").Returns(@"C:/hostedtoolcache/windows");

            // TFBuild BuildInfo
            Environment.GetEnvironmentVariable("SYSTEM_ACCESSTOKEN").Returns("f662dbe218144c86bdecb1e9b2eb336c");
            Environment.GetEnvironmentVariable("SYSTEM_DEBUG").Returns("true");
            Environment.GetEnvironmentVariable("BUILD_BUILDID").Returns("100234");
            Environment.GetEnvironmentVariable("BUILD_BUILDNUMBER").Returns("Build-20160927.1");
            Environment.GetEnvironmentVariable("BUILD_BUILDURI").Returns("vstfs:///Build/Build/1430");
            Environment.GetEnvironmentVariable("BUILD_QUEUEDBY")
                .Returns(@"[DefaultCollection]\Project Collection Service Accounts");
            Environment.GetEnvironmentVariable("BUILD_REQUESTEDFOR").Returns("Alistair Chapman");
            Environment.GetEnvironmentVariable("BUILD_REQUESTEDFOREMAIL").Returns("author@mail.com");
            Environment.GetEnvironmentVariable("BUILD_ARTIFACTSTAGINGDIRECTORY").Returns(@"c:\agent\_work\1\a");
            Environment.GetEnvironmentVariable("BUILD_BINARIESDIRECTORY").Returns(@"c:\agent\_work\1\b");
            Environment.GetEnvironmentVariable("BUILD_REASON").Returns("PullRequest");
            Environment.GetEnvironmentVariable("BUILD_SOURCESDIRECTORY").Returns(@"c:\agent\_work\1\s");
            Environment.GetEnvironmentVariable("BUILD_STAGINGDIRECTORY").Returns(@"c:\agent\_work\1\a");
            Environment.GetEnvironmentVariable("COMMON_TESTRESULTSDIRECTORY").Returns(@"c:\agent\_work\1\TestResults");

            // VSTS Build TriggeredBy
            Environment.GetEnvironmentVariable("BUILD_TRIGGEREDBY_BUILDID").Returns(@"1");
            Environment.GetEnvironmentVariable("BUILD_TRIGGEREDBY_DEFINITIONID").Returns(@"1");
            Environment.GetEnvironmentVariable("BUILD_TRIGGEREDBY_DEFINITIONNAME").Returns(@"Build");
            Environment.GetEnvironmentVariable("BUILD_TRIGGEREDBY_BUILDNUMBER").Returns(@"123");
            Environment.GetEnvironmentVariable("BUILD_TRIGGEREDBY_PROJECTID").Returns(@"456");

            // TFBuild PullRequestInfo
            Environment.GetEnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTID").Returns("1");
            Environment.GetEnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTNUMBER").Returns("1");
            Environment.GetEnvironmentVariable("SYSTEM_PULLREQUEST_ISFORK").Returns(@"False");
            Environment.GetEnvironmentVariable("SYSTEM_PULLREQUEST_SOURCEBRANCH").Returns(@"refs/heads/FeatureBranch");
            Environment.GetEnvironmentVariable("SYSTEM_PULLREQUEST_SOURCEREPOSITORYURI").Returns(@"https://fabrikamfiber.visualstudio.com/Project/_git/ProjectRepo");
            Environment.GetEnvironmentVariable("SYSTEM_PULLREQUEST_TARGETBRANCH").Returns(@"refs/heads/master");

            // TFBuild DefinitionInfo
            Environment.GetEnvironmentVariable("SYSTEM_DEFINITIONID").Returns("1855");
            Environment.GetEnvironmentVariable("BUILD_DEFINITIONNAME").Returns("Cake-CI");
            Environment.GetEnvironmentVariable("BUILD_DEFINITIONVERSION").Returns("47");

            // TFBuild TeamProjectInfo
            Environment.GetEnvironmentVariable("SYSTEM_TEAMPROJECT").Returns("TeamProject");
            Environment.GetEnvironmentVariable("SYSTEM_TEAMPROJECTID").Returns("D0A3B6B8-499B-4D4B-BD46-DB70C19E6D33");
            Environment.GetEnvironmentVariable("SYSTEM_TEAMFOUNDATIONCOLLECTIONURI")
                .Returns("https://fabrikamfiber.visualstudio.com/");
        }

        public AzurePipelinesEnvironmentInfo CreateEnvironmentInfo()
        {
            return new AzurePipelinesEnvironmentInfo(Environment);
        }

        public AzurePipelinesRepositoryInfo CreateRepositoryInfo()
        {
            return new AzurePipelinesRepositoryInfo(Environment);
        }

        public AzurePipelinesRepositoryInfo CreateRepositoryInfo(string repoType)
        {
            Environment.GetEnvironmentVariable("BUILD_REPOSITORY_PROVIDER").Returns(repoType);
            return CreateRepositoryInfo();
        }

        public AzurePipelinesAgentInfo CreateAgentInfo()
        {
            return new AzurePipelinesAgentInfo(Environment);
        }

        public AzurePipelinesAgentInfo CreateHostedAgentInfo()
        {
            Environment.GetEnvironmentVariable("AGENT_NAME").Returns("Hosted Agent");
            return new AzurePipelinesAgentInfo(Environment);
        }

        public AzurePipelinesBuildInfo CreateBuildInfo()
        {
            return new AzurePipelinesBuildInfo(Environment);
        }

        public AzurePipelinesPullRequestInfo CreatePullRequestInfo()
        {
            return new AzurePipelinesPullRequestInfo(Environment);
        }

        public AzurePipelinesDefinitionInfo CreateDefinitionInfo()
        {
            return new AzurePipelinesDefinitionInfo(Environment);
        }

        public AzurePipelinesTeamProjectInfo CreateTeamProjectInfo()
        {
            return new AzurePipelinesTeamProjectInfo(Environment);
        }
    }
}
