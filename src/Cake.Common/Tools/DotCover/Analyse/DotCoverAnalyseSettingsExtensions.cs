using System;

namespace Cake.Common.Tools.DotCover.Analyse
{
    /// <summary>
    /// Contains extensions for <see cref="DotCoverAnalyseSettings"/>.
    /// </summary>
    public static class DotCoverAnalyseSettingsExtensions
    {
        /// <summary>
        /// Adds the scope.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="scope">The scope.</param>
        /// <returns>The same <see cref="DotCoverAnalyseSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotCoverAnalyseSettings WithScope(this DotCoverAnalyseSettings settings, string scope)
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
        /// <returns>The same <see cref="DotCoverAnalyseSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotCoverAnalyseSettings WithFilter(this DotCoverAnalyseSettings settings, string filter)
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
        /// <returns>The same <see cref="DotCoverAnalyseSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotCoverAnalyseSettings WithAttributeFilter(this DotCoverAnalyseSettings settings, string attributeFilter)
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