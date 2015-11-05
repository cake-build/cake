﻿using Cake.Core;
using Xunit;

namespace Cake.Common.Tests.Unit
{
    public sealed class ReleaseNotesParserTests
    {
        public sealed class TheParseMethod
        {
            [Fact]
            public void Should_Throw_If_Content_Is_Null()
            {
                // Given
                var parser = new ReleaseNotesParser();

                // When
                var result = Record.Exception(() => parser.Parse(null));

                // Then
                Assert.IsArgumentNullException(result, "content");
            }

            [Fact]
            public void Should_Throw_If_Content_Is_Empty()
            {
                // Given
                var parser = new ReleaseNotesParser();

                // When
                var result = Record.Exception(() => parser.Parse(string.Empty));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Unknown release notes format.", result.Message);
            }

            public sealed class ComplexFormat
            {
                [Fact]
                public void Should_Throw_If_Header_Is_Missing_Version()
                {
                    // Given
                    var parser = new ReleaseNotesParser();
                    const string content = "### Hello World (Released 2014-06-29)";

                    // When
                    var result = Record.Exception(() => parser.Parse(content));

                    // Then
                    Assert.IsType<CakeException>(result);
                    Assert.Equal("Could not parse version from release notes header.", result.Message);
                }

                [Fact]
                public void Should_Parse_Release_Note_Version()
                {
                    // Given
                    var parser = new ReleaseNotesParser();
                    const string content = "### New in 0.1.9 (Releases 2014/06/28)\n* Line 1";

                    // When
                    var result = parser.Parse(content);

                    // Then
                    Assert.Equal("0.1.9", result[0].Version.ToString());
                }

                [Fact]
                public void Should_Parse_Release_Note_Text()
                {
                    // Given
                    var parser = new ReleaseNotesParser();
                    const string content = "### New in 0.1.9 (Releases 2014/06/28)\nLine 1";

                    // When
                    var result = parser.Parse(content);

                    // Then
                    Assert.Equal(1, result[0].Notes.Count);
                    Assert.Equal("Line 1", result[0].Notes[0]);
                }

                [Fact]
                public void Should_Remove_Bullets_From_Release_Note_Text()
                {
                    // Given
                    var parser = new ReleaseNotesParser();
                    const string content = "### New in 0.1.9 (Releases 2014/06/28)\n* Line 1";

                    // When
                    var result = parser.Parse(content);

                    // Then
                    Assert.Equal(1, result[0].Notes.Count);
                    Assert.Equal("Line 1", result[0].Notes[0]);
                }

                [Fact]
                public void Should_Return_Multiple_Release_Notes()
                {
                    // Given
                    var parser = new ReleaseNotesParser();
                    const string content = "### New in 0.1.9 (Releases 2014/06/28)\n* Line 1\n" +
                        "###New in 0.1.10\n* Line 2\n Line 3";

                    // When
                    var result = parser.Parse(content);

                    // Then
                    Assert.Equal(2, result.Count);
                }

                [Fact]
                public void Should_Return_Release_Notes_In_Descending_Order()
                {
                    // Given
                    var parser = new ReleaseNotesParser();
                    const string content = "### New in 0.1.9 (Releases 2014/06/28)\n* Line 1\n" +
                        "###New in 0.1.10\n* Line 2\n Line 3";

                    // When
                    var result = parser.Parse(content);

                    // Then
                    Assert.Equal(2, result.Count);
                    Assert.Equal("0.1.10", result[0].Version.ToString());
                    Assert.Equal("0.1.9", result[1].Version.ToString());
                }

                [Fact]
                public void Should_Remove_Empty_Lines_From_Release_Note_Text()
                {
                    // Given
                    var parser = new ReleaseNotesParser();
                    const string content = "### New in 0.1.9 (Releases 2014/06/28)\nLine 1\n  \n\t\n";

                    // When
                    var result = parser.Parse(content);

                    // Then
                    Assert.Equal(1, result[0].Notes.Count);
                    Assert.Equal("Line 1", result[0].Notes[0]);
                }
            }

            public sealed class SimpleFormat
            {
                [Fact]
                public void Should_Throw_If_Header_Is_Missing_Version()
                {
                    // Given
                    var parser = new ReleaseNotesParser();
                    const string content = "* - Hello World";

                    // When
                    var result = Record.Exception(() => parser.Parse(content));

                    // Then
                    Assert.IsType<CakeException>(result);
                    Assert.Equal("Could not parse version from release notes header.", result.Message);
                }

                [Fact]
                public void Should_Parse_Release_Note_Version()
                {
                    // Given
                    var parser = new ReleaseNotesParser();
                    const string content = "* 0.1.9 - Line 1";

                    // When
                    var result = parser.Parse(content);

                    // Then
                    Assert.Equal("0.1.9", result[0].Version.ToString());
                }

                [Fact]
                public void Should_Parse_Release_Note_Text()
                {
                    // Given
                    var parser = new ReleaseNotesParser();
                    const string content = "* 0.1.9 - Line 1";

                    // When
                    var result = parser.Parse(content);

                    // Then
                    Assert.Equal(1, result[0].Notes.Count);
                    Assert.Equal("Line 1", result[0].Notes[0]);
                }

                [Fact]
                public void Should_Remove_Empty_Lines_From_Release_Note_Text()
                {
                    // Given
                    var parser = new ReleaseNotesParser();
                    const string content = "* 0.1.9 Line 1\n\n  \n\t\n* 0.1.10 Line 2\n\t";

                    // When
                    var result = parser.Parse(content);

                    // Then
                    Assert.Equal(2, result.Count);
                }

                [Fact]
                public void Should_Return_Multiple_Release_Notes()
                {
                    // Given
                    var parser = new ReleaseNotesParser();
                    const string content = "* 0.1.9 Line 1\n* 0.1.10 Line 2";

                    // When
                    var result = parser.Parse(content);

                    // Then
                    Assert.Equal(2, result.Count);
                }

                [Fact]
                public void Should_Return_Release_Notes_In_Descending_Order()
                {
                    // Given
                    var parser = new ReleaseNotesParser();
                    const string content = "* 0.1.9 Line 1\n* 0.1.10 Line 2";

                    // When
                    var result = parser.Parse(content);

                    // Then
                    Assert.Equal(2, result.Count);
                    Assert.Equal("0.1.10", result[0].Version.ToString());
                    Assert.Equal("0.1.9", result[1].Version.ToString());
                }
            }
        }
    }
}