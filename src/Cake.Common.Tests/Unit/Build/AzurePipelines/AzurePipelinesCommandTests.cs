using System;
using System.Linq;
using Cake.Common.Build.AzurePipelines;
using Cake.Common.Build.AzurePipelines.Data;
using Cake.Common.Tests.Fakes;
using Cake.Common.Tests.Fixtures.Build;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Xunit;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.AzurePipelines
{
    public sealed class AzurePipelinesCommandTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given, When
                var writer = new FakeBuildSystemServiceMessageWriter();
                var result = Record.Exception(() => new AzurePipelinesCommands(null, writer));

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_Writer_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new AzurePipelinesCommands(new FakeEnvironment(PlatformFamily.Unknown), null));

                // Then
                AssertEx.IsArgumentNullException(result, "writer");
            }
        }

        public sealed class TheCommands
        {
            [Fact]
            public void Should_Not_Be_Null()
            {
                // Given
                var fixture = new AzurePipelinesFixture();

                // When
                var service = fixture.CreateAzurePipelinesService();

                // Then
                Assert.NotNull(service.Commands);
            }

            [Theory]
            [InlineData("warning")]
            [InlineData("message")]
            public void Should_Log_Warning_Message(string msg)
            {
                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();

                // When
                service.Commands.WriteWarning(msg);

                // Then
                Assert.Contains(fixture.Writer.Entries, m => m == $"##vso[task.logissue type=warning;]{msg}");
            }

            [Fact]
            public void Should_Log_Warning_Message_With_Data()
            {
                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();

                // When
                service.Commands.WriteWarning("build warning", new AzurePipelinesMessageData
                {
                    SourcePath = "./code file.cs",
                    LineNumber = 5,
                    ColumnNumber = 12,
                    ErrorCode = 9
                });

                // Then
                Assert.Contains(fixture.Writer.Entries, m => m == $"##vso[task.logissue sourcepath=./code file.cs;linenumber=5;columnnumber=12;code=9;type=warning;]build warning");
            }

            [Theory]
            [InlineData("error")]
            [InlineData("message")]
            public void Should_Log_Error_Message(string msg)
            {
                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();

                // When
                service.Commands.WriteError(msg);

                // Then
                Assert.Contains(fixture.Writer.Entries, m => m == $"##vso[task.logissue type=error;]{msg}");
            }

            [Fact]
            public void Should_Log_Error_Message_With_Data()
            {
                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();

                // When
                service.Commands.WriteError("build error", new AzurePipelinesMessageData
                {
                    SourcePath = "./code.cs",
                    LineNumber = 1,
                    ColumnNumber = 2,
                    ErrorCode = 3
                });

                // Then
                Assert.Contains(fixture.Writer.Entries, m => m == $"##vso[task.logissue sourcepath=./code.cs;linenumber=1;columnnumber=2;code=3;type=error;]build error");
            }

            [Fact]
            public void Should_Set_Current_Progress()
            {
                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();

                // When
                service.Commands.SetProgress(75, "Testing Provider");

                // Then
                Assert.Contains(fixture.Writer.Entries, m => m == $"##vso[task.setprogress value=75;]Testing Provider");
            }

            [Fact]
            public void Should_Complete_Current_Task()
            {
                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();

                // When
                service.Commands.CompleteCurrentTask();

                // Then
                Assert.Contains(fixture.Writer.Entries, m => m == $"##vso[task.complete ]DONE");
            }

            [Fact]
            public void Should_Complete_Current_Task_With_Status()
            {
                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();

                // When
                service.Commands.CompleteCurrentTask(AzurePipelinesTaskResult.Failed);

                // Then
                Assert.Contains(fixture.Writer.Entries, m => m == $"##vso[task.complete result=Failed;]DONE");
            }

            [Fact]
            public void Should_Create_New_Record()
            {
                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();

                // When
                var guid = service.Commands.CreateNewRecord("New record", "build", 1);

                // Then
                Assert.NotNull(guid);
                Assert.Contains(fixture.Writer.Entries, m => m == $"##vso[task.logdetail id={guid.ToString()};name=New record;type=build;order=1;]create new timeline record");
            }

            [Fact]
            public void Should_Create_New_Record_With_Data()
            {
                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();
                var date = DateTime.UtcNow;

                // When
                var guid = service.Commands.CreateNewRecord("New record", "build", 2, new AzurePipelinesRecordData
                {
                    StartTime = date,
                    Progress = 75,
                    Status = AzurePipelinesTaskStatus.Initialized
                });

                // Then
                Assert.NotNull(guid);
                Assert.Contains(fixture.Writer.Entries, m => m == $"##vso[task.logdetail starttime={date.ToString()};progress=75;state=Initialized;id={guid.ToString()};name=New record;type=build;order=2;]create new timeline record");
            }

            [Fact]
            public void Should_Update_Existing_Record()
            {
                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();
                var guid = Guid.NewGuid();
                var parent = Guid.NewGuid();

                // When
                service.Commands.UpdateRecord(guid, new AzurePipelinesRecordData
                {
                    Progress = 95,
                    Status = AzurePipelinesTaskStatus.InProgress,
                    ParentRecord = parent
                });

                // Then
                Assert.Contains(fixture.Writer.Entries, m => m == $"##vso[task.logdetail parentid={parent.ToString()};progress=95;state=InProgress;id={guid.ToString()};]update");
            }

            [Fact]
            public void Should_Set_Variable()
            {
                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();

                // When
                service.Commands.SetVariable("varname", "VarValue");

                // Then
                Assert.Contains(fixture.Writer.Entries,
                    m => m == $"##vso[task.setvariable variable=varname;]VarValue");
            }

            [Fact]
            public void Should_Set_Secret_Variable()
            {
                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();

                // When
                service.Commands.SetSecretVariable("Secret Variable", "Secret Value");

                // Then
                Assert.Contains(fixture.Writer.Entries, m => m == "##vso[task.setvariable variable=Secret Variable;issecret=true;]Secret Value");
            }

            [Fact]
            public void Should_Set_Output_Variable()
            {
                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();

                // When
                service.Commands.SetOutputVariable("Output Variable", "Output Value");

                // Then
                Assert.Contains(fixture.Writer.Entries, m => m == "##vso[task.setvariable variable=Output Variable;isOutput=true;]Output Value");
            }

            [Fact]
            public void Should_Upload_Task_Summary()
            {
                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();
                var path = FilePath.FromString("./summary.md").MakeAbsolute(fixture.Environment);

                // When
                service.Commands.UploadTaskSummary("./summary.md");

                // Then
                Assert.Contains(fixture.Writer.Entries, m => m == $"##vso[task.uploadsummary ]{path}");
            }

            [Fact]
            public void Should_Upload_Task_Log()
            {
                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();
                var path = FilePath.FromString("./logs/task.log").MakeAbsolute(fixture.Environment);

                // When
                service.Commands.UploadTaskLogFile("./logs/task.log");

                // Then
                Assert.Contains(fixture.Writer.Entries, m => m == $"##vso[task.uploadfile ]{path}");
            }

            [Theory]
            [InlineData("drop", AzurePipelinesArtifactType.Container, "./drop")]
            [InlineData("artifact", AzurePipelinesArtifactType.FilePath, "./dist/build/artifact.file")]
            [InlineData("ref", AzurePipelinesArtifactType.GitRef, "895a00ec66af875c1593a7563beb0edee400aba0")]
            public void Should_Link_Build_Artifacts(string name, AzurePipelinesArtifactType type, string location)
            {
                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();

                // When
                service.Commands.LinkArtifact(name, type, location);

                // Then
                Assert.Contains(fixture.Writer.Entries,
                    m => m == $"##vso[artifact.associate artifactname={name};type={type};]{location}");
            }

            [Fact]
            public void Should_Upload_To_Container()
            {
                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();
                var path = FilePath.FromString("./dist/package.nupkg").MakeAbsolute(fixture.Environment).FullPath;

                // When
                service.Commands.UploadArtifact("packages", "./dist/package.nupkg");

                // Then
                Assert.Contains(fixture.Writer.Entries,
                    m => m == $"##vso[artifact.upload containerfolder=packages;]{path}");
            }

            [Fact]
            public void Should_Upload_To_Container_Artifact()
            {
                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();
                var path = FilePath.FromString("./artifacts/results.trx").MakeAbsolute(fixture.Environment).FullPath;

                // When
                service.Commands.UploadArtifact("tests", "./artifacts/results.trx", "Test Results");

                // Then
                Assert.Contains(fixture.Writer.Entries, m => m == $"##vso[artifact.upload containerfolder=tests;artifactname=Test Results;]{path}");
            }

            [Fact]
            public void UploadArtifactDirectory_Should_Throw_If_Directory_Is_Null()
            {
                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();

                // When
                var result = Record.Exception(() => service.Commands.UploadArtifactDirectory(null));

                // Then
                AssertEx.IsArgumentNullException(result, "directory");
            }

            [Fact]
            public void Should_Upload_Directory_As_Container()
            {
                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();
                var path = DirectoryPath.FromString("./artifacts/Packages").MakeAbsolute(fixture.Environment).FullPath;

                // When
                service.Commands.UploadArtifactDirectory("./artifacts/Packages");

                // Then
                Assert.Contains(fixture.Writer.Entries, m => m == $"##vso[artifact.upload containerfolder=Packages;artifactname=Packages;]{path}");
            }

            [Fact]
            public void UploadArtifactDirectory_With_ArtifactName_Should_Throw_If_Directory_Is_Null()
            {
                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();

                // When
                var result = Record.Exception(() => service.Commands.UploadArtifactDirectory(null, "Packages"));

                // Then
                AssertEx.IsArgumentNullException(result, "directory");
            }

            [Fact]
            public void UploadArtifactDirectory_Should_Throw_If_ArtifactName_Is_Null()
            {
                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();

                // When
                var result = Record.Exception(() => service.Commands.UploadArtifactDirectory("./artifacts/Packages", null));

                // Then
                AssertEx.IsArgumentNullException(result, "artifactName");
            }

            [Fact]
            public void Should_Upload_Directory_As_Container_Artifact()
            {
                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();
                var path = DirectoryPath.FromString("./artifacts/Packages").MakeAbsolute(fixture.Environment).FullPath;

                // When
                service.Commands.UploadArtifactDirectory("./artifacts/Packages", "NuGet");

                // Then
                Assert.Contains(fixture.Writer.Entries, m => m == $"##vso[artifact.upload containerfolder=NuGet;artifactname=NuGet;]{path}");
            }

            [Fact]
            public void Should_Upload_Build_Log()
            {
                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();
                var path = FilePath.FromString("./dist/buildlog.txt").MakeAbsolute(fixture.Environment).FullPath;

                // When
                service.Commands.UploadBuildLogFile("./dist/buildlog.txt");

                // Then
                Assert.Contains(fixture.Writer.Entries, m => m == $"##vso[build.uploadlog ]{path}");
            }

            [Fact]
            public void Should_Update_Build_Number()
            {
                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();

                // When
                service.Commands.UpdateBuildNumber("CIBuild_1");

                // Then
                Assert.Contains(fixture.Writer.Entries, m => m == "##vso[build.updatebuildnumber ]CIBuild_1");
            }

            [Fact]
            public void Should_Add_Build_Tag()
            {
                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();

                // When
                service.Commands.AddBuildTag("Stable");

                // Then
                Assert.Contains(fixture.Writer.Entries, m => m == "##vso[build.addbuildtag ]Stable");
            }

            [Fact]
            public void Should_Publish_Test_Results()
            {
                const string expected = @"##vso[results.publish type=XUnit;mergeResults=true;platform=x86;config=Debug;runTitle='Cake Test Run 1 [master]';publishRunAttachments=true;resultFiles=C:\build\CAKE-CAKE-JOB1\artifacts\resultsXUnit.trx,C:\build\CAKE-CAKE-JOB1\artifacts\resultsJs.trx;]";

                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();
                var data = new AzurePipelinesPublishTestResultsData
                {
                    Configuration = "Debug",
                    MergeTestResults = true,
                    Platform = "x86",
                    PublishRunAttachments = true,
                    TestRunner = AzurePipelinesTestRunnerType.XUnit,
                    TestRunTitle = "Cake Test Run 1 [master]",
                    TestResultsFiles = new FilePath[]
                     {
                         "./artifacts/resultsXUnit.trx",
                         "./artifacts/resultsJs.trx"
                     }
                };

                // When
                service.Commands.PublishTestResults(data);

                // Then
                Assert.Equal(expected.Replace('\\', System.IO.Path.DirectorySeparatorChar), fixture.Writer.Entries.FirstOrDefault());
            }

            [Fact]
            public void Should_Publish_Test_Results_If_File_Path_Is_Relative()
            {
                const string expected = @"##vso[results.publish type=XUnit;mergeResults=true;platform=x86;config=Debug;runTitle='Cake Test Run 1 [master]';publishRunAttachments=true;resultFiles=C:\build\CAKE-CAKE-JOB1\artifacts\resultsXUnit.trx,C:\build\CAKE-CAKE-JOB1\artifacts\resultsJs.trx;]";

                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();
                var data = new AzurePipelinesPublishTestResultsData
                {
                    Configuration = "Debug",
                    MergeTestResults = true,
                    Platform = "x86",
                    PublishRunAttachments = true,
                    TestRunner = AzurePipelinesTestRunnerType.XUnit,
                    TestRunTitle = "Cake Test Run 1 [master]",
                    TestResultsFiles = new FilePath[]
                    {
                        "./artifacts/resultsXUnit.trx",
                        "./artifacts/resultsJs.trx"
                    }
                };

                // When
                service.Commands.PublishTestResults(data);

                // Then
                Assert.Equal(expected.Replace('\\', System.IO.Path.DirectorySeparatorChar), fixture.Writer.Entries.FirstOrDefault());
            }

            [Fact]
            public void Should_Publish_Test_Results_If_File_Path_Is_Absolute()
            {
                const string expected = @"##vso[results.publish type=XUnit;mergeResults=true;platform=x86;config=Debug;runTitle='Cake Test Run 1 [master]';publishRunAttachments=true;resultFiles=/build/CAKE-CAKE-JOB1/artifacts/resultsXUnit.trx,/build/CAKE-CAKE-JOB1/artifacts/resultsJs.trx;]";

                // Given
                var fixture = new AzurePipelinesFixture();
                fixture.Environment.WorkingDirectory.Returns("/build/CAKE-CAKE-JOB1");
                fixture.Environment.Platform.Family.Returns(PlatformFamily.OSX);
                var service = fixture.CreateAzurePipelinesService();
                var data = new AzurePipelinesPublishTestResultsData
                {
                    Configuration = "Debug",
                    MergeTestResults = true,
                    Platform = "x86",
                    PublishRunAttachments = true,
                    TestRunner = AzurePipelinesTestRunnerType.XUnit,
                    TestRunTitle = "Cake Test Run 1 [master]",
                    TestResultsFiles = new FilePath[]
                    {
                        "/build/CAKE-CAKE-JOB1/artifacts/resultsXUnit.trx",
                        "/build/CAKE-CAKE-JOB1/artifacts/resultsJs.trx"
                    }
                };

                // When
                service.Commands.PublishTestResults(data);

                // Then
                Assert.Equal(expected.Replace('/', System.IO.Path.DirectorySeparatorChar), fixture.Writer.Entries.FirstOrDefault());
            }

            [WindowsFact]
            public void Should_Publish_Test_Results_If_File_Path_Is_Absolute_Windows()
            {
                const string expected = @"##vso[results.publish type=XUnit;mergeResults=true;platform=x86;config=Debug;runTitle='Cake Test Run 1 [master]';publishRunAttachments=true;resultFiles=C:\build\CAKE-CAKE-JOB1\artifacts\resultsXUnit.trx,C:\build\CAKE-CAKE-JOB1\artifacts\resultsJs.trx;]";

                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();
                var data = new AzurePipelinesPublishTestResultsData
                {
                    Configuration = "Debug",
                    MergeTestResults = true,
                    Platform = "x86",
                    PublishRunAttachments = true,
                    TestRunner = AzurePipelinesTestRunnerType.XUnit,
                    TestRunTitle = "Cake Test Run 1 [master]",
                    TestResultsFiles = new FilePath[]
                    {
                        "C:\\build\\CAKE-CAKE-JOB1\\artifacts\\resultsXUnit.trx",
                        "C:\\build\\CAKE-CAKE-JOB1\\artifacts\\resultsJs.trx"
                    }
                };

                // When
                service.Commands.PublishTestResults(data);

                // Then
                Assert.Equal(expected.Replace('/', System.IO.Path.DirectorySeparatorChar), fixture.Writer.Entries.FirstOrDefault());
            }

            // TODO: Windows Fact, OSX Fact
            // TODO: TestResultFilePaths
            [Fact]
            public void Should_Publish_Code_Coverage()
            {
                const string expected = @"##vso[codecoverage.publish codecoveragetool=Cobertura;summaryfile=/build/CAKE-CAKE-JOB1/coverage/cobertura-coverage.xml;reportdirectory=/build/CAKE-CAKE-JOB1/coverage/report;]";

                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService(PlatformFamily.OSX, "/build/CAKE-CAKE-JOB1");
                var data = new AzurePipelinesPublishCodeCoverageData
                {
                    CodeCoverageTool = AzurePipelinesCodeCoverageToolType.Cobertura,
                    SummaryFileLocation = "./coverage/cobertura-coverage.xml",
                    ReportDirectory = "./coverage/report"
                };

                // When
                service.Commands.PublishCodeCoverage(data);

                // Then
                Assert.Equal(expected.Replace('/', System.IO.Path.DirectorySeparatorChar), fixture.Writer.Entries.FirstOrDefault());
            }

            [WindowsFact]
            public void Should_Publish_Code_Coverage_Windows()
            {
                const string expected = @"##vso[codecoverage.publish codecoveragetool=Cobertura;summaryfile=C:\build\CAKE-CAKE-JOB1\coverage\cobertura-coverage.xml;reportdirectory=C:\build\CAKE-CAKE-JOB1\coverage\report;]";

                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();
                var data = new AzurePipelinesPublishCodeCoverageData
                {
                    CodeCoverageTool = AzurePipelinesCodeCoverageToolType.Cobertura,
                    SummaryFileLocation = "./coverage/cobertura-coverage.xml",
                    ReportDirectory = "./coverage/report"
                };

                // When
                service.Commands.PublishCodeCoverage(data);

                // Then
                Assert.Equal(expected.Replace('\\', System.IO.Path.DirectorySeparatorChar), fixture.Writer.Entries.FirstOrDefault());
            }

            [Fact]
            public void Should_Publish_Code_Coverage_If_File_Path_Provided()
            {
                const string expected = @"##vso[codecoverage.publish codecoveragetool=Cobertura;summaryfile=C:\build\CAKE-CAKE-JOB1\coverage\cobertura-coverage.xml;reportdirectory=C:\build\CAKE-CAKE-JOB1\coverage\report;]";

                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();
                var data = new AzurePipelinesPublishCodeCoverageData
                {
                    CodeCoverageTool = AzurePipelinesCodeCoverageToolType.Cobertura,
                    ReportDirectory = "./coverage/report"
                };

                // When
                service.Commands.PublishCodeCoverage("./coverage/cobertura-coverage.xml", data);

                // Then
                Assert.Equal(expected.Replace('\\', System.IO.Path.DirectorySeparatorChar), fixture.Writer.Entries.FirstOrDefault());
            }

            [Fact]
            public void Should_Publish_Code_Coverage_If_File_Path_And_Action_Provided()
            {
                const string expected = @"##vso[codecoverage.publish codecoveragetool=Cobertura;summaryfile=C:\build\CAKE-CAKE-JOB1\coverage\cobertura-coverage.xml;reportdirectory=C:\build\CAKE-CAKE-JOB1\coverage\report;]";

                // Given
                var fixture = new AzurePipelinesFixture();
                var service = fixture.CreateAzurePipelinesService();

                // When
                service.Commands.PublishCodeCoverage("./coverage/cobertura-coverage.xml",
                    data =>
                    {
                        data.CodeCoverageTool = AzurePipelinesCodeCoverageToolType.Cobertura;
                        data.ReportDirectory = "./coverage/report";
                    });

                // Then
                Assert.Equal(expected.Replace('\\', System.IO.Path.DirectorySeparatorChar), fixture.Writer.Entries.FirstOrDefault());
            }
        }
    }
}
