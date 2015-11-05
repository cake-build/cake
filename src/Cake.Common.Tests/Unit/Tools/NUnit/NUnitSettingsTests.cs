using Cake.Common.Tools.NUnit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.NUnit
{
    public sealed class NUnitSettingsTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Enable_Shadow_Copying_By_Default()
            {
                // Given, When
                var settings = new NUnitSettings();

                // Then
                Assert.True(settings.ShadowCopy);
            }

            [Fact]
            public void Should_Use_Single_Process_By_Default()
            {
                // Given, When
                var settings = new NUnitSettings();

                // Then
                Assert.Equal(settings.Process, NUnitProcessOption.Single);
            }

            [Fact]
            public void Should_Not_Use_SingleThreadedApartment_By_Default()
            {
                // Given, When
                var settings = new NUnitSettings();

                // Then
                Assert.False(settings.UseSingleThreadedApartment);
            }

            [Fact]
            public void Should_Use_Default_AppDomainUsage_By_Default()
            {
                // Given, When
                var settings = new NUnitSettings();

                // Then
                Assert.Equal(settings.AppDomainUsage, NUnitAppDomainUsage.Default);
            }
        }
    }
}