// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.Bamboo.Data
{
    public sealed class BambooCommitInfoTests
    {
        public sealed class TheRepositoryRevisionProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BambooInfoFixture().CreateCommitInfo();

                // When
                var result = info.RepositoryRevision;

                // Then
                Assert.Equal("d4a3a4cb304548450e3cab2ff735f778ffe58d03", result);

            }
        }
    }
}
