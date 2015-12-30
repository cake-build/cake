﻿using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.NUnit;
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
                Assert.IsArgumentNullException(result, "context");
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
                Assert.IsArgumentNullException(result, "action");

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
                Assert.IsArgumentNullException(result, "outputPath");
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
                Assert.IsArgumentNullException(result, "settings");
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
                Assert.IsCakeException(result, "No tool was started.");
            }

            [Fact]
            public void Should_Capture_Tool_And_Arguments_From_Action()
            {
                // Given
                var fixture = new OpenCoverFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-target:\"/Working/tools/Test.exe\" "+
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
        }
    }
}
