using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Common
{
    /// <summary>
    /// Added helpers to retrieve enviroment information
    /// </summary>
    [CakeAliasCategory("Environment")]
    public static class EnvironmentExtensions
    {
        /// <summary>
        /// Retrieves the value of the enviroment variable.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="variable">The environment variable.</param>
        /// <returns></returns>
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
        /// Checks for the existance of a value for a given enviroment variable.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="variable">The environment variable.</param>
        /// <returns></returns>
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

            return !string.IsNullOrWhiteSpace(context.Environment.GetEnvironmentVariable(variable));
        }
    }
}
