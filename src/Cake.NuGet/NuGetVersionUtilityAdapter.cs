// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using NuGet;

namespace Cake.NuGet
{
    /// <summary>
    /// Adapts NuGet's <see cref="VersionUtility" /> to <see cref="INuGetFrameworkCompatibilityFilter" />,
    /// <see cref="IFrameworkNameParser" />.
    /// </summary>
    public sealed class NuGetVersionUtilityAdapter : INuGetFrameworkCompatibilityFilter, IFrameworkNameParser
    {
        /// <summary>
        /// This function tries to normalize a string that represents framework version names
        /// (the name of a framework version-specific folder in a nuget package, i.e., "net451" or "net35") into
        /// something a framework name that the package manager understands.
        /// </summary>
        /// <param name="frameworkName">value to be parsed.</param>
        /// <returns>
        /// A FrameworkName instance corresponding with the provided frameworkName token.
        /// When parsing is unsuccessful, returns a FrameworkName with an Identifier property of "Unsupported."
        /// </returns>
        /// <exception cref="ArgumentNullException">when frameworkName is null.</exception>
        public FrameworkName ParseFrameworkName(string frameworkName)
        {
            if (frameworkName == null)
            {
                throw new ArgumentNullException("frameworkName");
            }

            return VersionUtility.ParseFrameworkName(frameworkName);
        }

        /// <summary>
        /// Filters the provided list of items, returning only those items compatible with the given project framework.
        /// </summary>
        /// <typeparam name="T">the type of items being filtered</typeparam>
        /// <param name="projectFramework">The project framework.</param>
        /// <param name="items">The items.</param>
        /// <returns>The compatible items. Empty if try is unsuccessful.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// projectFramework or items
        /// </exception>
        public IEnumerable<T> GetCompatibleItems<T>(FrameworkName projectFramework, IEnumerable<T> items)
            where T : IFrameworkTargetable
        {
            if (projectFramework == null)
            {
                throw new ArgumentNullException("projectFramework");
            }
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            var nugetItems = items.Select(item => new FrameworkTargetableWrapper<T>(item)).ToArray();
            IEnumerable<FrameworkTargetableWrapper<T>> compatibleNugetItems;
            if (VersionUtility.TryGetCompatibleItems(projectFramework, nugetItems, out compatibleNugetItems))
            {
                return compatibleNugetItems.Select(item => item.WrappedItem);
            }

            return Enumerable.Empty<T>();
        }

        private class FrameworkTargetableWrapper<T> : global::NuGet.IFrameworkTargetable
            where T : IFrameworkTargetable
        {
            private readonly T _inner;

            public FrameworkTargetableWrapper(T inner)
            {
                _inner = inner;
            }

            public T WrappedItem
            {
                get { return _inner; }
            }

            public IEnumerable<FrameworkName> SupportedFrameworks
            {
                get { return _inner.SupportedFrameworks; }
            }
        }
    }
}
