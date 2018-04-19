// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cake.Core
{
    /// <summary>
    /// Extension methods for ICakeEnvironment
    /// </summary>
    public static class ICakeEnvironmentExtensions
    {
        /// <summary>
        /// Expands the environment variables in the text. Variables shoud be quoted with %.
        /// </summary>
        /// <param name="environment">The cake environment interface</param>
        /// <param name="text">A string containing the names of zero or more environment variables.</param>
        /// <returns>A string with each environment variable replaced by its value.</returns>
        public static string ExpandEnvironmentVariables(this ICakeEnvironment environment, string text)
        {
            var environmentVariables = environment.GetEnvironmentVariables();
            Regex r = new Regex("%(.*?)%");
            var matches = r.Matches(text);
            foreach (Match m in matches)
            {
                string varName = m.Groups[1].Value;
                if (environmentVariables.ContainsKey(varName))
                {
                    text = text.Replace(m.Value, environmentVariables[varName]);
                }
            }
            return text;
        }
    }
}
