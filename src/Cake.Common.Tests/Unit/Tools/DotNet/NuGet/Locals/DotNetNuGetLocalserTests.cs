// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNet.NuGet.Locals;
using Cake.Common.Tools.DotNet.NuGet.Locals;
using Cake.Testing;

namespace Cake.Common.Tests.Unit.Tools.DotNet.NuGet.Locals;

public sealed class DotNetNuGetLocalserTests
{
    public sealed class TheListMethod
    {
        [Fact]
        public void Should_Throw_If_Process_Was_Not_Started()
        {
            // Given
            var fixture = new DotNetNuGetLocalserFixture();
            fixture.GivenProcessCannotStart();

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsCakeException(result, ".NET CLI: Process was not started.");
        }

        [Fact]
        public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
        {
            // Given
            var fixture = new DotNetNuGetLocalserFixture();
            fixture.GivenProcessExitsWithCode(1);

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsCakeException(result, ".NET CLI: Process returned an error (exit code 1).");
        }

        [Fact]
        public void Should_Throw_If_Settings_Are_Null()
        {
            // Given
            var fixture = new DotNetNuGetLocalserFixture();
            fixture.Settings = null;
            fixture.GivenDefaultToolDoNotExist();

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsArgumentNullException(result, "settings");
        }

        [Fact]
        public void Should_Throw_If_Folder_Is_Invalid()
        {
            // Given
            var fixture = new DotNetNuGetLocalserFixture();
            fixture.Folder = (DotNetNuGetLocalsFolder)42;

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsArgumentOutOfRangeException(result, "folder");
        }

        [Theory]
        [InlineData(DotNetNuGetLocalsFolder.All, "nuget locals all --list")]
        [InlineData(DotNetNuGetLocalsFolder.HttpCache, "nuget locals http-cache --list")]
        [InlineData(DotNetNuGetLocalsFolder.GlobalPackages, "nuget locals global-packages --list")]
        [InlineData(DotNetNuGetLocalsFolder.Temp, "nuget locals temp --list")]
        [InlineData(DotNetNuGetLocalsFolder.PluginsCache, "nuget locals plugins-cache --list")]
        public void Should_Add_Folder_And_List_Arguments(DotNetNuGetLocalsFolder folder, string expected)
        {
            // Given
            var fixture = new DotNetNuGetLocalserFixture { Folder = folder };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(expected, result.Args);
        }

        [Fact]
        public void Should_Add_Force_English_Output()
        {
            // Given
            var fixture = new DotNetNuGetLocalserFixture();
            fixture.Settings.ForceEnglishOutput = true;

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("nuget locals all --list --force-english-output", result.Args);
        }
    }

    public sealed class TheClearMethod
    {
        [Fact]
        public void Should_Throw_If_Settings_Are_Null()
        {
            // Given
            var fixture = new DotNetNuGetLocalserFixture { Clear = true };
            fixture.Settings = null;
            fixture.GivenDefaultToolDoNotExist();

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsArgumentNullException(result, "settings");
        }

        [Theory]
        [InlineData(DotNetNuGetLocalsFolder.All, "nuget locals all --clear")]
        [InlineData(DotNetNuGetLocalsFolder.HttpCache, "nuget locals http-cache --clear")]
        [InlineData(DotNetNuGetLocalsFolder.GlobalPackages, "nuget locals global-packages --clear")]
        [InlineData(DotNetNuGetLocalsFolder.Temp, "nuget locals temp --clear")]
        [InlineData(DotNetNuGetLocalsFolder.PluginsCache, "nuget locals plugins-cache --clear")]
        public void Should_Add_Folder_And_Clear_Arguments(DotNetNuGetLocalsFolder folder, string expected)
        {
            // Given
            var fixture = new DotNetNuGetLocalserFixture
            {
                Folder = folder,
                Clear = true
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(expected, result.Args);
        }

        [Fact]
        public void Should_Add_Force_English_Output()
        {
            // Given
            var fixture = new DotNetNuGetLocalserFixture { Clear = true };
            fixture.Settings.ForceEnglishOutput = true;

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("nuget locals all --clear --force-english-output", result.Args);
        }
    }
}
