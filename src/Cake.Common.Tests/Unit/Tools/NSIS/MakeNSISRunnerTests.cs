using System.Collections.Generic;
using System.Linq;
using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.NSIS;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.NSIS
{
    // ReSharper disable once InconsistentNaming
    public sealed class MakeNSISRunnerTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Script_File_Is_Null()
            {
                // Given
                var fixture = new NSISFixture();
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run(null, new MakeNSISSettings()));

                // Then
                Assert.IsArgumentNullException(result, "scriptFile");
            }

            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new NSISFixture();
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run("/Working/File.lol", null));

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_MakeNSIS_Runner_Was_Not_Found()
            {
                // Given
                var fixture = new NSISFixture();
                fixture.Globber.Match("./tools/**/makensis.exe").Returns(Enumerable.Empty<Path>());
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run("/Test.nsi", new MakeNSISSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("MakeNSIS: Could not locate executable.", result.Message);
            }

            [Theory]
            [InlineData("C:/nsis/makensis.exe", "C:/nsis/makensis.exe")]
            [InlineData("./tools/nsis/makensis.exe", "/Working/tools/nsis/makensis.exe")]
            public void Should_Use_MakeNSIS_Runner_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new NSISFixture(expected);
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test.nsi", new MakeNSISSettings
                {
                    ToolPath = toolPath
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == expected),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Find_MakeNSIS_Runner_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new NSISFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test.nsi", new MakeNSISSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/makensis.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new NSISFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test.nsi", new MakeNSISSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.WorkingDirectory.FullPath == "/Working"));
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new NSISFixture();
                fixture.ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns((IProcess)null);
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run("./Test.nsi", new MakeNSISSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("MakeNSIS: Process was not started.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new NSISFixture();
                fixture.Process.GetExitCode().Returns(1);
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run("./Test.nsi", new MakeNSISSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("MakeNSIS: Process returned an error.", result.Message);
            }

            [Fact]
            public void Should_Add_Defines_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new NSISFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test.nsi", new MakeNSISSettings
                {
                    Defines = new Dictionary<string, string>
                    {
                        { "Foo", "Bar" },
                        { "Test", null },
                        { "Test2", string.Empty }
                    }
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "/DFoo=Bar /DTest /DTest2 /Working/Test.nsi"));
            }

            [Fact]
            public void Should_Add_NoChangeDirectory_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new NSISFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test.nsi", new MakeNSISSettings
                {
                    NoChangeDirectory = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "/NOCD /Working/Test.nsi"));
            }

            [Fact]
            public void Should_Add_NoConfig_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new NSISFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test.nsi", new MakeNSISSettings
                {
                    NoConfig = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "/NOCONFIG /Working/Test.nsi"));
            }
        }
    }
}