using Cake.Common.Build.Bamboo;
using Cake.Core;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class BambooFixture
    {
        public ICakeEnvironment Environment { get; set; }

        public BambooFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("C:\\build\\CAKE-CAKE-JOB1");
            Environment.GetEnvironmentVariable("bamboo_buildNumber").Returns((string)null);
        }

        public void IsRunningOnBamboo()
        {
            Environment.GetEnvironmentVariable("bamboo_buildNumber").Returns("28");
        }

        public BambooProvider CreateBambooService()
        {
            return new BambooProvider(Environment);
        }
    }
}