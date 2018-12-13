// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Cake.Core.IO
{
    /// <summary>
    /// Provides properties and instance methods for working with paths.
    /// This class must be inherited.
    /// </summary>
    public abstract class Path
    {
        /// <summary>
        /// Gets the full path.
        /// </summary>
        /// <value>The full path.</value>
        public string FullPath { get; }

        /// <summary>
        /// Gets a value indicating whether or not this path is relative.
        /// </summary>
        /// <value>
        /// <c>true</c> if this path is relative; otherwise, <c>false</c>.
        /// </value>
        public bool IsRelative { get; }

        /// <summary>
        /// Gets a value indicating whether or not this path is an UNC path.
        /// </summary>
        /// <value>
        /// <c>true</c> if this path is an UNC path; otherwise, <c>false</c>.
        /// </value>
        public bool IsUNC { get; }

        /// <summary>
        /// Gets the separator this path was normalized with.
        /// </summary>
        public char Separator { get; }

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
            path = path?.Trim() ?? string.Empty;

            IsUNC = path.StartsWith(@"\\");
            Separator = IsUNC ? '\\' : '/';

            if (IsUNC)
            {
                FullPath = path.Replace('/', Separator).Trim();
            }
            else
            {
                FullPath = path.Replace('\\', Separator).Trim();
            }

            // Relative paths are considered empty.
            FullPath = FullPath == "./" ? string.Empty : FullPath;

            // Remove relative part of a path.
            if (FullPath.StartsWith("./", StringComparison.Ordinal))
            {
                FullPath = FullPath.Substring(2);
            }

            // Remove trailing slashes.
            if (FullPath.Length > 1)
            {
                FullPath = FullPath.TrimEnd(Separator);
                if (IsUNC && string.IsNullOrWhiteSpace(FullPath))
                {
                    FullPath = @"\\";
                }
            }

            // Potential Windows path?
            if (FullPath.Length == 2)
            {
                if (FullPath.EndsWith(":", StringComparison.OrdinalIgnoreCase))
                {
                    FullPath = string.Concat(FullPath, Separator);
                }
            }

            // Relative path?
            IsRelative = !PathHelper.IsPathRooted(FullPath);

            // Extract path segments.
            Segments = FullPath.Split(new[] { Separator }, StringSplitOptions.RemoveEmptyEntries);
            if (!IsUNC)
            {
                if (FullPath.StartsWith("/") && Segments.Length > 0)
                {
                    Segments[0] = "/" + Segments[0];
                }
            }
            else
            {
                if (FullPath.StartsWith(@"\\"))
                {
                    if (Segments.Length > 0)
                    {
                        // Treat \\ as its own segment.
                        var segments = new string[Segments.Length + 1];
                        segments[0] = @"\\";
                        Segments.CopyTo(segments, 1);
                        Segments = segments;
                    }
                    else
                    {
                        Segments = new[] { @"\\" };
                    }
                }
            }
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
