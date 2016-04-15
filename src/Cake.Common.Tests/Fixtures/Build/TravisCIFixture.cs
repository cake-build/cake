using Cake.Common.Build.TravisCI;
using Cake.Core;
using Cake.Testing;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class TravisCIFixture
    {
        public ICakeEnvironment Environment { get; set; }
        public FakeLog Log { get; set; }

        public TravisCIFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("/home/travis/.local");
            Log = new FakeLog();
        }

        public void IsRunningOnTravicCI()
        {
            Environment.GetEnvironmentVariable("TRAVIS").Returns("true");
        }

        public TravisCIProvider CreateTravisCIProvider()
        {
            return new TravisCIProvider(Environment, Log);
        }
    }
}
