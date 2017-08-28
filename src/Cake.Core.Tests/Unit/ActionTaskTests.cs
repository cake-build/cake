// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.Tests.Fixtures;
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

            [Fact]
            public void Should_Throw_If_Builder_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => CakeTaskBuilderExtensions.DeferOnError(null));

                // Then
                AssertEx.IsArgumentNullException(result, "builder");
            }

            [Fact]
            public void Should_Throw_On_First_Failed_Action()
            {
                // Given
                var task = new ActionTask("task");
                var context = new CakeContextFixture().CreateContext();

                // When
                task.Actions.Add((c) => throw new NotImplementedException());
                task.Actions.Add((c) => throw new NotSupportedException());
                task.Actions.Add((c) => throw new OutOfMemoryException());
                var result = Record.Exception(() => task.Execute(context));

                // Then
                Assert.IsType<NotImplementedException>(result);
            }

            [Fact]
            public void Should_Aggregate_Exceptions_From_Actions()
            {
                // Given
                var task = new ActionTask("task");
                var context = new CakeContextFixture().CreateContext();

                // When
                task.Actions.Add((c) => throw new NotImplementedException());
                task.Actions.Add((c) => throw new NotSupportedException());
                task.Actions.Add((c) => throw new OutOfMemoryException());
                task.SetDeferExceptions(true);
                var result = Record.Exception(() => task.Execute(context));

                // Then
                Assert.IsType<AggregateException>(result);
                var ex = result as AggregateException;
                Assert.Contains(ex.InnerExceptions, x => x.GetType() == typeof(NotImplementedException));
                Assert.Contains(ex.InnerExceptions, x => x.GetType() == typeof(NotSupportedException));
                Assert.Contains(ex.InnerExceptions, x => x.GetType() == typeof(OutOfMemoryException));
            }

            [Fact]
            public void Should_Only_Aggregate_Exceptions_When_There_Are_Many()
            {
                // Given
                var task = new ActionTask("task");
                var context = new CakeContextFixture().CreateContext();

                // When
                task.Actions.Add((c) => throw new NotImplementedException());
                task.SetDeferExceptions(true);
                var result = Record.Exception(() => task.Execute(context));

                // Then
                Assert.IsType<NotImplementedException>(result);
            }
        }
    }
}