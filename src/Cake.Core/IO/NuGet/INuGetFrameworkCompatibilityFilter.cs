using System.Collections.Generic;
using System.Runtime.Versioning;

namespace Cake.Core.IO.NuGet
{
    /// <summary>
    /// Wrapper interface for NuGet's Target Framework compatibility functionality.
    /// </summary>
    public interface INuGetFrameworkCompatibilityFilter
    {
        /// <summary>
        /// Filters the provided list of items, returning only those items compatible with the given project framework.
        /// </summary>
        /// <typeparam name="T">the type of items being filtered</typeparam>
        /// <param name="projectFramework">The project framework.</param>
        /// <param name="items">The items.</param>
        /// <returns>The compatible items. Empty if try is unsuccessful.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// projectFramework
        /// or
        /// items
        /// </exception>
        IEnumerable<T> GetCompatibleItems<T>(FrameworkName projectFramework, IEnumerable<T> items) where T : IFrameworkTargetable;
    }
}