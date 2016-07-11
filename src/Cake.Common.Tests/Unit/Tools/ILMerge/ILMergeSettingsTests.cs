// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.ILMerge;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.ILMerge
{
    public sealed class ILMergeSettingsTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Set_Internalize_To_False_By_Default()
            {
                // Given, When
                var settings = new ILMergeSettings();

                // Then
                Assert.False(settings.Internalize);
            }

            [Fact]
            public void Should_Set_Target_Kind_To_Default_By_Default()
            {
                // Given, When
                var settings = new ILMergeSettings();

                // Then
                Assert.Equal(TargetKind.Default, settings.TargetKind);
            }
        }
    }
}
