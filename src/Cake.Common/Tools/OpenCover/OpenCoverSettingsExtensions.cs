// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;

namespace Cake.Common.Tools.OpenCover
{
    /// <summary>
    /// Contains extensions for <see cref="OpenCoverSettings"/>.
    /// </summary>
    public static class OpenCoverSettingsExtensions
    {
        /// <summary>
        /// Adds the filter.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>The same <see cref="OpenCoverSettings"/> instance so that multiple calls can be chained.</returns>
        public static OpenCoverSettings WithFilter(this OpenCoverSettings settings, string filter)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            settings.Filters.Add(filter);
            return settings;
        }

        /// <summary>
        /// Exclude a class or method by filter
        /// that match attributes that have been applied.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>The same <see cref="OpenCoverSettings"/> instance so that multiple calls can be chained.</returns>
        public static OpenCoverSettings ExcludeByAttribute(this OpenCoverSettings settings, string filter)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            settings.ExcludedAttributeFilters.Add(filter);
            return settings;
        }

        /// <summary>
        /// Exclude a class (or methods) by filter that match the filenames.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>The same <see cref="OpenCoverSettings"/> instance so that multiple calls can be chained.</returns>
        public static OpenCoverSettings ExcludeByFile(this OpenCoverSettings settings, string filter)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            settings.ExcludedFileFilters.Add(filter);
            return settings;
        }
    }
}
