using Cake.Core.IO;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Core.Tests.Unit.IO
{
    public sealed class PathCollapserTests
    {
        public sealed class TheCollapseMethod
        {
            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => PathCollapser.Collapse(null));

                // Then
                Assert.IsArgumentNullException(result, "path");
            }

            [Fact]
            public void Should_Collapse_Relative_Path()
            {
                // Given, When
                var path = PathCollapser.Collapse(new DirectoryPath("hello/temp/test/../../world"));

                // Then
                Assert.Equal("hello/world", path);
            }

            [Fact]
            public void Should_Collapse_Path_With_Separated_Ellipsis()
            {
                // Given, When
                var path = PathCollapser.Collapse(new DirectoryPath("hello/temp/../temp2/../world"));

                // Then
                Assert.Equal("hello/world", path);
            }
            
            [WindowsFact]
            public void Should_Collapse_Path_With_Windows_Root()
            {
                // Given, When
                var path = PathCollapser.Collapse(new DirectoryPath("c:/hello/temp/test/../../world"));

                // Then
                Assert.Equal("c:/hello/world", path);
            }

            [Fact]
            public void Should_Collapse_Path_With_Non_Windows_Root()
            {
                // Given, When
                var path = PathCollapser.Collapse(new DirectoryPath("/hello/temp/test/../../world"));

                // Then
                Assert.Equal("/hello/world", path);
            }
            
            [WindowsFact]
            public void Should_Stop_Collapsing_When_Windows_Root_Is_Reached()
            {
                // Given, When
                var path = PathCollapser.Collapse(new DirectoryPath("c:/../../../../../../temp"));

                // Then
                Assert.Equal("c:/temp", path);
            }

            [Fact]
            public void Should_Stop_Collapsing_When_Root_Is_Reached()
            {
                // Given, When
                var path = PathCollapser.Collapse(new DirectoryPath("/hello/../../../../../../temp"));

                // Then
                Assert.Equal("/temp", path);
            }
        }
    }
}