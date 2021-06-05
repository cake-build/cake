// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;
using Cake.Testing;
using NSubstitute;
using Xunit;

namespace Cake.Core.Tests.Unit
{
    public sealed class CakeEnvironmentTests
    {
        public sealed class LaunchDirectoryProperty
        {
            [Theory]
            [InlineData(PlatformFamily.Linux)]
            [InlineData(PlatformFamily.OSX)]
            [InlineData(PlatformFamily.Windows)]
            public void Should_Not_Change_When_WorkingDirectory_Changed(PlatformFamily family)
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                context.Environment.Returns(new FakeEnvironment(family));

                // When
                context.Environment.WorkingDirectory = "/build";

                // Then
                Assert.NotEqual(context.Environment.WorkingDirectory, context.Environment.LaunchDirectory);
            }
        }
    }
}
