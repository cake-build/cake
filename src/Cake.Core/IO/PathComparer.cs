// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Cake.Core.Polyfill;

namespace Cake.Core.IO
{
    /// <summary>
    /// Compares <see cref="Path"/> instances.
    /// </summary>
    public sealed class PathComparer : IEqualityComparer<Path>, IComparer<Path>
    {
        /// <summary>
        /// The default path comparer.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly PathComparer Default;

        static PathComparer()
        {
            Default = new PathComparer(EnvironmentHelper.IsUnix());
        }

        private readonly StringComparer _stringComparer;

        /// <summary>
        /// Gets a value indicating whether comparison is case sensitive.
        /// </summary>
        /// <value>
        /// <c>true</c> if comparison is case sensitive; otherwise, <c>false</c>.
        /// </value>
        public bool IsCaseSensitive { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathComparer"/> class.
        /// </summary>
        /// <param name="isCaseSensitive">if set to <c>true</c>, comparison is case sensitive.</param>
        public PathComparer(bool isCaseSensitive)
        {
            IsCaseSensitive = isCaseSensitive;
            _stringComparer = isCaseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathComparer"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public PathComparer(ICakeEnvironment environment)
            : this(environment?.Platform?.IsUnix() ?? throw new ArgumentNullException(nameof(environment)))
        {
        }

        /// <summary>
        /// Determines whether the specified <see cref="Path"/> instances are equal.
        /// </summary>
        /// <param name="x">The first <see cref="Path"/> to compare.</param>
        /// <param name="y">The second <see cref="Path"/> to compare.</param>
        /// <returns>
        /// True if the specified <see cref="Path"/> instances are equal; otherwise, false.
        /// </returns>
        public bool Equals(Path x, Path y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }

            return _stringComparer.Equals(x.FullPath, y.FullPath);
        }

        /// <summary>
        /// Returns a hash code for the specified <see cref="Path"/>.
        /// </summary>
        /// <param name="obj">The path.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public int GetHashCode(Path obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return _stringComparer.GetHashCode(obj.FullPath);
        }

        /// <summary>
        /// Compares two Paths and returns an indication of their relative sort order.
        /// </summary>
        /// <param name="x">The first <see cref="Path"/> to compare.</param>
        /// <param name="y">The second <see cref="Path"/> to compare.</param>
        /// <returns>A signed integer that indicates the relative values of x and y.
        /// <list type="table">
        /// <item>
        /// <term>Less than zero</term>
        /// <description>x precedes y in the sort order, or x is null and y is not null.</description>
        /// </item>
        /// <item>
        /// <term>Zero</term>
        /// <description>x is equal to y, or x and y are both null.</description>
        /// </item>
        /// <item>
        /// <term>Greater than zero</term>
        /// <description>
        /// x follows y in the sort order, or y is null and x is not null.
        /// </description>
        /// </item>
        /// </list>
        /// </returns>
        public int Compare([AllowNull] Path x, [AllowNull] Path y)
            => _stringComparer.Compare(x?.FullPath, y?.FullPath);
    }
}