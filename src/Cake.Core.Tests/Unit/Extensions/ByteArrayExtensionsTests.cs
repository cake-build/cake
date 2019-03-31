// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace Cake.Core.Tests.Unit.Extensions
{
    public class ByteArrayExtensionsTests
    {
        public class TheStartsWithMethod
        {
            [Fact]
            public void Should_Throw_When_Value_Is_Null()
            {
                var result = Record.Exception(() => ByteArrayExtensions.StartsWith(null, new byte[] { }));

                AssertEx.IsArgumentNullException(result, "value");
            }

            [Fact]
            public void Should_Throw_When_Prefix_Is_Null()
            {
                var value = new byte[] { 0x6E, 0x75, 0x6C, 0x6C };

                var result = Record.Exception(() => value.StartsWith(null));

                AssertEx.IsArgumentNullException(result, "prefix");
            }

            [Fact]
            public void Should_Return_False_When_Value_Is_Shorter_Than_Prefix()
            {
                var value = new byte[] { 0xEF, 0xBB };
                var prefix = new byte[] { 0xEF, 0xBB, 0xBF };

                var result = value.StartsWith(prefix);

                Assert.False(result);
            }

            [Fact]
            public void Should_Return_False_When_Value_Does_Not_Start_With_Prefix()
            {
                var value = new byte[] { 0x45, 0x61, 0x74 };
                var prefix = new byte[] { 0xEF, 0xBB, 0xBF };

                var result = value.StartsWith(prefix);

                Assert.False(result);
            }

            [Fact]
            public void Should_Return_True_When_Value_Starts_With_Prefix()
            {
                var value = new byte[] { 0xEF, 0xBB, 0xBF, 0x43, 0x61, 0x6B, 0x65 };
                var prefix = new byte[] { 0xEF, 0xBB, 0xBF };

                var result = value.StartsWith(prefix);

                Assert.True(result);
            }
        }
    }
}