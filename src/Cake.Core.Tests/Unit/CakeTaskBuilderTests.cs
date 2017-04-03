// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace Cake.Core.Tests.Unit
{
    public sealed class CakeTaskBuilderTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_Is_Provided_Task_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new CakeTaskBuilder<ActionTask>(null));

                // Then
                AssertEx.IsArgumentNullException(result, "task");
            }
        }

        public sealed class TheTaskProperty
        {
            [Fact]
            public void Should_Return_The_Task_Provided_To_The_Constructor()
            {
                // Given, When
                var task = new ActionTask("task");
                var builder = new CakeTaskBuilder<ActionTask>(task);

                // Then
                Assert.Equal(task, builder.Task);
            }
        }
    }
}