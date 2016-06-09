// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.Annotations;
using Xunit;

namespace Cake.Core.Tests.Unit.Annotations
{
    public sealed class CakePropertyAliasAttributeTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Default_Cache_Property_To_False()
            {
                // Given, When
                var attribute = new CakePropertyAliasAttribute();

                // Then
                Assert.False(attribute.Cache);
            }
        }
    }
}
