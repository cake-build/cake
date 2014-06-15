using Cake.Common.NUnit;
using Xunit;

namespace Cake.Common.Tests.Unit.NUnit
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
        }
    }
}
