using System;
using Cake.Core.Tests.Fixtures;
using Xunit;
using Xunit.Extensions;

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
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("fileSystem", ((ArgumentNullException)result).ParamName);
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
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("environment", ((ArgumentNullException)result).ParamName);           
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
                var result = Record.Exception(() => processor.Process(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("path", ((ArgumentNullException)result).ParamName);
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
                Assert.Equal(fixture.Source, result.Code);                
            }

            [Fact]
            public void Should_Return_Script_Path_Directory_As_Root_Path()
            {
                // Given
                var fixture = new ScriptProcessorFixture(
                    scriptPath: "/a/b/c/build.cake");
          
                // When                
                var result = fixture.Process();

                // Then
                Assert.Equal("/a/b/c", result.Root.FullPath);
            }

            [Theory]
            [InlineData("#r")]
            [InlineData("#r ")]
            [InlineData("#r \"")]
            [InlineData("#r \"test.dll")]
            [InlineData("#r test.dll\"")]
            public void Should_Throw_If_Reference_Directive_Is_Malformed(string source)
            {
                // Given
                var expected = string.Format("Reference directive is malformed: {0}", source);
                var fixture = new ScriptProcessorFixture(scriptSource: source);

                // When
                var result = Record.Exception(() => fixture.Process());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal(expected, result.Message);
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
                Assert.Equal("hello.dll", result.References[0].FullPath);
            }

            [Fact]
            public void Should_Return_Multiple_Assembly_References_Found_In_Source()
            {
                // Given
                const string source = "#r \"hello.dll\"\r\n#r \"world.dll\"\r\nConsole.WriteLine();";
                var fixture = new ScriptProcessorFixture(scriptSource: source);

                // When
                var result = fixture.Process();

                // Then
                Assert.Equal(2, result.References.Count);
                Assert.Equal("hello.dll", result.References[0].FullPath);
                Assert.Equal("world.dll", result.References[1].FullPath);
            }

            [Fact]
            public void Should_Return_Multiple_Assembly_References_Found_In_Source_Regardless_Of_Location()
            {
                // Given
                const string source = "#r \"hello.dll\"\r\nConsole.WriteLine();\r\n#r \"world.dll\"";
                var fixture = new ScriptProcessorFixture(scriptSource: source);

                // When
                var result = fixture.Process();

                // Then
                Assert.Equal(2, result.References.Count);
                Assert.Equal("hello.dll", result.References[0].FullPath);
                Assert.Equal("world.dll", result.References[1].FullPath);
            }
        }
    }
}
