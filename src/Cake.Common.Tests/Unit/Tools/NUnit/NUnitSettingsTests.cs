using Cake.Common.Tools.NUnit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.NUnit
{
    public sealed class NUnitSettingsTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Set_Results_Directory_To_Null_By_Default()
            {
                // Given, When
                var settings = new NUnitSettings();

                // Then
                Assert.Null(settings.ResultsDirectory);
            }

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
