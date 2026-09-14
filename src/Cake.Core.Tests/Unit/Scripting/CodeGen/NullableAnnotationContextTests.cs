// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Reflection;
using Cake.Core.Scripting.CodeGen;
using Cake.Core.Tests.Data;
using Xunit;

namespace Cake.Core.Tests.Unit.Scripting.CodeGen
{
    public sealed class NullableAnnotationContextTests
    {
        public sealed class TheGetNullableFlagMethod
        {
            [Theory]
            [InlineData(nameof(ConstraintFixture.NotNull), NullableAnnotationContext.NotAnnotated)]
            [InlineData(nameof(ConstraintFixture.NullableClass), NullableAnnotationContext.Annotated)]
            [InlineData(nameof(ConstraintFixture.NotNullClass), NullableAnnotationContext.NotAnnotated)]
            [InlineData(nameof(ConstraintFixture.UnconstrainedInDisabledContext), NullableAnnotationContext.Oblivious)]
            public void Should_Read_Flag_Of_Generic_Parameter(string name, byte expected)
            {
                // Given
                var argument = GetGenericArgument(name);

                // When
                var result = NullableAnnotationContext.GetNullableFlag(argument);

                // Then
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Should_Throw_If_Generic_Parameter_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => NullableAnnotationContext.GetNullableFlag(null));

                // Then
                AssertEx.IsArgumentNullException(result, "genericParameter");
            }
        }

        public sealed class TheRequiresMethod
        {
            [Fact]
            public void Should_Return_False_For_Oblivious_Alias()
            {
                // Given
                var method = GetMethod(nameof(MethodAliasGeneratorData.NonGeneric_ExtensionMethodWithNoParameters));

                // When
                var result = NullableAnnotationContext.Requires(method);

                // Then
                Assert.False(result);
            }

            [Theory]
            [InlineData(nameof(MethodAliasGeneratorData.NonGeneric_ExtensionMethodWithNullableParameter))]
            [InlineData(nameof(MethodAliasGeneratorData.NonGeneric_ExtensionMethodWithNullableReturnValue))]
            [InlineData(nameof(MethodAliasGeneratorData.NonGeneric_ExtensionMethodWithNotNullAndNullableParameters))]
            [InlineData(nameof(MethodAliasGeneratorData.NonGeneric_ExtensionMethodWithNullableGenericArgument))]
            [InlineData(nameof(MethodAliasGeneratorData.NonGeneric_ExtensionMethodWithNullableArrayElements))]
            [InlineData(nameof(MethodAliasGeneratorData.Generic_ExtensionMethodWithNullableTypeParameter))]
            [InlineData(nameof(MethodAliasGeneratorData.Generic_ExtensionMethodWithNullableClassConstraint))]
            [InlineData(nameof(MethodAliasGeneratorData.Generic_ExtensionMethodWithNotNullAndNewConstraints))]
            [InlineData(nameof(MethodAliasGeneratorData.Generic_ExtensionMethodWithNullableTypeParameterArgument))]
            public void Should_Return_True_For_Nullable_Aware_Alias(string name)
            {
                // Given
                var method = GetMethod(name);

                // When
                var result = NullableAnnotationContext.Requires(method);

                // Then
                Assert.True(result);
            }

            [Theory]
            [InlineData(nameof(MethodAliasGeneratorData.NonGeneric_ExtensionMethodWithNotNullReturnValue))]
            [InlineData(nameof(MethodAliasGeneratorData.NonGeneric_ExtensionMethodWithNotNullParameter))]
            [InlineData(nameof(MethodAliasGeneratorData.NonGeneric_ExtensionMethodWithNotNullParameterInDisabledContext))]

            // an unconstrained type parameter may be a nullable reference type, but that's not the
            // same as the alias author annotating it, and annotating it anyway breaks the call forward
            [InlineData(nameof(MethodAliasGeneratorData.Generic_ExtensionMethodWithUnconstrainedTypeParameter))]
            [InlineData(nameof(MethodAliasGeneratorData.Generic_ExtensionMethodWithUnconstrainedTypeParameterReturn))]
            [InlineData(nameof(MethodAliasGeneratorData.Generic_ExtensionMethodWithUnconstrainedTypeParameterArgument))]
            public void Should_Return_False_For_Alias_Without_Annotations(string name)
            {
                // Given
                var method = GetMethod(name);

                // When
                var result = NullableAnnotationContext.Requires(method);

                // Then
                Assert.False(result);
            }

            [Theory]
            [InlineData(nameof(ConstraintFixture.NotNull), true)]
            [InlineData(nameof(ConstraintFixture.NullableClass), true)]
            [InlineData(nameof(ConstraintFixture.NotNullClass), false)]
            [InlineData(nameof(ConstraintFixture.UnconstrainedInEnabledContext), false)]
            [InlineData(nameof(ConstraintFixture.UnconstrainedInDisabledContext), false)]
            public void Should_Consider_Nullable_Aware_Generic_Constraints(string name, bool expected)
            {
                // Given
                var method = GetFixtureMethod(name);

                // When
                var result = NullableAnnotationContext.Requires(method);

                // Then
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Should_Throw_If_Method_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => NullableAnnotationContext.Requires(null));

                // Then
                AssertEx.IsArgumentNullException(result, "method");
            }
        }

        public sealed class TheFormatMethod
        {
            [Theory]
            [InlineData(nameof(MethodAliasGeneratorData.NonGeneric_ExtensionMethodWithNullableParameter), "System.String?")]
            [InlineData(nameof(MethodAliasGeneratorData.NonGeneric_ExtensionMethodWithNotNullParameter), "System.String")]
            [InlineData(nameof(MethodAliasGeneratorData.NonGeneric_ExtensionMethodWithNullableArrayElements), "System.String?[]")]
            [InlineData(nameof(MethodAliasGeneratorData.NonGeneric_ExtensionMethodWithNullableArray), "System.String[]?")]
            [InlineData(nameof(MethodAliasGeneratorData.NonGeneric_ExtensionMethodWithNullableArrayAndElements), "System.String?[]?")]
            [InlineData(nameof(MethodAliasGeneratorData.NonGeneric_ExtensionMethodWithNullableGenericArgument), "System.Collections.Generic.IList<System.String?>")]
            [InlineData(nameof(MethodAliasGeneratorData.NonGeneric_ExtensionMethodWithNullableGenericType), "System.Collections.Generic.IList<System.String>?")]
            [InlineData(nameof(MethodAliasGeneratorData.NonGeneric_ExtensionMethodWithNullableTaskResult), "System.Threading.Tasks.Task<System.String?>")]
            [InlineData(nameof(MethodAliasGeneratorData.NonGeneric_ExtensionMethodWithNullableDictionaryValues), "System.Collections.Generic.Dictionary<System.String, System.String?>")]
            [InlineData(nameof(MethodAliasGeneratorData.Generic_ExtensionMethodWithUnconstrainedTypeParameter), "TTest")]
            [InlineData(nameof(MethodAliasGeneratorData.Generic_ExtensionMethodWithNullableTypeParameter), "TTest?")]
            [InlineData(nameof(MethodAliasGeneratorData.Generic_ExtensionMethodWithNullableClassConstraint), "TTest")]
            [InlineData(nameof(MethodAliasGeneratorData.Generic_ExtensionMethodWithNotNullAndNewConstraints), "TTest")]
            [InlineData(nameof(MethodAliasGeneratorData.Generic_ExtensionMethodWithNullableTypeParameterArgument), "System.Collections.Generic.IList<TTest?>")]
            [InlineData(nameof(MethodAliasGeneratorData.Generic_ExtensionMethodWithUnconstrainedTypeParameterArgument), "System.Collections.Generic.IList<TTest>")]
            public void Should_Format_Parameter_With_Inner_Annotations(string name, string expected)
            {
                // Given
                var method = GetMethod(name);
                var parameter = method.GetParameters()[1];

                // When
                var result = NullableAnnotationContext.Format(parameter);

                // Then
                Assert.Equal(expected, result);
            }

            [Theory]
            [InlineData(nameof(MethodAliasGeneratorData.NonGeneric_ExtensionMethodWithNullableReturnValue), "System.String?")]
            [InlineData(nameof(MethodAliasGeneratorData.NonGeneric_ExtensionMethodWithNotNullReturnValue), "System.String")]
            [InlineData(nameof(MethodAliasGeneratorData.NonGeneric_ExtensionMethodWithNullableGenericReturn), "System.Collections.Generic.IList<System.String?>")]
            [InlineData(nameof(MethodAliasGeneratorData.NonGeneric_ExtensionMethodWithDynamicReturnValue), "dynamic")]
            [InlineData(nameof(MethodAliasGeneratorData.NonGeneric_ExtensionMethodWithNoParameters), "void")]
            [InlineData(nameof(MethodAliasGeneratorData.Generic_ExtensionMethodWithUnconstrainedTypeParameterReturn), "TTest")]
            public void Should_Format_Return_With_Inner_Annotations(string name, string expected)
            {
                // Given
                var method = GetMethod(name);

                // When
                var result = NullableAnnotationContext.FormatReturn(method);

                // Then
                Assert.Equal(expected, result);
            }
        }

        public sealed class TheWrapMethod
        {
            [Fact]
            public void Should_Lead_With_Enable_And_End_With_Restore()
            {
                // Given
                const string code = "public void Foo() { }";

                // When
                var result = NullableAnnotationContext.Wrap(code).NormalizeGeneratedCode();

                // Then
                Assert.Equal(
                    "#nullable enable\r\npublic void Foo() { }\r\n#nullable restore".NormalizeLineEndings(),
                    result.NormalizeLineEndings());
            }

            [Fact]
            public void Should_Not_Wrap_Oblivious_Alias()
            {
                // Given
                var method = GetMethod(nameof(MethodAliasGeneratorData.NonGeneric_ExtensionMethodWithNoParameters));

                // When
                var result = NullableAnnotationContext.WrapIfRequired(method, "public void Foo() { }");

                // Then
                Assert.Equal("public void Foo() { }", result);
            }
        }

        private static MethodInfo GetMethod(string name)
        {
            return typeof(MethodAliasGeneratorData).GetMethods().Single(method => method.Name == name);
        }

        private static MethodInfo GetFixtureMethod(string name)
        {
            return typeof(ConstraintFixture).GetMethods().Single(method => method.Name == name);
        }

        private static Type GetGenericArgument(string name)
        {
            return GetFixtureMethod(name).GetGenericArguments()[0];
        }

        private static class ConstraintFixture
        {
            public static void UnconstrainedInDisabledContext<T>(T value)
            {
                throw new NotImplementedException();
            }

#nullable enable
            public static void NotNull<T>(T value)
                where T : notnull
            {
                throw new NotImplementedException();
            }

            public static void NullableClass<T>(T value)
                where T : class?
            {
                throw new NotImplementedException();
            }

            public static void NotNullClass<T>(T value)
                where T : class
            {
                throw new NotImplementedException();
            }

            public static void UnconstrainedInEnabledContext<T>(T value)
            {
                throw new NotImplementedException();
            }
#nullable disable
        }
    }
}
