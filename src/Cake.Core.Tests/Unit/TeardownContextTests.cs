// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Xunit;

namespace Cake.Core.Tests.Unit
{
    public sealed class TeardownContextTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given
                var exception = new Exception("Dummy Exception");

                // When
                var result = Record.Exception(() => new TeardownContext(null, exception));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }
        }
    }
}