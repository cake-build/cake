// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;

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
        /// <param name="scope">The scope.</param>
        /// <typeparam name="T">The settings type, derived from <see cref="DotCoverSettings"/></typeparam>
        /// <returns>The same <see cref="DotCoverSettings"/> instance so that multiple calls can be chained.</returns>
        public static T WithScope<T>(this T settings, string scope) where T : DotCoverSettings
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            settings.Scope.Add(scope);
            return settings;
        }

        /// <summary>
        /// Adds the filter
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="filter">The filter.</param>
        /// <typeparam name="T">The settings type, derived from <see cref="DotCoverSettings"/></typeparam>
        /// <returns>The same <see cref="DotCoverSettings"/> instance so that multiple calls can be chained.</returns>
        public static T WithFilter<T>(this T settings, string filter) where T : DotCoverSettings
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            settings.Filters.Add(filter);
            return settings;
        }

        /// <summary>
        /// Adds the attribute filter
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="attributeFilter">The filter.</param>
        /// <typeparam name="T">The settings type, derived from <see cref="DotCoverSettings"/></typeparam>
        /// <returns>The same <see cref="DotCoverSettings"/> instance so that multiple calls can be chained.</returns>
        public static T WithAttributeFilter<T>(this T settings, string attributeFilter) where T : DotCoverSettings
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            settings.AttributeFilters.Add(attributeFilter);
            return settings;
        }
    }
}
