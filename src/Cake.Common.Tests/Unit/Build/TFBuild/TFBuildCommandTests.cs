// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Build.TFBuild;
using Cake.Common.Build.TFBuild.Data;
using Cake.Common.Tests.Fixtures.Build;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.TFBuild
{
    public sealed class TFBuildCommandTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new TFBuildCommands(null, new NullLog()));

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new TFBuildCommands(new FakeEnvironment(PlatformFamily.Unknown), null));

                // Then
                AssertEx.IsArgumentNullException(result, "log");
            }
        }

        public sealed class TheCommands
        {
            [Fact]
            public void Should_Not_Be_Null()
            {
                // Given
                var fixture = new TFBuildFixture();

                // When
                var service = fixture.CreateTFBuildService();

                // Then
                Assert.NotNull(service.Commands);
            }

            [Theory]
            [InlineData("warning")]
            [InlineData("message")]
            public void Should_Log_Warning_Message(string msg)
            {
                // Given
                var fixture = new TFBuildFixture();
                var service = fixture.CreateTFBuildService();

                // When
                service.Commands.WriteWarning(msg);

                // Then
                Assert.Contains(fixture.Log.Entries, m => m.Message == $"##vso[task.logissue type=warning;]{msg}");
            }

            [Fact]
            public void Should_Log_Warning_Message_With_Data()
            {
                // Given
                var fixture = new TFBuildFixture();
                var service = fixture.CreateTFBuildService();

                // When
                service.Commands.WriteWarning("build warning", new TFBuildMessageData
                {
                    SourcePath = "./code file.cs",
                    LineNumber = 5,
                    ColumnNumber = 12,
                    ErrorCode = 9
                });

                // Then
                Assert.Contains(fixture.Log.Entries, m => m.Message == $"##vso[task.logissue sourcepath=./code file.cs;linenumber=5;columnnumber=12;code=9;type=warning;]build warning");
            }

            [Theory]
            [InlineData("error")]
            [InlineData("message")]
            public void Should_Log_Error_Message(string msg)
            {
                // Given
                var fixture = new TFBuildFixture();
                var service = fixture.CreateTFBuildService();

                // When
                service.Commands.WriteError(msg);

                // Then
                Assert.Contains(fixture.Log.Entries, m => m.Message == $"##vso[task.logissue type=error;]{msg}");
            }

            [Fact]
            public void Should_Log_Error_Message_With_Data()
            {
                // Given
                var fixture = new TFBuildFixture();
                var service = fixture.CreateTFBuildService();

                // When
                service.Commands.WriteError("build error", new TFBuildMessageData
                {
                    SourcePath = "./code.cs",
                    LineNumber = 1,
                    ColumnNumber = 2,
                    ErrorCode = 3
                });

                // Then
                Assert.Contains(fixture.Log.Entries, m => m.Message == $"##vso[task.logissue sourcepath=./code.cs;linenumber=1;columnnumber=2;code=3;type=error;]build error");
            }

            [Fact]
            public void Should_Set_Current_Progress()
            {
                // Given
                var fixture = new TFBuildFixture();
                var service = fixture.CreateTFBuildService();

                // When
                service.Commands.SetProgress(75, "Testing Provider");

                // Then
                Assert.Contains(fixture.Log.Entries, m => m.Message == $"##vso[task.setprogress value=75;]Testing Provider");
            }

            [Fact]
            public void Should_Complete_Current_Task()
            {
                // Given
                var fixture = new TFBuildFixture();
                var service = fixture.CreateTFBuildService();

                // When
                service.Commands.CompleteCurrentTask();

                // Then
                Assert.Contains(fixture.Log.Entries, m => m.Message == $"##vso[task.complete ]DONE");
            }

            [Fact]
            public void Should_Complete_Current_Task_With_Status()
            {
                // Given
                var fixture = new TFBuildFixture();
                var service = fixture.CreateTFBuildService();

                // When
                service.Commands.CompleteCurrentTask(TFBuildTaskResult.Failed);

                // Then
                Assert.Contains(fixture.Log.Entries, m => m.Message == $"##vso[task.complete result=Failed;]DONE");
            }

            [Fact]
            public void Should_Create_New_Record()
            {
                // Given
                var fixture = new TFBuildFixture();
                var service = fixture.CreateTFBuildService();

                // When
                var guid = service.Commands.CreateNewRecord("New record", "build", 1);

                // Then
                Assert.NotNull(guid);
                Assert.Contains(fixture.Log.Entries, m => m.Message == $"##vso[task.logdetail id={guid.ToString()};name=New record;type=build;order=1;]create new timeline record");
            }

            [Fact]
            public void Should_Create_New_Record_With_Data()
            {
                // Given
                var fixture = new TFBuildFixture();
                var service = fixture.CreateTFBuildService();
                var date = DateTime.UtcNow;

                // When
                var guid = service.Commands.CreateNewRecord("New record", "build", 2, new TFBuildRecordData
                {
                    StartTime = date,
                    Progress = 75,
                    Status = TFBuildTaskStatus.Initialized
                });

                // Then
                Assert.NotNull(guid);
                Assert.Contains(fixture.Log.Entries, m => m.Message == $"##vso[task.logdetail starttime={date.ToString()};progress=75;state=Initialized;id={guid.ToString()};name=New record;type=build;order=2;]create new timeline record");
            }

            [Fact]
            public void Should_Update_Existing_Record()
            {
                // Given
                var fixture = new TFBuildFixture();
                var service = fixture.CreateTFBuildService();
                var guid = Guid.NewGuid();
                var parent = Guid.NewGuid();

                // When
                service.Commands.UpdateRecord(guid, new TFBuildRecordData
                {
                    Progress = 95,
                    Status = TFBuildTaskStatus.InProgress,
                    ParentRecord = parent
                });

                // Then
                Assert.Contains(fixture.Log.Entries, m => m.Message == $"##vso[task.logdetail parentid={parent.ToString()};progress=95;state=InProgress;id={guid.ToString()};]update");
            }

            [Fact]
            public void Should_Set_Variable()
            {
                // Given
                var fixture = new TFBuildFixture();
                var service = fixture.CreateTFBuildService();

                // When
                service.Commands.SetVariable("varname", "VarValue");

                // Then
                Assert.Contains(fixture.Log.Entries,
                    m => m.Message == $"##vso[task.setvariable variable=varname;]VarValue");
            }

            [Fact]
            public void Should_Set_Secret_Variable()
            {
                // Given
                var fixture = new TFBuildFixture();
                var service = fixture.CreateTFBuildService();

                // When
                service.Commands.SetSecretVariable("Secret Variable", "Secret Value");

                // Then
                Assert.Contains(fixture.Log.Entries, m => m.Message == "##vso[task.setvariable variable=Secret Variable;issecret=true;]Secret Value");
            }

            [Fact]
            public void Should_Upload_Task_Summary()
            {
                // Given
                var fixture = new TFBuildFixture();
                var service = fixture.CreateTFBuildService();
                var path = FilePath.FromString("./summary.md").MakeAbsolute(fixture.Environment);

                // When
                service.Commands.UploadTaskSummary("./summary.md");

                // Then
                Assert.Contains(fixture.Log.Entries, m => m.Message == $"##vso[task.uploadsummary ]{path}");
            }

            [Fact]
            public void Should_Upload_Task_Log()
            {
                // Given
                var fixture = new TFBuildFixture();
                var service = fixture.CreateTFBuildService();
                var path = FilePath.FromString("./logs/task.log").MakeAbsolute(fixture.Environment);

                // When
                service.Commands.UploadTaskLogFile("./logs/task.log");

                // Then
                Assert.Contains(fixture.Log.Entries, m => m.Message == $"##vso[task.uploadfile ]{path}");
            }

            [Theory]
            [InlineData("drop", TFBuildArtifactType.Container, "./drop")]
            [InlineData("artifact", TFBuildArtifactType.FilePath, "./dist/build/artifact.file")]
            [InlineData("ref", TFBuildArtifactType.GitRef, "895a00ec66af875c1593a7563beb0edee400aba0")]
            public void Should_Link_Build_Artifacts(string name, TFBuildArtifactType type, string location)
            {
                // Given
                var fixture = new TFBuildFixture();
                var service = fixture.CreateTFBuildService();

                // When
                service.Commands.LinkArtifact(name, type, location);

                // Then
                Assert.Contains(fixture.Log.Entries,
                    m => m.Message == $"##vso[artifact.associate artifactname={name};type={type};]{location}");
            }

            [Fact]
            public void Should_Upload_To_Container()
            {
                // Given
                var fixture = new TFBuildFixture();
                var service = fixture.CreateTFBuildService();
                var path = FilePath.FromString("./dist/package.nupkg").MakeAbsolute(fixture.Environment).FullPath;

                // When
                service.Commands.UploadArtifact("packages", "./dist/package.nupkg");

                // Then
                Assert.Contains(fixture.Log.Entries,
                    m => m.Message == $"##vso[artifact.upload containerfolder=packages;]{path}");
            }

            [Fact]
            public void Should_Upload_To_Container_Artifact()
            {
                // Given
                var fixture = new TFBuildFixture();
                var service = fixture.CreateTFBuildService();
                var path = FilePath.FromString("./artifacts/results.trx").MakeAbsolute(fixture.Environment).FullPath;

                // When
                service.Commands.UploadArtifact("tests", "./artifacts/results.trx", "Test Results");

                // Then
                Assert.Contains(fixture.Log.Entries, m => m.Message == $"##vso[artifact.upload containerfolder=tests;artifactname=Test Results;]{path}");
            }

            [Fact]
            public void Should_Upload_Build_Log()
            {
                // Given
                var fixture = new TFBuildFixture();
                var service = fixture.CreateTFBuildService();
                var path = FilePath.FromString("./dist/buildlog.txt").MakeAbsolute(fixture.Environment).FullPath;

                // When
                service.Commands.UploadBuildLogFile("./dist/buildlog.txt");

                // Then
                Assert.Contains(fixture.Log.Entries, m => m.Message == $"##vso[build.uploadlog ]{path}");
            }

            [Fact]
            public void Should_Update_Build_Number()
            {
                // Given
                var fixture = new TFBuildFixture();
                var service = fixture.CreateTFBuildService();

                // When
                service.Commands.UpdateBuildNumber("CIBuild_1");

                // Then
                Assert.Contains(fixture.Log.Entries, m => m.Message == "##vso[build.updatebuildnumber ]CIBuild_1");
            }

            [Fact]
            public void Should_Add_Build_Tag()
            {
                // Given
                var fixture = new TFBuildFixture();
                var service = fixture.CreateTFBuildService();

                // When
                service.Commands.AddBuildTag("Stable");

                // Then
                Assert.Contains(fixture.Log.Entries, m => m.Message == "##vso[build.addbuildtag ]Stable");
            }
        }
    }
}
