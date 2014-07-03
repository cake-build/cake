using System.ComponentModel;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Common
{
    public static class ArgumentExtensions
    {
        [CakeMethodAlias]
        public static bool HasArgument(this ICakeContext context, string key)
        {
            return context.Arguments.HasArgument(key);
        }

        [CakeMethodAlias]
        public static T Argument<T>(this ICakeContext context, string key)
        {
            var value = context.Arguments.GetArgument(key);
            if (value == null)
            {
                throw new CakeException("Argument was not set.");
            }
            return Convert<T>(value);
        }

        [CakeMethodAlias]
        public static T Argument<T>(this ICakeContext context, string key, T defaultValue)
        {
            var value = context.Arguments.GetArgument(key);
            return value == null
                ? defaultValue
                : Convert<T>(value);
        }

        private static T Convert<T>(string value)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)converter.ConvertFromInvariantString(value);
        }
    }
}
