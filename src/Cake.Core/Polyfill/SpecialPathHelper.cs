// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using Cake.Core.IO;

namespace Cake.Core.Polyfill
{
    internal static class SpecialPathHelper
    {
        public static DirectoryPath GetFolderPath(ICakePlatform platform, SpecialPath path)
        {
            if (path == SpecialPath.LocalTemp)
            {
                return new DirectoryPath(System.IO.Path.GetTempPath());
            }

            var result = GetXPlatFolderPath(platform, path);
            if (result != null)
            {
                return new DirectoryPath(result);
            }

            const string format = "The special path '{0}' is not supported.";
            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, format, path));
        }

        private static string GetXPlatFolderPath(ICakePlatform platform, SpecialPath path)
        {
            if (platform.IsUnix())
            {
                return Native.Unix.GetFolder(path);
            }
            else if (platform.Family == PlatformFamily.Windows)
            {
                return Native.Windows.GetFolder(path);
            }
            throw new PlatformNotSupportedException();
        }
    }
}
