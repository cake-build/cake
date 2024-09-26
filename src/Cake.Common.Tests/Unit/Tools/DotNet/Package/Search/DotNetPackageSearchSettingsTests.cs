using Cake.Common.Tools.DotNet.Package.Search;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNet.Package.Search
{
    public sealed class DotNetPackageSearchSettingsTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Set_ExactMatch_To_False_By_Default()
            {
                // Given, When
                var settings = new DotNetPackageSearchSettings();

                // Then
                Assert.False(settings.ExactMatch);
            }

            [Fact]
            public void Should_Set_Take_To_Null_By_Default()
            {
                // Given, When
                var settings = new DotNetPackageSearchSettings();

                // Then
                Assert.Null(settings.Take);
            }

            [Fact]
            public void Should_Set_Skip_To_Null_By_Default()
            {
                // Given, When
                var settings = new DotNetPackageSearchSettings();

                // Then
                Assert.Null(settings.Skip);
            }

            [Fact]
            public void Should_Set_Prerelease_To_False_By_Default()
            {
                // Given, When
                var settings = new DotNetPackageSearchSettings();

                // Then
                Assert.False(settings.Prerelease);
            }
        }
    }
}
