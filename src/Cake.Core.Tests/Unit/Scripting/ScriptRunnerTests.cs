using System;
using System.Collections.Generic;
using System.Reflection;
using Cake.Core.IO;
using Cake.Core.Scripting;
using Cake.Core.Tests.Fixtures;
using NSubstitute;
using Xunit;

namespace Cake.Core.Tests.Unit.Scripting
{
    public sealed class ScriptRunnerTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Session_Factory_Is_Null()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                fixture.Engine = null;

                // When
                var result = Record.Exception(() => fixture.CreateScriptRunner());

                // Then
                Assert.IsArgumentNullException(result, "engine");
            }

            [Fact]
            public void Should_Throw_If_Script_Alias_Generator_Is_Null()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                fixture.AliasFinder = null;

                // When
                var result = Record.Exception(() => fixture.CreateScriptRunner());

                // Then
                Assert.IsArgumentNullException(result, "aliasFinder");
            }
        }

        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Script_Host_Is_Null()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                var runner = fixture.CreateScriptRunner();

                // When
                var result = Record.Exception(() => runner.Run(null, fixture.Script, fixture.ArgumentDictionary));

                // Then
                Assert.IsArgumentNullException(result, "host");
            }

            [Fact]
            public void Should_Throw_If_Script_Is_Null()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                var runner = fixture.CreateScriptRunner();

                // When
                var result = Record.Exception(() => runner.Run(fixture.Host, null, fixture.ArgumentDictionary));

                // Then
                Assert.IsArgumentNullException(result, "scriptPath");
            }

            [Fact]
            public void Should_Throw_If_Arguments_Are_Null()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                var runner = fixture.CreateScriptRunner();

                // When
                var result = Record.Exception(() => runner.Run(fixture.Host, fixture.Script, null));

                // Then
                Assert.IsArgumentNullException(result, "arguments");
            }

            [Fact]
            public void Should_Set_Arguments()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                fixture.ArgumentDictionary.Add("A", "B");
                var runner = fixture.CreateScriptRunner();

                // When
                runner.Run(fixture.Host, fixture.Script, fixture.ArgumentDictionary);

                // Then
                fixture.Arguments.Received(1).SetArguments(fixture.ArgumentDictionary);
            }

            [Fact]
            public void Should_Create_Session_Via_Session_Factory()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                var runner = fixture.CreateScriptRunner();

                // When
                runner.Run(fixture.Host, fixture.Script, fixture.ArgumentDictionary);

                // Then
                fixture.Engine.Received(1)
                    .CreateSession(fixture.Host, fixture.ArgumentDictionary);
            }

            [Fact]
            public void Should_Set_Working_Directory_To_Script_Directory()
            {
                // Given
                var fixture = new ScriptRunnerFixture("/build/build.cake");
                var runner = fixture.CreateScriptRunner();

                // When
                runner.Run(fixture.Host, fixture.Script, fixture.ArgumentDictionary);

                // Then
                fixture.Environment.Received(1).WorkingDirectory
                    = Arg.Is<DirectoryPath>(p => p.FullPath == "/build");
            }

            [Theory]
            [InlineData("mscorlib")]
            [InlineData("System")]
            [InlineData("System.Core")]
            [InlineData("System.Data")]
            [InlineData("System.Xml")]
            [InlineData("System.Xml.Linq")]
            [InlineData("Cake.Core")]
            public void Should_Add_References_To_Session(string @assemblyName)
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                var runner = fixture.CreateScriptRunner();

                // When
                runner.Run(fixture.Host, fixture.Script, fixture.ArgumentDictionary);

                // Then
                fixture.Session.Received(1).AddReference(
                    Arg.Is<Assembly>(a => a.FullName.StartsWith(assemblyName + ", ", StringComparison.OrdinalIgnoreCase)));
            }

            [Theory]
            [InlineData("System")]
            [InlineData("System.Collections.Generic")]
            [InlineData("System.Linq")]
            [InlineData("System.Text")]
            [InlineData("System.Threading.Tasks")]
            [InlineData("System.IO")]
            [InlineData("Cake.Core")]
            [InlineData("Cake.Core.IO")]            
            [InlineData("Cake.Core.Scripting")]
            [InlineData("Cake.Core.Diagnostics")]
            public void Should_Add_Namespaces_To_Session(string @namespace)
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                var runner = fixture.CreateScriptRunner();

                // When
                runner.Run(fixture.Host, fixture.Script, fixture.ArgumentDictionary);

                // Then
                fixture.Session.Received(1).ImportNamespace(@namespace);
            }

            [Fact]
            public void Should_Generate_Script_Aliases()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                var runner = fixture.CreateScriptRunner();

                // When
                runner.Run(fixture.Host, fixture.Script, fixture.ArgumentDictionary);

                // Then
                fixture.AliasFinder.Received(1).FindAliases(
                    Arg.Any<IEnumerable<Assembly>>());
            }

            [Fact]
            public void Should_Execute_Script_Code()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                var runner = fixture.CreateScriptRunner();

                // When
                runner.Run(fixture.Host, fixture.Script, fixture.ArgumentDictionary);

                // Then
                fixture.Session.Received(1).Execute(Arg.Any<Script>());
            }

            [Theory]
            [InlineData("test/build.cake")]
            [InlineData("./test/build.cake")]
            [InlineData("/test/build.cake")]
            public void Should_Remove_Directory_From_Script_Path(string path)
            {
                // Given
                var fixture = new ScriptRunnerFixture(path);
                fixture.ScriptProcessor = Substitute.For<IScriptProcessor>();
                var runner = fixture.CreateScriptRunner();

                // When
                runner.Run(fixture.Host, fixture.Script, fixture.ArgumentDictionary);

                // Then
                fixture.ScriptProcessor.Received(1).Process(
                    Arg.Is<FilePath>(p => p.FullPath == "build.cake"), 
                    Arg.Any<ScriptProcessorContext>());
            }
        }
    }
}
