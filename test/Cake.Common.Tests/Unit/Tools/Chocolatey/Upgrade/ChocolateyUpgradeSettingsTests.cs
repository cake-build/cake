using Cake.Common.Tools.Chocolatey.Upgrade;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.Chocolatey.Upgrade
{
    public sealed class ChocolateyUpgradeSettingsTests
    {
        [Fact]
        public void Should_Set_Prerelease_To_False_By_Default()
        {
            // Given, When
            var settings = new ChocolateyUpgradeSettings();

            // Then
            Assert.False(settings.Prerelease);
        }
    }
}