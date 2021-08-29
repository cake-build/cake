// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.IO;
using Xunit;

namespace Cake.Core.Tests.Unit.IO
{
    public sealed class FilePathConverterTests
    {
        public sealed class TheCanConvertFromMethod
        {
            [Fact]
            public void Should_Return_True_When_Source_Type_Is_String()
            {
                var converter = new FilePathConverter();

                var result = converter.CanConvertFrom(typeof(string));

                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_When_Source_Type_Is_Not_String()
            {
                var converter = new FilePathConverter();

                var result = converter.CanConvertFrom(typeof(DateTime));

                Assert.False(result);
            }
        }

        public sealed class TheConvertFromMethod
        {
            [Fact]
            public void Should_Convert_String_Value_To_File_Path()
            {
                var converter = new FilePathConverter();

                var result = converter.ConvertFrom("c:/data/work/file.txt");

                Assert.IsType<FilePath>(result);
                Assert.Equal("c:/data/work/file.txt", ((FilePath)result).FullPath);
            }

            [Fact]
            public void Should_Throw_NotSupportedException_When_Value_Is_Not_A_Valid_File_Path()
            {
                var converter = new FilePathConverter();

                var result = Record.Exception(() => converter.ConvertFrom(DateTime.Now));

                Assert.IsType<NotSupportedException>(result);
            }
        }

        public sealed class TheCanConvertToMethod
        {
            [Fact]
            public void Should_Return_True_When_Destination_Type_Is_String()
            {
                var converter = new FilePathConverter();

                var result = converter.CanConvertTo(typeof(string));

                Assert.True(result);
            }

            [Fact]
            public void Should_Return_True_When_Destination_Type_Is_FilePath()
            {
                var converter = new FilePathConverter();

                var result = converter.CanConvertTo(typeof(FilePath));

                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_When_Source_Type_Is_Not_FilePath()
            {
                var converter = new FilePathConverter();

                var result = converter.CanConvertTo(typeof(DateTime));

                Assert.False(result);
            }
        }

        public sealed class TheConvertToMethod
        {
            [Fact]
            public void Should_Convert_File_Path_To_String_Value_Using_FullPath()
            {
                var converter = new FilePathConverter();

                var result = converter.ConvertTo(FilePath.FromString("c:/data/work/file.txt"), typeof(string));

                Assert.IsType<string>(result);
                Assert.Equal("c:/data/work/file.txt", result);
            }

            [Fact]
            public void Should_Throw_NotSupportedException_When_Destination_Type_Is_Not_String()
            {
                var converter = new FilePathConverter();

                var result = Record.Exception(() =>
                    converter.ConvertTo(FilePath.FromString("c:/data/work/file.txt"), typeof(DateTime)));

                Assert.IsType<NotSupportedException>(result);
            }
        }
    }
}