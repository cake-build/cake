using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Common
{
    /// <summary>
    /// Contains functionality related to the environment.
    /// </summary>
    [CakeAliasCategory("Environment")]
    public static class EnvironmentAliases
    {
        /// <summary>
        /// Retrieves the value of the environment variable or <c>null</c> if the environment variable do not exist.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="variable">The environment variable.</param>
        /// <returns>The environment variable or <c>null</c> if the environment variable do not exist.</returns>
        /// <example>
        /// <code>
        /// Information("Home directory location: {0}", EnvironmentVariable("HOME") ?? "Unknown location");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static string EnvironmentVariable(this ICakeContext context, string variable)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (variable == null)
            {
                throw new ArgumentNullException("variable");
            }
            return context.Environment.GetEnvironmentVariable(variable);
        }

        /// <summary>
        /// Checks for the existence of a value for a given environment variable.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="variable">The environment variable.</param>
        /// <returns>
        ///   <c>true</c> if the environment variable exist; otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// context
        /// or
        /// variable
        /// </exception>
        /// <example>
        ///   <code>
        /// if (HasEnvironmentVariable("ONLY_AVAIL_UNDER_CI"))
        /// {
        /// Information("Running under CI");
        /// }
        /// else
        /// {
        /// Information("Running locally");
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static bool HasEnvironmentVariable(this ICakeContext context, string variable)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (variable == null)
            {
                throw new ArgumentNullException("variable");
            }
            return context.Environment.GetEnvironmentVariable(variable) != null;
        }
    }
}
