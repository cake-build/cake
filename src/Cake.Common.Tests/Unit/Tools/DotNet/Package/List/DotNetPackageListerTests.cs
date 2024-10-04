// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNet.Package.List;
using Cake.Common.Tools.DotNet;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNet.Package.List
{
    public sealed class DotNetPackageListerTests
    {
        public sealed class TheListMethod
        {
            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetPackageListerFixture();
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
                var fixture = new DotNetPackageListerFixture();
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
                var fixture = new DotNetPackageListerFixture();
                fixture.Settings = null;
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Add_Project_Argument()
            {
                // Given
                var fixture = new DotNetPackageListerFixture();
                fixture.Project = "ToDo.csproj";
                fixture.GivenPackgeListResult();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("list \"ToDo.csproj\" package --format json --output-version 1", result.Args);
            }

            [Fact]
            public void Should_Add_Additional_Arguments()
            {
                // Given
                var fixture = new DotNetPackageListerFixture();
                fixture.Settings.ConfigFile = "./nuget.config";
                fixture.Settings.Deprecated = true;
                fixture.Settings.Framework = "net7.0";
                fixture.Settings.HighestMinor = true;
                fixture.Settings.HighestPatch = true;
                fixture.Settings.Prerelease = true;
                fixture.Settings.Transitive = true;
                fixture.Settings.Interactive = true;
                fixture.Settings.Outdated = true;
                fixture.Settings.Source.Add("http://www.nuget.org/api/v2/package");
                fixture.Settings.Source.Add("http://www.symbolserver.org/");
                fixture.Settings.Vulnerable = true;
                fixture.GivenPackgeListResult();

                // When
                var result = fixture.Run();

                // Then
                var expected = "list package --config \"/Working/nuget.config\" --deprecated --framework net7.0 --highest-minor --highest-patch --include-prerelease --include-transitive --interactive --outdated ";
                expected += "--source \"http://www.nuget.org/api/v2/package\" --source \"http://www.symbolserver.org/\" --vulnerable --format json --output-version 1";
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Return_Correct_Result()
            {
                // Given
                var fixture = new DotNetPackageListerFixture();
                fixture.GivenPackgeListResult();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(1, fixture.Result.Version);
                Assert.Contains(fixture.Result.Projects, item => item.Path == "src/lib/MyProject.csproj");
            }
        }
    }
}
