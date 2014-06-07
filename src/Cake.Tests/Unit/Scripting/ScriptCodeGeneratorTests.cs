using System.Diagnostics;
using System.Linq;
using Cake.Core;
using Cake.Scripting;
using System;
using Xunit;

namespace Cake.Tests.Unit.Scripting
{
    public static class StaticClass
    {
        public static void NotAnExtensionMethod()
        {
            throw new NotImplementedException();
        }

        public static void NonGeneric_ExtensionMethodWithNoParameters(this ICakeContext context)
        {
            throw new NotImplementedException();
        }

        public static string NonGeneric_ExtensionMethodWithReturnValue(this ICakeContext context)
        {
            throw new NotImplementedException();
        }

        public static void NonGeneric_ExtensionMethodWithParameter(this ICakeContext context, int value)
        {
            throw new NotImplementedException();
        }

        public static void NonGeneric_ExtensionMethodWithGenericParameter(this ICakeContext context, Action<int> value)
        {
            throw new NotImplementedException();
        }

        public static void Generic_ExtensionMethod<TTest>(this ICakeContext context)
        {
            Debug.Assert(typeof (TTest) != null); // Resharper
            throw new NotImplementedException();
        }

        public static void Generic_ExtensionMethodWithParameter<TTest>(this ICakeContext context, TTest value)
        {
            throw new NotImplementedException();
        }

        public static TTest Generic_ExtensionMethodWithGenericReturnValue<TTest>(this ICakeContext context, TTest value)
        {
            throw new NotImplementedException();
        }
    }

    public sealed class ScriptCodeGeneratorTests
    {
        public sealed class TheGeneratorMethod
        {
            [Fact]
            public void Should_Throw_If_Method_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => ScriptCodeGenerator.Generate(null));

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
                var result = Record.Exception(() => ScriptCodeGenerator.Generate(method));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("The type 'Cake.Tests.Unit.Scripting.ScriptCodeGeneratorTests+TheGeneratorMethod' is not static.",
                    result.Message);
            }

            [Fact]
            public void Should_Throw_If_Method_Is_Not_An_Extension_Method_Static()
            {
                // Given
                var method = typeof(StaticClass).GetMethod("NotAnExtensionMethod");

                // When
                var result = Record.Exception(() => ScriptCodeGenerator.Generate(method));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("The method 'NotAnExtensionMethod' is not an extension method.",
                    result.Message);
            }

            [Fact]
            public void Should_Return_Correctly_Generated_Wrapper_For_Non_Generic_Type_Without_Arguments()
            {
                const string expected = "public void NonGeneric_ExtensionMethodWithNoParameters(){" +
                                        "Cake.Tests.Unit.Scripting.StaticClass.NonGeneric_ExtensionMethodWithNoParameters" +
                                        "(GetContext());}";

                var method = typeof(StaticClass).GetMethod("NonGeneric_ExtensionMethodWithNoParameters");

                // When
                var result = ScriptCodeGenerator.Generate(method);

                // Then
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Should_Return_Correctly_Generated_Wrapper_For_Non_Generic_Type_With_Argument()
            {
                const string expected = "public void NonGeneric_ExtensionMethodWithParameter(System.Int32 value){" +
                                        "Cake.Tests.Unit.Scripting.StaticClass.NonGeneric_ExtensionMethodWithParameter" +
                                        "(GetContext(),value);}";

                var method = typeof(StaticClass).GetMethod("NonGeneric_ExtensionMethodWithParameter");

                // When
                var result = ScriptCodeGenerator.Generate(method);

                // Then
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Should_Return_Correctly_Generated_Wrapper_For_Non_Generic_Type_With_Generic_Argument()
            {
                const string expected = "public void NonGeneric_ExtensionMethodWithGenericParameter(System.Action<System.Int32> value){" +
                                        "Cake.Tests.Unit.Scripting.StaticClass.NonGeneric_ExtensionMethodWithGenericParameter" +
                                        "(GetContext(),value);}";

                var method = typeof(StaticClass).GetMethod("NonGeneric_ExtensionMethodWithGenericParameter");

                // When
                var result = ScriptCodeGenerator.Generate(method);

                // Then
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Should_Return_Correctly_Generated_Wrapper_For_Method_With_Return_Value()
            {
                const string expected = "public System.String NonGeneric_ExtensionMethodWithReturnValue(){" +
                                        "return Cake.Tests.Unit.Scripting.StaticClass.NonGeneric_ExtensionMethodWithReturnValue" +
                                        "(GetContext());}";

                var method = typeof(StaticClass).GetMethod("NonGeneric_ExtensionMethodWithReturnValue");

                // When
                var result = ScriptCodeGenerator.Generate(method);

                // Then
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Should_Return_Correctly_Generated_Wrapper_For_Generic_Type_Without_Arguments()
            {
                const string expected = "public void Generic_ExtensionMethod<TTest>(){" +
                                        "Cake.Tests.Unit.Scripting.StaticClass.Generic_ExtensionMethod<TTest>" +
                                        "(GetContext());}";

                var method = typeof (StaticClass).GetMethods().SingleOrDefault(x => x.Name == "Generic_ExtensionMethod");

                // When
                var result = ScriptCodeGenerator.Generate(method);

                // Then
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Should_Return_Correctly_Generated_Wrapper_For_Generic_Type_With_Argument()
            {
                const string expected = "public void Generic_ExtensionMethodWithParameter<TTest>(TTest value){" +
                                        "Cake.Tests.Unit.Scripting.StaticClass.Generic_ExtensionMethodWithParameter<TTest>" +
                                        "(GetContext(),value);}";

                var method = typeof(StaticClass).GetMethods().SingleOrDefault(x => x.Name == "Generic_ExtensionMethodWithParameter");

                // When
                var result = ScriptCodeGenerator.Generate(method);

                // Then
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Should_Return_Correctly_Generated_Wrapper_For_Generic_Type_With_Generic_Return_Value()
            {
                const string expected = "public TTest Generic_ExtensionMethodWithGenericReturnValue<TTest>(TTest value){" +
                                        "return Cake.Tests.Unit.Scripting.StaticClass.Generic_ExtensionMethodWithGenericReturnValue<TTest>" +
                                        "(GetContext(),value);}";

                var method = typeof(StaticClass).GetMethods().SingleOrDefault(x => x.Name == "Generic_ExtensionMethodWithGenericReturnValue");

                // When
                var result = ScriptCodeGenerator.Generate(method);

                // Then
                Assert.Equal(expected, result);
            }
        }
    }
}
