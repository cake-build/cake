using System;
using System.Linq;

namespace Cake.Core
{
    /// <summary>
    /// Contains extension methods for <see cref="ICakeArguments"/>.
    /// </summary>
    public static class CakeArgumentsExtensions
    {
        /// <summary>
        /// Gets the value for an argument.
        /// </summary>
        /// <remarks>
        /// If multiple arguments with the same name are
        /// specified, the last argument value is returned.
        /// </remarks>
        /// <param name="arguments">The arguments.</param>
        /// <param name="name">The argument name.</param>
        /// <returns>The argument value.</returns>
        public static string GetArgument(this ICakeArguments arguments, string name)
        {
            return arguments.GetArguments(name).LastOrDefault();
        }

        /// <summary>
        /// Gets the value for an argument or uses default value.
        /// </summary>
        /// <remarks>
        /// If multiple arguments with the same name are
        /// specified, the last argument value is returned.
        /// </remarks>
        /// <param name="arguments">The arguments.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The argument value.</returns>
        public static string GetArgumentOrDefault(this ICakeArguments arguments, string name, string defaultValue)
        {
            return arguments.GetArgument(name) ?? defaultValue;
        }
    }
}