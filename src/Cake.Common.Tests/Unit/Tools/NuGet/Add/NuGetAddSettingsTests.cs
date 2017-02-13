// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.NuGet.Add;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.NuGet.Add
{
    public sealed class NuGetAddSettingsTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Set_Expand_To_False_By_Default()
            {
                // Given, When
                var settings = new NuGetAddSettings();

                // Then
                Assert.False(settings.Expand);
            }
        }
    }
}
