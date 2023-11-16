// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Cake.Core.Scripting.CodeGen;
using Cake.Core.Tests.Fixtures;
using VerifyXunit;
using Xunit;
using static Cake.Core.Tests.VerifyConfig;

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
            [InlineData("ExtensionMethodWithNoParameters")]
            [InlineData("ExtensionMethodWithParameter")]
            [InlineData("ExtensionMethodWithGenericParameter")]
            [InlineData("ExtensionMethodWithGenericExpressionParameter")]
            [InlineData("ExtensionMethodWithGenericExpressionArrayParameter")]
            [InlineData("ExtensionMethodWithGenericExpressionParamsArrayParameter")]
            [InlineData("ExtensionMethodWithReturnValue")]
            [InlineData("ExtensionMethodWithParameterArray")]
            [InlineData("ExtensionMethodWithOptionalObjectParameter")]
            [InlineData("ExtensionMethodWithOptionalBooleanParameter")]
            [InlineData("ExtensionMethodWithOptionalStringParameter")]
            [InlineData("ExtensionMethodWithOptionalEnumParameter")]
            [InlineData("ExtensionMethodWithOptionalCharParameter")]
            [InlineData("ExtensionMethodWithOptionalDecimalParameter")]
            [InlineData("ExtensionMethodWithOptionalNullableTParameter")]
            [InlineData("ExtensionMethodWithOptionalNullableBooleanParameter")]
            [InlineData("ExtensionMethodWithOptionalNullableCharParameter")]
            [InlineData("ExtensionMethodWithOptionalNullableEnumParameter")]
            [InlineData("ExtensionMethodWithOptionalNullableDecimalParameter")]
            [InlineData("ExtensionMethodWithOptionalNullableLongParameter")]
            [InlineData("ExtensionMethodWithOptionalNullableDoubleParameter")]
            [InlineData("ExtensionMethodWithReservedKeywordParameter")]
            [InlineData("ExtensionMethodWithOutputParameter")]
            [InlineData("ExtensionMethodWithGenericCollectionOfNestedType")]
            [InlineData("ExtensionMethodWithParameterAttributes")]
            [InlineData("ExtensionMethodWithDynamicReturnValue")]
            public Task Should_Return_Correct_Generated_Code_For_Non_Generic_Methods(string name)
            {
                // Given / When
                var result = _fixture.Generate("NonGeneric_" + name);

                // Then
                return VerifyCake(result)
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
                return VerifyCake(result)
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
                return VerifyCake(result)
                    .UseParameters(name);
            }
        }
    }
}