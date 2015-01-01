using Cake.Core.Annotations;

namespace Cake.Core.Tests.Fixtures
{
    internal static class PropertyAliasGeneratorFixture
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
        public static int PropertyAliasReturningInteger(this ICakeContext context)
        {
            return 42;
        }
    }
}
