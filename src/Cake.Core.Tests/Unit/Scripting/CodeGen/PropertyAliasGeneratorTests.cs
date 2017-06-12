// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Cake.Core.Scripting.CodeGen;
using Cake.Core.Tests.Fixtures;
using Xunit;

namespace Cake.Core.Tests.Unit.Scripting.CodeGen
{
    public sealed class PropertyAliasGeneratorTests
    {
        public sealed class TheGenerateMethod : IClassFixture<PropertyAliasGeneratorFixture>
        {
            private readonly PropertyAliasGeneratorFixture _fixture;

            public TheGenerateMethod(PropertyAliasGeneratorFixture fixture)
            {
                _fixture = fixture;
            }

            [Fact]
            public void Should_Throw_If_Method_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => PropertyAliasGenerator.Generate(null));

                // Then
                AssertEx.IsArgumentNullException(result, "method");
            }

            [Fact]
            public void Should_Throw_If_Declaring_Type_Is_Not_Static()
            {
                // Given
                var method = GetType().GetTypeInfo().GetMethod("Should_Throw_If_Declaring_Type_Is_Not_Static");

                // When
                var result = Record.Exception(() => PropertyAliasGenerator.Generate(method));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("The type 'Cake.Core.Tests.Unit.Scripting.CodeGen.PropertyAliasGeneratorTests+TheGenerateMethod' is not static.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Method_Is_Not_An_Extension_Method()
            {
                // Given, When
                var result = Record.Exception(() => _fixture.Generate("NotAnExtensionMethod"));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("The method 'NotAnExtensionMethod' is not an extension method.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Method_Is_Not_An_Cake_Property_Alias()
            {
                // Given, When
                var result = Record.Exception(() => _fixture.Generate("NotAScriptMethod"));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("The method 'NotAScriptMethod' is not a property alias.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Property_Alias_Have_More_Than_One_Argument()
            {
                // Given, When
                var result = Record.Exception(() => _fixture.Generate("PropertyAliasWithMoreThanOneMethod"));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("The property alias 'PropertyAliasWithMoreThanOneMethod' has an invalid signature.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Property_Alias_Do_Not_Have_A_Cake_Context_As_First_Parameter()
            {
                // Given, When
                var result = Record.Exception(() => _fixture.Generate("PropertyAliasWithoutContext"));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("The property alias 'PropertyAliasWithoutContext' has an invalid signature.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Method_Is_Generic()
            {
                // Given, When
                var result = Record.Exception(() => _fixture.Generate("GenericScriptMethod"));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("The property alias 'GenericScriptMethod' cannot be generic.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Property_Alias_Returns_Void()
            {
                // Given, When
                var result = Record.Exception(() => _fixture.Generate("PropertyAliasReturningVoid"));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("The property alias 'PropertyAliasReturningVoid' cannot return void.", result?.Message);
            }

            [Theory]
            [InlineData("NonCached_Value_Type")]
            public void Should_Return_Correct_Generated_Code_For_Non_Cached_Properties(string name)
            {
                // Given
                var expected = _fixture.GetExpectedData(name);

                // When
                var result = _fixture.Generate(name);

                // Then
                Assert.Equal(expected, result);
            }

            [Theory]
            [InlineData("Cached_Reference_Type")]
            [InlineData("Cached_Value_Type")]
            public void Should_Return_Correct_Generated_Code_For_Cached_Properties(string name)
            {
                // Given
                var expected = _fixture.GetExpectedData(name);

                // When
                var result = _fixture.Generate(name);

                // Then
                Assert.Equal(expected, result);
            }

            [Theory]
            [InlineData("NonCached_Obsolete_ImplicitWarning_NoMessage")]
            [InlineData("NonCached_Obsolete_ImplicitWarning_WithMessage")]
            [InlineData("NonCached_Obsolete_ExplicitWarning_WithMessage")]
            [InlineData("NonCached_Obsolete_ExplicitError_WithMessage")]
            public void Should_Return_Correct_Generated_Code_For_Non_Cached_Obsolete_Properties(string name)
            {
                // Given
                var expected = _fixture.GetExpectedData(name);

                // When
                var result = _fixture.Generate(name);

                // Then
                Assert.Equal(expected, result);
            }

            [Theory]
            [InlineData("Cached_Obsolete_ImplicitWarning_NoMessage")]
            [InlineData("Cached_Obsolete_ImplicitWarning_WithMessage")]
            [InlineData("Cached_Obsolete_ExplicitWarning_WithMessage")]
            [InlineData("Cached_Obsolete_ExplicitError_WithMessage")]
            public void Should_Return_Correct_Generated_Code_For_Cached_Obsolete_Properties(string name)
            {
                // Given
                var expected = _fixture.GetExpectedData(name);

                // When
                var result = _fixture.Generate(name);

                // Then
                Assert.Equal(expected, result);
            }
        }
    }
}