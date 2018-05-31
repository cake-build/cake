// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace Cake.Core.Tests.Unit
{
    public sealed class CakeEngineActionsTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Data_Service_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new CakeEngineActions(null));

                // Then
                AssertEx.IsArgumentNullException(result, "data");
            }
        }
    }
}
