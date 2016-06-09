// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Build.TravisCI.Data;
using Cake.Core;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class TravisCIInfoFixture
    {
        public ICakeEnvironment Environment { get; set; }

        public TravisCIInfoFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();

            // TravisCIEnvironmentInfo
            Environment.GetEnvironmentVariable("CI").Returns("true");
            Environment.GetEnvironmentVariable("TRAVIS").Returns("true");
            Environment.GetEnvironmentVariable("HOME").Returns("/home/travis");

            // TravisCIBuildInfo
            Environment.GetEnvironmentVariable("TRAVIS_BRANCH").Returns("master");
            Environment.GetEnvironmentVariable("TRAVIS_BUILD_DIR").Returns("/home/travis/build/");
            Environment.GetEnvironmentVariable("TRAVIS_BUILD_ID").Returns("122134370");
            Environment.GetEnvironmentVariable("TRAVIS_BUILD_NUMBER").Returns("934");
            Environment.GetEnvironmentVariable("TRAVIS_TEST_RESULT").Returns("0");
            Environment.GetEnvironmentVariable("TRAVIS_TAG").Returns("v0.10.0");

            // TravisCIJobInfo
            Environment.GetEnvironmentVariable("TRAVIS_JOB_ID").Returns("934");
            Environment.GetEnvironmentVariable("TRAVIS_JOB_NUMBER").Returns("934.2");
            Environment.GetEnvironmentVariable("TRAVIS_OS_NAME").Returns("osx");
            Environment.GetEnvironmentVariable("TRAVIS_SECURE_ENV_VARS").Returns("false");

            // TravisCIRepositoryInfo
            Environment.GetEnvironmentVariable("TRAVIS_COMMIT").Returns("6cbdbe8");
            Environment.GetEnvironmentVariable("TRAVIS_COMMIT_RANGE").Returns("6cb4d6...5ba6dbe8");
            Environment.GetEnvironmentVariable("TRAVIS_PULL_REQUEST").Returns("#786 (GH742) Added TravisCI build system support");
            Environment.GetEnvironmentVariable("TRAVIS_REPO_SLUG").Returns("4d65ba6");
        }

        public TravisCIBuildInfo CreateBuildInfo()
        {
            return new TravisCIBuildInfo(Environment);
        }

        public TravisCIJobInfo CreateJobInfo()
        {
            return new TravisCIJobInfo(Environment);
        }

        public TravisCIRepositoryInfo CreateRepositoryInfo()
        {
            return new TravisCIRepositoryInfo(Environment);
        }

        public TravisCIEnvironmentInfo CreateEnvironmentInfo()
        {
            return new TravisCIEnvironmentInfo(Environment);
        }
    }
}
