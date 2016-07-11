// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.Bitrise.Data
{
    public sealed class BitriseWorkflowInfoTests
    {
        public sealed class TheWorkflowIdProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BitriseInfoFixture().CreateWorkflowInfo();

                // When
                var result = info.WorkflowId;

                //Then
                Assert.Equal("Build & Test Cake on BitRise", result);
            }
        }

        public sealed class TheWorkflowTitleProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BitriseInfoFixture().CreateWorkflowInfo();

                // When
                var result = info.WorkflowTitle;

                //Then
                Assert.Equal("Build & Test Cake on BitRise", result);
            }
        }
    }
}
