// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNet.NuGet.Locals;

/// <summary>
/// Parses English <c>dotnet nuget locals --list</c> output.
/// </summary>
public static partial class DotNetNuGetLocalsParser
{
    [GeneratedRegex(@"^(?<key>http-cache|global-packages|temp|plugins-cache):\s*(?<path>.+)$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)]
    private static partial Regex LocalsLine();

    /// <summary>
    /// Parses redirected standard output from <c>dotnet nuget locals --list</c>.
    /// </summary>
    /// <param name="lines">The output lines.</param>
    /// <returns>The parsed local folder paths.</returns>
    public static DotNetNuGetLocalsListResult Parse(IEnumerable<string> lines)
    {
        DirectoryPath httpCache = null;
        DirectoryPath globalPackages = null;
        DirectoryPath temp = null;
        DirectoryPath pluginsCache = null;

        if (lines == null)
        {
            return new DotNetNuGetLocalsListResult(httpCache, globalPackages, temp, pluginsCache);
        }

        foreach (var raw in lines)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                continue;
            }

            var match = LocalsLine().Match(raw.Trim());
            if (!match.Success)
            {
                continue;
            }

            var path = new DirectoryPath(match.Groups["path"].Value.Trim());
            switch (match.Groups["key"].Value.ToLowerInvariant())
            {
                case "http-cache":
                    httpCache = path;
                    break;
                case "global-packages":
                    globalPackages = path;
                    break;
                case "temp":
                    temp = path;
                    break;
                case "plugins-cache":
                    pluginsCache = path;
                    break;
            }
        }

        return new DotNetNuGetLocalsListResult(httpCache, globalPackages, temp, pluginsCache);
    }
}
