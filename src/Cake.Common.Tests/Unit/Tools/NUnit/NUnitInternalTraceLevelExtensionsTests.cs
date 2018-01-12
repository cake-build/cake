// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tools.NUnit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.NUnit
{
    public sealed class NUnitInternalTraceLevelExtensionsTests
    {
        public sealed class TheGetArgumentValueMethod
        {
            [Theory]
            [InlineData(NUnitInternalTraceLevel.Off, "off")]
            [InlineData(NUnitInternalTraceLevel.Error, "error")]
            [InlineData(NUnitInternalTraceLevel.Warning, "warning")]
            [InlineData(NUnitInternalTraceLevel.Info, "info")]
            [InlineData(NUnitInternalTraceLevel.Debug, "verbose")]
#pragma warning disable xUnit1025 // InlineData should be unique within the Theory it belongs to
            [InlineData(NUnitInternalTraceLevel.Verbose, "verbose")]
#pragma warning restore xUnit1025 // InlineData should be unique within the Theory it belongs to
            public void Should_Return_Correct_Value(NUnitInternalTraceLevel level, string expected)
            {
                // Given, When
                var result = level.GetArgumentValue();

                // Then
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Should_Throw_On_Unexpected_Value()
            {
                // Given
                var level = (NUnitInternalTraceLevel)128;

                // When
                var ex = Record.Exception(() => level.GetArgumentValue());

                // Then
                Assert.IsType<ArgumentOutOfRangeException>(ex);
            }
        }
    }
}