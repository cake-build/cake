// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.TextTransform;
using Cake.Core;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.TextTransform
{
    public sealed class TextTransformRunnerTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Source_File_Is_Null()
            {
                // Given
                var fixture = new TextTransformFixture();
                fixture.SourceFile = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "sourceFile");
            }

            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new TextTransformFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Text_Transform_Runner_Was_Not_Found()
            {
                // Given
                var fixture = new TextTransformFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("TextTransform: Could not locate executable.", result?.Message);
            }

            [Theory]
            [InlineData("/bin/tools/TextTransform/TextTransform.exe", "/bin/tools/TextTransform/TextTransform.exe")]
            [InlineData("./tools/TextTransform/TextTransform.exe", "/Working/tools/TextTransform/TextTransform.exe")]
            public void Should_Use_Text_Transform_Runner_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new TextTransformFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Fact]
            public void Should_Find_Text_Transform_Runner_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new TextTransformFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/TextTransform.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Use_Provided_Source_File_In_Process_Arguments()
            {
                // Given
                var fixture = new TextTransformFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test.tt\"", result.Args);
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new TextTransformFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working", result.Process.WorkingDirectory.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new TextTransformFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("TextTransform: Process was not started.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new TextTransformFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("TextTransform: Process returned an error (exit code 1).", result?.Message);
            }

            [Fact]
            public void Should_Add_Assembly_To_Argument_If_Provided()
            {
                // Given
                var fixture = new TextTransformFixture();
                fixture.Settings.Assembly = "Cake.Core";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-r Cake.Core \"/Working/Test.tt\"", result.Args);
            }

            [Fact]
            public void Should_Add_OutputFile_If_Provided()
            {
                // Given
                var fixture = new TextTransformFixture();
                fixture.Settings.OutputFile = "Test.cs";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-out \"/Working/Test.cs\" \"/Working/Test.tt\"", result.Args);
            }

            [Fact]
            public void Should_Add_Namespace_If_Provided()
            {
                // Given
                var fixture = new TextTransformFixture();
                fixture.Settings.Namespace = "Cake.Core";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-u Cake.Core \"/Working/Test.tt\"", result.Args);
            }

            [Fact]
            public void Should_Add_Refernce_Path_If_Provided()
            {
                // Given
                var fixture = new TextTransformFixture();
                fixture.Settings.ReferencePath = "./Cake/";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-P \"/Working/Cake\" \"/Working/Test.tt\"", result.Args);
            }

            [Fact]
            public void Should_Add_Include_Directory_If_Provided()
            {
                // Given
                var fixture = new TextTransformFixture();
                fixture.Settings.IncludeDirectory = "./Transforms/";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-I \"/Working/Transforms\" \"/Working/Test.tt\"", result.Args);
            }
        }
    }
}