// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.AppVeyor.Data
{
    public sealed class AppVeyorCommitInfoTests
    {
        public sealed class TheAuthorProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AppVeyorInfoFixture().CreateCommitInfo();

                // When
                var result = info.Author;

                // Then
                Assert.Equal("Patrik Svensson", result);
            }
        }

        public sealed class TheEmailProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AppVeyorInfoFixture().CreateCommitInfo();

                // When
                var result = info.Email;

                // Then
                Assert.Equal("author@mail.com", result);
            }
        }

        public sealed class TheExtendedMessageProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AppVeyorInfoFixture().CreateCommitInfo();

                // When
                var result = info.ExtendedMessage;

                // Then
                Assert.Equal("Testing stuff.", result);
            }
        }

        public sealed class TheMessageProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AppVeyorInfoFixture().CreateCommitInfo();

                // When
                var result = info.Message;

                // Then
                Assert.Equal("A test commit.", result);
            }
        }

        public sealed class TheIdProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AppVeyorInfoFixture().CreateCommitInfo();

                // When
                var result = info.Id;

                // Then
                Assert.Equal("01c08e7b0f3434b1c6c30c880be33ed7331e8639", result);
            }
        }

        public sealed class TheTimestampProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AppVeyorInfoFixture().CreateCommitInfo();

                // When
                var result = info.Timestamp;

                // Then
                Assert.Equal("1/5/2015 3:13:01 AM", result);
            }
        }
    }
}
