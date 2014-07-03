using System;
using Cake.Core;
using Cake.Scripting.CodeGen;
using Cake.Tests.Fixtures;
using Xunit;

namespace Cake.Tests.Unit.Scripting.CodeGen
{
    public sealed class PropertyAliasGeneratorTests
    {
        public sealed class TheGenerateMethod
        {
            [Fact]
            public void Should_Throw_If_Method_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => PropertyAliasGenerator.Generate(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("method", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Declaring_Type_Is_Not_Static()
            {
                // Given
                var method = GetType().GetMethod("Should_Throw_If_Declaring_Type_Is_Not_Static");

                // When
                var result = Record.Exception(() => PropertyAliasGenerator.Generate(method));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("The type 'Cake.Tests.Unit.Scripting.CodeGen.PropertyAliasGeneratorTests+TheGenerateMethod' is not static.",
                    result.Message);
            }

            [Fact]
            public void Should_Throw_If_Method_Is_Not_An_Extension_Method()
            {
                // Given
                var method = typeof(PropertyAliasGeneratorFixture).GetMethod("NotAnExtensionMethod");

                // When
                var result = Record.Exception(() => PropertyAliasGenerator.Generate(method));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("The method 'NotAnExtensionMethod' is not an extension method.", 
                    result.Message);
            }

            [Fact]
            public void Should_Throw_If_Method_Is_Not_An_Cake_Property_Alias()
            {
                // Given
                var method = typeof(PropertyAliasGeneratorFixture).GetMethod("NotAScriptMethod");

                // When
                var result = Record.Exception(() => PropertyAliasGenerator.Generate(method));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("The method 'NotAScriptMethod' is not a Cake property alias.",
                    result.Message);
            }

            [Fact]
            public void Should_Throw_If_Property_Alias_Have_More_Than_One_Argument()
            {
                // Given
                var method = typeof(PropertyAliasGeneratorFixture).GetMethod("PropertyAliasWithMoreThanOneMethod");

                // When
                var result = Record.Exception(() => PropertyAliasGenerator.Generate(method));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("The cake property alias method 'PropertyAliasWithMoreThanOneMethod' has an invalid signature.",
                    result.Message);
            }

            [Fact]
            public void Should_Throw_If_Property_Alias_Do_Not_Have_A_Cake_Context_As_First_Parameter()
            {
                // Given
                var method = typeof(PropertyAliasGeneratorFixture).GetMethod("PropertyAliasWithoutContext");

                // When
                var result = Record.Exception(() => PropertyAliasGenerator.Generate(method));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("The cake property alias method 'PropertyAliasWithoutContext' has an invalid signature.",
                    result.Message);
            }

            [Fact]
            public void Should_Throw_If_Method_Is_Generic()
            {
                // Given
                var method = typeof(PropertyAliasGeneratorFixture).GetMethod("GenericScriptMethod");

                // When
                var result = Record.Exception(() => PropertyAliasGenerator.Generate(method));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("The cake property alias method 'GenericScriptMethod' cannot be generic.",
                    result.Message);
            }

            [Fact]
            public void Should_Throw_If_Property_Alias_Returns_Void()
            {
                // Given
                var method = typeof(PropertyAliasGeneratorFixture).GetMethod("PropertyAliasReturningVoid");

                // When
                var result = Record.Exception(() => PropertyAliasGenerator.Generate(method));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("The cake property alias method 'PropertyAliasReturningVoid' cannot return void.",
                    result.Message);
            }

            [Fact]
            public void Should_Generate_Correct_Code_For_Valid_Property_Alias()
            {
                // Given
                const string expected = "public System.Int32 PropertyAliasReturningInteger{get{return " +
                                        "Cake.Tests.Fixtures.PropertyAliasGeneratorFixture.PropertyAliasReturningInteger(GetContext());}}";

                var method = typeof(PropertyAliasGeneratorFixture).GetMethod("PropertyAliasReturningInteger");

                // When
                var result = PropertyAliasGenerator.Generate(method);

                // Then
                Assert.Equal(expected, result);
            }
        }
    }
}
