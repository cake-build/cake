using Cake.Common.Build.Jenkins;
using Cake.Core;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class JenkinsFixture
    {
        public ICakeEnvironment Environment { get; set; }

        public JenkinsFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("C:\\build\\CAKE-CAKE-JOB1");
        }

        public void IsRunningOnJenkins()
        {
            Environment.GetEnvironmentVariable("JENKINS_URL").Returns("http://localhost:8080/view/All/builds");
        }
        
        public JenkinsProvider CreateJenkinsProvider()
        {
            return new JenkinsProvider(Environment);
        }
    }
}
