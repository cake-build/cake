// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.MSBuild;
using Cake.Testing;

namespace Cake.Common.Tests.Unit.Tools.MSBuild;

public sealed class VSWhereMSBuildInstallationLocatorTests
{
    public sealed class TheFindInstallationMethod
    {
        [Fact]
        public void Should_Search_All_Products_Requiring_MSBuild()
        {
            // Given
            var fixture = new VSWhereMSBuildInstallationLocatorFixture();

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("-products \"*\" -latest -requires Microsoft.Component.MSBuild -property installationPath -nologo", result.Args);
        }

        [Fact]
        public void Should_Add_Version_Range_If_Provided()
        {
            // Given
            var fixture = new VSWhereMSBuildInstallationLocatorFixture();
            fixture.VersionRange = "[17.0,18.0)";

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("-products \"*\" -latest -version \"[17.0,18.0)\" -requires Microsoft.Component.MSBuild -property installationPath -nologo", result.Args);
        }

        [Fact]
        public void Should_Add_Prerelease_If_Requested()
        {
            // Given
            var fixture = new VSWhereMSBuildInstallationLocatorFixture();
            fixture.IncludePrerelease = true;

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("-products \"*\" -latest -requires Microsoft.Component.MSBuild -property installationPath -prerelease -nologo", result.Args);
        }

        [Fact]
        public void Should_Return_Installation_Path()
        {
            // Given
            var fixture = new VSWhereMSBuildInstallationLocatorFixture();
            fixture.GivenStandardOutput("/CustomVS/BuildTools");

            // When
            fixture.Run();

            // Then
            Assert.Equal("/CustomVS/BuildTools", fixture.InstallationPath.FullPath);
        }

        [Fact]
        public void Should_Return_Null_If_No_Installation_Was_Found()
        {
            // Given
            var fixture = new VSWhereMSBuildInstallationLocatorFixture();

            // When
            fixture.Run();

            // Then
            Assert.Null(fixture.InstallationPath);
        }

        [Fact]
        public void Should_Return_Null_If_VSWhere_Was_Not_Found()
        {
            // Given
            var fixture = new VSWhereMSBuildInstallationLocatorFixture();
            fixture.GivenStandardOutput("/CustomVS/BuildTools");
            fixture.GivenDefaultToolDoNotExist();

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.Null(result);
            Assert.Null(fixture.InstallationPath);
        }

        [Fact]
        public void Should_Return_Null_If_VSWhere_Fails()
        {
            // Given
            var fixture = new VSWhereMSBuildInstallationLocatorFixture();
            fixture.GivenStandardOutput("/CustomVS/BuildTools");
            fixture.GivenProcessExitsWithCode(1);

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.Null(result);
            Assert.Null(fixture.InstallationPath);
        }
    }
}
