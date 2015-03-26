using Cake.Common.Tools.NuGet.Update;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.NuGet.Update
{
    public sealed class NuGetUpdateSettingsTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Set_Safe_To_False_By_Default()
            {
                // Given, When
                var settings = new NuGetUpdateSettings();

                // Then
                Assert.False(settings.Safe);
            }

            [Fact]
            public void Should_Set_Prerelease_To_False_By_Default()
            {
                // Given, When
                var settings = new NuGetUpdateSettings();

                // Then
                Assert.False(settings.Prerelease);
            }
        }
    }
}
