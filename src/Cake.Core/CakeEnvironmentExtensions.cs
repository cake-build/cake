// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.RegularExpressions;

namespace Cake.Core
{
    /// <summary>
    /// Contains extensions for <see cref="ICakeEnvironment"/>.
    /// </summary>
    public static class CakeEnvironmentExtensions
    {
        private static readonly Regex _regex = new Regex("%(.*?)%");

        /// <summary>
        /// Expands the environment variables in the provided text.
        /// </summary>
        /// <example>
        /// <code>
        /// var expanded = environment.ExpandEnvironmentVariables("%APPDATA%/foo");
        /// </code>
        /// </example>
        /// <param name="environment">The environment.</param>
        /// <param name="text">A string containing the names of zero or more environment variables.</param>
        /// <returns>A string with each environment variable replaced by its value.</returns>
        public static string ExpandEnvironmentVariables(this ICakeEnvironment environment, string text)
        {
            var variables = environment.GetEnvironmentVariables();

            var matches = _regex.Matches(text);
            foreach (Match match in matches)
            {
                string value = match.Groups[1].Value;
                if (variables.ContainsKey(value))
                {
                    text = text.Replace(match.Value, variables[value]);
                }
            }

            return text;
        }
    }
}
