// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.IO;
using Xunit;

namespace Cake.Core.Tests.Unit.IO
{
    public sealed class DirectoryPathConverterTests
    {
        public sealed class TheCanConvertFromMethod
        {
            [Fact]
            public void Should_Return_True_When_Source_Type_Is_String()
            {
                var converter = new DirectoryPathConverter();

                var result = converter.CanConvertFrom(typeof(string));

                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_When_Source_Type_Is_Not_String()
            {
                var converter = new DirectoryPathConverter();

                var result = converter.CanConvertFrom(typeof(DateTime));

                Assert.False(result);
            }
        }

        public sealed class TheConvertFromMethod
        {
            [Fact]
            public void Should_Convert_String_Value_To_Directory_Path()
            {
                var converter = new DirectoryPathConverter();

                var result = converter.ConvertFrom("c:/data/work");

                Assert.IsType<DirectoryPath>(result);
                Assert.Equal("c:/data/work", ((DirectoryPath)result).FullPath);
            }

            [Fact]
            public void Should_Throw_NotSupportedException_When_Value_Is_Not_A_Valid_Directory_Path()
            {
                var converter = new DirectoryPathConverter();

                var result = Record.Exception(() => converter.ConvertFrom(DateTime.Now));

                Assert.IsType<NotSupportedException>(result);
            }
        }

        public sealed class TheCanConvertToMethod
        {
            [Fact]
            public void Should_Return_True_When_Destination_Type_Is_String()
            {
                var converter = new DirectoryPathConverter();

                var result = converter.CanConvertTo(typeof(string));

                Assert.True(result);
            }

            [Fact]
            public void Should_Return_True_When_Destination_Type_Is_DirectoryPath()
            {
                var converter = new DirectoryPathConverter();

                var result = converter.CanConvertTo(typeof(DirectoryPath));

                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_When_Source_Type_Is_Not_DirectoryPath()
            {
                var converter = new DirectoryPathConverter();

                var result = converter.CanConvertTo(typeof(DateTime));

                Assert.False(result);
            }
        }

        public sealed class TheConvertToMethod
        {
            [Fact]
            public void Should_Convert_Directory_Path_To_String_Value_Using_FullPath()
            {
                var converter = new DirectoryPathConverter();

                var result = converter.ConvertTo(DirectoryPath.FromString("c:/data/work"), typeof(string));

                Assert.IsType<string>(result);
                Assert.Equal("c:/data/work", result);
            }

            [Fact]
            public void Should_Throw_NotSupportedException_When_Destination_Type_Is_Not_String()
            {
                var converter = new DirectoryPathConverter();

                var result = Record.Exception(() =>
                    converter.ConvertTo(DirectoryPath.FromString("c:/data/work"), typeof(DateTime)));

                Assert.IsType<NotSupportedException>(result);
            }
        }
    }
}