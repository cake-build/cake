using System;
using System.Diagnostics;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Tests.Fixtures
{
    internal static class RoslynMethodAliasGeneratorFixture
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
    }
}