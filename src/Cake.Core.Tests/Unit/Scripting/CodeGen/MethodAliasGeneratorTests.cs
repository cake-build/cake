// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Cake.Core.Scripting.CodeGen;
using Cake.Core.Tests.Fixtures;
using VerifyXunit;
using Xunit;
using static VerifyXunit.Verifier;

namespace Cake.Core.Tests.Unit.Scripting.CodeGen
{
    [UsesVerify]
    public sealed class MethodAliasGeneratorTests
    {
        [UsesVerify]
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
                AssertEx.IsArgumentNullException(result, "method");
            }

            [Theory]
            [InlineData("NonGeneric_ExtensionMethodWithNoParameters")]
            [InlineData("NonGeneric_ExtensionMethodWithParameter")]
            [InlineData("NonGeneric_ExtensionMethodWithGenericParameter")]
            [InlineData("NonGeneric_ExtensionMethodWithGenericExpressionParameter")]
            [InlineData("NonGeneric_ExtensionMethodWithGenericExpressionArrayParameter")]
            [InlineData("NonGeneric_ExtensionMethodWithGenericExpressionParamsArrayParameter")]
            [InlineData("NonGeneric_ExtensionMethodWithReturnValue")]
            [InlineData("NonGeneric_ExtensionMethodWithParameterArray")]
            [InlineData("NonGeneric_ExtensionMethodWithOptionalObjectParameter")]
            [InlineData("NonGeneric_ExtensionMethodWithOptionalBooleanParameter")]
            [InlineData("NonGeneric_ExtensionMethodWithOptionalStringParameter")]
            [InlineData("NonGeneric_ExtensionMethodWithOptionalEnumParameter")]
            [InlineData("NonGeneric_ExtensionMethodWithOptionalCharParameter")]
            [InlineData("NonGeneric_ExtensionMethodWithOptionalDecimalParameter")]
            [InlineData("NonGeneric_ExtensionMethodWithOptionalNullableTParameter")]
            [InlineData("NonGeneric_ExtensionMethodWithOptionalNullableBooleanParameter")]
            [InlineData("NonGeneric_ExtensionMethodWithOptionalNullableCharParameter")]
            [InlineData("NonGeneric_ExtensionMethodWithOptionalNullableEnumParameter")]
            [InlineData("NonGeneric_ExtensionMethodWithOptionalNullableDecimalParameter")]
            [InlineData("NonGeneric_ExtensionMethodWithOptionalNullableLongParameter")]
            [InlineData("NonGeneric_ExtensionMethodWithOptionalNullableDoubleParameter")]
            [InlineData("NonGeneric_ExtensionMethodWithReservedKeywordParameter")]
            [InlineData("NonGeneric_ExtensionMethodWithOutputParameter")]
            [InlineData("NonGeneric_ExtensionMethodWithGenericCollectionOfNestedType")]
            [InlineData("NonGeneric_ExtensionMethodWithParameterAttributes")]
            [InlineData("NonGeneric_ExtensionMethodWithDynamicReturnValue")]
            public Task Should_Return_Correct_Generated_Code_For_Non_Generic_Methods(string name)
            {
                // Given / When
                var result = _fixture.Generate(name);

                // Then
                return Verify(result)
                    .UseParameters(name);
            }

            [Theory]
            [InlineData("Generic_ExtensionMethod")]
            [InlineData("Generic_ExtensionMethodWithParameter")]
            [InlineData("Generic_ExtensionMethodWithGenericReturnValue")]
            [InlineData("Generic_ExtensionMethodWithGenericReturnValueAndTypeParamConstraints")]
            public Task Should_Return_Correct_Generated_Code_For_Generic_Methods(string name)
            {
                // Given / When
                var result = _fixture.Generate(name);

                // Then
                return Verify(result)
                    .UseParameters(name);
            }

            [Theory]
            [InlineData("Obsolete_ImplicitWarning_NoMessage")]
            [InlineData("Obsolete_ImplicitWarning_WithMessage")]
            [InlineData("Obsolete_ExplicitWarning_WithMessage")]
            [InlineData("Obsolete_ExplicitError_WithMessage")]
            public Task Should_Return_Correct_Generated_Code_For_Obsolete_Methods(string name)
            {
                // Given / When
                var result = _fixture.Generate(name);

                // Then
                return Verify(result)
                    .UseParameters(name);
            }
        }
    }
}