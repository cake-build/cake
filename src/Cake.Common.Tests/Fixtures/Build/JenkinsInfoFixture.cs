// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Build.Jenkins.Data;
using Cake.Core;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class JenkinsInfoFixture
    {
        public ICakeEnvironment Environment { get; set; }

        public JenkinsInfoFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();

            // JenkinsEnvironmentInfo
            Environment.GetEnvironmentVariable("WORKSPACE").Returns("C:\\Jenkins\\build\\456");
            Environment.GetEnvironmentVariable("EXECUTOR_NUMBER").Returns("2112");
            Environment.GetEnvironmentVariable("JENKINS_HOME").Returns("C:\\Jenkins\\build");
            Environment.GetEnvironmentVariable("JENKINS_URL").Returns("http://localhost:8080/jenkins/");

            // JenkinsBuildInfo
            Environment.GetEnvironmentVariable("BUILD_NUMBER").Returns("456");
            Environment.GetEnvironmentVariable("BUILD_DISPLAY_NAME").Returns("#456");
            Environment.GetEnvironmentVariable("BUILD_ID").Returns("456");
            Environment.GetEnvironmentVariable("BUILD_TAG").Returns("jenkins-JOB1-456");
            Environment.GetEnvironmentVariable("BUILD_URL").Returns("http://localhost:8080/jenkins/job/cake/456/");

            // JenkinsRepositoryInfo
            Environment.GetEnvironmentVariable("GIT_COMMIT").Returns("67d423d36dd15b191a53ab3ddb613dc4b95be8b3");
            Environment.GetEnvironmentVariable("GIT_BRANCH").Returns("CAKE-BRANCH");
            Environment.GetEnvironmentVariable("SVN_REVISION").Returns("REVISION-NUMBER");
            Environment.GetEnvironmentVariable("SVN_URL").Returns("svn://127.0.0.1/cake-build");
            Environment.GetEnvironmentVariable("CVS_BRANCH").Returns("DEVBRANCH");

            // JenkinsNodeInfo
            Environment.GetEnvironmentVariable("NODE_NAME").Returns("master");
            Environment.GetEnvironmentVariable("NODE_LABELS").Returns("cake development build");

            // JenkinsJobInfo
            Environment.GetEnvironmentVariable("JOB_NAME").Returns("JOB1");
            Environment.GetEnvironmentVariable("JOB_URL").Returns("http://localhost:8080/jenkins/job/cake/");
        }

        public JenkinsBuildInfo CreateBuildInfo()
        {
            return new JenkinsBuildInfo(Environment);
        }

        public JenkinsJobInfo CreateJobInfo()
        {
            return new JenkinsJobInfo(Environment);
        }

        public JenkinsNodeInfo CreateNodeInfo()
        {
            return new JenkinsNodeInfo(Environment);
        }

        public JenkinsRepositoryInfo CreateRepositoryInfo()
        {
            return new JenkinsRepositoryInfo(Environment);
        }

        public JenkinsEnvironmentInfo CreateEnvironmentInfo()
        {
            return new JenkinsEnvironmentInfo(Environment);
        }
    }
}
