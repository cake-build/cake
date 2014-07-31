using System;
using System.Linq;
using Cake.Core.Scripting.Processing;
using Cake.Core.Tests.Fixtures;
using Xunit;

namespace Cake.Core.Tests.Unit.Scripting.Processing
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
                var result = Record.Exception(() => processor.Process(null, new ScriptProcessorContext()));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("path", ((ArgumentNullException)result).ParamName);
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
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("context", ((ArgumentNullException)result).ParamName);
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
                Assert.Equal(fixture.Source, result.GetScriptCode());                
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
                Assert.Equal("hello.dll", result.References.ElementAt(0));
                Assert.Equal("world.dll", result.References.ElementAt(1));
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
                Assert.Equal("hello.dll", result.References.ElementAt(0));
                Assert.Equal("world.dll", result.References.ElementAt(1));
            }

            [Fact]
            public void Should_Process_Single_Script_Reference_Found_In_Source()
            {
                // Given
                const string source = "#l \"hello.cake\"\r\nConsole.WriteLine();";
                var fixture = new ScriptProcessorFixture(scriptSource: source);
                fixture.FileSystem.GetCreatedFile("/Working/hello.cake");

                // When
                var result = fixture.Process();

                // Then
                Assert.Equal(2, result.ProcessedScripts.Count);
                Assert.Equal("/Working/build.cake", result.ProcessedScripts.ElementAt(0));
                Assert.Equal("/Working/hello.cake", result.ProcessedScripts.ElementAt(1));
            }

            [Fact]
            public void Should_Process_Multiple_Script_References_Found_In_Source()
            {
                // Given
                const string source = "#l \"hello.cake\"\r\n#l \"world.cake\"\r\nConsole.WriteLine();";
                var fixture = new ScriptProcessorFixture(scriptSource: source);
                fixture.FileSystem.GetCreatedFile("/Working/hello.cake");
                fixture.FileSystem.GetCreatedFile("/Working/world.cake");

                // When
                var result = fixture.Process();

                // Then
                Assert.Equal(3, result.ProcessedScripts.Count);
                Assert.Equal("/Working/build.cake", result.ProcessedScripts.ElementAt(0));
                Assert.Equal("/Working/hello.cake", result.ProcessedScripts.ElementAt(1));
                Assert.Equal("/Working/world.cake", result.ProcessedScripts.ElementAt(2));
            }
        }
    }
}
