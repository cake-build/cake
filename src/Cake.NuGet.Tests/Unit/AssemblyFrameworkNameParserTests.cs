// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Linq;
using System.Runtime.Versioning;
using NSubstitute;
using Xunit;

namespace Cake.NuGet.Tests.Unit
{
    public sealed class AssemblyFrameworkNameParserTests
    {
        public sealed class TheParseMethod
        {
            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given
                var parser = new AssemblyFrameworkNameParser(Substitute.For<IFrameworkNameParser>());

                // When
                var result = Record.Exception(() => parser.Parse(null));

                // Then
                Assert.IsArgumentNullException(result, "path");
            }

            [Theory]
            [InlineData("lib/foo.dll", "Unsupported")]
            [InlineData("lib/sub/foo.dll", "Unsupported")]
            [InlineData("lib/fx1/foo.dll", "fx1")]
            [InlineData("lib/sub/fx2/foo.dll", "fx2")]
            [InlineData("fx2/foo.dll", "fx2")]
            [InlineData("fx2/sub/foo.dll", "fx2")]
            [InlineData("lib/fx1/sub/foo.dll", "fx1")]
            [InlineData("foo.dll", null)]
            public void Should_Parse_Framework_Name_Tokens_From_Path(string path, string expectedFrameworkNameIdentifier)
            {
                // Given
                var dummyValidFrameworkTokens = new[] { "fx1", "fx2" };

                var nameParser = Substitute.For<IFrameworkNameParser>();
                nameParser.ParseFrameworkName(Arg.Any<string>()).Returns(ci =>
                {
                    var token = ci.Arg<string>();
                    return new FrameworkName(dummyValidFrameworkTokens.Contains(token) ? token : "Unsupported",
                        new Version());
                });

                var pathParser = new AssemblyFrameworkNameParser(nameParser);

                // When
                var result = pathParser.Parse(path);

                // Then
                Assert.Equal(expectedFrameworkNameIdentifier, result != null ? result.Identifier : null);
            }
        }
    }
}
