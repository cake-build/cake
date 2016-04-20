using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.Bitrise.Data
{
    public sealed class BitriseProvisioningInfo
    {
        public sealed class TheProvisioningUrlProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BitriseInfoFixture().CreateProvisioningInfo();

                // When
                var result = info.ProvisionUrl;

                //Then
                Assert.Equal("file://cake-build/cake/cake.provision", result);
            }
        }

        public sealed class TheCertificateUrlProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BitriseInfoFixture().CreateProvisioningInfo();

                // When
                var result = info.CertificateUrl;

                //Then
                Assert.Equal("file://cake-build/cake/Cert.p12", result);
            }
        }

        public sealed class TheCertificatePassPhraseProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BitriseInfoFixture().CreateProvisioningInfo();

                // When
                var result = info.CertificatePassphrase;

                //Then
                Assert.Equal("CAKE", result);
            }
        }
    }
}
