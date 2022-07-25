// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Text;

namespace Cake.Common.Tools.MSBuild
{
    internal static class MSBuildPropertyExtensions
    {
        private static readonly IReadOnlyDictionary<char, string> _escapeLookup = new Dictionary<char, string>
        {
            { ';', "%3B" },
            { ',', "%2C" },
            { ' ', "%20" },
            { '\r', "%0D" },
            { '\n', "%0A" }
        };

        private static readonly HashSet<string> _propertiesNotEscapeSemicolons = new HashSet<string>
        {
            "DefineConstants",
            "ExcludeFilesFromDeployment"
        };

        internal static string BuildMSBuildPropertyParameterString<TValue>(this KeyValuePair<string, TValue> property)
            where TValue : ICollection<string>
        {
            var propertyParameterString = new StringBuilder();
            var last = property.Value.Count - 1;
            var index = 0;

            var escapeSemicolons = property.Key.AllowEscapeSemicolon();
            foreach (var parameter in property.Value)
            {
                if (string.IsNullOrEmpty(parameter))
                {
                    index++;
                    continue;
                }

                propertyParameterString.Append(parameter.EscapeMSBuildPropertySpecialCharacters(escapeSemicolons));
                propertyParameterString.Append(index != last ? ";" : null);

                index++;
            }

            return propertyParameterString.ToString();
        }

        private static string EscapeMSBuildPropertySpecialCharacters(this string value, bool escapeSemicolons)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            var escapedBuilder = new StringBuilder();
            foreach (var c in value)
            {
                if ((!escapeSemicolons && c.Equals(';')) || !_escapeLookup.TryGetValue(c, out var newChar))
                {
                    escapedBuilder.Append(c);
                }
                else
                {
                    escapedBuilder.Append(newChar);
                }
            }

            return escapedBuilder.ToString();
        }

        private static bool AllowEscapeSemicolon(this string propertyName)
        {
            return !_propertiesNotEscapeSemicolons.Contains(propertyName);
        }
    }
}
