// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core.CommandLineArguments;
using Xunit;

namespace Cake.Core.Tests.Unit
{
    public sealed class CommandLineArgumentsParserTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Parse_Arguments_Properly()
            {
                // Given
                var arguments = new[] { "Program", "-t", "verbose", "optional", "file=text.txt" };

                // When
                var result = CommandLineArgumentsParser.Parse(arguments);

                // Then
                Assert.Collection(result,
                    pair => AssertPairEqual(pair, "t", "verbose"),
                    pair => AssertPairEqual(pair, "optional", string.Empty),
                    pair => AssertPairEqual(pair, "file", "text.txt"));
            }

            private static void AssertPairEqual(KeyValuePair<string, string> pair, string expectedKey, string expectedValue)
            {
                Assert.Equal(expectedKey, pair.Key);
                Assert.Equal(expectedValue, pair.Value);
            }
        }
    }
}