// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Cake.Core.Annotations;

namespace Cake.Core.Tests.Data
{
    internal static class MethodAliasGeneratorData
    {
        internal enum TestNestedEnum
        {
            Unknown,
            One,
            Two
        }

        public static void NotAnExtensionMethod()
        {
            throw new NotImplementedException();
        }

        public static void NotAScriptMethod(this ICakeContext context)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static void NonGeneric_ExtensionMethodWithNoParameters(this ICakeContext context)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static string NonGeneric_ExtensionMethodWithReturnValue(this ICakeContext context)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static void NonGeneric_ExtensionMethodWithParameter(this ICakeContext context, int value)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static void NonGeneric_ExtensionMethodWithOutputParameter(this ICakeContext context, out IDisposable arg)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static void NonGeneric_ExtensionMethodWithGenericParameter(this ICakeContext context, Action<int> value)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static void NonGeneric_ExtensionMethodWithGenericExpressionParameter(this ICakeContext context, Expression<Func<string, string>> expression)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static void NonGeneric_ExtensionMethodWithGenericExpressionArrayParameter(this ICakeContext context, Expression<Func<string, string>>[] expression)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static void NonGeneric_ExtensionMethodWithGenericExpressionParamsArrayParameter(this ICakeContext context, params Expression<Func<string, string>>[] expression)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static void NonGeneric_ExtensionMethodWithArrayParameter(this ICakeContext context, string[] values)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static void Generic_ExtensionMethod<TTest>(this ICakeContext context)
        {
            Debug.Assert(typeof(TTest) != null); // ReSharper
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static void Generic_ExtensionMethodWithParameter<TTest>(this ICakeContext context, TTest value)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static TTest Generic_ExtensionMethodWithGenericReturnValue<TTest>(this ICakeContext context, TTest value)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static TOut Generic_ExtensionMethodWithGenericReturnValueAndTypeParamConstraints<TIn, TOut>(this ICakeContext context, TIn arg)
            where TIn : class, new()
            where TOut : System.Collections.ArrayList, IDisposable
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static void NonGeneric_ExtensionMethodWithParameterArray(this ICakeContext context, params int[] values)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        [Obsolete]
        public static void Obsolete_ImplicitWarning_NoMessage(this ICakeContext context)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        [Obsolete("Please use Foo.Bar instead.")]
        public static void Obsolete_ImplicitWarning_WithMessage(this ICakeContext context)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        [Obsolete("Please use Foo.Bar instead.", false)]
        public static void Obsolete_ExplicitWarning_WithMessage(this ICakeContext context)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        [Obsolete("Please use Foo.Bar instead.", true)]
        public static void Obsolete_ExplicitError_WithMessage(this ICakeContext context)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static void NonGeneric_ExtensionMethodWithOptionalObjectParameter(this ICakeContext context, int value, object option = null)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static void NonGeneric_ExtensionMethodWithOptionalBooleanParameter(this ICakeContext context, int value, bool flag = false)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static void NonGeneric_ExtensionMethodWithOptionalStringParameter(this ICakeContext context, int value, string s = @"there is a ""string"" here and a \t tab")
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static void NonGeneric_ExtensionMethodWithOptionalEnumParameter(this ICakeContext context, int value, AttributeTargets targets = AttributeTargets.Class)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static void NonGeneric_ExtensionMethodWithOptionalCharParameter(this ICakeContext context, string s, char c = 's')
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static void NonGeneric_ExtensionMethodWithOptionalDecimalParameter(this ICakeContext context, string s, decimal value = 12.12m)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static void NonGeneric_ExtensionMethodWithOptionalNullableTParameter(this ICakeContext context, string s, int? value = 0)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static void NonGeneric_ExtensionMethodWithOptionalNullableBooleanParameter(this ICakeContext context, string s, bool? value = false)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static void NonGeneric_ExtensionMethodWithOptionalNullableCharParameter(this ICakeContext context, string s, char? value = 's')
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static void NonGeneric_ExtensionMethodWithOptionalNullableEnumParameter(this ICakeContext context, string s, AttributeTargets? targets = AttributeTargets.Class)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static void NonGeneric_ExtensionMethodWithOptionalNullableDecimalParameter(this ICakeContext context, string s, decimal? value = 123.12m)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static void NonGeneric_ExtensionMethodWithOptionalNullableLongParameter(this ICakeContext context, string s, long? value = 1234567890L)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static void NonGeneric_ExtensionMethodWithOptionalNullableDoubleParameter(this ICakeContext context, string s, double? value = 1234567890.12)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static void NonGeneric_ExtensionMethodWithReservedKeywordParameter(this ICakeContext context, int @new)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static void NonGeneric_ExtensionMethodWithGenericCollectionOfNestedType(this ICakeContext context, ICollection<Cake.Core.Tests.Data.MethodAliasGeneratorData.TestNestedEnum> items)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static void NonGeneric_ExtensionMethodWithParameterAttributes(this ICakeContext context, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static dynamic NonGeneric_ExtensionMethodWithDynamicReturnValue(this ICakeContext context)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
#nullable enable
        public static void NonGeneric_ExtensionMethodWithNullableParameter(this ICakeContext context, string? parameter)
#nullable disable
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
#nullable enable
        public static string? NonGeneric_ExtensionMethodWithNullableReturnValue(this ICakeContext context)
#nullable disable
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
#nullable enable
        public static string NonGeneric_ExtensionMethodWithNotNullReturnValue(this ICakeContext context)
#nullable disable
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
#nullable enable
        public static void NonGeneric_ExtensionMethodWithNotNullParameter(this ICakeContext context, string parameter)
#nullable disable
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
#nullable enable
        public static void NonGeneric_ExtensionMethodWithNotNullAndNullableParameters(this ICakeContext context, string notNull, string? nullable)
#nullable disable
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static void NonGeneric_ExtensionMethodWithNotNullParameterInDisabledContext(
            this ICakeContext context,
#nullable enable
            string parameter)
#nullable disable
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
#nullable enable
        public static void NonGeneric_ExtensionMethodWithNullableArrayElements(this ICakeContext context, string?[] values)
#nullable disable
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
#nullable enable
#pragma warning disable SA1011
        public static void NonGeneric_ExtensionMethodWithNullableArray(this ICakeContext context, string[]? values)
#pragma warning restore SA1011
#nullable disable
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
#nullable enable
#pragma warning disable SA1011
        public static void NonGeneric_ExtensionMethodWithNullableArrayAndElements(this ICakeContext context, string?[]? values)
#pragma warning restore SA1011
#nullable disable
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
#nullable enable
        public static void NonGeneric_ExtensionMethodWithNullableGenericArgument(this ICakeContext context, IList<string?> values)
#nullable disable
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
#nullable enable
        public static void NonGeneric_ExtensionMethodWithNullableGenericType(this ICakeContext context, IList<string>? values)
#nullable disable
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
#nullable enable
        public static IList<string?> NonGeneric_ExtensionMethodWithNullableGenericReturn(this ICakeContext context)
#nullable disable
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
#nullable enable
        public static void NonGeneric_ExtensionMethodWithNullableTaskResult(this ICakeContext context, Task<string?> task)
#nullable disable
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
#nullable enable
        public static void NonGeneric_ExtensionMethodWithNullableDictionaryValues(this ICakeContext context, Dictionary<string, string?> values)
#nullable disable
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
#nullable enable
        public static void NonGeneric_ExtensionMethodWithNullableParamsArray(this ICakeContext context, params string?[] values)
#nullable disable
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
#nullable enable
        public static void Generic_ExtensionMethodWithUnconstrainedTypeParameter<TTest>(this ICakeContext context, TTest value)
#nullable disable
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
#nullable enable
        public static TTest Generic_ExtensionMethodWithUnconstrainedTypeParameterReturn<TTest>(this ICakeContext context, TTest value)
#nullable disable
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
#nullable enable
        public static void Generic_ExtensionMethodWithNullableTypeParameter<TTest>(this ICakeContext context, TTest? value)
#nullable disable
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
#nullable enable
        public static void Generic_ExtensionMethodWithNullableClassConstraint<TTest>(this ICakeContext context, TTest value)
            where TTest : class?
#nullable disable
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
#nullable enable
        public static void Generic_ExtensionMethodWithNotNullAndNewConstraints<TTest>(this ICakeContext context, TTest value)
            where TTest : notnull, new()
#nullable disable
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
#nullable enable
        public static void Generic_ExtensionMethodWithNullableTypeParameterArgument<TTest>(this ICakeContext context, IList<TTest?> values)
            where TTest : class
#nullable disable
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
#nullable enable
        public static void Generic_ExtensionMethodWithUnconstrainedTypeParameterArgument<TTest>(this ICakeContext context, IList<TTest> values)
#nullable disable
        {
            throw new NotImplementedException();
        }
    }
}