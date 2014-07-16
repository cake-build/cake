using System.ComponentModel;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Common
{
    /// <summary>
    /// Contains functionality related to arguments.
    /// </summary>
    public static class ArgumentExtensions
    {
        /// <summary>
        /// Determines whether or not the specified argument exist.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The argument name.</param>
        /// <returns>Whether or not the specified argument exist.</returns>
        [CakeMethodAlias]
        public static bool HasArgument(this ICakeContext context, string name)
        {
            return context.Arguments.HasArgument(name);
        }

        /// <summary>
        /// Gets an argument.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">The context.</param>
        /// <param name="name">The argument name.</param>
        /// <returns>The value of the argument.</returns>
        [CakeMethodAlias]
        public static T Argument<T>(this ICakeContext context, string name)
        {
            var value = context.Arguments.GetArgument(name);
            if (value == null)
            {
                throw new CakeException("Argument was not set.");
            }
            return Convert<T>(value);
        }

        /// <summary>
        /// Gets an argument.
        /// </summary>
        /// <typeparam name="T">The argument type.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="defaultValue">The default value of the argument.</param>
        /// <returns>The value of the argument.</returns>
        [CakeMethodAlias]
        public static T Argument<T>(this ICakeContext context, string name, T defaultValue)
        {
            var value = context.Arguments.GetArgument(name);
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
