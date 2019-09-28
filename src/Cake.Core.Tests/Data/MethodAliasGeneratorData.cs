// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
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
    }
}