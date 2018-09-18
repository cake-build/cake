using System.Collections.Generic;

namespace Cake.Core.IO.Globbing
{
    internal sealed class GlobTokenBuffer
    {
        private readonly Queue<GlobToken> _tokens;

        public GlobTokenBuffer(IEnumerable<GlobToken> tokens)
        {
            _tokens = new Queue<GlobToken>(tokens);
        }

        public GlobToken Peek()
        {
            if (_tokens.Count == 0)
            {
                return null;
            }
            return _tokens.Peek();
        }

        public GlobToken Read()
        {
            if (_tokens.Count == 0)
            {
                return null;
            }
            return _tokens.Dequeue();
        }
    }
}
