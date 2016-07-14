// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;
using Xunit.Sdk;

namespace Cake.Tests.Unit.Asserts
{
    public sealed class AssertTests
    {
        public sealed class NotNull
        {
            [Fact]
            public void Should_Not_Throw_If_Object_Is_Not_Null()
            {
                // Given
                var @object = new object();

                // When, Then
                Assert.NotNull(@object);
            }

            [Fact]
            public void Should_Not_Throw_With_User_Message_If_Object_Is_Not_Null()
            {
                // Given
                var @object = new object();

                // When, Then
                Assert.NotNull(@object, "User Message");
            }

            [Fact]
            public void Should_Throw_If_Object_Is_Null()
            {
                // Given
                object @object = null;

                // When
                var exception = Assert.Throws<NotNullException>(() => Assert.NotNull(@object));

                // Then
                Assert.Equal("Assert.NotNull() Failure.", exception.Message);
            }

            [Fact]
            public void Should_Throw_With_User_Message_If_Object_Is_Null()
            {
                // Given
                object @object = null;

                // When
                var exception = Assert.Throws<NotNullException>(() => Assert.NotNull(@object, "User Message"));

                // Then
                Assert.Equal("User Message. Assert.NotNull() Failure.", exception.Message);
            }
        }
    }
}