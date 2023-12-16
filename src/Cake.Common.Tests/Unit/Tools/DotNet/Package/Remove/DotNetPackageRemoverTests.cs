// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNet.Package.Remove;
using Cake.Common.Tools.DotNet;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNet.Package.Remove
{
    public sealed class DotNetPackageRemoverTests
    {
        public sealed class TheRemoveMethod
        {
            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetPackageRemoverFixture();
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
                var fixture = new DotNetPackageRemoverFixture();
                fixture.PackageName = "Microsoft.AspNetCore.StaticFiles";
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET CLI: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Throw_If_PackageName_Is_Null()
            {
                // Given
                var fixture = new DotNetPackageRemoverFixture();
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
                var fixture = new DotNetPackageRemoverFixture();
                fixture.PackageName = "Microsoft.AspNetCore.StaticFiles";
                fixture.Project = "ToDo.csproj";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("remove \"ToDo.csproj\" package Microsoft.AspNetCore.StaticFiles", result.Args);
            }
        }
    }
}
