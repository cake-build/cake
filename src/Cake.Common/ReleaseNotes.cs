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
        private readonly Version _version;
        private readonly List<string> _notes;

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
        public Version Version
        {
            get { return _version; }
        }

        /// <summary>
        /// Gets the release notes.
        /// </summary>
        /// <value>The release notes.</value>
        public IReadOnlyList<string> Notes
        {
            get { return _notes; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReleaseNotes"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="notes">The notes.</param>
        public ReleaseNotes(Version version, IEnumerable<string> notes)
        {
            if (version == null)
            {
                throw new ArgumentNullException("version");
            }
            _version = version;
            _notes = new List<string>(notes ?? Enumerable.Empty<string>());
        }
    }
}
