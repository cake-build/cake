// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.Polyfill;

namespace Cake.Core.IO
{
    internal static class PathCollapser
    {
        public static string Collapse(Path path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            var isUncPath = path.IsUNC;
            var isWindowsPlatform = EnvironmentHelper.IsWindows(EnvironmentHelper.GetPlatformFamily());
            var minStackHeight = 0;
            var stack = new Stack<string>();
            var segments = path.FullPath.Split('/', '\\');
            if (!path.IsRelative)
            {
                if (isUncPath)
                {
                    // first two segments are string.Empty, followed by server and share
                    minStackHeight = 3;
                }
                else if (isWindowsPlatform)
                {
                    // first segment is c:
                    minStackHeight = 1;
                }
                else
                {
                    // first segment is string.Empty
                    minStackHeight = 1;
                }
            }

            foreach (var segment in segments)
            {
                if (segment == ".")
                {
                    continue;
                }
                if (segment == "..")
                {
                    if (stack.Count > minStackHeight)
                    {
                        stack.Pop();
                    }
                    continue;
                }
                stack.Push(segment);
            }
            var collapsed = string.Join(path.Separator.ToString(), stack.Reverse());
            if (collapsed != string.Empty)
            {
                return collapsed;
            }

            if (path.IsRelative)
            {
                return ".";
            }

            if (isUncPath)
            {
                return @"\\";
            }

            return isWindowsPlatform ? path.Segments[0] : "/";
        }
    }
}