using Cake.Common.Tools.NuGet.List;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.NuGet.List
{
    public sealed class NuGetListSettingsTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Set_AllVersions_To_False_By_Default()
            {
                // Given, When
                var settings = new NuGetListSettings();

                // Then
                Assert.False(settings.AllVersions);
            }

            [Fact]
            public void Should_Set_IncludeDelisted_To_False_By_Default()
            {
                // Given, When
                var settings = new NuGetListSettings();

                // Then
                Assert.False(settings.IncludeDelisted);
            }

            [Fact]
            public void Should_Set_Prerelease_To_False_By_Default()
            {
                // Given, When
                var settings = new NuGetListSettings();

                // Then
                Assert.False(settings.Prerelease);
            }
        }
    }
}
