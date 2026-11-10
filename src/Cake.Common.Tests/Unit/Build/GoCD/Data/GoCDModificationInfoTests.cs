// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.GoCD.Data;

namespace Cake.Common.Tests.Unit.Build.GoCD.Data;

public sealed class GoCDModificationInfoTests
{
    public sealed class TheModifiedTimeProperty
    {
        [Fact]
        public void Should_Treat_Unspecified_As_Utc()
        {
            // Given
            var info = new GoCDModificationInfo();

            // When
            info.ModifiedTime = new DateTime(2015, 6, 22, 7, 20, 13, DateTimeKind.Unspecified);

            // Then
            Assert.Equal(1434957613000, info.ModifiedTimeUnixMilliseconds);
            Assert.Equal(DateTimeKind.Utc, info.ModifiedTime.Kind);
        }

        [Fact]
        public void Should_Convert_Local_Using_Offset()
        {
            // Given
            var info = new GoCDModificationInfo();
            var local = new DateTime(2015, 6, 22, 7, 20, 13, DateTimeKind.Local);

            // When
            info.ModifiedTime = local;

            // Then
            Assert.Equal(new DateTimeOffset(local).ToUnixTimeMilliseconds(), info.ModifiedTimeUnixMilliseconds);
        }
    }
}
