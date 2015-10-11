using Cake.Common.Tools.NuGet.Install;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.NuGet.Install
{
    public sealed class NuGetInstallSettingsTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Set_NoCache_To_False_By_Default()
            {
                // Given, When
                var settings = new NuGetInstallSettings();

                // Then
                Assert.False(settings.NoCache);
            }

            [Fact]
            public void Should_Set_RequireConsent_To_False_By_Default()
            {
                // Given, When
                var settings = new NuGetInstallSettings();

                // Then
                Assert.False(settings.RequireConsent);
            }

            [Fact]
            public void Should_Set_Prerelease_To_False_By_Default()
            {
                // Given, When
                var settings = new NuGetInstallSettings();

                // Then
                Assert.False(settings.Prerelease);
            }

            [Fact]
            public void Should_Set_ExcludeVersion_To_False_By_Default()
            {
                // Given, When
                var settings = new NuGetInstallSettings();

                // Then
                Assert.False(settings.ExcludeVersion);
            }
        }
    }
}