// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Tests.Fixtures;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Core.Tests.Unit.Scripting.Processors
{
    public sealed class LoadDirectiveProcessorTests
    {
        [Theory]
        [InlineData("#l \"utils.cake\"")]
        [InlineData("#load \"utils.cake\"")]
        public void Should_Process_Single_Script_Reference_Found_In_Source(string source)
        {
            // Given
            var fixture = new ScriptAnalyzerFixture();
            fixture.AddFileLoadDirectiveProvider();
            fixture.GivenScriptExist("/Working/script.cake", source);
            fixture.GivenScriptExist("/Working/utils.cake", "Console.WriteLine();");

            // When
            var result = fixture.Analyze("/Working/script.cake");

            // Then
            Assert.Equal(1, result.Script.Includes.Count);
            Assert.Equal("/Working/utils.cake", result.Script.Includes[0].Path.FullPath);
        }

        [Theory]
        [InlineData("#l \"test/my utils.cake\"")]
        [InlineData("#load \"test/my utils.cake\"")]
        public void Should_Process_Single_Script_Reference_With_Spaces_In_File_Name_Found_In_Source(string source)
        {
            // Given
            var fixture = new ScriptAnalyzerFixture();
            fixture.AddFileLoadDirectiveProvider();
            fixture.GivenScriptExist("/Working/script.cake", source);
            fixture.GivenScriptExist("/Working/test/my utils.cake", "Console.WriteLine();");

            // When
            var result = fixture.Analyze("/Working/script.cake");

            // Then
            Assert.Equal(1, result.Script.Includes.Count);
            Assert.Equal("/Working/test/my utils.cake", result.Script.Includes[0].Path.FullPath);
        }

        [Theory]
        [InlineData("utils.cs")]
        [InlineData("utils.foo")]
        public void Should_Process_Single_Script_Reference_With_Any_Extension_Found_In_Source(string filename)
        {
            // Given
            var fixture = new ScriptAnalyzerFixture();
            fixture.AddFileLoadDirectiveProvider();
            fixture.GivenScriptExist("/Working/script.cake", $"#l \"{filename}\"");
            fixture.GivenScriptExist($"/Working/{filename}", "Console.WriteLine();");

            // When
            var result = fixture.Analyze("/Working/script.cake");

            // Then
            Assert.Equal(1, result.Script.Includes.Count);
            Assert.Equal($"/Working/{filename}", result.Script.Includes[0].Path.FullPath);
        }

        [Theory]
        [InlineData("#l \"utils.cake\"\n#l \"other.cake\"")]
        [InlineData("#load \"utils.cake\"\n#load \"other.cake\"")]
        public void Should_Process_Multiple_Script_References_Found_In_Source(string source)
        {
            // Given
            var fixture = new ScriptAnalyzerFixture();
            fixture.AddFileLoadDirectiveProvider();
            fixture.GivenScriptExist("/Working/script.cake", source);
            fixture.GivenScriptExist("/Working/utils.cake", "Console.WriteLine();");
            fixture.GivenScriptExist("/Working/other.cake", "Console.WriteLine();");

            // When
            var result = fixture.Analyze("/Working/script.cake");

            // Then
            Assert.Equal(2, result.Script.Includes.Count);
            Assert.Equal("/Working/utils.cake", result.Script.Includes[0].Path.FullPath);
            Assert.Equal("/Working/other.cake", result.Script.Includes[1].Path.FullPath);
        }

        [Theory]
        [InlineData("#l \"%CAKE_TEST_SCRIPT_PATH%/utils.cake\"")]
        public void Should_Process_Environment_Variable_Found_In_Source(string source)
        {
            // Given
            var fixture = new ScriptAnalyzerFixture();
            fixture.Environment.SetEnvironmentVariable("CAKE_TEST_SCRIPT_PATH", "test");
            fixture.AddFileLoadDirectiveProvider();
            fixture.GivenScriptExist("/Working/script.cake", source);
            fixture.GivenScriptExist("/Working/test/utils.cake", "Console.WriteLine();");

            // When
            var result = fixture.Analyze("/Working/script.cake");

            // Then
            Assert.Equal(1, result.Script.Includes.Count);
            Assert.Equal("/Working/test/utils.cake", result.Script.Includes[0].Path.FullPath);
        }

        [Theory]
        [InlineData("#l \"%CAKE_TEST_SCRIPT_BASE_PATH%/%CAKE_TEST_SCRIPT_PATH%/utils.cake\"")]
        public void Should_Process_Multiple_Environment_Variable_Found_In_Source(string source)
        {
            // Given
            var fixture = new ScriptAnalyzerFixture();
            fixture.Environment.SetEnvironmentVariable("CAKE_TEST_SCRIPT_BASE_PATH", "test");
            fixture.Environment.SetEnvironmentVariable("CAKE_TEST_SCRIPT_PATH", "scripts");
            fixture.AddFileLoadDirectiveProvider();
            fixture.GivenScriptExist("/Working/script.cake", source);
            fixture.GivenScriptExist("/Working/test/scripts/utils.cake", "Console.WriteLine();");

            // When
            var result = fixture.Analyze("/Working/script.cake");

            // Then
            Assert.Equal(1, result.Script.Includes.Count);
            Assert.Equal("/Working/test/scripts/utils.cake", result.Script.Includes[0].Path.FullPath);
        }

        [Theory]
        [InlineData("#load \"scripts/*\"")]
        [InlineData("#load \"scripts/*.*\"")]
        [InlineData("#load \"scripts/**/*\"")]
        [InlineData("#load \"scripts/**/*.*\"")]
        [InlineData("#load \"scripts/{utils,other}.{cs,kake}\"")]
        [InlineData("#load \"/Working/scripts/*\"")]
        [InlineData("#load \"/Working/scripts/*.*\"")]
        [InlineData("#load \"/Working/scripts/**/*\"")]
        [InlineData("#load \"/Working/scripts/**/*.*\"")]
        [InlineData("#load \"/Working/scripts/{utils,other}.{cs,kake}\"")]
        public void Should_Ignore_Globber_Matches_With_Invalid_Extensions(string source)
        {
            // Given
            var fixture = new ScriptAnalyzerFixture();
            fixture.AddFileLoadDirectiveProvider();
            fixture.GivenScriptExist("/Working/bootstrap.cake", source);
            fixture.GivenScriptExist("/Working/scripts/utils.cs", "Console.WriteLine();");
            fixture.GivenScriptExist("/Working/scripts/other.kake", "Console.WriteLine();");

            // When
            var result = fixture.Analyze("/Working/bootstrap.cake");

            // Then
            Assert.Equal(0, result.Script.Includes.Count);
        }

        [Theory]
        [InlineData("#load \"scripts/*\"")]
        [InlineData("#load \"scripts/*.*\"")]
        [InlineData("#load \"scripts/*.cake\"")]
        [InlineData("#load \"scripts/**/*\"")]
        [InlineData("#load \"scripts/**/*.*\"")]
        [InlineData("#load \"scripts/**/*.cake\"")]
        [InlineData("#load \"scripts/{utils,other}.cake\"")]
        [InlineData("#load \"/Working/scripts/*\"")]
        [InlineData("#load \"/Working/scripts/*.*\"")]
        [InlineData("#load \"/Working/scripts/*.cake\"")]
        [InlineData("#load \"/Working/scripts/**/*\"")]
        [InlineData("#load \"/Working/scripts/**/*.*\"")]
        [InlineData("#load \"/Working/scripts/**/*.cake\"")]
        [InlineData("#load \"/Working/scripts/{utils,other}.cake\"")]
        public void Should_Process_Globber_Matches_With_Valid_Extension(string source)
        {
            // Given
            var fixture = new ScriptAnalyzerFixture();
            fixture.AddFileLoadDirectiveProvider();
            fixture.GivenScriptExist("/Working/bootstrap.cake", source);
            fixture.GivenScriptExist("/Working/scripts/utils.cake", "Console.WriteLine();");
            fixture.GivenScriptExist("/Working/scripts/other.cake", "Console.WriteLine();");

            // When
            var result = fixture.Analyze("/Working/bootstrap.cake");

            // Then
            Assert.Equal(2, result.Script.Includes.Count);
            Assert.Equal("/Working/scripts/utils.cake", result.Script.Includes[0].Path.FullPath);
            Assert.Equal("/Working/scripts/other.cake", result.Script.Includes[1].Path.FullPath);
        }

        [Fact]
        public void Should_Insert_Line_Directives_When_Processing_Load_Directives()
        {
            // Given
            var fixture = new ScriptAnalyzerFixture();
            fixture.AddFileLoadDirectiveProvider();
            fixture.GivenScriptExist("/Working/a.cake", "int x=0;\n#l b.cake\nint y=2;");
            fixture.GivenScriptExist("/Working/b.cake", "int z=1;\n#l c.cake\nint p=4;");
            fixture.GivenScriptExist("/Working/c.cake", "int o=3;\n#r d.dll");

            // When
            var result = fixture.Analyze("/Working/a.cake");

            // Then
            Assert.Equal(13, result.Lines.Count);
            Assert.Equal(result.Lines[0], "#line 1 \"/Working/a.cake\"");
            Assert.Equal(result.Lines[1], "int x=0;");
            Assert.Equal(result.Lines[2], "#line 1 \"/Working/b.cake\"");
            Assert.Equal(result.Lines[3], "int z=1;");
            Assert.Equal(result.Lines[4], "#line 1 \"/Working/c.cake\"");
            Assert.Equal(result.Lines[5], "int o=3;");
            Assert.Equal(result.Lines[6], "// #r d.dll");
            Assert.Equal(result.Lines[7], "#line 2 \"/Working/b.cake\"");
            Assert.Equal(result.Lines[8], "// #l c.cake");
            Assert.Equal(result.Lines[9], "int p=4;");
            Assert.Equal(result.Lines[10], "#line 2 \"/Working/a.cake\"");
            Assert.Equal(result.Lines[11], "// #l b.cake");
            Assert.Equal(result.Lines[12], "int y=2;");
        }

        [Theory]
        [InlineData("#load \"/utils.cake\"")]
        [InlineData("#load \"local:?path=/utils.cake\"")]
        public void Should_Process_AbsolutePath_Script_Reference_Found_In_Source(string source)
        {
            // Given
            var fixture = new ScriptAnalyzerFixture();
            fixture.AddFileLoadDirectiveProvider();
            fixture.GivenScriptExist("/Working/script.cake", source);
            fixture.GivenScriptExist("/utils.cake", "Console.WriteLine();");

            // When
            var result = fixture.Analyze("/Working/script.cake");

            // Then
            Assert.Equal(1, result.Script.Includes.Count);
            Assert.Equal("/utils.cake", result.Script.Includes[0].Path.FullPath);
        }

        [Theory]
        [InlineData("#load \"test/utils.cake\"")]
        [InlineData("#load \"local:?path=test/utils.cake\"")]
        public void Should_Process_RelativePath_Script_Reference_Found_In_Source(string source)
        {
            // Given
            var fixture = new ScriptAnalyzerFixture();
            fixture.AddFileLoadDirectiveProvider();
            fixture.GivenScriptExist("/Working/script.cake", source);
            fixture.GivenScriptExist("/Working/test/utils.cake", "Console.WriteLine();");

            // When
            var result = fixture.Analyze("/Working/script.cake");

            // Then
            Assert.Equal(1, result.Script.Includes.Count);
            Assert.Equal("/Working/test/utils.cake", result.Script.Includes[0].Path.FullPath);
        }

        [WindowsTheory]
        [InlineData("#load \"C:/utils.cake\"")]
        [InlineData("#load \"local:?path=C:/utils.cake\"")]
        public void Should_Process_WindowsAbsolutePath_Script_Reference_Found_In_Source(string source)
        {
            // Given
            var fixture = new ScriptAnalyzerFixture(windows: true);
            fixture.AddFileLoadDirectiveProvider();
            fixture.GivenScriptExist("C:/Working/script.cake", source);
            fixture.GivenScriptExist("C:/utils.cake", "Console.WriteLine();");

            // When
            var result = fixture.Analyze("C:/Working/script.cake");

            // Then
            Assert.Equal(1, result.Script.Includes.Count);
            Assert.Equal("C:/utils.cake", result.Script.Includes[0].Path.FullPath);
        }

        [WindowsTheory]
        [InlineData("#load \"test/utils.cake\"")]
        [InlineData("#load \"local:?path=test/utils.cake\"")]
        public void Should_Process_WindowsRelativePath_Script_Reference_Found_In_Source(string source)
        {
            // Given
            var fixture = new ScriptAnalyzerFixture(windows: true);
            fixture.AddFileLoadDirectiveProvider();
            fixture.GivenScriptExist("C:/Working/script.cake", source);
            fixture.GivenScriptExist("C:/Working/test/utils.cake", "Console.WriteLine();");

            // When
            var result = fixture.Analyze("C:/Working/script.cake");

            // Then
            Assert.Equal(1, result.Script.Includes.Count);
            Assert.Equal("C:/Working/test/utils.cake", result.Script.Includes[0].Path.FullPath);
        }
    }
}
