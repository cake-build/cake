// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.Scripting.CodeGen;
using Cake.Core.Tests.Fixtures;
using Xunit;

namespace Cake.Core.Tests.Unit.Scripting.CodeGen
{
    public sealed class MethodAliasGeneratorTests
    {
        public sealed class TheGeneratorMethod : IClassFixture<MethodAliasGeneratorFixture>
        {
            private readonly MethodAliasGeneratorFixture _fixture;

            public TheGeneratorMethod(MethodAliasGeneratorFixture fixture)
            {
                _fixture = fixture;
            }

            [Fact]
            public void Should_Throw_If_Method_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => MethodAliasGenerator.Generate(null));

                // Then
                Assert.IsArgumentNullException(result, "method");
            }

            [Theory]
            [InlineData("NonGeneric_ExtensionMethodWithNoParameters")]
            [InlineData("NonGeneric_ExtensionMethodWithParameter")]
            [InlineData("NonGeneric_ExtensionMethodWithGenericParameter")]
            [InlineData("NonGeneric_ExtensionMethodWithReturnValue")]
            [InlineData("NonGeneric_ExtensionMethodWithParameterArray")]
            public void Should_Return_Correct_Generated_Code_For_Non_Generic_Methods(string name)
            {
                // Given
                var expected = _fixture.GetExpectedCode(name);

                // When
                var result = _fixture.Generate(name);

                // Then
                Assert.Equal(expected, result);
            }

            [Theory]
            [InlineData("Generic_ExtensionMethod")]
            [InlineData("Generic_ExtensionMethodWithParameter")]
            [InlineData("Generic_ExtensionMethodWithGenericReturnValue")]
            public void Should_Return_Correct_Generated_Code_For_Generic_Methods(string name)
            {
                // Given
                var expected = _fixture.GetExpectedCode(name);

                // When
                var result = _fixture.Generate(name);

                // Then
                Assert.Equal(expected, result);
            }

            [Theory]
            [InlineData("Obsolete_ImplicitWarning_NoMessage")]
            [InlineData("Obsolete_ImplicitWarning_WithMessage")]
            [InlineData("Obsolete_ExplicitWarning_WithMessage")]
            [InlineData("Obsolete_ExplicitError_WithMessage")]
            public void Should_Return_Correct_Generated_Code_For_Obsolete_Methods(string name)
            {
                // Given
                var expected = _fixture.GetExpectedCode(name);

                // When
                var result = _fixture.Generate(name);

                // Then
                Assert.Equal(expected, result);
            }
        }
    }
}
