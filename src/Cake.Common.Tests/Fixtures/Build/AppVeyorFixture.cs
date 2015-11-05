using Cake.Common.Build.AppVeyor;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class AppVeyorFixture
    {
        public ICakeEnvironment Environment { get; set; }
        public IProcessRunner ProcessRunner { get; set; }

        public AppVeyorFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("/Working");
            Environment.GetEnvironmentVariable("APPVEYOR").Returns((string)null);

            ProcessRunner = Substitute.For<IProcessRunner>();
        }

        public void IsRunningOnAppVeyor()
        {
            Environment.GetEnvironmentVariable("APPVEYOR").Returns("True");
        }

        public AppVeyorProvider CreateAppVeyorService()
        {
            return new AppVeyorProvider(Environment, ProcessRunner);
        }
    }
}