// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Build.Bitrise.Data;
using Cake.Core;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    public class BitriseInfoFixture
    {
        public ICakeEnvironment Environment { get; set; }

        public BitriseInfoFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();

            // Bitrise ApplicationInfo
            Environment.GetEnvironmentVariable("BITRISE_APP_TITLE").Returns("CAKE-EXE");
            Environment.GetEnvironmentVariable("BITRISE_APP_URL").Returns("https://www.bitrise.io/app/089v339k300ba3cd");
            Environment.GetEnvironmentVariable("BITRISE_APP_SLUG").Returns("089v339k300ba3cd");

            // Bitrise BuildInfo
            Environment.GetEnvironmentVariable("BITRISE_BUILD_NUMBER").Returns("456");
            Environment.GetEnvironmentVariable("BITRISE_BUILD_URL").Returns("https://www.bitrise.io/build/e794ed892f3a59dd");
            Environment.GetEnvironmentVariable("BITRISE_BUILD_SLUG").Returns("e794ed892f3a59dd");
            Environment.GetEnvironmentVariable("BITRISE_BUILD_TRIGGER_TIMESTAMP").Returns("2016-03-12 23:49:26");
            Environment.GetEnvironmentVariable("BITRISE_BUILD_STATUS").Returns("true");

            // Bitrise DirectoryInfo
            Environment.GetEnvironmentVariable("BITRISE_SOURCE_DIR").Returns("/Users/vagrant/git");
            Environment.GetEnvironmentVariable("BITRISE_DEPLOY_DIR").Returns("/Users/vagrant/deploy");

            // Bitrise ProvisioningInfo
            Environment.GetEnvironmentVariable("BITRISE_PROVISION_URL").Returns("file://cake-build/cake/cake.provision");
            Environment.GetEnvironmentVariable("BITRISE_CERTIFICATE_URL").Returns("file://cake-build/cake/Cert.p12");
            Environment.GetEnvironmentVariable("BITRISE_CERTIFICATE_PASSPHRASE").Returns("CAKE");

            // Bitrise RepositoryInfo
            Environment.GetEnvironmentVariable("GIT_REPOSITORY_URL").Returns("git@github.com:/cake-build/cake.git");
            Environment.GetEnvironmentVariable("BITRISE_GIT_BRANCH").Returns("cake-branch");
            Environment.GetEnvironmentVariable("BITRISE_GIT_TAG").Returns("v0.0.1");
            Environment.GetEnvironmentVariable("BITRISE_GIT_COMMIT").Returns("63dd7b");
            Environment.GetEnvironmentVariable("BITRISE_PULL_REQUEST").Returns("[WIP] Bitrise cake support #000");

            // Bitrise WorkflowInfo
            Environment.GetEnvironmentVariable("BITRISE_TRIGGERED_WORKFLOW_ID").Returns("Build & Test Cake on BitRise");
            Environment.GetEnvironmentVariable("BITRISE_TRIGGERED_WORKFLOW_TITLE").Returns("Build & Test Cake on BitRise");
        }

        public BitriseApplicationInfo CreateApplicationInfo()
        {
            return new BitriseApplicationInfo(Environment);
        }

        public BitriseBuildInfo CreateBuildInfo()
        {
            return new BitriseBuildInfo(Environment);
        }

        public BitriseDirectoryInfo CreateDirectoryInfo()
        {
            return new BitriseDirectoryInfo(Environment);
        }

        public BitriseProvisioningInfo CreateProvisioningInfo()
        {
            return new BitriseProvisioningInfo(Environment);
        }

        public BitriseRepositoryInfo CreateRepositoryInfo()
        {
            return new BitriseRepositoryInfo(Environment);
        }

        public BitriseWorkflowInfo CreateWorkflowInfo()
        {
            return new BitriseWorkflowInfo(Environment);
        }
    }
}
