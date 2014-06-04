using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cake.Core;
using Cake.Core.IO;
using Cake.Scripting;
using Cake.Tests.Fixtures;
using NSubstitute;
using Xunit;
using Xunit.Extensions;

namespace Cake.Tests.Unit
{
    public sealed class CakeApplicationTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Arguments_Were_Null()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                var application = fixture.CreateApplication();

                // When
                var result = Record.Exception(() => application.Run(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("options", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Arguments_Were_Empty()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                var application = fixture.CreateApplication();

                // When
                var result = Record.Exception(() => application.Run(new CakeOptions()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("No script provided.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Script_File_Does_Not_Exist()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                fixture.File.Exists.Returns(false);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Could not find script '/Build/script.csx'.", result.Message);
            }

            [Fact]
            public void Should_Bootstrap_Application_To_Directory_Where_Cake_Resides()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                fixture.Environment.GetApplicationRoot().Returns("/Application");

                // When
                fixture.Run();

                // Then
                fixture.Bootstrapper.Received(1).Bootstrap(
                    Arg.Is<DirectoryPath>(p => p.FullPath == "/Application"));
            }

            [Fact]
            public void Should_Set_Working_Directory_To_The_Scripts_Directory()
            {
                // Given
                var fixture = new CakeApplicationFixture();

                // When
                fixture.Run();

                // Then
                Assert.Equal("/Build", fixture.Environment.WorkingDirectory.FullPath);
            }

            [Fact]
            public void Should_Read_Code_From_Provided_File()
            {
                // Given
                var fixture = new CakeApplicationFixture();

                // When
                fixture.Run();

                // Then
                fixture.ScriptRunner.Received(1).Run(
                    Arg.Any<ScriptHost>(),
                    Arg.Any<IEnumerable<Assembly>>(),
                    Arg.Any<IEnumerable<string>>(), 
                    Arg.Is<string>(x => x == "var lol = 123;"));
            }

            [Fact]
            public void Should_Set_Working_Directory_To_Script_Directory()
            {
                // Given
                var fixture = new CakeApplicationFixture();

                // When
                fixture.Run();

                // Then
                fixture.Environment.Received().WorkingDirectory 
                    = Arg.Is<DirectoryPath>(x => x.FullPath == "/Build");
            }

            [Fact]
            public void Should_Append_Script_Directory_To_Initial_Working_Directory_If_Script_Directory_Is_Relative()
            {
                // Given
                var fixture = new CakeApplicationFixture("Build/script.csx");

                // When
                fixture.Run();

                // Then
                fixture.Environment.Received().WorkingDirectory
                    = Arg.Is<DirectoryPath>(x => x.FullPath == "/Working/Build");
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
            [InlineData("Cake.MSBuild")]
            [InlineData("Cake.XUnit")]
            public void Should_Provide_Script_Runner_With_References(string assemblyName)
            {
                // Given
                var fixture = new CakeApplicationFixture();

                // When
                fixture.Run();

                // Then
                fixture.ScriptRunner.Received(1).Run(
                    Arg.Any<ScriptHost>(),
                    Arg.Is<IEnumerable<Assembly>>(c => c.Any(x => x.FullName.StartsWith(assemblyName + ","))),
                    Arg.Any<IEnumerable<string>>(),
                    Arg.Any<string>());
            }

            //System", "System.Collections.Generic", "System.Linq", "System.Text", "System.Threading.Tasks", "System.IO

            [Theory]
            [InlineData("System")]
            [InlineData("System.Collections.Generic")]
            [InlineData("System.Linq")]
            [InlineData("System.Text")]
            [InlineData("System.Threading.Tasks")]
            [InlineData("System.IO")]
            [InlineData("Cake")]
            [InlineData("Cake.Core")]
            [InlineData("Cake.Core.IO")]
            [InlineData("Cake.Core.Diagnostics")]
            [InlineData("Cake.MSBuild")]
            [InlineData("Cake.XUnit")]
            public void Should_Provide_Script_Runner_With_Namespaces(string @namespace)
            {
                // Given
                var fixture = new CakeApplicationFixture();

                // When
                fixture.Run();

                // Then
                fixture.ScriptRunner.Received(1).Run(
                    Arg.Any<ScriptHost>(),
                    Arg.Any<IEnumerable<Assembly>>(),
                    Arg.Is<IEnumerable<string>>(c => c.Any(x => x == @namespace)),
                    Arg.Any<string>());
            }
        }
    }
}
