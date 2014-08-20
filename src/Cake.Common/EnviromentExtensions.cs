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
        public static string GetEnvVar(this ICakeContext context, string variable)
        {
            return context.Environment.GetEnvironmentVariable(variable);
        }
    }
}
