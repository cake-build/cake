// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.AzurePipelines;
using Cake.Common.Tests.Fakes;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class AzurePipelinesFixture
    {
        public ICakeEnvironment Environment { get; set; }

        public FakeBuildSystemServiceMessageWriter Writer { get; set; }

        public AzurePipelinesFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("C:\\build\\CAKE-CAKE-JOB1");
            Environment.GetEnvironmentVariable("TF_BUILD").Returns((string)null);
            Environment.Platform.Family.Returns(PlatformFamily.Windows);
            Writer = new FakeBuildSystemServiceMessageWriter();
        }

        public void IsRunningOnAzurePipelines()
        {
            Environment.GetEnvironmentVariable("TF_BUILD").Returns("True");
        }

        public AzurePipelinesProvider CreateAzurePipelinesService() => new AzurePipelinesProvider(Environment, Writer);

        public AzurePipelinesProvider CreateAzurePipelinesService(PlatformFamily platformFamily, DirectoryPath workingDirectory)
        {
            Environment.Platform.Family.Returns(platformFamily);
            Environment.WorkingDirectory.Returns(workingDirectory);
            return CreateAzurePipelinesService();
        }
    }
}