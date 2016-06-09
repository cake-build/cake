// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core.Annotations;

namespace Cake.Core.Tests.Data
{
    internal static class PropertyAliasGeneratorData
    {
        public static void NotAnExtensionMethod()
        {
        }

        public static void NotAScriptMethod(this ICakeContext context)
        {
        }

        [CakePropertyAlias]
        // ReSharper disable once UnusedTypeParameter
        public static void GenericScriptMethod<T>(this ICakeContext context)
        {
        }

        [CakePropertyAlias]
        public static void PropertyAliasWithMoreThanOneMethod(this ICakeContext context, int number)
        {
        }

        [CakePropertyAlias]
        public static void PropertyAliasWithoutContext(this int number)
        {
        }

        [CakePropertyAlias]
        public static void PropertyAliasReturningVoid(this ICakeContext context)
        {
        }

        [CakePropertyAlias]
        public static int NonCached_Value_Type(this ICakeContext context)
        {
            return 42;
        }

        [CakePropertyAlias(Cache = true)]
        public static string Cached_Reference_Type(this ICakeContext context)
        {
            return "Hello World";
        }

        [CakePropertyAlias(Cache = true)]
        public static bool Cached_Value_Type(this ICakeContext context)
        {
            return true;
        }

        [CakePropertyAlias]
        [Obsolete]
        public static int NonCached_Obsolete_ImplicitWarning_NoMessage(this ICakeContext context)
        {
            throw new NotImplementedException();
        }

        [CakePropertyAlias]
        [Obsolete("Please use Foo.Bar instead.")]
        public static int NonCached_Obsolete_ImplicitWarning_WithMessage(this ICakeContext context)
        {
            throw new NotImplementedException();
        }

        [CakePropertyAlias]
        [Obsolete("Please use Foo.Bar instead.", false)]
        public static int NonCached_Obsolete_ExplicitWarning_WithMessage(this ICakeContext context)
        {
            throw new NotImplementedException();
        }

        [CakePropertyAlias]
        [Obsolete("Please use Foo.Bar instead.", true)]
        public static int NonCached_Obsolete_ExplicitError_WithMessage(this ICakeContext context)
        {
            throw new NotImplementedException();
        }

        [CakePropertyAlias(Cache = true)]
        [Obsolete]
        public static int Cached_Obsolete_ImplicitWarning_NoMessage(this ICakeContext context)
        {
            throw new NotImplementedException();
        }

        [CakePropertyAlias(Cache = true)]
        [Obsolete("Please use Foo.Bar instead.")]
        public static int Cached_Obsolete_ImplicitWarning_WithMessage(this ICakeContext context)
        {
            throw new NotImplementedException();
        }

        [CakePropertyAlias(Cache = true)]
        [Obsolete("Please use Foo.Bar instead.", false)]
        public static int Cached_Obsolete_ExplicitWarning_WithMessage(this ICakeContext context)
        {
            throw new NotImplementedException();
        }

        [CakePropertyAlias(Cache = true)]
        [Obsolete("Please use Foo.Bar instead.", true)]
        public static int Cached_Obsolete_ExplicitError_WithMessage(this ICakeContext context)
        {
            throw new NotImplementedException();
        }
    }
}
