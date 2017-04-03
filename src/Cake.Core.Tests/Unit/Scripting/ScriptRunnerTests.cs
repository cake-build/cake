// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using Cake.Core.IO;
using Cake.Core.Scripting;
using Cake.Core.Scripting.Analysis;
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
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                fixture.Environment = null;

                // When
                var result = Record.Exception(() => fixture.CreateScriptRunner());

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_Script_Engine_Is_Null()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                fixture.Engine = null;

                // When
                var result = Record.Exception(() => fixture.CreateScriptRunner());

                // Then
                AssertEx.IsArgumentNullException(result, "engine");
            }

            [Fact]
            public void Should_Throw_If_Script_Alias_Finder_Is_Null()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                fixture.AliasFinder = null;

                // When
                var result = Record.Exception(() => fixture.CreateScriptRunner());

                // Then
                AssertEx.IsArgumentNullException(result, "aliasFinder");
            }

            [Fact]
            public void Should_Throw_If_Script_Analyzer_Is_Null()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                fixture.ScriptAnalyzer = null;

                // When
                var result = Record.Exception(() => fixture.CreateScriptRunner());

                // Then
                AssertEx.IsArgumentNullException(result, "analyzer");
            }

            [Fact]
            public void Should_Throw_If_Script_Conventions_Is_Null()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                fixture.ScriptConventions = null;

                // When
                var result = Record.Exception(() => fixture.CreateScriptRunner());

                // Then
                AssertEx.IsArgumentNullException(result, "conventions");
            }

            [Fact]
            public void Should_Throw_If_AssemblyLoader_Is_Null()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                fixture.AssemblyLoader = null;

                // When
                var result = Record.Exception(() => fixture.CreateScriptRunner());

                // Then
                AssertEx.IsArgumentNullException(result, "assemblyLoader");
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
                AssertEx.IsArgumentNullException(result, "host");
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
                AssertEx.IsArgumentNullException(result, "scriptPath");
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
                AssertEx.IsArgumentNullException(result, "arguments");
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
                Assert.Equal("/build", fixture.Environment.WorkingDirectory.FullPath);
            }

#if !NETCORE
            [Theory]
            [InlineData("mscorlib")]
            [InlineData("System")]
            [InlineData("System.Core")]
            [InlineData("System.Data")]
            [InlineData("System.Xml")]
            [InlineData("System.Xml.Linq")]
            public void Should_Add_References_To_Session(string assemblyName)
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
#endif

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
                fixture.ScriptAnalyzer = Substitute.For<IScriptAnalyzer>();
                fixture.ScriptAnalyzer.Analyze(Arg.Any<FilePath>())
                    .Returns(new ScriptAnalyzerResult(new ScriptInformation(path), new List<string>()));
                var runner = fixture.CreateScriptRunner();

                // When
                runner.Run(fixture.Host, fixture.Script, fixture.ArgumentDictionary);

                // Then
                fixture.ScriptAnalyzer.Received(1).Analyze(
                    Arg.Is<FilePath>(f => f.FullPath == "build.cake"));
            }

            [Fact]
            public void Should_Install_Tools_At_Default_Location_If_No_Configuration_Is_Present()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                var runner = fixture.CreateScriptRunner();

                // When
                runner.Run(fixture.Host, fixture.Script, fixture.ArgumentDictionary);

                // Then
                fixture.ScriptProcessor.Received(1).InstallTools(
                    Arg.Any<ScriptAnalyzerResult>(),
                    Arg.Is<DirectoryPath>(path => path.FullPath == "/Working/tools"));
            }

            [Fact]
            public void Should_Install_Tools_At_Location_In_Configuration()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                fixture.Configuration.GetValue(Constants.Paths.Tools).Returns("./stuff");
                var runner = fixture.CreateScriptRunner();

                // When
                runner.Run(fixture.Host, fixture.Script, fixture.ArgumentDictionary);

                // Then
                fixture.ScriptProcessor.Received(1).InstallTools(
                    Arg.Any<ScriptAnalyzerResult>(),
                    Arg.Is<DirectoryPath>(path => path.FullPath == "/Working/stuff"));
            }

            [Fact]
            public void Should_Install_Addins_At_Default_Location_If_No_Configuration_Is_Present()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                var runner = fixture.CreateScriptRunner();

                // When
                runner.Run(fixture.Host, fixture.Script, fixture.ArgumentDictionary);

                // Then
                fixture.ScriptProcessor.Received(1).InstallAddins(
                    Arg.Any<ScriptAnalyzerResult>(),
                    Arg.Is<DirectoryPath>(path => path.FullPath == "/Working/tools/Addins"));
            }

            [Fact]
            public void Should_Install_Addins_At_Location_In_Configuration()
            {
                // Given
                var fixture = new ScriptRunnerFixture();
                fixture.Configuration.GetValue(Constants.Paths.Addins).Returns("./stuff");
                var runner = fixture.CreateScriptRunner();

                // When
                runner.Run(fixture.Host, fixture.Script, fixture.ArgumentDictionary);

                // Then
                fixture.ScriptProcessor.Received(1).InstallAddins(
                    Arg.Any<ScriptAnalyzerResult>(),
                    Arg.Is<DirectoryPath>(path => path.FullPath == "/Working/stuff"));
            }

            [Fact]
            public void Should_Send_Absolute_Install_Path_To_Script_Processor_When_Installing_Tools()
            {
                // Given
                var fixture = new ScriptRunnerFixture("/Working/foo/build.cake");
                var runner = fixture.CreateScriptRunner();

                // When
                runner.Run(fixture.Host, "./foo/build.cake", fixture.ArgumentDictionary);

                // Then
                fixture.ScriptProcessor.Received(1).InstallTools(
                    Arg.Any<ScriptAnalyzerResult>(),
                    Arg.Is<DirectoryPath>(p => p.FullPath == "/Working/foo/tools"));
            }
        }
    }
}