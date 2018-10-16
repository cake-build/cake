// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Cake.Core.IO.Globbing
{
    internal sealed class GlobTokenBuffer
    {
        private readonly Queue<GlobToken> _tokens;

        public int Count => _tokens.Count;

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
