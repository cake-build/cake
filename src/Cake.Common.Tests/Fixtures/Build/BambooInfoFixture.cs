// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Build.Bamboo.Data;
using Cake.Core;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class BambooInfoFixture
    {
        public ICakeEnvironment Environment { get; set; }

        public BambooInfoFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();

            //BambooBuildInfo
            Environment.GetEnvironmentVariable("bamboo_build_working_directory").Returns("C:\\build\\CAKE-CAKE-JOB1");
            Environment.GetEnvironmentVariable("bamboo_buildNumber").Returns("28");
            Environment.GetEnvironmentVariable("bamboo_buildKey").Returns("CAKE-CAKE-JOB1");
            Environment.GetEnvironmentVariable("bamboo_buildResultKey").Returns("CAKE-CAKE-JOB1-28");
            Environment.GetEnvironmentVariable("bamboo_buildResultsUrl").Returns("https://cakebuild.atlassian.net/builds/browse/CAKE-CAKE-JOB1-28");
            Environment.GetEnvironmentVariable("bamboo_buildTimeStamp").Returns("2015-12-15T22:53:37.847+01:00");

            //BambooCustomBuildInfo
            Environment.GetEnvironmentVariable("bamboo_customRevision").Returns("Cake with Iceing");

            //BambooCommitInfo
            Environment.GetEnvironmentVariable("bamboo_planRepository_revision").Returns("d4a3a4cb304548450e3cab2ff735f778ffe58d03");

            //BambooPlanInfo
            Environment.GetEnvironmentVariable("bamboo_planKey").Returns("CAKE-CAKE");
            Environment.GetEnvironmentVariable ("bamboo_planName").Returns ("cake-bamboo - dev");
            Environment.GetEnvironmentVariable("bamboo_shortJobKey").Returns("JOB1");
            Environment.GetEnvironmentVariable("bamboo_shortJobName").Returns("Build Cake");
            Environment.GetEnvironmentVariable("bamboo_shortPlanKey").Returns("CAKE");
            Environment.GetEnvironmentVariable("bamboo_shortPlanName").Returns("Cake");

            //BambooRepositoryInfo
            Environment.GetEnvironmentVariable("bamboo_planRepository_type").Returns("git");
            Environment.GetEnvironmentVariable("bamboo_repository_name").Returns("Cake/Develop");
            Environment.GetEnvironmentVariable("bamboo_planRepository_branch").Returns("develop");
        }

        public BambooBuildInfo CreateBuildInfo()
        {
            return new BambooBuildInfo(Environment);
        }

        public BambooPlanInfo CreatePlanInfo()
        {
            return new BambooPlanInfo(Environment);
        }

        public BambooCommitInfo CreateCommitInfo()
        {
            return new BambooCommitInfo(Environment);
        }

        public BambooRepositoryInfo CreateRepositoryInfo()
        {
            return new BambooRepositoryInfo(Environment);
        }

        public BambooCustomBuildInfo CreateCustomBuildInfo()
        {
            return new BambooCustomBuildInfo(Environment);
        }

        public BambooEnvironmentInfo CreateEnvironmentInfo()
        {
            return new BambooEnvironmentInfo(Environment);
        }
    }
}
