// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNet.Package.Add;
using Cake.Common.Tools.DotNet;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNet.Package.Add
{
    public sealed class DotNetPackageAdderTests
    {
        public sealed class TheAddMethod
        {
            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetPackageAdderFixture();
                fixture.PackageName = "Microsoft.AspNetCore.StaticFiles";
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
                var fixture = new DotNetPackageAdderFixture();
                fixture.PackageName = "Microsoft.AspNetCore.StaticFiles";
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
                var fixture = new DotNetPackageAdderFixture();
                fixture.PackageName = "Microsoft.AspNetCore.StaticFiles";
                fixture.Settings = null;
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_PackageName_Is_Null()
            {
                // Given
                var fixture = new DotNetPackageAdderFixture();
                fixture.PackageName = null;
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "packageName");
            }

            [Fact]
            public void Should_Add_Project_Argument()
            {
                // Given
                var fixture = new DotNetPackageAdderFixture();
                fixture.PackageName = "Microsoft.AspNetCore.StaticFiles";
                fixture.Project = "ToDo.csproj";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("add \"ToDo.csproj\" package Microsoft.AspNetCore.StaticFiles", result.Args);
            }

            [Fact]
            public void Should_Add_Additional_Arguments()
            {
                // Given
                var fixture = new DotNetPackageAdderFixture();
                fixture.PackageName = "Microsoft.AspNetCore.StaticFiles";
                fixture.Settings.Framework = "net8.0";
                fixture.Settings.Interactive = true;
                fixture.Settings.NoRestore = true;
                fixture.Settings.PackageDirectory = "./src/project";
                fixture.Settings.Prerelease = true;
                fixture.Settings.Source = "http://www.nuget.org/api/v2/package";
                fixture.Settings.Version = "1.0.0";
                fixture.Settings.Verbosity = DotNetVerbosity.Diagnostic;

                // When
                var result = fixture.Run();

                // Then
                var expected = "add package Microsoft.AspNetCore.StaticFiles --framework net8.0 --interactive --no-restore --package-directory \"/Working/src/project\" --prerelease --source \"http://www.nuget.org/api/v2/package\" --version \"1.0.0\" --verbosity diagnostic";
                Assert.Equal(expected, result.Args);
            }
        }
    }
}
