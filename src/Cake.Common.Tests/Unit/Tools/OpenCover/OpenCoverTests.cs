﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.NUnit;
using Cake.Common.Tools.OpenCover;
using Cake.Common.Tools.XUnit;
using Cake.Core.IO;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.OpenCover
{
    public sealed class OpenCoverTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given
                var fixture = new OpenCoverFixture();
                fixture.Context = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Action_Is_Null()
            {
                // Given
                var fixture = new OpenCoverFixture();
                fixture.Action = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "action");
            }

            [Fact]
            public void Should_Throw_If_Output_File_Is_Null()
            {
                // Given
                var fixture = new OpenCoverFixture();
                fixture.OutputPath = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "outputPath");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new OpenCoverFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_No_Tool_Was_Intercepted()
            {
                // Given
                var fixture = new OpenCoverFixture();
                fixture.Action = context => { };

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "No tool was started.");
            }

            [Fact]
            public void Should_Capture_Tool_And_Arguments_From_Action()
            {
                // Given
                var fixture = new OpenCoverFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-target:\"/Working/tools/Test.exe\" " +
                             "-targetargs:\"-argument\" " +
                             "-register:user -output:\"/Working/result.xml\"", result.Args);
            }

            [Theory]
            [InlineData("")]
            [InlineData(null)]
            public void Should_Not_Capture_Arguments_From_Action_If_Excluded(string arguments)
            {
                // Given
                var fixture = new OpenCoverFixture();
                fixture.Action = context =>
                {
                    context.ProcessRunner.Start(
                        new FilePath("/Working/tools/Test.exe"),
                        new ProcessSettings()
                        {
                            Arguments = arguments
                        });
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-target:\"/Working/tools/Test.exe\" " +
                             "-register:user -output:\"/Working/result.xml\"", result.Args);
            }

            [Fact]
            public void Should_Append_Filters()
            {
                // Given
                var fixture = new OpenCoverFixture();
                fixture.Settings.Filters.Add("filter1");
                fixture.Settings.Filters.Add("filter2");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-target:\"/Working/tools/Test.exe\" " +
                             "-targetargs:\"-argument\" " +
                             "-filter:\"filter1 filter2\" " +
                             "-register:user -output:\"/Working/result.xml\"", result.Args);
            }

            [Fact]
            public void Should_Append_Attribute_Filters()
            {
                // Given
                var fixture = new OpenCoverFixture();
                fixture.Settings.ExcludedAttributeFilters.Add("filter1");
                fixture.Settings.ExcludedAttributeFilters.Add("filter2");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-target:\"/Working/tools/Test.exe\" " +
                             "-targetargs:\"-argument\" " +
                             "-excludebyattribute:\"filter1;filter2\" " +
                             "-register:user -output:\"/Working/result.xml\"", result.Args);
            }

            [Fact]
            public void Should_Append_File_Filters()
            {
                // Given
                var fixture = new OpenCoverFixture();
                fixture.Settings.ExcludedFileFilters.Add("filter1");
                fixture.Settings.ExcludedFileFilters.Add("filter2");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-target:\"/Working/tools/Test.exe\" " +
                             "-targetargs:\"-argument\" " +
                             "-excludebyfile:\"filter1;filter2\" " +
                             "-register:user -output:\"/Working/result.xml\"", result.Args);
            }

            [Fact]
            public void Should_Capture_XUnit()
            {
                // Given
                var fixture = new OpenCoverFixture();
                fixture.FileSystem.CreateFile("/Working/tools/xunit.console.exe");
                fixture.Action = context =>
                {
                    context.XUnit2(
                        new FilePath[] { "./Test.dll" },
                        new XUnit2Settings { ShadowCopy = false });
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-target:\"/Working/tools/xunit.console.exe\" " +
                             "-targetargs:\"\\\"/Working/Test.dll\\\" -noshadow\" " +
                             "-register:user -output:\"/Working/result.xml\"", result.Args);
            }

            [Fact]
            public void Should_Capture_NUnit()
            {
                // Given
                var fixture = new OpenCoverFixture();
                fixture.FileSystem.CreateFile("/Working/tools/nunit-console.exe");
                fixture.Action = context =>
                {
                    context.NUnit(
                        new FilePath[] { "./Test.dll" },
                        new NUnitSettings { ShadowCopy = false });
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-target:\"/Working/tools/nunit-console.exe\" " +
                             "-targetargs:\"\\\"/Working/Test.dll\\\" -noshadow\" " +
                             "-register:user -output:\"/Working/result.xml\"", result.Args);
            }

            [Fact]
            public void Should_Use_Specified_Register()
            {
                // Given
                var fixture = new OpenCoverFixture();
                fixture.Settings.Register = new OpenCoverRegisterOptionDll("Path32");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-target:\"/Working/tools/Test.exe\" " +
                             "-targetargs:\"-argument\" " +
                             "-register:Path32 -output:\"/Working/result.xml\"", result.Args);
            }

            [Fact]
            public void Should_Use_No_Register()
            {
                // Given
                var fixture = new OpenCoverFixture();
                fixture.Settings.Register = null;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-target:\"/Working/tools/Test.exe\" " +
                             "-targetargs:\"-argument\" " +
                             "-output:\"/Working/result.xml\"", result.Args);
            }

            [Fact]
            public void Should_Use_Admin_Register()
            {
                // Given
                var fixture = new OpenCoverFixture();
                fixture.Settings.Register = new OpenCoverRegisterOptionAdmin();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-target:\"/Working/tools/Test.exe\" " +
                             "-targetargs:\"-argument\" " +
                             "-register -output:\"/Working/result.xml\"", result.Args);
            }

            [Fact]
            public void Should_Use_User_Register()
            {
                // Given
                var fixture = new OpenCoverFixture();
                fixture.Settings.Register = new OpenCoverRegisterOptionUser();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-target:\"/Working/tools/Test.exe\" " +
                             "-targetargs:\"-argument\" " +
                             "-register:user -output:\"/Working/result.xml\"", result.Args);
            }

            [Fact]
            public void Should_Not_break_when_old_string_registrations_are_used()
            {
                // Given
                var fixture = new OpenCoverFixture();
#pragma warning disable CS0618 // Type or member is obsolete
                fixture.Settings.Register = "Path64";
#pragma warning restore CS0618 // Type or member is obsolete

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-target:\"/Working/tools/Test.exe\" " +
                             "-targetargs:\"-argument\" " +
                             "-register:Path64 -output:\"/Working/result.xml\"", result.Args);
            }

            [Fact]
            public void Should_Add_ReturnTargetCode_If_ReturnTargetCodeOffset_Is_Set()
            {
                // Given
                var fixture = new OpenCoverFixture();
                fixture.Settings.ReturnTargetCodeOffset = 100;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-target:\"/Working/tools/Test.exe\" " +
                             "-targetargs:\"-argument\" " +
                             "-register:user " +
                             "-returntargetcode:100 " +
                             "-output:\"/Working/result.xml\"", result.Args);
            }

            [Fact]
            public void Should_Append_SkipAutoProps()
            {
                // Given
                var fixture = new OpenCoverFixture();
                fixture.Settings.SkipAutoProps = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-target:\"/Working/tools/Test.exe\" " +
                             "-targetargs:\"-argument\" " +
                             "-skipautoprops " +
                             "-register:user -output:\"/Working/result.xml\"", result.Args);
            }

            [Fact]
            public void Should_Append_OldStyle()
            {
                // Given
                var fixture = new OpenCoverFixture();
                fixture.Settings.OldStyle = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-target:\"/Working/tools/Test.exe\" " +
                             "-targetargs:\"-argument\" " +
                             "-oldStyle " +
                             "-register:user -output:\"/Working/result.xml\"", result.Args);
            }

            [Fact]
            public void Should_Append_MergeOutput()
            {
                // Given
                var fixture = new OpenCoverFixture();
                fixture.Settings.MergeOutput = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-target:\"/Working/tools/Test.exe\" " +
                             "-targetargs:\"-argument\" " +
                             "-mergeoutput " +
                             "-register:user -output:\"/Working/result.xml\"", result.Args);
            }

            [Fact]
            public void Should_Append_Exclude_Directories()
            {
                // Given
                var fixture = new OpenCoverFixture();
                fixture.Settings.ExcludeDirectories.Add("dir1");
                fixture.Settings.ExcludeDirectories.Add("dir2");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-target:\"/Working/tools/Test.exe\" " +
                             "-targetargs:\"-argument\" " +
                             "-register:user -output:\"/Working/result.xml\" " +
                             "-excludedirs:\"/Working/dir1;/Working/dir2\"", result.Args);
            }

            [Fact]
            public void Should_Append_LogLevel()
            {
                // Given
                var fixture = new OpenCoverFixture();
                fixture.Settings.LogLevel = OpenCoverLogLevel.All;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-target:\"/Working/tools/Test.exe\" " +
                             "-targetargs:\"-argument\" " +
                             "-register:user -output:\"/Working/result.xml\" " +
                             "-log:All", result.Args);
            }

            [Fact]
            public void Should_Append_HideSkipped()
            {
                // Given
                var fixture = new OpenCoverFixture();
                fixture.Settings.HideSkippedOption = OpenCoverHideSkippedOption.File
                                                     | OpenCoverHideSkippedOption.MissingPdb
                                                     | OpenCoverHideSkippedOption.Filter
                                                     | OpenCoverHideSkippedOption.All
                                                     | OpenCoverHideSkippedOption.Attribute;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-target:\"/Working/tools/Test.exe\" " +
                             "-targetargs:\"-argument\" " +
                             "-register:user -output:\"/Working/result.xml\" " +
                             "-hideskipped:All", result.Args);
            }

            [Fact]
            public void Should_Append_MergeByHash()
            {
                // Given
                var fixture = new OpenCoverFixture();
                fixture.Settings.MergeByHash = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-target:\"/Working/tools/Test.exe\" " +
                             "-targetargs:\"-argument\" " +
                             "-register:user -output:\"/Working/result.xml\" " +
                             "-mergebyhash", result.Args);
            }

            [Fact]
            public void Should_Append_NoDefaultFilters()
            {
                // Given
                var fixture = new OpenCoverFixture();
                fixture.Settings.NoDefaultFilters = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-target:\"/Working/tools/Test.exe\" " +
                             "-targetargs:\"-argument\" " +
                             "-register:user -output:\"/Working/result.xml\" " +
                             "-nodefaultfilters", result.Args);
            }

            [Fact]
            public void Should_Append_Search_Directories()
            {
                // Given
                var fixture = new OpenCoverFixture();
                fixture.Settings.SearchDirectories.Add("dir1");
                fixture.Settings.SearchDirectories.Add("dir2");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-target:\"/Working/tools/Test.exe\" " +
                             "-targetargs:\"-argument\" " +
                             "-register:user -output:\"/Working/result.xml\" " +
                             "-searchdirs:\"/Working/dir1;/Working/dir2\"", result.Args);
            }

            [Fact]
            public void Should_Append_Service()
            {
                // Given
                var fixture = new OpenCoverFixture();
                fixture.Settings.IsService = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-target:\"/Working/tools/Test.exe\" " +
                             "-targetargs:\"-argument\" " +
                             "-register:user -output:\"/Working/result.xml\" " +
                             "-service", result.Args);
            }

            [Fact]
            public void Should_Append_TargetDir()
            {
                // Given
                var fixture = new OpenCoverFixture();
                fixture.Settings.TargetDirectory = "TarDir";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-target:\"/Working/tools/Test.exe\" " +
                             "-targetargs:\"-argument\" " +
                             "-register:user -output:\"/Working/result.xml\" " +
                             "-targetdir:\"/Working/TarDir\"", result.Args);
            }
        }
    }
}