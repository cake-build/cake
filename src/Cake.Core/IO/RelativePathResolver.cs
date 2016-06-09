// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Linq;

namespace Cake.Core.IO
{
    internal static class RelativePathResolver
    {
        public static DirectoryPath Resolve(DirectoryPath from, DirectoryPath to)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }
            if (to == null)
            {
                throw new ArgumentNullException("to");
            }
            if (to.IsRelative)
            {
                throw new InvalidOperationException("Target path must be an absolute path.");
            }
            if (from.IsRelative)
            {
                throw new InvalidOperationException("Source path must be an absolute path.");
            }
            if (from.Segments[0] != to.Segments[0])
            {
                throw new InvalidOperationException("Paths must share a common prefix.");
            }

            if (string.CompareOrdinal(from.FullPath, to.FullPath) == 0)
            {
                return new DirectoryPath(".");
            }

            var minimumSegmentsLength = Math.Min(from.Segments.Length, to.Segments.Length);
            var numberOfSharedSegments = 1;

            for (var i = 1; i < minimumSegmentsLength; i++)
            {
                if (string.CompareOrdinal(from.Segments[i], to.Segments[i]) != 0)
                {
                    break;
                }

                numberOfSharedSegments++;
            }

            var fromSegments = Enumerable.Repeat("..", from.Segments.Length - numberOfSharedSegments);
            var toSegments = to.Segments.Skip(numberOfSharedSegments);

            var relativePath = System.IO.Path.Combine(fromSegments.Concat(toSegments).ToArray());

            return new DirectoryPath(relativePath);
        }
    }
}
