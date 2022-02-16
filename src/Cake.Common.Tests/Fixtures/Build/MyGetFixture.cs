using Cake.Common.Build.MyGet;
using Cake.Common.Tests.Fakes;
using Cake.Core;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class MyGetFixture
    {
        public ICakeEnvironment Environment { get; set; }

        public FakeBuildSystemServiceMessageWriter Writer { get; set; }

        public MyGetFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();
            Environment.GetEnvironmentVariable("BuildRunner").Returns((string)null);
            Writer = new FakeBuildSystemServiceMessageWriter();
        }

        public void IsRunningOnMyGet(bool? capitalCase = default)
        {
            var buildRunnerValue = "MyGet";
            if (capitalCase.HasValue)
            {
                buildRunnerValue = capitalCase.Value ? "MYGET" : "myget";
            }

            Environment.GetEnvironmentVariable("BuildRunner").Returns(buildRunnerValue);
        }

        public MyGetProvider CreateMyGetProvider() => new MyGetProvider(Environment, Writer);
    }
}
