using Cake.Common.Tools.NuGet.Restore;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.NuGet.Restore
{
    public sealed class NuGetRestoreSettingsTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Set_NoCache_To_False_By_Default()
            {
                // Given, When
                var settings = new NuGetRestoreSettings();

                // Then
                Assert.False(settings.NoCache);
            }

            [Fact]
            public void Should_Set_RequireConsent_To_False_By_Default()
            {
                // Given, When
                var settings = new NuGetRestoreSettings();

                // Then
                Assert.False(settings.RequireConsent);
            }
        }
    }
}
