// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.IO;

namespace Cake.Scripting.Mono.CodeGen.Parsing
{
    internal sealed class ScriptBuffer : IDisposable
    {
        private readonly StringReader _reader;

        public int Position
        {
            get; private set;
        }

        public int PreviousToken
        {
            get; private set;
        }

        public int CurrentToken
        {
            get; private set;
        }

        public int NextToken
        {
            get; private set;
        }

        public bool ReachedEnd
        {
            get; private set;
        }

        public ScriptBuffer(string content)
        {
            _reader = new StringReader(content);

            // Set initial values.
            CurrentToken = ' ';
            Position = -1;
        }

        public void Dispose()
        {
            _reader.Dispose();
        }

        public bool Read()
        {
            if (_reader.Peek() == -1)
            {
                ReachedEnd = true;
                return false;
            }

            PreviousToken = CurrentToken;
            CurrentToken = _reader.Read();
            NextToken = _reader.Peek();
            Position++;

            return true;
        }

        public int Peek()
        {
            return _reader.Peek();
        }

        public void EatWhiteSpace()
        {
            while (true)
            {
                if (!Read())
                {
                    break;
                }
                var current = (char)CurrentToken;
                if (!char.IsWhiteSpace(current))
                {
                    break;
                }
            }
        }
    }
}
