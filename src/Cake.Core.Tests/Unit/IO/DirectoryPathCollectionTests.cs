// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Cake.Core.IO;
using Xunit;

namespace Cake.Core.Tests.Unit.IO
{
    public sealed class DirectoryPathCollectionTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Comparer_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new DirectoryPathCollection(Enumerable.Empty<DirectoryPath>(), null));

                // Then
                AssertEx.IsArgumentNullException(result, "comparer");
            }
        }

        public sealed class TheCountProperty
        {
            [Fact]
            public void Should_Return_The_Number_Of_Paths_In_The_Collection()
            {
                // Given
                var collection = new DirectoryPathCollection(
                    new[] { new DirectoryPath("A.txt"), new DirectoryPath("B.txt") },
                    new PathComparer(false));

                // When, Then
                Assert.Equal(2, collection.Count);
            }
        }

        public sealed class TheAddMethod
        {
            public sealed class WithSinglePath
            {
                [Fact]
                public void Should_Add_Path_If_Not_Already_Present()
                {
                    // Given
                    var collection = new DirectoryPathCollection(new PathComparer(false));
                    collection.Add(new DirectoryPath("B"));

                    // When
                    collection.Add(new DirectoryPath("A"));

                    // Then
                    Assert.Equal(2, collection.Count);
                }

                [Theory]
                [InlineData(true, 2)]
                [InlineData(false, 1)]
                public void Should_Respect_File_System_Case_Sensitivity_When_Adding_Path(bool caseSensitive, int expectedCount)
                {
                    // Given
                    var collection = new DirectoryPathCollection(new PathComparer(caseSensitive));
                    collection.Add(new DirectoryPath("A"));

                    // When
                    collection.Add(new DirectoryPath("a"));

                    // Then
                    Assert.Equal(expectedCount, collection.Count);
                }
            }

            public sealed class WithMultiplePaths
            {
                [Fact]
                public void Should_Add_Paths_That_Are_Not_Present()
                {
                    // Given
                    var collection = new DirectoryPathCollection(new DirectoryPath[] { "A", "B" }, new PathComparer(false));

                    // When
                    collection.Add(new DirectoryPath[] { "A", "B", "C" });

                    // Then
                    Assert.Equal(3, collection.Count);
                }

                [Theory]
                [InlineData(true, 5)]
                [InlineData(false, 3)]
                public void Should_Respect_File_System_Case_Sensitivity_When_Adding_Paths(bool caseSensitive, int expectedCount)
                {
                    // Given
                    var collection = new DirectoryPathCollection(new DirectoryPath[] { "A", "B" }, new PathComparer(caseSensitive));

                    // When
                    collection.Add(new DirectoryPath[] { "a", "b", "c" });

                    // Then
                    Assert.Equal(expectedCount, collection.Count);
                }
            }
        }

        public sealed class TheRemoveMethod
        {
            public sealed class WithSinglePath
            {
                [Theory]
                [InlineData(true, 1)]
                [InlineData(false, 0)]
                public void Should_Respect_File_System_Case_Sensitivity_When_Removing_Path(bool caseSensitive, int expectedCount)
                {
                    // Given
                    var collection = new DirectoryPathCollection(new PathComparer(caseSensitive));
                    collection.Add(new DirectoryPath("A"));

                    // When
                    collection.Remove(new DirectoryPath("a"));

                    // Then
                    Assert.Equal(expectedCount, collection.Count);
                }
            }

            public sealed class WithMultiplePaths
            {
                [Theory]
                [InlineData(true, 2)]
                [InlineData(false, 0)]
                public void Should_Respect_File_System_Case_Sensitivity_When_Removing_Paths(bool caseSensitive, int expectedCount)
                {
                    // Given
                    var collection = new DirectoryPathCollection(new DirectoryPath[] { "A", "B" }, new PathComparer(caseSensitive));

                    // When
                    collection.Remove(new DirectoryPath[] { "a", "b", "c" });

                    // Then
                    Assert.Equal(expectedCount, collection.Count);
                }
            }
        }

        public sealed class ThePlusOperator
        {
            public sealed class WithSinglePath
            {
                [Fact]
                public void Should_Respect_File_System_Case_Sensitivity_When_Adding_Path()
                {
                    // Given
                    var collection = new DirectoryPathCollection(new PathComparer(false));
                    collection.Add("B");

                    // When
                    var result = collection + new DirectoryPath("A");

                    // Then
                    Assert.Equal(2, result.Count);
                }

                [Fact]
                public void Should_Return_New_Collection()
                {
                    // Given
                    var collection = new DirectoryPathCollection(new PathComparer(false));

                    // When
                    var result = collection + new DirectoryPath("A");

                    // Then
                    Assert.False(ReferenceEquals(result, collection));
                }
            }

            public sealed class WithMultiplePaths
            {
                [Fact]
                public void Should_Respect_File_System_Case_Sensitivity_When_Adding_Paths()
                {
                    // Given
                    var comparer = new PathComparer(false);
                    var collection = new DirectoryPathCollection(comparer);
                    var second = new DirectoryPathCollection(new DirectoryPath[] { "A", "B" }, comparer);

                    // When
                    var result = collection + second;

                    // Then
                    Assert.Equal(2, result.Count);
                }

                [Fact]
                public void Should_Return_New_Collection()
                {
                    // Given
                    var comparer = new PathComparer(false);
                    var collection = new DirectoryPathCollection(comparer);
                    var second = new DirectoryPathCollection(new DirectoryPath[] { "A", "B" }, comparer);

                    // When
                    var result = collection + second;

                    // Then
                    Assert.False(ReferenceEquals(result, collection));
                }
            }
        }

        public sealed class TheMinusOperator
        {
            public sealed class WithSinglePath
            {
                [Theory]
                [InlineData(true, 2)]
                [InlineData(false, 1)]
                public void Should_Respect_File_System_Case_Sensitivity_When_Removing_Paths(bool caseSensitive, int expectedCount)
                {
                    // Given
                    var comparer = new PathComparer(caseSensitive);
                    var collection = new DirectoryPathCollection(comparer);
                    collection.Add("A");
                    collection.Add("B");

                    // When
                    var result = collection - new DirectoryPath("a");

                    // Then
                    Assert.Equal(expectedCount, result.Count);
                }

                [Fact]
                public void Should_Return_New_Collection()
                {
                    // Given
                    var collection = new DirectoryPathCollection(new PathComparer(false));
                    collection.Add("A");
                    collection.Add("B");

                    // When
                    var result = collection - new DirectoryPath("A");

                    // Then
                    Assert.False(ReferenceEquals(result, collection));
                }
            }

            public sealed class WithMultiplePaths
            {
                [Theory]
                [InlineData(true, 3)]
                [InlineData(false, 1)]
                public void Should_Respect_File_System_Case_Sensitivity_When_Removing_Paths(bool caseSensitive, int expectedCount)
                {
                    // Given
                    var collection = new DirectoryPathCollection(new PathComparer(caseSensitive));
                    collection.Add("A");
                    collection.Add("B");
                    collection.Add("C");

                    // When
                    var result = collection - new[] { new DirectoryPath("b"), new DirectoryPath("c") };

                    // Then
                    Assert.Equal(expectedCount, result.Count);
                }

                [Fact]
                public void Should_Return_New_Collection()
                {
                    // Given
                    var collection = new DirectoryPathCollection(new PathComparer(false));
                    collection.Add("A");
                    collection.Add("B");
                    collection.Add("C");

                    // When
                    var result = collection - new[] { new DirectoryPath("B"), new DirectoryPath("C") };

                    // Then
                    Assert.False(ReferenceEquals(result, collection));
                }
            }
        }
    }
}