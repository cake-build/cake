using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cake.Common.Build.TFBuild.Data;
using Cake.Core;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    public sealed class TFBuildInfoFixture
    {
        public ICakeEnvironment Environment { get; set; }

        public TFBuildInfoFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();

            // VSTS RepositoryInfo
            Environment.GetEnvironmentVariable("BUILD_SOURCEVERSION").Returns("4efbc1ffb993dfbcf024e6a9202865cc0b6d9c50");
            Environment.GetEnvironmentVariable("BUILD_SOURCETFVCSHELVESET").Returns("Shelveset1");
            Environment.GetEnvironmentVariable("BUILD_REPOSITORY_NAME").Returns("cake");
            Environment.GetEnvironmentVariable("BUILD_REPOSITORY_PROVIDER").Returns("GitHub");
            Environment.GetEnvironmentVariable("BUILD_SOURCEBRANCHNAME").Returns("develop");

            // VSTS AgentInfo
            Environment.GetEnvironmentVariable("AGENT_BUILDDIRECTORY").Returns(@"c:\agent\_work\1");
            Environment.GetEnvironmentVariable("AGENT_HOMEDIRECTORY").Returns(@"c:\agent");
            Environment.GetEnvironmentVariable("AGENT_WORKFOLDER").Returns(@"c:\agent\_work");
            Environment.GetEnvironmentVariable("AGENT_ID").Returns("71");
            Environment.GetEnvironmentVariable("AGENT_NAME").Returns("Agent-1");
            Environment.GetEnvironmentVariable("AGENT_MACHINE_NAME").Returns("BuildServer");

            // VSTS BuildInfo
            Environment.GetEnvironmentVariable("BUILD_BUILDID").Returns("100234");
            Environment.GetEnvironmentVariable("BUILD_BUILDNUMBER").Returns("Build-20160927.1");
            Environment.GetEnvironmentVariable("BUILD_BUILDURI").Returns("vstfs:///Build/Build/1430");
            Environment.GetEnvironmentVariable("BUILD_QUEUEDBY")
                .Returns(@"[DefaultCollection]\Project Collection Service Accounts");
            Environment.GetEnvironmentVariable("BUILD_REQUESTEDFOR").Returns("Alistair Chapman");
            Environment.GetEnvironmentVariable("BUILD_REQUESTEDFOREMAIL").Returns("author@mail.com");

            // VSTS DefinitionInfo
            Environment.GetEnvironmentVariable("SYSTEM_DEFINITIONID").Returns("1855");
            Environment.GetEnvironmentVariable("BUILD_DEFINITIONNAME").Returns("Cake-CI");
            Environment.GetEnvironmentVariable("BUILD_DEFINITIONVERSION").Returns("47");

            // VSTS TeamProjectInfo
            Environment.GetEnvironmentVariable("SYSTEM_TEAMPROJECT").Returns("TeamProject");
            Environment.GetEnvironmentVariable("SYSTEM_TEAMPROJECTID").Returns("D0A3B6B8-499B-4D4B-BD46-DB70C19E6D33");
            Environment.GetEnvironmentVariable("SYSTEM_TEAMFOUNDATIONCOLLECTIONURI")
                .Returns("https://fabrikamfiber.visualstudio.com/");
        }

        public TFBuildEnvironmentInfo CreateEnvironmentInfo()
        {
            return new TFBuildEnvironmentInfo(Environment);
        }

        public TFBuildRepositoryInfo CreateRepositoryInfo()
        {
            return new TFBuildRepositoryInfo(Environment);
        }

        public TFBuildRepositoryInfo CreateRepositoryInfo(string repoType)
        {
            Environment.GetEnvironmentVariable("BUILD_REPOSITORY_PROVIDER").Returns(repoType);
            return CreateRepositoryInfo();
        }

        public TFBuildAgentInfo CreateAgentInfo()
        {
            return new TFBuildAgentInfo(Environment);
        }

        public TFBuildAgentInfo CreateHostedAgentInfo()
        {
            Environment.GetEnvironmentVariable("AGENT_NAME").Returns("Hosted Agent");
            return new TFBuildAgentInfo(Environment);
        }

        public TFBuildInfo CreateBuildInfo()
        {
            return new TFBuildInfo(Environment);
        }

        public TFBuildDefinitionInfo CreateDefinitionInfo()
        {
            return new TFBuildDefinitionInfo(Environment);
        }

        public TFBuildTeamProjectInfo CreateTeamProjectInfo()
        {
            return new TFBuildTeamProjectInfo(Environment);
        }
    }
}
