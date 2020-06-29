// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotCover
{
    /// <summary>
    /// Contains extensions for <see cref="DotCoverCoverageSettings"/>.
    /// </summary>
    public static class DotCoverCoverageSettingsExtensions
    {
        /// <summary>
        /// Adds the scope.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="scope">The scope.</param>
        /// <typeparam name="T">The settings type, derived from <see cref="DotCoverCoverageSettings"/>.</typeparam>
        /// <returns>The same <see cref="DotCoverCoverageSettings"/> instance so that multiple calls can be chained.</returns>
        public static T WithScope<T>(this T settings, string scope) where T : DotCoverCoverageSettings
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            settings.Scope.Add(scope);
            return settings;
        }

        /// <summary>
        /// Adds the filter.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="filter">The filter.</param>
        /// <typeparam name="T">The settings type, derived from <see cref="DotCoverCoverageSettings"/>.</typeparam>
        /// <returns>The same <see cref="DotCoverCoverageSettings"/> instance so that multiple calls can be chained.</returns>
        public static T WithFilter<T>(this T settings, string filter) where T : DotCoverCoverageSettings
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            settings.Filters.Add(filter);
            return settings;
        }

        /// <summary>
        /// Adds the attribute filter.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="attributeFilter">The filter.</param>
        /// <typeparam name="T">The settings type, derived from <see cref="DotCoverCoverageSettings"/>.</typeparam>
        /// <returns>The same <see cref="DotCoverCoverageSettings"/> instance so that multiple calls can be chained.</returns>
        public static T WithAttributeFilter<T>(this T settings, string attributeFilter) where T : DotCoverCoverageSettings
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            settings.AttributeFilters.Add(attributeFilter);
            return settings;
        }

        /// <summary>
        /// Adds the filter.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="filter">The process filter.</param>
        /// <typeparam name="T">The settings type, derived from <see cref="DotCoverCoverageSettings"/>.</typeparam>
        /// <returns>The same <see cref="DotCoverCoverageSettings"/> instance so that multiple calls can be chained.</returns>
        public static T WithProcessFilter<T>(this T settings, string filter) where T : DotCoverCoverageSettings
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            settings.ProcessFilters.Add(filter);
            return settings;
        }
    }
}