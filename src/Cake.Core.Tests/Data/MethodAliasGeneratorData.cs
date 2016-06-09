// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Diagnostics;
using Cake.Core.Annotations;

namespace Cake.Core.Tests.Data
{
    internal static class MethodAliasGeneratorData
    {
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
        public static void NonGeneric_ExtensionMethodWithGenericParameter(this ICakeContext context, Action<int> value)
        {
            throw new NotImplementedException();
        }

        [CakeMethodAlias]
        public static void Generic_ExtensionMethod<TTest>(this ICakeContext context)
        {
            Debug.Assert(typeof(TTest) != null); // Resharper
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
    }
}
