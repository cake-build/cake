using Cake.Common.Tools.NuGet;
using Cake.Common.Tools.NuGet.Push;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.NuGet
{
    public sealed class NuGetPushSettingsTests
    {
        public sealed class TheNonInteractiveProperty
        {
            [Fact]
            public void Should_Default_To_True()
            {
                // Given
                var settings = new NuGetPushSettings();

                // Then
                Assert.True(settings.NonInteractive);
            }
        }
    }
}
