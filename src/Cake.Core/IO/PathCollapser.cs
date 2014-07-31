using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Core.IO
{
    internal sealed class PathCollapser
    {
        public static string Collapse(Path path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            //if (path.IsRelative)
            //{
            //    throw new ArgumentException("Path to be collapsed cannot be relative.", "path");
            //}

            var stack = new Stack<string>();
            var segments = path.FullPath.Split(new[] { '/', '\\' });
            foreach (var segment in segments)
            {
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
            return string.Join("/", stack.Reverse());
        }
    }
}
