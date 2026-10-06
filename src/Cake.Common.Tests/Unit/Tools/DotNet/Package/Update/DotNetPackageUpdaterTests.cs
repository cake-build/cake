// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNet.Package.Update;
using Cake.Common.Tools.DotNet;
using Cake.Testing;

namespace Cake.Common.Tests.Unit.Tools.DotNet.Package.Update;

public sealed class DotNetPackageUpdaterTests
{
    public sealed class TheUpdateMethod
    {
        [Fact]
        public void Should_Throw_If_Process_Was_Not_Started()
        {
            // Given
            var fixture = new DotNetPackageUpdaterFixture();
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
            var fixture = new DotNetPackageUpdaterFixture();
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
            var fixture = new DotNetPackageUpdaterFixture();
            fixture.Settings = null;
            fixture.GivenDefaultToolDoNotExist();

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsArgumentNullException(result, "settings");
        }

        [Fact]
        public void Should_Update_All_Packages()
        {
            // Given
            var fixture = new DotNetPackageUpdaterFixture();

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("package update", result.Args);
        }

        [Fact]
        public void Should_Add_Package_Argument()
        {
            // Given
            var fixture = new DotNetPackageUpdaterFixture();
            fixture.Packages = ["Newtonsoft.Json"];

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("package update \"Newtonsoft.Json\"", result.Args);
        }

        [Fact]
        public void Should_Add_Project_Argument()
        {
            // Given
            var fixture = new DotNetPackageUpdaterFixture();
            fixture.Project = "ToDo.csproj";

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("package update --project \"/Working/ToDo.csproj\"", result.Args);
        }

        [Fact]
        public void Should_Prefer_Project_Argument_Over_Settings()
        {
            // Given
            var fixture = new DotNetPackageUpdaterFixture();
            fixture.Project = "ToDo.csproj";
            fixture.Settings.Project = "FromSettings.csproj";

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("package update --project \"/Working/ToDo.csproj\"", result.Args);
        }

        [Fact]
        public void Should_Add_Project_From_Settings()
        {
            // Given
            var fixture = new DotNetPackageUpdaterFixture();
            fixture.Settings.Project = "ToDo.csproj";

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("package update --project \"/Working/ToDo.csproj\"", result.Args);
        }

        [Fact]
        public void Should_Add_Additional_Arguments()
        {
            // Given
            var fixture = new DotNetPackageUpdaterFixture();
            fixture.Packages = ["Newtonsoft.Json"];
            fixture.Project = "ToDo.csproj";
            fixture.Settings.Interactive = true;
            fixture.Settings.Vulnerable = true;
            fixture.Settings.Verbosity = DotNetVerbosity.Diagnostic;

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("package update \"Newtonsoft.Json\" --project \"/Working/ToDo.csproj\" --interactive --vulnerable --verbosity diagnostic", result.Args);
        }

        [Fact]
        public void Should_Add_Multiple_Packages()
        {
            // Given
            var fixture = new DotNetPackageUpdaterFixture();
            fixture.Packages = ["Contoso.Utilities", "Fabrikam.WebApi@1.2.3"];

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("package update \"Contoso.Utilities\" \"Fabrikam.WebApi@1.2.3\"", result.Args);
        }
    }
}
