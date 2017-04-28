// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Xunit;

namespace Cake.Core.Tests.Unit
{
    public sealed class CakeArgumentChecksTests
    {
        public sealed class TheNotNullExtension
        {
            [Fact]
            public void Should_Throw_If_Parameter_Is_Null()
            {
                // Given
                const string value = "foo";

                // When
                var result = Record.Exception(() => value.NotNull(null));

                // Then
                Assert.IsArgumentNullException(result, "parameterName");
            }

            [Fact]
            public void Should_Throw_If_Parameter_Is_Empty()
            {
                // Given
                const string value = "foo";

                // When
                var result = Record.Exception(() => value.NotNull(string.Empty));

                // Then
                Assert.IsArgumentOutOfRangeException(result, "parameterName");
            }

            [Fact]
            public void Should_Throw_If_Parameter_Is_WhiteSpace()
            {
                // Given
                const string value = "foo";

                // When
                var result = Record.Exception(() => value.NotNull(" "));

                // Then
                Assert.IsArgumentOutOfRangeException(result, "parameterName");
            }

            [Fact]
            public void Should_Throw_If_Value_Is_Null()
            {
                // Given
                const string value = null;

                // When
                var result = Record.Exception(() => value.NotNull("foo"));

                // Then
                Assert.IsArgumentNullException(result, "foo");
            }

            [Theory]
            [InlineData("foo")]
            public void Should_Not_Throw_If_Value_Is_Set(object value)
            {
                value.NotNull("foo");
            }
        }

        public sealed class TheNotNullWithPropertyNameExtension
        {
            [Fact]
            public void Should_Throw_If_Parameter_Is_Null()
            {
                // Given
                const string value = "foo";

                // When
                var result = Record.Exception(() => value.NotNull(null, "foo"));

                // Then
                Assert.IsArgumentNullException(result, "parameterName");
            }

            [Fact]
            public void Should_Throw_If_Parameter_Is_Empty()
            {
                // Given
                const string value = "foo";

                // When
                var result = Record.Exception(() => value.NotNull(string.Empty, "foo"));

                // Then
                Assert.IsArgumentOutOfRangeException(result, "parameterName");
            }

            [Fact]
            public void Should_Throw_If_Parameter_Is_WhiteSpace()
            {
                // Given
                const string value = "foo";

                // When
                var result = Record.Exception(() => value.NotNull(" ", "foo"));

                // Then
                Assert.IsArgumentOutOfRangeException(result, "parameterName");
            }

            [Fact]
            public void Should_Throw_If_Property_Is_Null()
            {
                // Given
                const string value = "foo";

                // When
                var result = Record.Exception(() => value.NotNull("foo", null));

                // Then
                Assert.IsArgumentNullException(result, "propertyName");
            }

            [Fact]
            public void Should_Throw_If_Property_Is_Empty()
            {
                // Given
                const string value = "foo";

                // When
                var result = Record.Exception(() => value.NotNull("foo", string.Empty));

                // Then
                Assert.IsArgumentOutOfRangeException(result, "propertyName");
            }

            [Fact]
            public void Should_Throw_If_Property_Is_WhiteSpace()
            {
                // Given
                const string value = "foo";

                // When
                var result = Record.Exception(() => value.NotNull("foo", " "));

                // Then
                Assert.IsArgumentOutOfRangeException(result, "propertyName");
            }

            [Fact]
            public void Should_Throw_If_Value_Is_Null()
            {
                // Given
                const string value = null;

                // When
                var result = Record.Exception(() => value.NotNull("foo", "bar"));

                // Then
                Assert.IsArgumentException(result, "foo.bar", "");
            }

            [Theory]
            [InlineData("foo")]
            public void Should_Not_Throw_If_Value_Is_Set(object value)
            {
                value.NotNull("foo", "bar");
            }
        }

        public sealed class TheNotNullOrEmptyForStringsExtension
        {
            [Fact]
            public void Should_Throw_If_Parameter_Is_Null()
            {
                // Given
                const string value = "foo";

                // When
                var result = Record.Exception(() => value.NotNullOrEmpty(null));

                // Then
                Assert.IsArgumentNullException(result, "parameterName");
            }

            [Fact]
            public void Should_Throw_If_Parameter_Is_Empty()
            {
                // Given
                const string value = "foo";

                // When
                var result = Record.Exception(() => value.NotNullOrEmpty(string.Empty));

                // Then
                Assert.IsArgumentOutOfRangeException(result, "parameterName");
            }

            [Fact]
            public void Should_Throw_If_Parameter_Is_WhiteSpace()
            {
                // Given
                const string value = "foo";

                // When
                var result = Record.Exception(() => value.NotNullOrEmpty(" "));

                // Then
                Assert.IsArgumentOutOfRangeException(result, "parameterName");
            }

            [Fact]
            public void Should_Throw_If_Value_Is_Null()
            {
                // Given
                const string value = null;

                // When
                var result = Record.Exception(() => value.NotNullOrEmpty("foo"));

                // Then
                Assert.IsArgumentNullException(result, "foo");
            }

            [Fact]
            public void Should_Throw_If_Value_Is_Empty()
            {
                // Given
                var value = string.Empty;

                // When
                var result = Record.Exception(() => value.NotNullOrEmpty("foo"));

                // Then
                Assert.IsArgumentOutOfRangeException(result, "foo");
            }

            [Theory]
            [InlineData("foo")]
            public void Should_Not_Throw_If_Value_Is_Valid(string value)
            {
                value.NotNullOrEmpty("foo");
            }
        }

        public sealed class TheNotNullOrWhiteSpaceExtension
        {
            [Fact]
            public void Should_Throw_If_Parameter_Is_Null()
            {
                // Given
                const string value = "foo";

                // When
                var result = Record.Exception(() => value.NotNullOrWhiteSpace(null));

                // Then
                Assert.IsArgumentNullException(result, "parameterName");
            }

            [Fact]
            public void Should_Throw_If_Parameter_Is_Empty()
            {
                // Given
                const string value = "foo";

                // When
                var result = Record.Exception(() => value.NotNullOrWhiteSpace(string.Empty));

                // Then
                Assert.IsArgumentOutOfRangeException(result, "parameterName");
            }

            [Fact]
            public void Should_Throw_If_Parameter_Is_WhiteSpace()
            {
                // Given
                const string value = "foo";

                // When
                var result = Record.Exception(() => value.NotNullOrWhiteSpace(" "));

                // Then
                Assert.IsArgumentOutOfRangeException(result, "parameterName");
            }

            [Fact]
            public void Should_Throw_If_Value_Is_Null()
            {
                // Given
                const string value = null;

                // When
                var result = Record.Exception(() => value.NotNullOrWhiteSpace("foo"));

                // Then
                Assert.IsArgumentNullException(result, "foo");
            }

            [Fact]
            public void Should_Throw_If_Value_Is_Empty()
            {
                // Given
                var value = string.Empty;

                // When
                var result = Record.Exception(() => value.NotNullOrWhiteSpace("foo"));

                // Then
                Assert.IsArgumentOutOfRangeException(result, "foo");
            }

            [Fact]
            public void Should_Throw_If_Value_Is_WhiteSpace()
            {
                // Given
                const string value = " ";

                // When
                var result = Record.Exception(() => value.NotNullOrWhiteSpace("foo"));

                // Then
                Assert.IsArgumentOutOfRangeException(result, "foo");
            }

            [Theory]
            [InlineData("foo")]
            public void Should_Not_Throw_If_Value_Is_Valid(string value)
            {
                value.NotNullOrWhiteSpace("foo");
            }
        }

        public sealed class TheNotNegativeExtension
        {
            [Fact]
            public void Should_Throw_If_Parameter_Is_Null()
            {
                // Given
                const int value = 1;

                // When
                var result = Record.Exception(() => value.NotNegative(null));

                // Then
                Assert.IsArgumentNullException(result, "parameterName");
            }

            [Fact]
            public void Should_Throw_If_Parameter_Is_Empty()
            {
                // Given
                const int value = 1;

                // When
                var result = Record.Exception(() => value.NotNegative(string.Empty));

                // Then
                Assert.IsArgumentOutOfRangeException(result, "parameterName");
            }

            [Fact]
            public void Should_Throw_If_Parameter_Is_WhiteSpace()
            {
                // Given
                const int value = 1;

                // When
                var result = Record.Exception(() => value.NotNegative(" "));

                // Then
                Assert.IsArgumentOutOfRangeException(result, "parameterName");
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(int.MinValue)]
            public void Should_Throw_If_Value_Is_Negative(int value)
            {
                // When
                var result = Record.Exception(() => value.NotNegative("foo"));

                // Then
                Assert.IsArgumentOutOfRangeException(result, "foo");
            }

            [Theory]
            [InlineData(0)]
            [InlineData(1)]
            [InlineData(int.MaxValue)]
            public void Should_Not_Throw_If_Value_Is_Valid(int value)
            {
                value.NotNegative("foo");
            }
        }

        public sealed class TheNotNegativeOrZeroExtension
        {
            [Fact]
            public void Should_Throw_If_Parameter_Is_Null()
            {
                // Given
                const int value = 1;

                // When
                var result = Record.Exception(() => value.NotNegativeOrZero(null));

                // Then
                Assert.IsArgumentNullException(result, "parameterName");
            }

            [Fact]
            public void Should_Throw_If_Parameter_Is_Empty()
            {
                // Given
                const int value = 1;

                // When
                var result = Record.Exception(() => value.NotNegativeOrZero(string.Empty));

                // Then
                Assert.IsArgumentOutOfRangeException(result, "parameterName");
            }

            [Fact]
            public void Should_Throw_If_Parameter_Is_WhiteSpace()
            {
                // Given
                const int value = 1;

                // When
                var result = Record.Exception(() => value.NotNegativeOrZero(" "));

                // Then
                Assert.IsArgumentOutOfRangeException(result, "parameterName");
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(int.MinValue)]
            public void Should_Throw_If_Value_Is_Negative(int value)
            {
                // When
                var result = Record.Exception(() => value.NotNegativeOrZero("foo"));

                // Then
                Assert.IsArgumentOutOfRangeException(result, "foo");
            }

            [Fact]
            public void Should_Throw_If_Value_Is_Zero()
            {
                // Given
                var value = 0;

                // When
                var result = Record.Exception(() => value.NotNegativeOrZero("foo"));

                // Then
                Assert.IsArgumentOutOfRangeException(result, "foo");
            }

            [Theory]
            [InlineData(1)]
            [InlineData(int.MaxValue)]
            public void Should_Not_Throw_If_Value_Is_Valid(int value)
            {
                value.NotNegative("foo");
            }
        }

        public sealed class TheNotNullOrEmptyForEnumerablesExtension
        {
            [Fact]
            public void Should_Throw_If_Parameter_Is_Null()
            {
                // Given
                var value = new List<string> { "foo" };

                // When
                var result = Record.Exception(() => value.NotNullOrEmpty(null));

                // Then
                Assert.IsArgumentNullException(result, "parameterName");
            }

            [Fact]
            public void Should_Throw_If_Parameter_Is_Empty()
            {
                // Given
                var value = new List<string> { "foo" };

                // When
                var result = Record.Exception(() => value.NotNullOrEmpty(string.Empty));

                // Then
                Assert.IsArgumentOutOfRangeException(result, "parameterName");
            }

            [Fact]
            public void Should_Throw_If_Parameter_Is_WhiteSpace()
            {
                // Given
                var value = new List<string> { "foo" };

                // When
                var result = Record.Exception(() => value.NotNullOrEmpty(" "));

                // Then
                Assert.IsArgumentOutOfRangeException(result, "parameterName");
            }

            [Fact]
            public void Should_Throw_If_Value_Is_Null()
            {
                // Given
                const List<int> value = null;

                // When
                var result = Record.Exception(() => value.NotNullOrEmpty("foo"));

                // Then
                Assert.IsArgumentNullException(result, "foo");
            }

            [Fact]
            public void Should_Throw_If_Value_Is_Empty()
            {
                // Given
                var value = new List<int>();

                // When
                var result = Record.Exception(() => value.NotNullOrEmpty("foo"));

                // Then
                Assert.IsArgumentException(result, "foo", "");
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("foo")]
            public void Should_Not_Throw_If_Value_Is_Valid(string value)
            {
                // Given
                var values = new List<string> { value };

                // When
                values.NotNullOrEmpty("foo");

                // Then
            }
        }

        public sealed class TheContainsNoNullsExtension
        {
            [Fact]
            public void Should_Throw_If_Parameter_Is_Null()
            {
                // Given
                var value = new List<string> { "foo" };

                // When
                var result = Record.Exception(() => value.ContainsNoNulls(null));

                // Then
                Assert.IsArgumentNullException(result, "parameterName");
            }

            [Fact]
            public void Should_Throw_If_Parameter_Is_Empty()
            {
                // Given
                var value = new List<string> { "foo" };

                // When
                var result = Record.Exception(() => value.ContainsNoNulls(string.Empty));

                // Then
                Assert.IsArgumentOutOfRangeException(result, "parameterName");
            }

            [Fact]
            public void Should_Throw_If_Parameter_Is_WhiteSpace()
            {
                // Given
                var value = new List<string> { "foo" };

                // When
                var result = Record.Exception(() => value.ContainsNoNulls(" "));

                // Then
                Assert.IsArgumentOutOfRangeException(result, "parameterName");
            }

            [Fact]
            public void Should_Throw_If_Value_Is_Null()
            {
                // Given
                List<int> value = null;

                // When
                var result = Record.Exception(() => value.ContainsNoNulls("foo"));

                // Then
                Assert.IsArgumentNullException(result, "foo");
            }

            [Fact]
            public void Should_Throw_If_Value_Is_Empty()
            {
                // Given
                var value = new List<int>();

                // When
                var result = Record.Exception(() => value.ContainsNoNulls("foo"));

                // Then
                Assert.IsArgumentException(result, "foo", "");
            }

            [Fact]
            public void Should_Throw_If_Value_Contains_Null()
            {
                // Given
                var value = new List<string> { null };

                // When
                var result = Record.Exception(() => value.ContainsNoNulls("foo"));

                // Then
                Assert.IsArgumentOutOfRangeException(result, "foo");
            }

            [Theory]
            [InlineData("")]
            [InlineData("foo")]
            public void Should_Not_Throw_If_Value_Is_Valid(string value)
            {
                // Given
                var values = new List<string> { value };

                // When
                values.ContainsNoNulls("foo");

                // Then
            }
        }
    }
}
