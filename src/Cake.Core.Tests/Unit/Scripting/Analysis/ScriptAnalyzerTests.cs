// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Cake.Core.Tests.Fixtures;
using Xunit;

namespace Cake.Core.Tests.Unit.Scripting.Analysis
{
    public sealed class ScriptAnalyzerTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Given
                var fixture = new ScriptAnalyzerFixture();
                fixture.FileSystem = null;

                // When
                var result = Record.Exception(() => fixture.CreateAnalyzer());

                // Then
                AssertEx.IsArgumentNullException(result, "fileSystem");
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fixture = new ScriptAnalyzerFixture();
                fixture.Environment = null;

                // When
                var result = Record.Exception(() => fixture.CreateAnalyzer());

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given
                var fixture = new ScriptAnalyzerFixture();
                fixture.Log = null;

                // When
                var result = Record.Exception(() => fixture.CreateAnalyzer());

                // Then
                AssertEx.IsArgumentNullException(result, "log");
            }
        }

        public sealed class TheProcessMethod
        {
            [Fact]
            public void Should_Throw_If_File_Path_Is_Null()
            {
                // Given
                var fixture = new ScriptAnalyzerFixture();

                // When
                var result = Record.Exception(() => fixture.Analyze(null));

                // Then
                AssertEx.IsArgumentNullException(result, "path");
            }

            [Fact]
            public void Should_Throw_If_Script_Was_Not_Found()
            {
                // Given
                var fixture = new ScriptAnalyzerFixture();

                // When
                var result = Record.Exception(() => fixture.Analyze("/Working/notfound.cake"));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Could not find script '/Working/notfound.cake'.", result?.Message);
            }

            [Fact]
            public void Should_Return_Code_Read_From_File()
            {
                // Given
                var fixture = new ScriptAnalyzerFixture();
                fixture.GivenScriptExist("/Working/script.cake", "Console.ReadKey();");

                // When
                var result = fixture.Analyze("/Working/script.cake");

                // Then
                Assert.Equal(2, result.Lines.Count);
                Assert.Equal("#line 1 \"/Working/script.cake\"", result.Lines[0]);
                Assert.Equal("Console.ReadKey();", result.Lines[1]);
            }

            [Theory]
            [InlineData("#r \"hello.dll\"")]
            [InlineData("#reference \"hello.dll\"")]
            public void Should_Return_Single_Assembly_Reference_Found_In_Source(string source)
            {
                // Given
                var fixture = new ScriptAnalyzerFixture();
                fixture.GivenScriptExist("/Working/script.cake", source);

                // When
                var result = fixture.Analyze("/Working/script.cake");

                // Then
                Assert.Equal(1, result.Script.References.Count);
                Assert.Equal("hello.dll", result.Script.References.ElementAt(0));
            }

            [Theory]
            [InlineData("#r \"hello world.dll\"")]
            [InlineData("#reference \"hello world.dll\"")]
            public void Should_Return_Single_Assembly_Reference_With_Space_In_File_Name_Found_In_Source(string source)
            {
                // Given
                var fixture = new ScriptAnalyzerFixture();
                fixture.GivenScriptExist("/Working/script.cake", source);

                // When
                var result = fixture.Analyze("/Working/script.cake");

                // Then
                Assert.Equal(1, result.Script.References.Count);
                Assert.Equal("hello world.dll", result.Script.References.ElementAt(0));
            }

            [Theory]
            [InlineData("#r \"hello.dll\"\n#r \"world.dll\"")]
            [InlineData("#r \"hello.dll\"\nConsole.WriteLine();\n#r \"world.dll\"")]
            public void Should_Return_Multiple_Assembly_References_Found_In_Source(string source)
            {
                // Given
                var fixture = new ScriptAnalyzerFixture();
                fixture.GivenScriptExist("/Working/script.cake", source);

                // When
                var result = fixture.Analyze("/Working/script.cake");

                // Then
                Assert.Equal(2, result.Script.References.Count);
                Assert.Equal("hello.dll", result.Script.References.ElementAt(0));
                Assert.Equal("world.dll", result.Script.References.ElementAt(1));
            }

            [Fact]
            public void Should_Process_Using_Directives()
            {
                // Given
                var fixture = new ScriptAnalyzerFixture();
                fixture.GivenScriptExist("/Working/script.cake", "using Cake.Core;");

                // When
                var result = fixture.Analyze("/Working/script.cake");

                // Then
                Assert.Equal(1, result.Script.Namespaces.Count);
                Assert.Equal("Cake.Core", result.Script.Namespaces.ElementAt(0));
            }

            [Fact]
            public void Should_Process_Using_Alias_Directives()
            {
                // Given
                var fixture = new ScriptAnalyzerFixture();
                fixture.GivenScriptExist("/Working/script.cake", "using Core = Cake.Core;");

                // When
                var result = fixture.Analyze("/Working/script.cake");

                // Then
                Assert.Equal(1, result.Script.UsingAliases.Count);
                Assert.Equal("using Core = Cake.Core;", result.Script.UsingAliases.ElementAt(0));
            }

            [Fact]
            public void Should_Keep_Using_Block()
            {
                // Given
                var fixture = new ScriptAnalyzerFixture();
                fixture.GivenScriptExist("/Working/script.cake", "using(new Temp())\n{\n}");

                // When
                var result = fixture.Analyze("/Working/script.cake");

                // Then
                Assert.Equal(0, result.Script.UsingAliases.Count);
                Assert.Equal(0, result.Script.Namespaces.Count);
                Assert.Equal(4, result.Lines.Count);
                Assert.Equal(result.Lines[0], "#line 1 \"/Working/script.cake\"");
                Assert.Equal(result.Lines[1], "using(new Temp())");
                Assert.Equal(result.Lines[2], "{");
                Assert.Equal(result.Lines[3], "}");
            }

            [Fact]
            public void Should_Comment_Out_Shebang()
            {
                // Given
                var fixture = new ScriptAnalyzerFixture();
                fixture.GivenScriptExist("/Working/script.cake", "#!/usr/bin/cake\nConsole.WriteLine();");

                // When
                var result = fixture.Analyze("/Working/script.cake");

                // Then
                Assert.Equal(3, result.Lines.Count);
                Assert.Equal(result.Lines[0], "#line 1 \"/Working/script.cake\"");
                Assert.Equal(result.Lines[1], "// #!/usr/bin/cake");
                Assert.Equal(result.Lines[2], "Console.WriteLine();");
            }

            [Fact]
            public void Should_Process_Addin_Directive_Without_Source()
            {
                // Given
                var fixture = new ScriptAnalyzerFixture();
                fixture.GivenScriptExist("/Working/script.cake", "#addin \"Hello.World\"");

                // When
                var result = fixture.Analyze("/Working/script.cake");

                // Then
                Assert.Equal(1, result.Script.Addins.Count);
                Assert.Equal("nuget:?package=Hello.World", result.Script.Addins.ElementAt(0).OriginalString);
            }

            [Fact]
            public void Should_Process_Addin_Directive_Using_Uri()
            {
                // Given
                var fixture = new ScriptAnalyzerFixture();
                fixture.GivenScriptExist("/Working/script.cake", "#addin \"npm:?package=node\"");

                // When
                var result = fixture.Analyze("/Working/script.cake");

                // Then
                Assert.Equal(1, result.Script.Addins.Count);
                Assert.Equal("npm:?package=node", result.Script.Addins.ElementAt(0).OriginalString);
            }

            [Fact]
            public void Should_Process_Addin_Directive_With_Source()
            {
                // Given
                var fixture = new ScriptAnalyzerFixture();
                fixture.GivenScriptExist("/Working/script.cake", "#addin \"Hello.World\" \"http://source\"");

                // When
                var result = fixture.Analyze("/Working/script.cake");

                // Then
                Assert.Equal(1, result.Script.Addins.Count);
                Assert.Equal("nuget:http://source/?package=Hello.World", result.Script.Addins.ElementAt(0).OriginalString);
            }

            [Fact]
            public void Should_Process_Tool_Directive_Without_Source()
            {
                // Given
                var fixture = new ScriptAnalyzerFixture();
                fixture.GivenScriptExist("/Working/script.cake", "#tool \"Hello.World\"");

                // When
                var result = fixture.Analyze("/Working/script.cake");

                // Then
                Assert.Equal(1, result.Script.Tools.Count);
                Assert.Equal("nuget:?package=Hello.World", result.Script.Tools.ElementAt(0).OriginalString);
            }

            [Fact]
            public void Should_Process_Tool_Directive_With_Source()
            {
                // Given
                var fixture = new ScriptAnalyzerFixture();
                fixture.GivenScriptExist("/Working/script.cake", "#tool \"Hello.World\" \"http://source\"");

                // When
                var result = fixture.Analyze("/Working/script.cake");

                // Then
                Assert.Equal(1, result.Script.Tools.Count);
                Assert.Equal("nuget:http://source/?package=Hello.World", result.Script.Tools.ElementAt(0).OriginalString);
            }

            [Fact]
            public void Should_Process_Tool_Directive_Using_Uri()
            {
                // Given
                var fixture = new ScriptAnalyzerFixture();
                fixture.GivenScriptExist("/Working/script.cake", "#tool \"npm:?package=node\"");

                // When
                var result = fixture.Analyze("/Working/script.cake");

                // Then
                Assert.Equal(1, result.Script.Tools.Count);
                Assert.Equal("npm:?package=node", result.Script.Tools.ElementAt(0).OriginalString);
            }

            [Fact]
            public void Should_Process_Break_Directive()
            {
                // Given
                var fixture = new ScriptAnalyzerFixture();
                fixture.GivenScriptExist("/Working/script.cake", "#break");

                // When
                var result = fixture.Analyze("/Working/script.cake");

                // Then
                Assert.Equal(2, result.Lines.Count);
                Assert.Equal(result.Lines[0], "#line 1 \"/Working/script.cake\"");
                Assert.Equal(result.Lines[1], @"if (System.Diagnostics.Debugger.IsAttached) { System.Diagnostics.Debugger.Break(); }");
            }
        }
    }
}