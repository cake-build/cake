// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.OctopusDeploy;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.OctopusDeploy
{
    public sealed class OctoPackTests
    {
        public sealed class ThePackMethod
        {
            [Fact]
            public void Should_Throw_If_Id_Is_Null()
            {
                // Given
                var fixture = new OctopusDeployPackerFixture();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "id");
            }

            [Fact]
            public void Should_Add_Id_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployPackerFixture();
                fixture.Id = "MyPackage";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("pack --id MyPackage", result.Args);
            }

            [Fact]
            public void Should_Add_Version_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployPackerFixture();
                fixture.Id = "MyPackage";
                fixture.Settings.Version = "1.2.3";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("pack --id MyPackage --version 1.2.3", result.Args);
            }

            [Fact]
            public void Should_Add_OutFolder_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployPackerFixture();
                fixture.Id = "MyPackage";
                fixture.Settings.OutFolder = "out";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("pack --id MyPackage --outFolder \"/Working/out\"", result.Args);
            }

            [Fact]
            public void Should_Add_BasePath_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployPackerFixture();
                fixture.Id = "MyPackage";
                fixture.Settings.BasePath = "base";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("pack --id MyPackage --basePath \"/Working/base\"", result.Args);
            }

            [Fact]
            public void Should_Add_Author_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployPackerFixture();
                fixture.Id = "MyPackage";
                fixture.Settings.Author = "author";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("pack --id MyPackage --author \"author\"", result.Args);
            }

            [Fact]
            public void Should_Add_Title_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployPackerFixture();
                fixture.Id = "MyPackage";
                fixture.Settings.Title = "title";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("pack --id MyPackage --title \"title\"", result.Args);
            }

            [Fact]
            public void Should_Add_Description_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployPackerFixture();
                fixture.Id = "MyPackage";
                fixture.Settings.Description = "description";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("pack --id MyPackage --description \"description\"", result.Args);
            }

            [Fact]
            public void Should_Add_ReleaseNotes_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployPackerFixture();
                fixture.Id = "MyPackage";
                fixture.Settings.ReleaseNotes = "releasenotes";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("pack --id MyPackage --releaseNotes \"releasenotes\"", result.Args);
            }

            [Fact]
            public void Should_Add_ReleaseNotesFile_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployPackerFixture();
                fixture.Id = "MyPackage";
                fixture.Settings.ReleaseNotesFile = "releasenotes.md";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("pack --id MyPackage --releaseNotesFile \"/Working/releasenotes.md\"", result.Args);
            }

            [Fact]
            public void Should_Add_Include_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployPackerFixture();
                fixture.Id = "MyPackage";
                fixture.Settings.Include = new[]
                {
                    "bin/*.dll",
                    "bin/*.pdb"
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("pack --id MyPackage --include \"bin/*.dll\" --include \"bin/*.pdb\"", result.Args);
            }

            [Fact]
            public void Should_Add_Overwrite_To_Arguments_If_True()
            {
                // Given
                var fixture = new OctopusDeployPackerFixture();
                fixture.Id = "MyPackage";
                fixture.Settings.Overwrite = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("pack --id MyPackage --overwrite", result.Args);
            }

            [Fact]
            public void Should_Add_Format_To_Arguments_If_Zip()
            {
               // Given
               var fixture = new OctopusDeployPackerFixture();
               fixture.Id = "MyPackage";
               fixture.Settings.Format = OctopusPackFormat.Zip;

               // When
               var result = fixture.Run();

               // Then
               Assert.Equal("pack --id MyPackage --format Zip", result.Args);
            }
        }
    }
}
