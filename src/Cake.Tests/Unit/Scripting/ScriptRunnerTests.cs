using System;
using System.Collections.Generic;
using System.Reflection;
using Cake.Core.IO;
using Cake.Tests.Fixtures;
using NSubstitute;
using Xunit;
using Xunit.Extensions;

namespace Cake.Tests.Unit.Scripting
{
    public sealed class ScriptRunnerTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                fixture.FileSystem = null;

                // When
                var result = Record.Exception(() => fixture.CreateScriptRunner());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("fileSystem", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                fixture.Environment = null;

                // When
                var result = Record.Exception(() => fixture.CreateScriptRunner());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("environment", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Arguments_Is_Null()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                fixture.Arguments = null;

                // When
                var result = Record.Exception(() => fixture.CreateScriptRunner());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("arguments", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Session_Factory_Is_Null()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                fixture.SessionFactory = null;

                // When
                var result = Record.Exception(() => fixture.CreateScriptRunner());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("sessionFactory", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Script_Alias_Generator_Is_Null()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                fixture.AliasGenerator = null;

                // When
                var result = Record.Exception(() => fixture.CreateScriptRunner());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("aliasGenerator", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Script_Processor_Is_Null()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                fixture.Processor = null;

                // When
                var result = Record.Exception(() => fixture.CreateScriptRunner());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("processor", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Script_Host_Is_Null()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                fixture.Host = null;

                // When
                var result = Record.Exception(() => fixture.CreateScriptRunner());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("host", ((ArgumentNullException)result).ParamName);
            }
        }

        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Initialize_Script_Session_Factory()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                var runner = fixture.CreateScriptRunner();

                // When
                runner.Run(fixture.Options);

                // Then
                fixture.SessionFactory.Received(1).Initialize();
            }

            [Fact]
            public void Should_Set_Arguments()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                fixture.Options.Arguments.Add("A", "B");
                var runner = fixture.CreateScriptRunner();

                // When
                runner.Run(fixture.Options);

                // Then
                Assert.True(fixture.Arguments.HasArgument("A"));
            }

            [Fact]
            public void Should_Create_Session_Via_Session_Factory()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                var runner = fixture.CreateScriptRunner();

                // When
                runner.Run(fixture.Options);

                // Then
                fixture.SessionFactory.Received(1).CreateSession(fixture.Host);
            }

            [Fact]
            public void Should_Set_Working_Directory_To_Script_Directory()
            {
                // Given
                var fixture = new ScriptRunnerFixture("/build/build.cake");
                var runner = fixture.CreateScriptRunner();

                // When
                runner.Run(fixture.Options);

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
            [InlineData("Cake")]
            [InlineData("Cake.Core")]
            [InlineData("Cake.Common")]
            public void Should_Add_References_To_Session(string @assemblyName)
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                var runner = fixture.CreateScriptRunner();

                // When
                runner.Run(fixture.Options);

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
            [InlineData("Cake")]
            [InlineData("Cake.Scripting")]
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
                runner.Run(fixture.Options);

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
                runner.Run(fixture.Options);

                // Then
                fixture.AliasGenerator.Received(1).Generate(fixture.Session,
                    Arg.Any<IEnumerable<Assembly>>());
            }

            [Fact]
            public void Should_Execute_Script_Code()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                var runner = fixture.CreateScriptRunner();

                // When
                runner.Run(fixture.Options);

                // Then
                fixture.Session.Received(1).Execute(fixture.Source);
            }
        }
    }
}
