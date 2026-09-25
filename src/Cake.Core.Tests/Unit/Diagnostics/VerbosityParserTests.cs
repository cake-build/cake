// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Diagnostics;
using Xunit;

namespace Cake.Core.Tests.Unit.Diagnostics;

public sealed class VerbosityParserTests
{
    public sealed class TheTryParseMethod
    {
        [Theory]
        [InlineData("q", Verbosity.Quiet)]
        [InlineData("quiet", Verbosity.Quiet)]
        [InlineData("Quiet", Verbosity.Quiet)]
        [InlineData("m", Verbosity.Minimal)]
        [InlineData("minimal", Verbosity.Minimal)]
        [InlineData("n", Verbosity.Normal)]
        [InlineData("normal", Verbosity.Normal)]
        [InlineData("v", Verbosity.Verbose)]
        [InlineData("verbose", Verbosity.Verbose)]
        [InlineData("d", Verbosity.Diagnostic)]
        [InlineData("diagnostic", Verbosity.Diagnostic)]
        [InlineData(" Diagnostic ", Verbosity.Diagnostic)]
        public void Should_Parse_Known_Aliases(string value, Verbosity expected)
        {
            // When
            var result = VerbosityParser.TryParse(value, out var verbosity);

            // Then
            Assert.True(result);
            Assert.Equal(expected, verbosity);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("lol")]
        [InlineData("2")]
        public void Should_Not_Parse_Invalid_Values(string value)
        {
            // When
            var result = VerbosityParser.TryParse(value, out var verbosity);

            // Then
            Assert.False(result);
            Assert.Equal(default, verbosity);
        }
    }
}
