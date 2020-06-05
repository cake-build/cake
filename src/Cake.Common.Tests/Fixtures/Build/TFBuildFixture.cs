// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.TFBuild;
using Cake.Common.Tests.Fakes;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class TFBuildFixture
    {
        public ICakeEnvironment Environment { get; set; }

        public FakeBuildSystemServiceMessageWriter Writer { get; set; }

        public TFBuildFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("C:\\build\\CAKE-CAKE-JOB1");
            Environment.GetEnvironmentVariable("TF_BUILD").Returns((string)null);
            Environment.Platform.Family.Returns(PlatformFamily.Windows);
            Writer = new FakeBuildSystemServiceMessageWriter();
        }

        public void IsRunningOnTFS() => IsRunningOnAzurePipelines();

        public void IsRunningOnVSTS() => IsRunningOnAzurePipelinesHosted();

        public void IsRunningOnAzurePipelines()
        {
            Environment.GetEnvironmentVariable("TF_BUILD").Returns("True");
            Environment.GetEnvironmentVariable("AGENT_NAME").Returns("On Premises");
        }

        public void IsRunningOnAzurePipelinesHosted(string agentHostName = "Hosted Agent")
        {
            Environment.GetEnvironmentVariable("TF_BUILD").Returns("True");
            Environment.GetEnvironmentVariable("AGENT_NAME").Returns(agentHostName);
        }

        public TFBuildProvider CreateTFBuildService() => new TFBuildProvider(Environment, Writer);

        public TFBuildProvider CreateTFBuildService(PlatformFamily platformFamily, DirectoryPath workingDirectory)
        {
            Environment.Platform.Family.Returns(platformFamily);
            Environment.WorkingDirectory.Returns(workingDirectory);
            return CreateTFBuildService();
        }
    }
}