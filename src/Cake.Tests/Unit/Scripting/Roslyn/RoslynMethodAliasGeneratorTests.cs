using System.Linq;
using Cake.Scripting.Roslyn;
using Cake.Tests.Fixtures;
using Xunit;

namespace Cake.Tests.Unit.Scripting.Roslyn
{
    public sealed class RoslynMethodAliasGeneratorTests
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
            public void Should_Return_Correctly_Generated_Wrapper_For_Non_Generic_Type_Without_Arguments()
            {
                const string expected = "public void NonGeneric_ExtensionMethodWithNoParameters(){" +
                                        "Cake.Tests.Fixtures.RoslynMethodAliasGeneratorFixture.NonGeneric_ExtensionMethodWithNoParameters" +
                                        "(Context);}";

                var method = typeof(RoslynMethodAliasGeneratorFixture).GetMethod("NonGeneric_ExtensionMethodWithNoParameters");

                // When
                var result = MethodAliasGenerator.Generate(method);

                // Then
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Should_Return_Correctly_Generated_Wrapper_For_Non_Generic_Type_With_Argument()
            {
                const string expected = "public void NonGeneric_ExtensionMethodWithParameter(System.Int32 value){" +
                                        "Cake.Tests.Fixtures.RoslynMethodAliasGeneratorFixture.NonGeneric_ExtensionMethodWithParameter" +
                                        "(Context, value);}";

                var method = typeof(RoslynMethodAliasGeneratorFixture).GetMethod("NonGeneric_ExtensionMethodWithParameter");

                // When
                var result = MethodAliasGenerator.Generate(method);

                // Then
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Should_Return_Correctly_Generated_Wrapper_For_Non_Generic_Type_With_Generic_Argument()
            {
                const string expected = "public void NonGeneric_ExtensionMethodWithGenericParameter(System.Action<System.Int32> value){" +
                                        "Cake.Tests.Fixtures.RoslynMethodAliasGeneratorFixture.NonGeneric_ExtensionMethodWithGenericParameter" +
                                        "(Context, value);}";

                var method = typeof(RoslynMethodAliasGeneratorFixture).GetMethod("NonGeneric_ExtensionMethodWithGenericParameter");

                // When
                var result = MethodAliasGenerator.Generate(method);

                // Then
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Should_Return_Correctly_Generated_Wrapper_For_Method_With_Return_Value()
            {
                const string expected = "public System.String NonGeneric_ExtensionMethodWithReturnValue(){" +
                                        "return Cake.Tests.Fixtures.RoslynMethodAliasGeneratorFixture.NonGeneric_ExtensionMethodWithReturnValue" +
                                        "(Context);}";

                var method = typeof(RoslynMethodAliasGeneratorFixture).GetMethod("NonGeneric_ExtensionMethodWithReturnValue");

                // When
                var result = MethodAliasGenerator.Generate(method);

                // Then
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Should_Return_Correctly_Generated_Wrapper_For_Generic_Type_Without_Arguments()
            {
                const string expected = "public void Generic_ExtensionMethod<TTest>(){" +
                                        "Cake.Tests.Fixtures.RoslynMethodAliasGeneratorFixture.Generic_ExtensionMethod<TTest>" +
                                        "(Context);}";

                var method = typeof(RoslynMethodAliasGeneratorFixture).GetMethods().SingleOrDefault(x => x.Name == "Generic_ExtensionMethod");

                // When
                var result = MethodAliasGenerator.Generate(method);

                // Then
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Should_Return_Correctly_Generated_Wrapper_For_Generic_Type_With_Argument()
            {
                const string expected = "public void Generic_ExtensionMethodWithParameter<TTest>(TTest value){" +
                                        "Cake.Tests.Fixtures.RoslynMethodAliasGeneratorFixture.Generic_ExtensionMethodWithParameter<TTest>" +
                                        "(Context, value);}";

                var method = typeof(RoslynMethodAliasGeneratorFixture).GetMethods().SingleOrDefault(x => x.Name == "Generic_ExtensionMethodWithParameter");

                // When
                var result = MethodAliasGenerator.Generate(method);

                // Then
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Should_Return_Correctly_Generated_Wrapper_For_Generic_Type_With_Generic_Return_Value()
            {
                const string expected = "public TTest Generic_ExtensionMethodWithGenericReturnValue<TTest>(TTest value){" +
                                        "return Cake.Tests.Fixtures.RoslynMethodAliasGeneratorFixture.Generic_ExtensionMethodWithGenericReturnValue<TTest>" +
                                        "(Context, value);}";

                var method = typeof(RoslynMethodAliasGeneratorFixture).GetMethods().SingleOrDefault(x => x.Name == "Generic_ExtensionMethodWithGenericReturnValue");

                // When
                var result = MethodAliasGenerator.Generate(method);

                // Then
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Should_Return_Correctly_Generated_Wrapper_For_Non_Generic_Type_With_Parameter_Array_Argument()
            {
                const string expected = "public void NonGeneric_ExtensionMethodWithParameterArray(params System.Int32[] values){" +
                                        "Cake.Tests.Fixtures.RoslynMethodAliasGeneratorFixture.NonGeneric_ExtensionMethodWithParameterArray" +
                                        "(Context, values);}";

                var method = typeof(RoslynMethodAliasGeneratorFixture).GetMethod("NonGeneric_ExtensionMethodWithParameterArray");

                // When
                var result = MethodAliasGenerator.Generate(method);

                // Then
                Assert.Equal(expected, result);
            }
        }
    }
}
