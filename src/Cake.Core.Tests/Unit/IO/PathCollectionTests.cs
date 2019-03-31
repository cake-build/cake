// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Core.IO;
using Xunit;

namespace Cake.Core.Tests.Unit.IO
{
    public sealed class PathCollectionTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Use_PathComparer_Default_If_Comparer_Is_Null()
            {
                // Given
                var collection = new PathCollection();

                // Then
                Assert.Equal(PathComparer.Default, collection.Comparer);
            }
        }

        public sealed class TheCountProperty
        {
            [Fact]
            public void Should_Return_The_Number_Of_Paths_In_The_Collection()
            {
                // Given
                var collection = new PathCollection(
                    new Path[] { new DirectoryPath("A"), new FilePath("A.txt"), new DirectoryPath("B"), new FilePath("B.txt") },
                    new PathComparer(false));

                // When, Then
                Assert.Equal(4, collection.Count);
            }
        }

        public sealed class TheAddMethod
        {
            public sealed class WithSinglePath
            {
                [Fact]
                public void Should_Add_DirectoryPath_If_Not_Already_Present()
                {
                    // Given
                    var collection = new PathCollection(new DirectoryPath[] { "A" }, new PathComparer(false));

                    // When
                    collection.Add(new DirectoryPath("B"));

                    // Then
                    Assert.Equal(2, collection.Count);
                }

                [Fact]
                public void Should_Add_FilePath_If_Not_Already_Present()
                {
                    // Given
                    var collection = new PathCollection(new FilePath[] { "A.txt" }, new PathComparer(false));

                    // When
                    collection.Add(new FilePath("B.txt"));

                    // Then
                    Assert.Equal(2, collection.Count);
                }

                [Theory]
                [InlineData(true, 2)]
                [InlineData(false, 1)]
                public void Should_Respect_File_System_Case_Sensitivity_When_Adding_DirectoryPath(bool caseSensitive, int expectedCount)
                {
                    // Given
                    var collection = new PathCollection(new DirectoryPath[] { "A" }, new PathComparer(caseSensitive));

                    // When
                    collection.Add(new DirectoryPath("a"));

                    // Then
                    Assert.Equal(expectedCount, collection.Count);
                }

                [Theory]
                [InlineData(true, 2)]
                [InlineData(false, 1)]
                public void Should_Respect_File_System_Case_Sensitivity_When_Adding_FilePath(bool caseSensitive, int expectedCount)
                {
                    // Given
                    var collection = new PathCollection(new FilePath[] { "A.TXT" }, new PathComparer(caseSensitive));

                    // When
                    collection.Add(new FilePath("a.txt"));

                    // Then
                    Assert.Equal(expectedCount, collection.Count);
                }
            }

            public sealed class WithMultiplePaths
            {
                [Fact]
                public void Should_Add_DirectoryPaths_That_Are_Not_Present()
                {
                    // Given
                    var collection = new PathCollection(new DirectoryPath[] { "A", "B" }, new PathComparer(false));

                    // When
                    collection.Add(new DirectoryPath[] { "A", "B", "C" });

                    // Then
                    Assert.Equal(3, collection.Count);
                }

                [Fact]
                public void Should_Add_FilePaths_That_Are_Not_Present()
                {
                    // Given
                    var collection = new PathCollection(new FilePath[] { "A.txt", "B.txt" }, new PathComparer(false));

                    // When
                    collection.Add(new FilePath[] { "A.txt", "B.txt", "C.txt" });

                    // Then
                    Assert.Equal(3, collection.Count);
                }

                [Theory]
                [InlineData(true, 5)]
                [InlineData(false, 3)]
                public void Should_Respect_File_System_Case_Sensitivity_When_Adding_DirectoryPaths(bool caseSensitive, int expectedCount)
                {
                    // Given
                    var collection = new PathCollection(new DirectoryPath[] { "A", "B" }, new PathComparer(caseSensitive));

                    // When
                    collection.Add(new DirectoryPath[] { "a", "b", "c" });

                    // Then
                    Assert.Equal(expectedCount, collection.Count);
                }

                [Theory]
                [InlineData(true, 5)]
                [InlineData(false, 3)]
                public void Should_Respect_File_System_Case_Sensitivity_When_Adding_FilePaths(bool caseSensitive, int expectedCount)
                {
                    // Given
                    var collection = new PathCollection(new FilePath[] { "A.TXT", "B.TXT" }, new PathComparer(caseSensitive));

                    // When
                    collection.Add(new FilePath[] { "a.txt", "b.txt", "c.txt" });

                    // Then
                    Assert.Equal(expectedCount, collection.Count);
                }

                [Fact]
                public void Should_Throw_If_Paths_Is_Null()
                {
                    // Given
                    var collection = new PathCollection();

                    // When
                    var result = Record.Exception(() => collection.Add((IEnumerable<FilePath>)null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "paths");
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
                public void Should_Respect_File_System_Case_Sensitivity_When_Removing_DirectoryPath(bool caseSensitive, int expectedCount)
                {
                    // Given
                    var collection = new PathCollection(new DirectoryPath[] { "A" }, new PathComparer(caseSensitive));

                    // When
                    collection.Remove(new DirectoryPath("a"));

                    // Then
                    Assert.Equal(expectedCount, collection.Count);
                }

                [Theory]
                [InlineData(true, 1)]
                [InlineData(false, 0)]
                public void Should_Respect_File_System_Case_Sensitivity_When_Removing_FilePath(bool caseSensitive, int expectedCount)
                {
                    // Given
                    var collection = new PathCollection(new FilePath[] { "A.TXT" }, new PathComparer(caseSensitive));

                    // When
                    collection.Remove(new FilePath("a.txt"));

                    // Then
                    Assert.Equal(expectedCount, collection.Count);
                }
            }

            public sealed class WithMultiplePaths
            {
                [Theory]
                [InlineData(true, 2)]
                [InlineData(false, 0)]
                public void Should_Respect_File_System_Case_Sensitivity_When_Removing_DirectoryPaths(bool caseSensitive, int expectedCount)
                {
                    // Given
                    var collection = new PathCollection(new DirectoryPath[] { "A", "B" }, new PathComparer(caseSensitive));

                    // When
                    collection.Remove(new DirectoryPath[] { "a", "b", "c" });

                    // Then
                    Assert.Equal(expectedCount, collection.Count);
                }

                [Theory]
                [InlineData(true, 2)]
                [InlineData(false, 0)]
                public void Should_Respect_File_System_Case_Sensitivity_When_Removing_FilePaths(bool caseSensitive, int expectedCount)
                {
                    // Given
                    var collection = new PathCollection(new FilePath[] { "A.TXT", "B.TXT" }, new PathComparer(caseSensitive));

                    // When
                    collection.Remove(new FilePath[] { "a.txt", "b.txt", "c.txt" });

                    // Then
                    Assert.Equal(expectedCount, collection.Count);
                }

                [Fact]
                public void Should_Throw_If_Paths_Is_Null()
                {
                    // Given
                    var collection = new PathCollection();

                    // When
                    var result = Record.Exception(() => collection.Remove((IEnumerable<FilePath>)null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "paths");
                }
            }
        }

        public sealed class ThePlusOperator
        {
            public sealed class WithSinglePath
            {
                [Theory]
                [InlineData(true, 2)]
                [InlineData(false, 1)]
                public void Should_Respect_File_System_Case_Sensitivity_When_Adding_DirectoryPath(bool caseSensitive, int expectedCount)
                {
                    // Given
                    var collection = new PathCollection(new DirectoryPath[] { "A" }, new PathComparer(caseSensitive));

                    // When
                    var result = collection + new DirectoryPath("a");

                    // Then
                    Assert.Equal(expectedCount, result.Count);
                }

                [Theory]
                [InlineData(true, 2)]
                [InlineData(false, 1)]
                public void Should_Respect_File_System_Case_Sensitivity_When_Adding_FilePath(bool caseSensitive, int expectedCount)
                {
                    // Given
                    var collection = new PathCollection(new FilePath[] { "A.TXT" }, new PathComparer(caseSensitive));

                    // When
                    var result = collection + new FilePath("a.txt");

                    // Then
                    Assert.Equal(expectedCount, result.Count);
                }

                [Fact]
                public void Should_Return_New_Collection_When_Adding_DirectoryPath()
                {
                    // Given
                    var collection = new PathCollection(new DirectoryPath[] { "A" }, new PathComparer(false));

                    // When
                    var result = collection + new DirectoryPath("B");

                    // Then
                    Assert.False(ReferenceEquals(result, collection));
                }

                [Fact]
                public void Should_Return_New_Collection_When_Adding_FilePath()
                {
                    // Given
                    var collection = new PathCollection(new FilePath[] { "A.txt" }, new PathComparer(false));

                    // When
                    var result = collection + new FilePath("B.txt");

                    // Then
                    Assert.False(ReferenceEquals(result, collection));
                }

                [Fact]
                public void Should_Throw_If_Collection_Is_Null()
                {
                    // Given, When
                    var result = Record.Exception(() => (PathCollection)null + new FilePath("A.txt"));

                    // Then
                    AssertEx.IsArgumentNullException(result, "collection");
                }
            }

            public sealed class WithMultiplePaths
            {
                [Theory]
                [InlineData(true, 5)]
                [InlineData(false, 3)]
                public void Should_Respect_File_System_Case_Sensitivity_When_Adding_DirectoryPaths(bool caseSensitive, int expectedCount)
                {
                    // Given
                    var collection = new PathCollection(new DirectoryPath[] { "A", "B" }, new PathComparer(caseSensitive));

                    // When
                    var result = collection + new DirectoryPath[] { "a", "b", "c" };

                    // Then
                    Assert.Equal(expectedCount, result.Count);
                }

                [Theory]
                [InlineData(true, 5)]
                [InlineData(false, 3)]
                public void Should_Respect_File_System_Case_Sensitivity_When_Adding_FilePaths(bool caseSensitive, int expectedCount)
                {
                    // Given
                    var collection = new PathCollection(new FilePath[] { "A.TXT", "B.TXT" }, new PathComparer(caseSensitive));

                    // When
                    var result = collection + new FilePath[] { "a.txt", "b.txt", "c.txt" };

                    // Then
                    Assert.Equal(expectedCount, result.Count);
                }

                [Fact]
                public void Should_Return_New_Collection_When_Adding_DirectoryPaths()
                {
                    // Given
                    var collection = new PathCollection(new DirectoryPath[] { "A", "B" }, new PathComparer(false));

                    // When
                    var result = collection + new DirectoryPath[] { "C", "D" };

                    // Then
                    Assert.False(ReferenceEquals(result, collection));
                }

                [Fact]
                public void Should_Return_New_Collection_When_Adding_FilePaths()
                {
                    // Given
                    var collection = new PathCollection(new FilePath[] { "A.txt", "B.txt" }, new PathComparer(false));

                    // When
                    var result = collection + new FilePath[] { "C.txt", "D.txt" };

                    // Then
                    Assert.False(ReferenceEquals(result, collection));
                }

                [Fact]
                public void Should_Throw_If_Collection_Is_Null()
                {
                    // Given, When
                    var result = Record.Exception(() => (PathCollection)null + new FilePath[] { "A.txt" });

                    // Then
                    AssertEx.IsArgumentNullException(result, "collection");
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
                public void Should_Respect_File_System_Case_Sensitivity_When_Removing_DirectoryPath(bool caseSensitive, int expectedCount)
                {
                    // Given
                    var collection = new PathCollection(new DirectoryPath[] { "A", "B" }, new PathComparer(caseSensitive));

                    // When
                    var result = collection - new DirectoryPath("a");

                    // Then
                    Assert.Equal(expectedCount, result.Count);
                }

                [Theory]
                [InlineData(true, 2)]
                [InlineData(false, 1)]
                public void Should_Respect_File_System_Case_Sensitivity_When_Removing_FilePath(bool caseSensitive, int expectedCount)
                {
                    // Given
                    var collection = new PathCollection(new FilePath[] { "A.TXT", "B.TXT" }, new PathComparer(caseSensitive));

                    // When
                    var result = collection - new FilePath("a.txt");

                    // Then
                    Assert.Equal(expectedCount, result.Count);
                }

                [Fact]
                public void Should_Return_New_Collection_When_Removing_DirectoryPathPath()
                {
                    // Given
                    var collection = new PathCollection(new DirectoryPath[] { "A", "B" }, new PathComparer(false));

                    // When
                    var result = collection - new DirectoryPath("A");

                    // Then
                    Assert.False(ReferenceEquals(result, collection));
                }

                [Fact]
                public void Should_Return_New_Collection_When_Removing_FilePath()
                {
                    // Given
                    var collection = new PathCollection(new FilePath[] { "A.txt", "B.txt" }, new PathComparer(false));

                    // When
                    var result = collection - new FilePath("A.txt");

                    // Then
                    Assert.False(ReferenceEquals(result, collection));
                }

                [Fact]
                public void Should_Throw_If_Collection_Is_Null()
                {
                    // Given, When
                    var result = Record.Exception(() => (PathCollection)null - new FilePath("A.txt"));

                    // Then
                    AssertEx.IsArgumentNullException(result, "collection");
                }
            }

            public sealed class WithMultiplePaths
            {
                [Theory]
                [InlineData(true, 3)]
                [InlineData(false, 1)]
                public void Should_Respect_File_System_Case_Sensitivity_When_Removing_DirectoryPaths(bool caseSensitive, int expectedCount)
                {
                    // Given
                    var collection = new PathCollection(new DirectoryPath[] { "A", "B", "C" }, new PathComparer(caseSensitive));

                    // When
                    var result = collection - new DirectoryPath[] { "b", "c" };

                    // Then
                    Assert.Equal(expectedCount, result.Count);
                }

                [Theory]
                [InlineData(true, 3)]
                [InlineData(false, 1)]
                public void Should_Respect_File_System_Case_Sensitivity_When_Removing_FilePaths(bool caseSensitive, int expectedCount)
                {
                    // Given
                    var collection = new PathCollection(new FilePath[] { "A.TXT", "B.TXT", "C.TXT" }, new PathComparer(caseSensitive));

                    // When
                    var result = collection - new FilePath[] { "b.txt", "c.txt" };

                    // Then
                    Assert.Equal(expectedCount, result.Count);
                }

                [Fact]
                public void Should_Return_New_Collection_When_Removing_DirectoryPaths()
                {
                    // Given
                    var collection = new PathCollection(new DirectoryPath[] { "A", "B", "C" }, new PathComparer(false));

                    // When
                    var result = collection - new DirectoryPath[] { "B", "C" };

                    // Then
                    Assert.False(ReferenceEquals(result, collection));
                }

                [Fact]
                public void Should_Return_New_Collection_When_Removing_FilePaths()
                {
                    // Given
                    var collection = new PathCollection(new FilePath[] { "A.txt", "B.txt", "C.txt" }, new PathComparer(false));

                    // When
                    var result = collection - new FilePath[] { "B.txt", "C.txt" };

                    // Then
                    Assert.False(ReferenceEquals(result, collection));
                }

                [Fact]
                public void Should_Throw_If_Collection_Is_Null()
                {
                    // Given, When
                    var result = Record.Exception(() => (PathCollection)null - new FilePath[] { "A.txt" });

                    // Then
                    AssertEx.IsArgumentNullException(result, "collection");
                }
            }
        }
    }
}