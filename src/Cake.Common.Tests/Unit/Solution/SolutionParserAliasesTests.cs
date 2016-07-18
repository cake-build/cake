using Cake.Common.Solution;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Solution
{
    public sealed class SolutionParserAliasesTests
    {
        public sealed class TheParseSolutionMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => SolutionAliases.ParseSolution(null, null));

                // Then
                Assert.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_SolutionPath_Is_Null()
            {
                // Given
                var cakeContext = Substitute.For<ICakeContext>();
                var cakeEnvironment = Substitute.For<ICakeEnvironment>();
                var fileSystem = Substitute.For<IFileSystem>();
                cakeContext.Environment.Returns(cakeEnvironment);
                cakeContext.FileSystem.Returns(fileSystem);

                // When
                var result = Record.Exception(() => SolutionAliases.ParseSolution(cakeContext, null));

                // Then
                Assert.IsArgumentNullException(result, "solutionPath");
            }
        }
    }
}