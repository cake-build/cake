using Cake.Common.Tests.Fixtures;
using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.SignTool;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.SignTool
{
    public sealed class SignToolSignRunnerTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.FileSystem = null;

                // When
                var result = Record.Exception(() => fixture.CreateRunner());

                // Then
                Assert.IsArgumentNullException(result, "fileSystem");
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.Environment = null;

                // When
                var result = Record.Exception(() => fixture.CreateRunner());

                // Then
                Assert.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_Process_Runner_Is_Null()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.ProcessRunner = null;

                // When
                var result = Record.Exception(() => fixture.CreateRunner());

                // Then
                Assert.IsArgumentNullException(result, "processRunner");
            }
        }

        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Assembly_Path_Is_Null()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run(null, new SignToolSignSettings()));

                // Then
                Assert.IsArgumentNullException(result, "assemblyPath");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run("./a.dll", null));

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Assembly_Do_Not_Exist()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.AssemblyFile.Exists.Returns(false);

                // When
                var result = Record.Exception(() => fixture.RunTool());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("SignTool SIGN: The assembly '/Working/a.dll' do not exist.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_No_Timestamp_Server_URL_Has_Been_Specified()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.Settings.TimeStampUri = null;

                // When
                var result = Record.Exception(() => fixture.RunTool());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("SignTool SIGN: Timestamp server URL is required but not specified.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Certificate_Path_Is_Null()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.Settings.CertPath = null;

                // When
                var result = Record.Exception(() => fixture.RunTool());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("SignTool SIGN: Certificate path is required but not specified.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Certificate_File_Do_Not_Exist()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.CertificateFile.Exists.Returns(false);

                // When
                var result = Record.Exception(() => fixture.RunTool());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("SignTool SIGN: The certificate '/Working/cert.pfx' do not exist.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Password_Is_Null()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.Settings.Password = null;

                // When
                var result = Record.Exception(() => fixture.RunTool());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("SignTool SIGN: Password is required but not specified.", result.Message);
            }

            [Fact]
            public void Should_Use_Default_Tool_Path_If_None_Is_Specified()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();

                // When
                fixture.RunTool();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/Default/tool.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Use_Provided_Tool_Path_If_Specified()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
           
                var tool = Substitute.For<IFile>();
                tool.Exists.Returns(true);

                fixture.FileSystem.GetFile(Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/SignTool.exe")).Returns(tool);
                fixture.Settings.ToolPath = "./tools/SignTool.exe";

                // When
                fixture.RunTool();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/SignTool.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Call_Sign_Tool_With_Correct_Parameters()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();

                // When
                fixture.RunTool();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "SIGN /t \"https://t.com/\" /f \"/Working/cert.pfx\" /p secret \"/Working/a.dll\""));                
            }
        }
    }
}
