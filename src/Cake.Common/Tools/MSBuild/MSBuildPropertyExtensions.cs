// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        internal static string EscapeMSBuildPropertySpecialCharacters(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            var escapedBuilder = new StringBuilder();
            foreach (var c in value)
            {
                if (_escapeLookup.TryGetValue(c, out string newChar))
                {
                    escapedBuilder.Append(newChar);
                }
                else
                {
                    escapedBuilder.Append(c);
                }
            }

            return escapedBuilder.ToString();
        }
    }
}
