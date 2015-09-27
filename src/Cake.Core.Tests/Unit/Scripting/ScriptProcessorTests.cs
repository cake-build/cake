using System.Linq;
using Cake.Core.Scripting;
using Cake.Core.Tests.Fixtures;
using Cake.Testing;
using Xunit;

namespace Cake.Core.Tests.Unit.Scripting
{
    public sealed class ScriptProcessorTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Given
                var fixture = new ScriptProcessorFixture();
                fixture.FileSystem = null;

                // When
                var result = Record.Exception(() => fixture.CreateProcessor());

                // Then
                Assert.IsArgumentNullException(result, "fileSystem");
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fixture = new ScriptProcessorFixture();
                fixture.Environment = null;

                // When
                var result = Record.Exception(() => fixture.CreateProcessor());

                // Then
                Assert.IsArgumentNullException(result, "environment");
            }
        }

        public sealed class TheProcessMethod
        {
            [Fact]
            public void Should_Throw_If_File_Path_Is_Null()
            {
                // Given
                var fixture = new ScriptProcessorFixture();
                var processor = fixture.CreateProcessor();

                // When
                var result = Record.Exception(() => processor.Process(null, new ScriptProcessorContext()));

                // Then
                Assert.IsArgumentNullException(result, "path");
            }

            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given
                var fixture = new ScriptProcessorFixture();
                var processor = fixture.CreateProcessor();

                // When
                var result = Record.Exception(() => processor.Process("./build.cake", null));

                // Then
                Assert.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Script_Was_Not_Found()
            {
                // Given
                var fixture = new ScriptProcessorFixture("./build.cake", scriptExist: false);

                // When
                var result = Record.Exception(() => fixture.Process());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Could not find script '/Working/build.cake'.", result.Message);
            }

            [Fact]
            public void Should_Return_Code_Read_From_File()
            {
                // Given
                var fixture = new ScriptProcessorFixture();

                // When
                var result = fixture.Process();

                // Then
                Assert.Equal(fixture.GetExpectedSource(), fixture.GetActualSource(result));
            }

            [Fact]
            public void Should_Return_Single_Assembly_Reference_Found_In_Source()
            {
                // Given
                const string source = "#r \"hello.dll\"\r\nConsole.WriteLine();";
                var fixture = new ScriptProcessorFixture(scriptSource: source);

                // When
                var result = fixture.Process();

                // Then
                Assert.Equal(1, result.References.Count);
                Assert.Equal("hello.dll", result.References.ElementAt(0));
            }

            [Theory]
            [InlineData("#r \"hello.dll\"\r\n#r \"world.dll\"\r\nConsole.WriteLine();")]
            [InlineData("#reference \"hello.dll\"\r\n#reference \"world.dll\"\r\nConsole.WriteLine();")]
            [InlineData("#reference \"hello.dll\"\r\n#r \"world.dll\"\r\nConsole.WriteLine();")]
            public void Should_Return_Multiple_Assembly_References_Found_In_Source(string source)
            {
                // Given
                var fixture = new ScriptProcessorFixture(scriptSource: source);

                // When
                var result = fixture.Process();

                // Then
                Assert.Equal(2, result.References.Count);
                Assert.Equal("hello.dll", result.References.ElementAt(0));
                Assert.Equal("world.dll", result.References.ElementAt(1));
            }

            [Theory]
            [InlineData("#r \"hello.dll\"\r\nConsole.WriteLine();\r\n#r \"world.dll\"")]
            [InlineData("#reference \"hello.dll\"\r\nConsole.WriteLine();\r\n#reference \"world.dll\"")]
            [InlineData("#reference \"hello.dll\"\r\nConsole.WriteLine();\r\n#r \"world.dll\"")]
            public void Should_Return_Multiple_Assembly_References_Found_In_Source_Regardless_Of_Location(string source)
            {
                // Given
                var fixture = new ScriptProcessorFixture(scriptSource: source);

                // When
                var result = fixture.Process();

                // Then
                Assert.Equal(2, result.References.Count);
                Assert.Equal("hello.dll", result.References.ElementAt(0));
                Assert.Equal("world.dll", result.References.ElementAt(1));
            }

            [Theory]
            [InlineData("#l \"hello.cake\"\r\nConsole.WriteLine();")]
            [InlineData("#load \"hello.cake\"\r\nConsole.WriteLine();")]
            public void Should_Process_Single_Script_Reference_Found_In_Source(string source)
            {
                // Given
                var fixture = new ScriptProcessorFixture(scriptSource: source);
                fixture.FileSystem.CreateFile("/Working/hello.cake");

                // When
                var result = fixture.Process();

                // Then
                Assert.Equal(2, result.ProcessedScripts.Count);
                Assert.Equal("/Working/build.cake", result.ProcessedScripts.ElementAt(0));
                Assert.Equal("/Working/hello.cake", result.ProcessedScripts.ElementAt(1));
            }

            [Theory]
            [InlineData("#l \"hello.cake\"\r\n#l \"world.cake\"\r\nConsole.WriteLine();")]
            [InlineData("#load \"hello.cake\"\r\n#load \"world.cake\"\r\nConsole.WriteLine();")]
            [InlineData("#load \"hello.cake\"\r\n#l \"world.cake\"\r\nConsole.WriteLine();")]
            public void Should_Process_Multiple_Script_References_Found_In_Source(string source)
            {
                // Given
                var fixture = new ScriptProcessorFixture(scriptSource: source);
                fixture.FileSystem.CreateFile("/Working/hello.cake");
                fixture.FileSystem.CreateFile("/Working/world.cake");

                // When
                var result = fixture.Process();

                // Then
                Assert.Equal(3, result.ProcessedScripts.Count);
                Assert.Equal("/Working/build.cake", result.ProcessedScripts.ElementAt(0));
                Assert.Equal("/Working/hello.cake", result.ProcessedScripts.ElementAt(1));
                Assert.Equal("/Working/world.cake", result.ProcessedScripts.ElementAt(2));
            }

            [Fact]
            public void Should_Process_Using_Directives()
            {
                // Given
                var source = "using System.IO;\r\nConsole.WriteLine();";
                var fixture = new ScriptProcessorFixture(scriptSource: source);

                // When
                var result = fixture.Process();

                // Then
                Assert.Contains("System.IO", result.Namespaces);
            }

            [Fact]
            public void Should_Process_Using_Alias_Directives()
            {
                // Given
                var fixture = new ScriptProcessorFixture(scriptSource: "using ClassAlias = N1.N2.Class;\r\nConsole.WriteLine();");

                // When
                var result = fixture.Process();

                // Then
                Assert.Equal(2, result.Lines.Count);
                Assert.Equal("#line 1 \"/Working/build.cake\"", result.Lines.ElementAt(0));
                Assert.Equal("Console.WriteLine();", result.Lines.ElementAt(1));
            }

            [Fact]
            public void Should_Keep_Using_Block()
            {
                // Given
                var fixture = new ScriptProcessorFixture(scriptSource: "using (ClassAlias)\r\n{\r\n}\r\nConsole.WriteLine();");

                // When
                var result = fixture.Process();

                // Then
                Assert.Equal(5, result.Lines.Count);
                Assert.Equal("#line 1 \"/Working/build.cake\"", result.Lines.ElementAt(0));
                Assert.Equal("using (ClassAlias)", result.Lines.ElementAt(1));
                Assert.Equal("{", result.Lines.ElementAt(2));
                Assert.Equal("}", result.Lines.ElementAt(3));
                Assert.Equal("Console.WriteLine();", result.Lines.ElementAt(4));
            }

            [Fact]
            public void Should_Remove_Shebang()
            {
                // Given
                var fixture = new ScriptProcessorFixture(scriptSource: "#!usr/bin/cake\r\nConsole.WriteLine();");

                // When
                var result = fixture.Process();

                // Then
                Assert.Equal(2, result.Lines.Count);
                Assert.Equal("#line 1 \"/Working/build.cake\"", result.Lines.ElementAt(0));
                Assert.Equal("Console.WriteLine();", result.Lines.ElementAt(1));
            }
        }
    }
}
