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
    }
}