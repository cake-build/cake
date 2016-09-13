// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.GoCD.Data;
using Cake.Core;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class GoCDInfoFixture
    {
        public ICakeEnvironment Environment { get; set; }

        public GoCDInfoFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();

            // GoCDEnvironmentInfo
            Environment.GetEnvironmentVariable("GO_SERVER_URL").Returns("https://127.0.0.1:8154/go");
            Environment.GetEnvironmentVariable("GO_ENVIRONMENT_NAME").Returns("Development");
            Environment.GetEnvironmentVariable("GO_JOB_NAME").Returns("linux-firefox");
            Environment.GetEnvironmentVariable("GO_TRIGGER_USER").Returns("changes");

            // GoCDPipelineInfo
            Environment.GetEnvironmentVariable("GO_PIPELINE_NAME").Returns("main");
            Environment.GetEnvironmentVariable("GO_PIPELINE_COUNTER").Returns("2345");
            Environment.GetEnvironmentVariable("GO_PIPELINE_LABEL").Returns("1.1.2345");

            // GoCDCommitInfo
            Environment.GetEnvironmentVariable("bamboo_planRepository_revision").Returns("d4a3a4cb304548450e3cab2ff735f778ffe58d03");

            // GoCDRepositoryInfo
            Environment.GetEnvironmentVariable("GO_REVISION").Returns("123");
            Environment.GetEnvironmentVariable("GO_TO_REVISION").Returns("124");
            Environment.GetEnvironmentVariable("GO_FROM_REVISION").Returns("122");

            // GoCDStageInfo
            Environment.GetEnvironmentVariable("GO_STAGE_NAME").Returns("dev");
            Environment.GetEnvironmentVariable("GO_STAGE_COUNTER").Returns("1");
        }

        public GoCDEnvironmentInfo CreateEnvironmentInfo()
        {
            return new GoCDEnvironmentInfo(Environment);
        }

        public GoCDPipelineInfo CreatePipelineInfo()
        {
            return new GoCDPipelineInfo(Environment);
        }

        public GoCDRepositoryInfo CreateRepositoryInfo()
        {
            return new GoCDRepositoryInfo(Environment);
        }

        public GoCDStageInfo CreateStageInfo()
        {
            return new GoCDStageInfo(Environment);
        }
    }
}