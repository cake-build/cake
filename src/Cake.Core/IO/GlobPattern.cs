// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core.IO
{
    /// <summary>
    /// The file system globber pattern.
    /// </summary>
    public sealed class GlobPattern
    {
        /// <summary>
        /// Gets the the globber pattern.
        /// </summary>
        public string Pattern { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobPattern"/> class.
        /// </summary>
        /// <param name="pattern">The globber pattern.</param>
        public GlobPattern(string pattern)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern), "Invalid pattern supplied.");
            }

            Pattern = pattern;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="GlobPattern"/>.
        /// </summary>
        /// <param name="pattern">The string pattern.</param>
        /// <returns>A <see cref="GlobPattern"/>.</returns>
        public static implicit operator GlobPattern(string pattern)
        {
            return pattern is null ? null : FromString(pattern);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="GlobPattern"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="pattern">The glob pattern.</param>
        /// <returns>A <see cref="GlobPattern"/>.</returns>
        public static implicit operator string(GlobPattern pattern)
        {
            return pattern?.Pattern;
        }

        /// <summary>
        /// Performs a conversion from <see cref="System.String"/> to <see cref="GlobPattern"/>.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns>A <see cref="GlobPattern"/>.</returns>
        public static GlobPattern FromString(string pattern)
        {
            return new GlobPattern(pattern);
        }

        /// <summary>
        /// Determines whether two <see cref="GlobPattern"/>. instances are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        /// <c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is GlobPattern pattern &&
                   StringComparer.Ordinal.Equals(Pattern, pattern.Pattern);
        }

        /// <summary>
        /// Returns the hash code for the <see cref="GlobPattern"/>.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(Pattern);
        }

         /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => Pattern;
    }
}