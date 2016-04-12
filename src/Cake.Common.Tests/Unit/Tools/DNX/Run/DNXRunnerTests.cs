using Cake.Common.Tests.Fixtures.Tools.DNX.Run;
using Cake.Common.Tools.DNX.Run;
using Cake.Core.IO;
using Cake.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DNX.Run
{
    public sealed class DNXRunnerTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DNXRunnerFixture();
                fixture.Settings = null;
                fixture.Command = "test";
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Command_Is_Null()
            {
                // Given
                var fixture = new DNXRunnerFixture();
                fixture.Settings = new DNXRunSettings() { Version = "default" };
                fixture.Command = "";
                fixture.DirectoryPath = "./src/MyProject/";
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsArgumentNullException(result, "command");
            }

            [Fact]
            public void Should_Throw_If_DirectoryPath_Is_Null()
            {
                // Given
                var fixture = new DNXRunnerFixture();
                fixture.Settings = new DNXRunSettings() { Version = "default" };
                fixture.Command = "test";
                fixture.DirectoryPath = null;
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsArgumentNullException(result, "directoryPath");
            }

            [Fact]
            public void Should_Throw_If_DNX_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new DNXRunnerFixture();
                fixture.Command = "test";
                fixture.Settings = new DNXRunSettings() { Version = "default" };
                fixture.DirectoryPath = "./src/MyProject/";
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsCakeException(result, "DNX: Could not locate executable.");
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DNXRunnerFixture();
                fixture.Command = "test";
                fixture.DirectoryPath = "./src/MyProject/";
                fixture.Settings = new DNXRunSettings() { Version = "default" };
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsCakeException(result, "DNX: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new DNXRunnerFixture();
                fixture.Command = "test";
                fixture.DirectoryPath = "./src/MyProject/";
                fixture.Settings = new DNXRunSettings() { Version = "default" };
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsCakeException(result, "DNX: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Use_DirectoryPath_As_WorkingDir_If_Set()
            {
                // Given
                var fixture = new DNXRunnerFixture();
                fixture.DirectoryPath = "./src/MyProject";
                fixture.Command = "test";
                fixture.Settings = new DNXRunSettings() { Version = "default" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("run default test", result.Args);
                Assert.Equal("/Working/src/MyProject", result.Process.WorkingDirectory.FullPath);
            }

            [Fact]
            public void Should_Use_Project_If_Set()
            {
                // Given
                var fixture = new DNXRunnerFixture();
                fixture.DirectoryPath = "./src/MyProject/";
                fixture.Command = "test";
                fixture.Settings = new DNXRunSettings() { Project = new DirectoryPath("./tests/MyTest/"), Version = "default" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("run default --project \"tests/MyTest\" test", result.Args);
            }

            [Fact]
            public void Should_Use_Framework_If_Set()
            {
                // Given
                var fixture = new DNXRunnerFixture();
                fixture.DirectoryPath = "./src/MyProject/";
                fixture.Command = "test";
                fixture.Settings = new DNXRunSettings() { Framework = "dnxcore50", Version = "default" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("run default --framework \"dnxcore50\" test", result.Args);
            }

            [Fact]
            public void Should_Use_Configuration_If_Set()
            {
                // Given
                var fixture = new DNXRunnerFixture();
                fixture.DirectoryPath = "./src/MyProject/";
                fixture.Command = "test";
                fixture.Settings = new DNXRunSettings() { Configuration = "Debug", Version = "default" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("run default --configuration \"Debug\" test", result.Args);
            }

            [Fact]
            public void Should_Use_AppBase_If_Set()
            {
                // Given
                var fixture = new DNXRunnerFixture();
                fixture.DirectoryPath = "./src/MyProject/";
                fixture.Command = "test";
                fixture.Settings = new DNXRunSettings() { AppBase = "./tests/MyTest/", Version = "default" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("run default --appbase \"tests/MyTest\" test", result.Args);
            }

            [Fact]
            public void Should_Use_Lib_If_Set()
            {
                // Given
                var fixture = new DNXRunnerFixture();
                fixture.DirectoryPath = "./src/MyProject/";
                fixture.Command = "test";
                fixture.Settings = new DNXRunSettings() { Version = "default" };
                fixture.Settings.Lib.Add(new DirectoryPath("./tests/MyTest/"));

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("run default --lib \"tests/MyTest\" test", result.Args);
            }
        }
    }
}
