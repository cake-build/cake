// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotCover
{
    /// <summary>
    /// Contains extensions for <see cref="DotCoverSettings"/>.
    /// </summary>
    public static class DotCoverSettingsExtensions
    {
        /// <summary>
        /// Adds the scope.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="configFile">The DotCover configuration file.</param>
        /// <typeparam name="T">The settings type, derived from <see cref="DotCoverSettings"/>.</typeparam>
        /// <returns>The same <see cref="DotCoverSettings"/> instance so that multiple calls can be chained.</returns>
        public static T WithConfigFile<T>(this T settings, FilePath configFile)
            where T : DotCoverSettings
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.ConfigFile = configFile;
            return settings;
        }
    }
}
