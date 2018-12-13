// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;
using Cake.Core.Tests.Fixtures;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Core.Tests.Unit.IO.Globbing
{
    public sealed class UncGlobberTests
    {
        [WindowsFact]
        public void Should_Return_Files_And_Folders_For_Pattern_Ending_With_Wildcard()
        {
            // Given
            var fixture = GlobberFixture.Windows();

            // When
            var result = fixture.Match(@"\\Server\**\*");

            // Then
            Assert.Equal(15, result.Length);
            AssertEx.ContainsDirectoryPath(result, @"\\Server\Foo");
            AssertEx.ContainsDirectoryPath(result, @"\\Server\Foo\Bar");
            AssertEx.ContainsDirectoryPath(result, @"\\Server\Foo\Baz");
            AssertEx.ContainsDirectoryPath(result, @"\\Server\Foo\Bar\Baz");
            AssertEx.ContainsDirectoryPath(result, @"\\Server\Bar");
            AssertEx.ContainsFilePath(result, @"\\Server\Foo\Bar\Qux.c");
            AssertEx.ContainsFilePath(result, @"\\Server\Foo\Bar\Qex.c");
            AssertEx.ContainsFilePath(result, @"\\Server\Foo\Bar\Qux.h");
            AssertEx.ContainsFilePath(result, @"\\Server\Foo\Baz\Qux.c");
            AssertEx.ContainsFilePath(result, @"\\Server\Foo\Bar\Baz\Qux.c");
            AssertEx.ContainsFilePath(result, @"\\Server\Foo.Bar.Test.dll");
            AssertEx.ContainsFilePath(result, @"\\Server\Bar.Qux.Test.dll");
            AssertEx.ContainsFilePath(result, @"\\Server\Quz.FooTest.dll");
            AssertEx.ContainsFilePath(result, @"\\Server\Bar\Qux.c");
            AssertEx.ContainsFilePath(result, @"\\Server\Bar\Qux.h");
        }

        [WindowsFact]
        public void Should_Throw_If_No_Share_Name_Has_Been_Specified()
        {
            // Given
            var fixture = GlobberFixture.Windows();

            // When
            var result = Record.Exception(() => fixture.Match(@"\\"));

            // Then
            Assert.NotNull(result);
            Assert.Equal(@"The pattern '\\' has no server part specified.", result.Message);
        }

        [WindowsTheory]
        [InlineData(@"\\fo?")]
        [InlineData(@"\\fo*")]
        [InlineData(@"\\fo?\bar")]
        [InlineData(@"\\fo*\bar")]
        public void Should_Throw_If_Invalid_Share_Name_Has_Been_Specified(string input)
        {
            // Given
            var fixture = GlobberFixture.Windows();

            // When
            var result = Record.Exception(() => fixture.Match(input));

            // Then
            Assert.NotNull(result);
            Assert.Equal($"The pattern '{input}' has an invalid server part specified.", result.Message);
        }

        [WindowsFact]
        public void Can_Traverse_Recursively()
        {
            // Given
            var fixture = GlobberFixture.Windows();

            // When
            var result = fixture.Match(@"\\Server\**\*.c");

            // Then
            Assert.Equal(5, result.Length);
            AssertEx.ContainsFilePath(result, @"\\Server\Foo\Bar\Qux.c");
            AssertEx.ContainsFilePath(result, @"\\Server\Foo\Baz\Qux.c");
            AssertEx.ContainsFilePath(result, @"\\Server\Foo\Bar\Qex.c");
            AssertEx.ContainsFilePath(result, @"\\Server\Foo\Bar\Baz/Qux.c");
            AssertEx.ContainsFilePath(result, @"\\Server\Bar\Qux.c");
        }

        [WindowsFact]
        public void Should_Be_Able_To_Visit_Parent_Using_Double_Dots()
        {
            // Given
            var fixture = GlobberFixture.Windows();

            // When
            var result = fixture.Match(@"\\Server\Foo\..\Foo\Bar\Qux.c");

            // Then
            Assert.Single(result);
            Assert.IsType<FilePath>(result[0]);
            AssertEx.ContainsFilePath(result, @"\\Server\Foo\Bar\Qux.c");
        }

        [WindowsFact]
        public void Should_Return_Single_Path_For_Absolute_File_Path_Without_Glob_Pattern()
        {
            // Given
            var fixture = GlobberFixture.Windows();

            // When
            var result = fixture.Match(@"\\Server\Foo\Bar\Qux.c");

            // Then
            Assert.Single(result);
            Assert.IsType<FilePath>(result[0]);
            AssertEx.ContainsFilePath(result, @"\\Server\Foo\Bar\Qux.c");
        }

        [WindowsFact]
        public void Should_Return_Single_Path_For_Absolute_Directory_Path_Without_Glob_Pattern()
        {
            // Given
            var fixture = GlobberFixture.Windows();

            // When
            var result = fixture.Match(@"\\Server\Foo\Bar");

            // Then
            Assert.Single(result);
            AssertEx.ContainsDirectoryPath(result, @"\\Server\Foo\Bar");
        }

        [WindowsFact]
        public void Should_Return_Files_And_Folders_For_Pattern_Containing_Wildcard()
        {
            // Given
            var fixture = GlobberFixture.Windows();

            // When
            var result = fixture.Match(@"\\Server\Foo\*\Qux.c");

            // Then
            Assert.Equal(2, result.Length);
            AssertEx.ContainsFilePath(result, @"\\Server\Foo\Bar\Qux.c");
            AssertEx.ContainsFilePath(result, @"\\Server\Foo\Baz\Qux.c");
        }

        [WindowsFact]
        public void Should_Return_Files_And_Folders_For_Pattern_Ending_With_Character_Wildcard()
        {
            // Given
            var fixture = GlobberFixture.Windows();

            // When
            var result = fixture.Match(@"\\Server\Foo\Bar\Q?x.c");

            // Then
            Assert.Equal(2, result.Length);
            AssertEx.ContainsFilePath(result, @"\\Server\Foo\Bar\Qux.c");
            AssertEx.ContainsFilePath(result, @"\\Server\Foo\Bar\Qex.c");
        }

        [WindowsFact]
        public void Should_Return_Files_And_Folders_For_Pattern_Containing_Character_Wildcard()
        {
            // Given
            var fixture = GlobberFixture.Windows();

            // When
            var result = fixture.Match(@"\\Server\Foo\Ba?\Qux.c");

            // Then
            Assert.Equal(2, result.Length);
            AssertEx.ContainsFilePath(result, @"\\Server\Foo\Bar\Qux.c");
            AssertEx.ContainsFilePath(result, @"\\Server\Foo\Baz\Qux.c");
        }

        [WindowsFact]
        public void Should_Return_Files_For_Pattern_Ending_With_Character_Wildcard_And_Dot()
        {
            // Given
            var fixture = GlobberFixture.Windows();

            // When
            var result = fixture.Match(@"\\Server\*.Test.dll");

            // Then
            Assert.Equal(2, result.Length);
            AssertEx.ContainsFilePath(result, @"\\Server\Foo.Bar.Test.dll");
            AssertEx.ContainsFilePath(result, @"\\Server\Bar.Qux.Test.dll");
        }

        [WindowsFact]
        public void Should_Return_File_For_Recursive_Wildcard_Pattern_Ending_With_Wildcard_Regex()
        {
            // Given
            var fixture = GlobberFixture.Windows();

            // When
            var result = fixture.Match(@"\\Server\**\*.c");

            // Then
            Assert.Equal(5, result.Length);
            AssertEx.ContainsFilePath(result, @"\\Server\Foo\Bar\Qux.c");
            AssertEx.ContainsFilePath(result, @"\\Server\Foo\Bar\Qex.c");
            AssertEx.ContainsFilePath(result, @"\\Server\Foo\Baz\Qux.c");
            AssertEx.ContainsFilePath(result, @"\\Server\Foo\Bar\Baz\Qux.c");
            AssertEx.ContainsFilePath(result, @"\\Server\Bar\Qux.c");
        }

        [WindowsFact]
        public void Should_Return_Only_Folders_For_Pattern_Ending_With_Recursive_Wildcard()
        {
            // Given
            var fixture = GlobberFixture.Windows();

            // When
            var result = fixture.Match(@"\\Server\**");

            // Then
            Assert.Equal(6, result.Length);
            AssertEx.ContainsDirectoryPath(result, @"\\Server");
            AssertEx.ContainsDirectoryPath(result, @"\\Server\Foo");
            AssertEx.ContainsDirectoryPath(result, @"\\Server\Foo\Bar");
            AssertEx.ContainsDirectoryPath(result, @"\\Server\Foo\Baz");
            AssertEx.ContainsDirectoryPath(result, @"\\Server\Foo\Bar\Baz");
            AssertEx.ContainsDirectoryPath(result, @"\\Server\Bar");
        }

        [WindowsFact]
        public void Should_Include_Files_In_Root_Folder_When_Using_Recursive_Wildcard()
        {
            // Given
            var fixture = GlobberFixture.Windows();

            // When
            var result = fixture.Match(@"\\Foo\**\Bar.baz");

            // Then
            Assert.Single(result);
            AssertEx.ContainsFilePath(result, @"\\Foo\Bar.baz");
        }

        [WindowsFact]
        public void Should_Include_Folder_In_Root_Folder_When_Using_Recursive_Wildcard()
        {
            // Given
            var fixture = GlobberFixture.Windows();

            // When
            var result = fixture.Match(@"\\Foo\**\Bar");

            // Then
            Assert.Single(result);
            AssertEx.ContainsDirectoryPath(result, @"\\Foo\Bar");
        }

        [WindowsFact]
        public void Should_Parse_Glob_Expressions_With_Parenthesis_In_Them()
        {
            // Given
            var fixture = GlobberFixture.Windows();

            // When
            var result = fixture.Match(@"\\Foo (Bar)\Baz.*");

            // Then
            Assert.Single(result);
            AssertEx.ContainsFilePath(result, @"\\Foo (Bar)\Baz.c");
        }

        [WindowsFact]
        public void Should_Parse_Glob_Expressions_With_AtSign_In_Them()
        {
            // Given
            var fixture = GlobberFixture.Windows();

            // When
            var result = fixture.Match(@"\\Foo@Bar\Baz.*");

            // Then
            Assert.Single(result);
            AssertEx.ContainsFilePath(result, @"\\Foo@Bar\Baz.c");
        }

        [WindowsFact]
        public void Should_Parse_Glob_Expressions_With_Relative_Directory_Not_At_The_Beginning()
        {
            // Given
            var fixture = GlobberFixture.Windows();

            // When
            var result = fixture.Match(@"\\Server\.\*.Test.dll");

            // Then
            Assert.Equal(2, result.Length);
            AssertEx.ContainsFilePath(result, @"\\Server\Foo.Bar.Test.dll");
            AssertEx.ContainsFilePath(result, @"\\Server\Bar.Qux.Test.dll");
        }

        [WindowsFact]
        public void Should_Parse_Glob_Expressions_With_Unicode_Characters_And_Ending_With_Identifier()
        {
            // Given
            var fixture = GlobberFixture.Windows();

            // When
            var result = fixture.Match(@"\\嵌套\**\文件.延期");

            // Then
            Assert.Single(result);
            AssertEx.ContainsFilePath(result, @"\\嵌套\目录\文件.延期");
        }

        [WindowsFact]
        public void Should_Parse_Glob_Expressions_With_Unicode_Characters_And_Not_Ending_With_Identifier()
        {
            // Given
            var fixture = GlobberFixture.Windows();

            // When
            var result = fixture.Match(@"\\嵌套\**\文件.*");

            // Then
            Assert.Single(result);
            AssertEx.ContainsFilePath(result, @"\\嵌套\目录\文件.延期");
        }
    }
}
