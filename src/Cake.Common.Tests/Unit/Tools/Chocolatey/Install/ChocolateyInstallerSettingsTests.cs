// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.Chocolatey.Install;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.Chocolatey.Install
{
    public sealed class ChocolateyInstallerSettingsTests
    {
        [Fact]
        public void Should_Set_Prerelease_To_False_By_Default()
        {
            // Given, When
            var settings = new ChocolateyInstallSettings();

            // Then
            Assert.False(settings.Prerelease);
        }
    }
}
