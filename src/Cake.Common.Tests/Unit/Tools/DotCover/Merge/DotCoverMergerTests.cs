// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tests.Fixtures.Tools.DotCover.Merge;
using Cake.Common.Tools.DotCover;
using Cake.Core.IO;
using Xunit;
namespace Cake.Common.Tests.Unit.Tools.DotCover.Merge
{
    public sealed class DotCoverMergerTests
    {
        public sealed class TheMergeMethod
        {
            [Fact]
            public void Should_Throw_If_Source_Files_Is_Null()
            {
                // Given
                var fixture = new DotCoverMergerFixture();
                fixture.SourceFiles = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "sourceFiles");
            }

            [Fact]
            public void Should_Throw_If_Source_Files_Is_Empty()
            {
                // Given
                var fixture = new DotCoverMergerFixture();
                fixture.SourceFiles = new List<FilePath>();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "sourceFiles");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotCoverMergerFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            #region New Parameter Syntax

            [Fact]
            public void Should_Ignore_Output_If_Not_Set()
            {
                // Given
                var fixture = new DotCoverMergerFixture();
                fixture.SourceFiles = new List<FilePath> { new ("/Working/result1.dcvr"), new ("/Working/result2.dcvr") };
                fixture.OutputFile = null;
                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("merge " +
                             "--snapshot-source \"/Working/result1.dcvr,/Working/result2.dcvr\"", result.Args);
            }

            [Fact]
            public void Should_Set_Correct_Arguments()
            {
                // Given
                var fixture = new DotCoverMergerFixture();
                fixture.SourceFiles = new List<FilePath> { new ("/Working/result1.dcvr"), new ("/Working/result2.dcvr") };
                fixture.OutputFile = new FilePath("/Working/output.dcvr");
                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("merge " +
                             "--snapshot-source \"/Working/result1.dcvr,/Working/result2.dcvr\" " +
                             "--snapshot-output \"/Working/output.dcvr\"", result.Args);
            }

            [Fact]
            public void Should_Append_TemporaryDirectory()
            {
                // Given
                var fixture = new DotCoverMergerFixture();
                fixture.Settings.TemporaryDirectory = new DirectoryPath("/Working/temp");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("merge " +
                             "--snapshot-source \"/Working/result1.dcvr,/Working/result2.dcvr\" " +
                             "--snapshot-output \"/Working/result.dcvr\" " +
                             "--temporary-directory \"/Working/temp\"", result.Args);
            }

            [Fact]
            public void Should_Not_Append_Null_TemporaryDirectory()
            {
                // Given
                var fixture = new DotCoverMergerFixture();
                fixture.Settings.TemporaryDirectory = null;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("merge " +
                             "--snapshot-source \"/Working/result1.dcvr,/Working/result2.dcvr\" " +
                             "--snapshot-output \"/Working/result.dcvr\"", result.Args);
            }

            [Fact]
            public void Should_Append_LogFile()
            {
                // Given
                var fixture = new DotCoverMergerFixture();
                fixture.Settings.LogFile = "./logfile.log";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("merge " +
                             "--snapshot-source \"/Working/result1.dcvr,/Working/result2.dcvr\" " +
                             "--snapshot-output \"/Working/result.dcvr\" " +
                             "--log-file \"/Working/logfile.log\"", result.Args);
            }

            #endregion

            #region Legacy Parameter Syntax

            [Fact]
            public void Should_Append_LogFile_LegacySyntax()
            {
                // Given
                var fixture = new DotCoverMergerFixture();
                fixture.Settings.LogFile = "./logfile.log";
                fixture.Settings.UseLegacySyntax = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("merge " +
                             "/Source=\"/Working/result1.dcvr;/Working/result2.dcvr\" " +
                             "/Output=\"/Working/result.dcvr\" " +
                             "/LogFile=\"/Working/logfile.log\"", result.Args);
            }

            [Fact]
            public void Should_Append_ConfigurationFile_LegacySyntax()
            {
                // Given
                var fixture = new DotCoverMergerFixture();
                fixture.Settings.WithConfigFile(new FilePath("./config.xml"));
                fixture.Settings.UseLegacySyntax = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("merge \"/Working/config.xml\" " +
                             "/Source=\"/Working/result1.dcvr;/Working/result2.dcvr\" " +
                             "/Output=\"/Working/result.dcvr\"", result.Args);
            }

            [Fact]
            public void Should_Throw_If_Output_File_Is_Null_LegacySyntax()
            {
                // Given
                var fixture = new DotCoverMergerFixture();
                fixture.OutputFile = null;
                fixture.Settings.UseLegacySyntax = true;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "outputFile");
            }

            #endregion
        }
    }
}