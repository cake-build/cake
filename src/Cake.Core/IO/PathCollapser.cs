// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Core.IO
{
    internal static class PathCollapser
    {
        public static string Collapse(Path path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            var stack = new Stack<string>();
            var segments = path.FullPath.Split('/', '\\');
            foreach (var segment in segments)
            {
                if (segment == ".")
                {
                    continue;
                }
                if (segment == "..")
                {
                    if (stack.Count > 1)
                    {
                        stack.Pop();
                    }
                    continue;
                }
                stack.Push(segment);
            }
            string collapsed = string.Join("/", stack.Reverse());
            return collapsed == string.Empty ? "." : collapsed;
        }
    }
}
