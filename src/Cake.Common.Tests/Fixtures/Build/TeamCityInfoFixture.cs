// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.TeamCity.Data;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class TeamCityInfoFixture
    {
        public IFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }

        public TeamCityInfoFixture()
        {
            Environment = FakeEnvironment.CreateUnixEnvironment();
            FileSystem = new FakeFileSystem(Environment);
            ((FakeFileSystem)FileSystem).CreateDirectory("/Working");

            ((FakeEnvironment)Environment).SetEnvironmentVariable("TEAMCITY_BUILDCONF_NAME", @"Cake Build");
            ((FakeEnvironment)Environment).SetEnvironmentVariable("BUILD_NUMBER", "10-Foo");
            ((FakeEnvironment)Environment).SetEnvironmentVariable("TEAMCITY_PROJECT_NAME", "Cake");
            ((FakeEnvironment)Environment).SetEnvironmentVariable("TEAMCITY_BUILD_PROPERTIES_FILE", "/Working/teamcity.build.properties");
            ((FakeEnvironment)Environment).SetEnvironmentVariable("Git_Branch", "refs/pull-requests/7/from");
            ((FakeEnvironment)Environment).SetEnvironmentVariable("BUILD_START_DATE", "20200822");
            ((FakeEnvironment)Environment).SetEnvironmentVariable("BUILD_START_TIME", "123456");
        }

        public void SetBuildPropertiesContent(string xml)
        {
            ((FakeFileSystem)FileSystem).GetFile("/Working/teamcity.build.properties.xml").SetContent(xml);
        }

        public void SetConfigPropertiesContent(string xml)
        {
            ((FakeFileSystem)FileSystem).GetFile("/Working/teamcity.config.configuration.xml").SetContent(xml);
        }

        public void SetRunnerPropertiesContent(string xml)
        {
            ((FakeFileSystem)FileSystem).GetFile("/Working/teamcity.runner.configuration.xml").SetContent(xml);
        }

        public void SetGitBranch(string branch)
        {
            ((FakeEnvironment)Environment).SetEnvironmentVariable("Git_Branch", branch);
        }

        public void SetBuildStartDate(string startDate)
        {
            ((FakeEnvironment)Environment).SetEnvironmentVariable("BUILD_START_DATE", startDate);
        }

        public void SetBuildStartTime(string startTime)
        {
            ((FakeEnvironment)Environment).SetEnvironmentVariable("BUILD_START_TIME", startTime);
        }

        public TeamCityPullRequestInfo CreatePullRequestInfo()
        {
            return new TeamCityPullRequestInfo(Environment, CreateBuildInfo());
        }

        public TeamCityEnvironmentInfo CreateEnvironmentInfo()
        {
            return new TeamCityEnvironmentInfo(Environment, FileSystem);
        }

        public TeamCityBuildInfo CreateBuildInfo()
        {
            return new TeamCityBuildInfo(Environment, FileSystem);
        }

        public TeamCityProjectInfo CreateProjectInfo()
        {
            return new TeamCityProjectInfo(Environment);
        }
    }
}