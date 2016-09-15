// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Linq;

namespace Cake.Core.IO
{
    /// <summary>
    /// Provides properties and instance methods for working with paths.
    /// This class must be inherited.
    /// </summary>
    public abstract class Path
    {
        private static readonly char[] _invalidPathCharacters;

        /// <summary>
        /// Gets the full path.
        /// </summary>
        /// <value>The full path.</value>
        public string FullPath { get; }

        /// <summary>
        /// Gets a value indicating whether this path is relative.
        /// </summary>
        /// <value>
        /// <c>true</c> if this path is relative; otherwise, <c>false</c>.
        /// </value>
        public bool IsRelative { get; }

        /// <summary>
        /// Gets the segments making up the path.
        /// </summary>
        /// <value>The segments making up the path.</value>
        public string[] Segments { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Path"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        protected Path(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Path cannot be empty.", nameof(path));
            }

            FullPath = path.Replace('\\', '/').Trim();
            FullPath = FullPath == "./" ? string.Empty : FullPath;

            // Remove relative part of a path.
            if (FullPath.StartsWith("./", StringComparison.Ordinal))
            {
                FullPath = FullPath.Substring(2);
            }

            // Remove trailing slashes.
            FullPath = FullPath.TrimEnd('/', '\\');

#if !UNIX
            if (FullPath.EndsWith(":", StringComparison.OrdinalIgnoreCase))
            {
                FullPath = string.Concat(FullPath, "/");
            }
#endif

            // Relative path?
            IsRelative = !System.IO.Path.IsPathRooted(FullPath);

            // Extract path segments.
            Segments = FullPath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (FullPath.StartsWith("/") && Segments.Length > 0)
            {
                Segments[0] = "/" + Segments[0];
            }

            // Validate the path.
            foreach (var character in path)
            {
                if (_invalidPathCharacters.Contains(character))
                {
                    const string format = "Illegal characters in directory path ({0}).";
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, format, character), nameof(path));
                }
            }
        }

        static Path()
        {
            _invalidPathCharacters = System.IO.Path.GetInvalidPathChars().Concat(new[] { '*', '?' }).ToArray();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this path.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return FullPath;
        }
    }
}