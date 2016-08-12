// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Common
{
    /// <summary>
    /// Represent release notes.
    /// </summary>
    public sealed class ReleaseNotes
    {
        private readonly List<string> _notes;

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
        public Version Version { get; }

        /// <summary>
        /// Gets the release notes.
        /// </summary>
        /// <value>The release notes.</value>
        public IReadOnlyList<string> Notes => _notes;

        /// <summary>
        /// Gets the raw text of the line that <see cref="Version"/> was extracted from.
        /// </summary>
        /// <value>The raw text of the Version line.</value>
        public string RawVersionLine { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReleaseNotes"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="notes">The notes.</param>
        /// <param name="rawVersionLine">The raw text of the version line.</param>
        public ReleaseNotes(Version version, IEnumerable<string> notes, string rawVersionLine)
        {
            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }
            Version = version;
            RawVersionLine = rawVersionLine;
            _notes = new List<string>(notes ?? Enumerable.Empty<string>());
        }
    }
}