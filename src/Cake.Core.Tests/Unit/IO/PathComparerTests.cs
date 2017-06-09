// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;
using Cake.Core.Polyfill;
using Xunit;

namespace Cake.Core.Tests.Unit.IO
{
    public sealed class PathComparerTests
    {
        public sealed class TheEqualsMethod
        {
            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Same_Asset_Instances_Is_Considered_Equal(bool isCaseSensitive)
            {
                // Given, When
                var comparer = new PathComparer(isCaseSensitive);
                var path = new FilePath("shaders/basic.vert");

                // Then
                Assert.True(comparer.Equals(path, path));
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Two_Null_Paths_Are_Considered_Equal(bool isCaseSensitive)
            {
                // Given
                var comparer = new PathComparer(isCaseSensitive);

                // When
                var result = comparer.Equals(null, null);

                // Then
                Assert.True(result);
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Paths_Are_Considered_Inequal_If_Any_Is_Null(bool isCaseSensitive)
            {
                // Given
                var comparer = new PathComparer(isCaseSensitive);

                // When
                var result = comparer.Equals(null, new FilePath("test.txt"));

                // Then
                Assert.False(result);
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Same_Paths_Are_Considered_Equal(bool isCaseSensitive)
            {
                // Given, When
                var comparer = new PathComparer(isCaseSensitive);
                var first = new FilePath("shaders/basic.vert");
                var second = new FilePath("shaders/basic.vert");

                // Then
                Assert.True(comparer.Equals(first, second));
                Assert.True(comparer.Equals(second, first));
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Different_Paths_Are_Not_Considered_Equal(bool isCaseSensitive)
            {
                // Given, When
                var comparer = new PathComparer(isCaseSensitive);
                var first = new FilePath("shaders/basic.vert");
                var second = new FilePath("shaders/basic.frag");

                // Then
                Assert.False(comparer.Equals(first, second));
                Assert.False(comparer.Equals(second, first));
            }

            [Theory]
            [InlineData(true, false)]
            [InlineData(false, true)]
            public void Same_Paths_But_Different_Casing_Are_Considered_Equal_Depending_On_Case_Sensitivity(bool isCaseSensitive, bool expected)
            {
                // Given, When
                var comparer = new PathComparer(isCaseSensitive);
                var first = new FilePath("shaders/basic.vert");
                var second = new FilePath("SHADERS/BASIC.VERT");

                // Then
                Assert.Equal(expected, comparer.Equals(first, second));
                Assert.Equal(expected, comparer.Equals(second, first));
            }
        }

        public sealed class TheGetHashCodeMethod
        {
            [Fact]
            public void Should_Throw_If_Other_Path_Is_Null()
            {
                // Given
                var comparer = new PathComparer(true);

                // When
                var result = Record.Exception(() => comparer.GetHashCode(null));

                // Then
                AssertEx.IsArgumentNullException(result, "obj");
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Same_Paths_Get_Same_Hash_Code(bool isCaseSensitive)
            {
                // Given, When
                var comparer = new PathComparer(isCaseSensitive);
                var first = new FilePath("shaders/basic.vert");
                var second = new FilePath("shaders/basic.vert");

                // Then
                Assert.Equal(comparer.GetHashCode(first), comparer.GetHashCode(second));
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Different_Paths_Get_Different_Hash_Codes(bool isCaseSensitive)
            {
                // Given, When
                var comparer = new PathComparer(isCaseSensitive);
                var first = new FilePath("shaders/basic.vert");
                var second = new FilePath("shaders/basic.frag");

                // Then
                Assert.NotEqual(comparer.GetHashCode(first), comparer.GetHashCode(second));
            }

            [Theory]
            [InlineData(true, false)]
            [InlineData(false, true)]
            public void Same_Paths_But_Different_Casing_Get_Same_Hash_Code_Depending_On_Case_Sensitivity(bool isCaseSensitive, bool expected)
            {
                // Given, When
                var comparer = new PathComparer(isCaseSensitive);
                var first = new FilePath("shaders/basic.vert");
                var second = new FilePath("SHADERS/BASIC.VERT");

                // Then
                Assert.Equal(expected, comparer.GetHashCode(first) == comparer.GetHashCode(second));
            }
        }

        public sealed class TheDefaultProperty
        {
            [Fact]
            public void Should_Return_Correct_Comparer_Depending_On_Operative_System()
            {
                // Given
                var expected = EnvironmentHelper.IsUnix();

                // When
                var instance = PathComparer.Default;

                // Then
                Assert.Equal(expected, instance.IsCaseSensitive);
            }
        }

        public sealed class TheIsCaseSensitiveProperty
        {
            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Should_Return_Whether_Or_Not_The_Comparer_Is_Case_Sensitive(bool isCaseSensitive)
            {
                // Given, When
                var comparer = new PathComparer(isCaseSensitive);

                // Then
                Assert.Equal(isCaseSensitive, comparer.IsCaseSensitive);
            }
        }
    }
}