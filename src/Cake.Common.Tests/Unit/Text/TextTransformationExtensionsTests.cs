// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Text;
using Cake.Core;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Text
{
    public sealed class TextTransformationExtensionsTests
    {
        public sealed class TheWithTokenMethod
        {
            [Fact]
            public void Should_Register_The_Provided_Token_With_The_Template()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var transformation = TextTransformationAliases.TransformText(
                    context, "<%greeting%> World!");

                // When
                transformation.WithToken("greeting", "Hello");

                // Then
                Assert.Equal("Hello World!", transformation.ToString());
            }

            [Fact]
            public void Should_Return_Same_Instance()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var transformation = TextTransformationAliases.TransformText(
                    context, "<%greeting%> World!");

                // When
                var result = transformation.WithToken("greeting", "Hello");

                // Then
                Assert.Same(transformation, result);
            }
        }
    }
}
