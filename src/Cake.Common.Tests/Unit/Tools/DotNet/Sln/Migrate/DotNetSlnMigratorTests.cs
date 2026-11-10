// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNet.Sln.Migrate;
using Cake.Common.Tools.DotNet;
using Cake.Testing;

namespace Cake.Common.Tests.Unit.Tools.DotNet.Sln.Migrate;

public sealed class DotNetSlnMigratorTests
{
    public sealed class TheMigrateMethod
    {
        [Fact]
        public void Should_Throw_If_Process_Was_Not_Started()
        {
            // Given
            var fixture = new DotNetSlnMigratorFixture();
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
            var fixture = new DotNetSlnMigratorFixture();
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
            var fixture = new DotNetSlnMigratorFixture();
            fixture.Settings = null;
            fixture.GivenDefaultToolDoNotExist();

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsArgumentNullException(result, "settings");
        }

        [Fact]
        public void Should_Not_Add_Solution_Argument()
        {
            // Given
            var fixture = new DotNetSlnMigratorFixture();
            fixture.Solution = null;

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("sln migrate", result.Args);
        }

        [Fact]
        public void Should_Add_Solution_Argument()
        {
            // Given
            var fixture = new DotNetSlnMigratorFixture();
            fixture.Solution = "hwapp.sln";

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("sln \"/Working/hwapp.sln\" migrate", result.Args);
        }

        [Fact]
        public void Should_Add_Additional_Arguments()
        {
            // Given
            var fixture = new DotNetSlnMigratorFixture();
            fixture.Solution = "hwapp.sln";
            fixture.Settings.Verbosity = DotNetVerbosity.Diagnostic;

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("sln \"/Working/hwapp.sln\" migrate --verbosity diagnostic", result.Args);
        }
    }
}
