﻿///////////////////////////////////////////////////////////////////////
// Portions of this code was ported from glob-js by Kevin Thompson.
// https://github.com/kthompson/glob-js
///////////////////////////////////////////////////////////////////////

namespace Cake.Core.IO.Globbing.Nodes
{
    internal sealed class UnixRoot : GlobNode
    {
        public override string Render()
        {
            return string.Empty;
        }

        public override void Accept(GlobVisitor visitor, GlobVisitorContext context)
        {
            visitor.VisitUnixRoot(this, context);
        }

        public override string ToString()
        {
            return "$";
        }
    }
}