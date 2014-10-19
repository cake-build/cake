using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Common
{
    /// <summary>
    /// Contains functionality related to the environment.
    /// </summary>
    [CakeAliasCategory("Environment")]
    public static class EnvironmentExtensions
    {
        /// <summary>
        /// Retrieves the value of the environment variable or <c>null</c> if the environment variable do not exist.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="variable">The environment variable.</param>
        /// <returns>The environment variable or <c>null</c> if the environment variable do not exist.</returns>
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
        /// <returns><c>true</c> if the environment variable exist; otherwise <c>false</c>.</returns>
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
