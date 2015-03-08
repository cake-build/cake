using System.Linq;
using Cake.Core.Scripting.CodeGen;
using Cake.Core.Tests.Fixtures;
using Xunit;

namespace Cake.Core.Tests.Unit.Scripting.CodeGen
{
    public sealed class MethodAliasGeneratorTests
    {
        public sealed class TheGeneratorMethod
        {
            [Fact]
            public void Should_Throw_If_Method_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => MethodAliasGenerator.Generate(null));

                // Then
                Assert.IsArgumentNullException(result, "method");
            }

            [Fact]
            public void Should_Throw_If_Declaring_Type_Is_Not_Static()
            {
                // Given
                var method = GetType().GetMethod("Should_Throw_If_Declaring_Type_Is_Not_Static");

                // When
                var result = Record.Exception(() => MethodAliasGenerator.Generate(method));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("The type 'Cake.Core.Tests.Unit.Scripting.CodeGen.MethodAliasGeneratorTests+TheGeneratorMethod' is not static.",
                    result.Message);
            }

            [Fact]
            public void Should_Throw_If_Method_Is_Not_An_Extension_Method()
            {
                // Given
                var method = typeof(MethodAliasGeneratorFixture).GetMethod("NotAnExtensionMethod");

                // When
                var result = Record.Exception(() => MethodAliasGenerator.Generate(method));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("The method 'NotAnExtensionMethod' is not an extension method.",
                    result.Message);
            }

            [Fact]
            public void Should_Throw_If_Method_Is_Not_An_Cake_Script_Method()
            {
                // Given
                var method = typeof(MethodAliasGeneratorFixture).GetMethod("NotAScriptMethod");

                // When
                var result = Record.Exception(() => MethodAliasGenerator.Generate(method));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("The method 'NotAScriptMethod' is not a method alias.",
                    result.Message);
            }

            [Fact]
            public void Should_Return_Correctly_Generated_Wrapper_For_Non_Generic_Type_Without_Arguments()
            {
                const string expected = "public void NonGeneric_ExtensionMethodWithNoParameters(){" +
                                        "Cake.Core.Tests.Fixtures.MethodAliasGeneratorFixture.NonGeneric_ExtensionMethodWithNoParameters" +
                                        "(Context);}";

                var method = typeof(MethodAliasGeneratorFixture).GetMethod("NonGeneric_ExtensionMethodWithNoParameters");

                // When
                var result = MethodAliasGenerator.Generate(method);

                // Then
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Should_Return_Correctly_Generated_Wrapper_For_Non_Generic_Type_With_Argument()
            {
                const string expected = "public void NonGeneric_ExtensionMethodWithParameter(System.Int32 value){" +
                                        "Cake.Core.Tests.Fixtures.MethodAliasGeneratorFixture.NonGeneric_ExtensionMethodWithParameter" +
                                        "(Context, value);}";

                var method = typeof(MethodAliasGeneratorFixture).GetMethod("NonGeneric_ExtensionMethodWithParameter");

                // When
                var result = MethodAliasGenerator.Generate(method);

                // Then
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Should_Return_Correctly_Generated_Wrapper_For_Non_Generic_Type_With_Generic_Argument()
            {
                const string expected = "public void NonGeneric_ExtensionMethodWithGenericParameter(System.Action<System.Int32> value){" +
                                        "Cake.Core.Tests.Fixtures.MethodAliasGeneratorFixture.NonGeneric_ExtensionMethodWithGenericParameter" +
                                        "(Context, value);}";

                var method = typeof(MethodAliasGeneratorFixture).GetMethod("NonGeneric_ExtensionMethodWithGenericParameter");

                // When
                var result = MethodAliasGenerator.Generate(method);

                // Then
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Should_Return_Correctly_Generated_Wrapper_For_Method_With_Return_Value()
            {
                const string expected = "public System.String NonGeneric_ExtensionMethodWithReturnValue(){" +
                                        "return Cake.Core.Tests.Fixtures.MethodAliasGeneratorFixture.NonGeneric_ExtensionMethodWithReturnValue" +
                                        "(Context);}";

                var method = typeof(MethodAliasGeneratorFixture).GetMethod("NonGeneric_ExtensionMethodWithReturnValue");

                // When
                var result = MethodAliasGenerator.Generate(method);

                // Then
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Should_Return_Correctly_Generated_Wrapper_For_Generic_Type_Without_Arguments()
            {
                const string expected = "public void Generic_ExtensionMethod<TTest>(){" +
                                        "Cake.Core.Tests.Fixtures.MethodAliasGeneratorFixture.Generic_ExtensionMethod<TTest>" +
                                        "(Context);}";

                var method = typeof(MethodAliasGeneratorFixture).GetMethods().SingleOrDefault(x => x.Name == "Generic_ExtensionMethod");

                // When
                var result = MethodAliasGenerator.Generate(method);

                // Then
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Should_Return_Correctly_Generated_Wrapper_For_Generic_Type_With_Argument()
            {
                const string expected = "public void Generic_ExtensionMethodWithParameter<TTest>(TTest value){" +
                                        "Cake.Core.Tests.Fixtures.MethodAliasGeneratorFixture.Generic_ExtensionMethodWithParameter<TTest>" +
                                        "(Context, value);}";

                var method = typeof(MethodAliasGeneratorFixture).GetMethods().SingleOrDefault(x => x.Name == "Generic_ExtensionMethodWithParameter");

                // When
                var result = MethodAliasGenerator.Generate(method);

                // Then
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Should_Return_Correctly_Generated_Wrapper_For_Generic_Type_With_Generic_Return_Value()
            {
                const string expected = "public TTest Generic_ExtensionMethodWithGenericReturnValue<TTest>(TTest value){" +
                                        "return Cake.Core.Tests.Fixtures.MethodAliasGeneratorFixture.Generic_ExtensionMethodWithGenericReturnValue<TTest>" +
                                        "(Context, value);}";

                var method = typeof(MethodAliasGeneratorFixture).GetMethods().SingleOrDefault(x => x.Name == "Generic_ExtensionMethodWithGenericReturnValue");

                // When
                var result = MethodAliasGenerator.Generate(method);

                // Then
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Should_Return_Correctly_Generated_Wrapper_For_Non_Generic_Type_With_Parameter_Array_Argument()
            {
                const string expected = "public void NonGeneric_ExtensionMethodWithParameterArray(params System.Int32[] values){" +
                                        "Cake.Core.Tests.Fixtures.MethodAliasGeneratorFixture.NonGeneric_ExtensionMethodWithParameterArray" +
                                        "(Context, values);}";

                var method = typeof(MethodAliasGeneratorFixture).GetMethod("NonGeneric_ExtensionMethodWithParameterArray");

                // When
                var result = MethodAliasGenerator.Generate(method);

                // Then
                Assert.Equal(expected, result);
            }
        }
    }
}
