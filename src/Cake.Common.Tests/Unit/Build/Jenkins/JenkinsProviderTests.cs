// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.Jenkins;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.Jenkins
{
    public sealed class JenkinsProviderTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new JenkinsProvider(null));

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }
        }

        public sealed class TheIsRunningOnJenkinsProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_Jenkins()
            {
                // Given
                var jenkins = Substitute.For<IJenkinsProvider>();
                jenkins.IsRunningOnJenkins.Returns(true);

                // When
                var result = jenkins.IsRunningOnJenkins;

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_If_Not_Running_On_Jenkins()
            {
                // Given
                var jenkins = Substitute.For<IJenkinsProvider>();

                // When
                var result = jenkins.IsRunningOnJenkins;

                // Then
                Assert.False(result);
            }
        }
    }
}