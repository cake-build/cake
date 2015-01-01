using Cake.Common.Tools.NuGet.Sources;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.NuGet.Sources
{
    public sealed class NuGetSourcesSettingsTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Set_IsSensitiveSource_To_False_By_Default()
            {
                // Given, When
                var settings = new NuGetSourcesSettings();

                // Then
                Assert.False(settings.IsSensitiveSource);
            }
        }
    }
}