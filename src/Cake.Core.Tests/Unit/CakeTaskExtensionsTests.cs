// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace Cake.Core.Tests.Unit
{
    public sealed class CakeTaskExtensionsTests
    {
        public sealed class TheAddCriteriaMethod
        {
            [Fact]
            public void Should_Throw_If_Criteria_Is_Null()
            {
                // Given
                var task = new CakeTask("task");

                // When
                var result = Record.Exception(() => task.AddCriteria(null));

                // Then
                AssertEx.IsArgumentNullException(result, "predicate");
            }

            [Fact]
            public void Should_Add_Criteria()
            {
                // Given
                var task = new CakeTask("task");

                // When
                task.AddCriteria(context => true);

                // Then
                Assert.Single(task.Criterias);
            }
        }
    }
}