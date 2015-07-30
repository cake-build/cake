///////////////////////////////////////////////////////////////////////
// Portions of this code was ported from glob-js by Kevin Thompson.
// https://github.com/kthompson/glob-js
///////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Diagnostics;

namespace Cake.Core.IO.Globbing
{
    [DebuggerDisplay("{ToString(),nq}")]
    internal abstract class GlobNode
    {
        public GlobNode Next { get; internal set; }

        public abstract string Render();

        public abstract void Accept(GlobVisitor visitor, GlobVisitorContext context);

        public override string ToString()
        {
            var parts = new List<string>();
            var current = this;
            while (current != null)
            {
                parts.Add(current.ToString());
                current = current.Next;
            }
            return string.Join("/", parts);
        }
    }
}
