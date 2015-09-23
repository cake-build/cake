using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core.IO.Globbing.Nodes;

namespace Cake.Core.IO.Globbing
{
    internal static class GlobNodeValidator
    {
        public static void Validate(GlobNode node)
        {
            var previous = (GlobNode)null;
            var current = node;
            while (current != null)
            {
                if (previous != null && previous is RecursiveWildcardSegment)
                {
                    if (current is ParentSegment)
                    {
                        throw new NotSupportedException("Visiting a parent that is a recursive wildcard is not supported.");
                    }
                }
                previous = current;
                current = current.Next;
            }
        }
    }
}
