// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace Cake.Core.Tests.Unit
{
    public sealed class ActionTaskTests
    {
        public sealed class TheAddActionMethod
        {
            [Fact]
            public void Should_Throw_If_Action_Is_Null()
            {
                // Given
                var task = new ActionTask("task");

                // When
                var result = Record.Exception(() => task.AddAction(null));

                // Then
                AssertEx.IsArgumentNullException(result, "action");
            }

            [Fact]
            public void Should_Add_Action_To_Task()
            {
                // Given
                var task = new ActionTask("task");

                // When
                task.AddAction(c => { });

                // Then
                Assert.Equal(1, task.Actions.Count);
            }
        }
    }
}