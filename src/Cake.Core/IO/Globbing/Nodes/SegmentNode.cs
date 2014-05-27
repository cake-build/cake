///////////////////////////////////////////////////////////////////////
// Portions of this code was ported from glob-js by Kevin Thompson.
// https://github.com/kthompson/glob-js
///////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Linq;

namespace Cake.Core.IO.Globbing.Nodes
{
    internal sealed class SegmentNode : Node
    {
        private readonly List<Node> _items;

        public override bool IsWildcard
        {
            get { return _items.Any(x => x.IsWildcard); }
        }

        public SegmentNode(List<Node> items)
        {
            _items = items;
        }

        public override string Render()
        {
            return string.Join(string.Empty, _items.Select(x => x.Render()));
        }
    }
}