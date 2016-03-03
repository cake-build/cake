using Cake.Common.Build.Bitrise;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class BitriseFixture
    {
        public ICakeEnvironment Environment { get; set; }
        public IProcessRunner ProcessRunner { get; set; }

        public BitriseFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("/Working");
        }

        public void IsRunningOnBitrise()
        {
            Environment.GetEnvironmentVariable("BITRISE_BUILD_URL").Returns("True");
        }

        public BitriseProvider CreateBitriseService()
        {
            return new BitriseProvider(Environment);
        }
    }
}
