// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using NSubstitute;
using Xunit;

namespace Cake.Core.Tests.Unit
{
    public sealed class TaskSetupContextTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given
                var task = Substitute.For<ICakeTaskInfo>();

                // When
                var result = Record.Exception(() => new TaskTeardownContext(null, task, TimeSpan.Zero, false));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Task_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() => new TaskTeardownContext(context, null, TimeSpan.Zero, false));

                // Then
                AssertEx.IsArgumentNullException(result, "task");
            }
        }
    }
}