using System.ComponentModel;
using Cake.Core;

namespace Cake.Common.IO
{
    public static class ArgumentExtensions
    {
        public static bool HasArgument(this ICakeContext context, string key)
        {
            return context.Arguments.HasArgument(key);
        }

        public static T Argument<T>(this ICakeContext context, string key)
        {
            var value = context.Arguments.GetArgument(key);
            if (value == null)
            {
                throw new CakeException("Argument was not set.");
            }
            return Convert<T>(value);
        }

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
