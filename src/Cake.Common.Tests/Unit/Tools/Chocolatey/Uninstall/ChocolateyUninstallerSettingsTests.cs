// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.Chocolatey.Uninstall;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.Chocolatey.Uninstall
{
    public sealed class ChocolateyUninstallerSettingsTests
    {
        [Fact]
        public void Should_Set_Global_Arguments_To_False_By_Default()
        {
            // Given, When
            var settings = new ChocolateyUninstallSettings();

            // Then
            Assert.False(settings.GlobalArguments);
        }

        [Fact]
        public void Should_Set_Global_Package_Arguments_To_False_By_Default()
        {
            // Given, When
            var settings = new ChocolateyUninstallSettings();

            // Then
            Assert.False(settings.GlobalPackageParameters);
        }

        [Fact]
        public void Should_Set_All_Versions_To_False_By_Default()
        {
            // Given, When
            var settings = new ChocolateyUninstallSettings();

            // Then
            Assert.False(settings.AllVersions);
        }

        [Fact]
        public void Should_Set_Ignore_Package_Exit_Codes_To_False_By_Default()
        {
            // Given, When
            var settings = new ChocolateyUninstallSettings();

            // Then
            Assert.False(settings.IgnorePackageExitCodes);
        }

        [Fact]
        public void Should_Set_Use_Package_Exit_Codes_To_False_By_Default()
        {
            // Given, When
            var settings = new ChocolateyUninstallSettings();

            // Then
            Assert.False(settings.UsePackageExitCodes);
        }

        [Fact]
        public void Should_Set_Auto_Uninstaller_To_False_By_Default()
        {
            // Given, When
            var settings = new ChocolateyUninstallSettings();

            // Then
            Assert.False(settings.UseAutoUninstaller);
        }

        [Fact]
        public void Should_Set_Skip_Auto_Uninstaller_To_False_By_Default()
        {
            // Given, When
            var settings = new ChocolateyUninstallSettings();

            // Then
            Assert.False(settings.SkipAutoUninstaller);
        }

        [Fact]
        public void Should_Set_Fail_On_Auto_Uninstaller_To_False_By_Default()
        {
            // Given, When
            var settings = new ChocolateyUninstallSettings();

            // Then
            Assert.False(settings.FailOnAutoUninstaller);
        }

        [Fact]
        public void Should_Set_Ignore_Auto_Uninstaller_To_False_By_Default()
        {
            // Given, When
            var settings = new ChocolateyUninstallSettings();

            // Then
            Assert.False(settings.IgnoreAutoUninstaller);
        }
    }
}
