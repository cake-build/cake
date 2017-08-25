// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tests.Fixtures.Tools.DotCover.Merge;
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
            public void Should_Throw_If_Output_File_Is_Null()
            {
                // Given
                var fixture = new DotCoverMergerFixture();
                fixture.OutputFile = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "outputFile");
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

            [Fact]
            public void Should_Append_LogFile()
            {
                // Given
                var fixture = new DotCoverMergerFixture();
                fixture.Settings.LogFile = "./logfile.log";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("Merge " +
                             "/Source=\"/Working/result1.dcvr;/Working/result2.dcvr\" " +
                             "/Output=\"/Working/result.dcvr\" " +
                             "/LogFile=\"/Working/logfile.log\"", result.Args);
            }
        }
    }
}