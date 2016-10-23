using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cake.Common.Build.TFBuild;
using Cake.Core;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class TFBuildFixture
    {
        public ICakeEnvironment Environment { get; set; }

        public TFBuildFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("C:\\build\\CAKE-CAKE-JOB1");
            Environment.GetEnvironmentVariable("TF_BUILD").Returns((string)null);
        }

        public void IsRunningOnVSTS()
        {
            Environment.GetEnvironmentVariable("TF_BUILD").Returns("True");
            Environment.GetEnvironmentVariable("AGENT_NAME").Returns("Hosted Agent");
        }

        public void IsRunningOnTFS()
        {
            Environment.GetEnvironmentVariable("TF_BUILD").Returns("True");
            Environment.GetEnvironmentVariable("AGENT_NAME").Returns("On Premises");
        }

        public TFBuildProvider CreateTFBuildService()
        {
            return new TFBuildProvider(Environment);
        }
    }
}
