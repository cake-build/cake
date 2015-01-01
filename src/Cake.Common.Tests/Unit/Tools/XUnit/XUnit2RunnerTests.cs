using Cake.Common.Tests.Fixtures;
using Cake.Common.Tools.XUnit;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.XUnit
{
    public sealed class XUnit2RunnerTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Assembly_Path_Is_Null()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run(null, new XUnit2Settings()));

                // Then
                Assert.IsArgumentNullException(result, "assemblyPath");
            }

            [Fact]
            public void Should_Throw_If_XUnit_Runner_Was_Not_Found()
            {
                // Given
                var fixture = new XUnit2RunnerFixture(defaultToolExist: false);
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run("./Test1.dll", new XUnit2Settings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("xUnit.net (v2): Could not locate executable.", result.Message);
            }

            [Theory]            
            [InlineData("C:/xUnit/xunit.exe", "C:/xUnit/xunit.exe")]
            [InlineData("./tools/xUnit/xunit.exe", "/Working/tools/xUnit/xunit.exe")]
            public void Should_Use_XUnit_Runner_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new XUnit2RunnerFixture(expected);
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test1.dll", new XUnit2Settings
                {
                    ToolPath = toolPath
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == expected),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Find_XUnit_Runner_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test1.dll", new XUnit2Settings());

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/xunit.console.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Use_Provided_Assembly_Paths_In_Process_Arguments()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();                                
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test1.dll", new XUnit2Settings());

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), 
                    Arg.Is<ProcessSettings>(p => p.Arguments.Render() == "\"/Working/Test1.dll\""));
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test1.dll", new XUnit2Settings());

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), 
                    Arg.Is<ProcessSettings>(p => p.WorkingDirectory.FullPath == "/Working"));                
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                fixture.ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns((IProcess)null);
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run("./Test1.dll", new XUnit2Settings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("xUnit.net (v2): Process was not started.", result.Message);     
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                fixture.Process.GetExitCode().Returns(1);
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run("./Test1.dll", new XUnit2Settings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("xUnit.net (v2): Process returned an error.", result.Message);                  
            }

            [Fact]
            public void Should_Throw_If_HtmlReport_Is_Set_But_OutputDirectory_Is_Null()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run("./Test1.dll", new XUnit2Settings
                {
                    HtmlReport = true
                }));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Cannot generate HTML report when no output directory has been set.", result.Message); 
            }

            [Fact]
            public void Should_Generate_Html_Report_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test1.dll", new XUnit2Settings
                {
                    OutputDirectory = "/Output",
                    HtmlReport = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), 
                    Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "\"/Working/Test1.dll\" \"-html\" \"/Output/Test1.dll.html\""));
            }

            [Fact]
            public void Should_Throw_If_XmlReport_Is_Set_But_OutputDirectory_Is_Null()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run("./Test1.dll", new XUnit2Settings
                {
                    XmlReport = true
                }));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Cannot generate XML report when no output directory has been set.", result.Message);
            }

            [Fact]
            public void Should_Generate_Xml_Report_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test1.dll", new XUnit2Settings
                {
                    OutputDirectory = "/Output",
                    XmlReport = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), 
                    Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "\"/Working/Test1.dll\" \"-xml\" \"/Output/Test1.dll.xml\""));
            }


            [Fact]
            public void Should_Throw_If_XmlReportV1_Is_Set_But_OutputDirectory_Is_Null()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run("./Test1.dll", new XUnit2Settings
                {
                    XmlReportV1 = true
                }));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Cannot generate XML report when no output directory has been set.", result.Message);
            }

            [Fact]
            public void Should_Generate_Legacy_Xml_Report_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test1.dll", new XUnit2Settings
                {
                    OutputDirectory = "/Output",
                    XmlReportV1 = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "\"/Working/Test1.dll\" \"-xmlv1\" \"/Output/Test1.dll.xml\""));
            }


            [Fact]
            public void Should_Not_Use_Shadow_Copying_If_Disabled_In_Settings()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test1.dll", new XUnit2Settings
                {
                    ShadowCopy = false
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), 
                    Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "\"/Working/Test1.dll\" \"-noshadow\""));
            }
        }
    }
}
