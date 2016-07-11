// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.ContinuaCI.Data
{
    public sealed class ContinuaCIProjectInfoTests
    {
        public sealed class TheNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateProjectInfo();

                // When
                var result = info.Name;

                // Then
                Assert.Equal("the project from hell", result);
            }
        }
    }
}
