// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.GoCD.Data
{
    public sealed class GoCDRepositoryInfoTests
    {
        public sealed class TheRevisionProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GoCDInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.Revision;

                // Then
                Assert.Equal("123", result);
            }
        }

        public sealed class TheFromRevisionProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GoCDInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.FromRevision;

                // Then
                Assert.Equal("122", result);
            }
        }

        public sealed class TheToRevisionProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GoCDInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.ToRevision;

                // Then
                Assert.Equal("124", result);
            }
        }
    }
}