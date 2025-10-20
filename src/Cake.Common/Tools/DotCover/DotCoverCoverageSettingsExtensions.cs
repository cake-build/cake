// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

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
            ArgumentNullException.ThrowIfNull(settings);
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
            ArgumentNullException.ThrowIfNull(settings);
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
            ArgumentNullException.ThrowIfNull(settings);
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
            ArgumentNullException.ThrowIfNull(settings);
            settings.ProcessFilters.Add(filter);
            return settings;
        }

        /// <summary>
        /// Adds an assembly name to exclude from analysis.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="assemblyName">The assembly name to exclude. Wildcards (*) are allowed.</param>
        /// <typeparam name="T">The settings type, derived from <see cref="DotCoverCoverageSettings"/>.</typeparam>
        /// <returns>The same <see cref="DotCoverCoverageSettings"/> instance so that multiple calls can be chained.</returns>
        public static T WithExcludeAssembly<T>(this T settings, string assemblyName) where T : DotCoverCoverageSettings
        {
            ArgumentNullException.ThrowIfNull(settings);
            settings.ExcludeAssemblies.Add(assemblyName);
            return settings;
        }

        /// <summary>
        /// Adds a fully qualified attribute name to exclude from analysis.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="attributeName">The fully qualified attribute name. Wildcards (*) are allowed.</param>
        /// <typeparam name="T">The settings type, derived from <see cref="DotCoverCoverageSettings"/>.</typeparam>
        /// <returns>The same <see cref="DotCoverCoverageSettings"/> instance so that multiple calls can be chained.</returns>
        public static T WithExcludeAttribute<T>(this T settings, string attributeName) where T : DotCoverCoverageSettings
        {
            ArgumentNullException.ThrowIfNull(settings);
            settings.ExcludeAttributes.Add(attributeName);
            return settings;
        }

        /// <summary>
        /// Adds a process name to ignore during analysis.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="processName">The process name to ignore. Wildcards (*) are allowed.</param>
        /// <typeparam name="T">The settings type, derived from <see cref="DotCoverCoverageSettings"/>.</typeparam>
        /// <returns>The same <see cref="DotCoverCoverageSettings"/> instance so that multiple calls can be chained.</returns>
        public static T WithExcludeProcess<T>(this T settings, string processName) where T : DotCoverCoverageSettings
        {
            ArgumentNullException.ThrowIfNull(settings);
            settings.ExcludeProcesses.Add(processName);
            return settings;
        }
    }
}