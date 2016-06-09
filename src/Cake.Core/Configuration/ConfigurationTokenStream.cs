// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.Configuration.Parser;

namespace Cake.Core.Configuration
{
    internal sealed class ConfigurationTokenStream : IReadOnlyList<ConfigurationToken>
    {
        private readonly List<ConfigurationToken> _tokens;
        private int _position;

        public ConfigurationToken Current
        {
            get
            {
                return _position >= Count ? null : _tokens[_position];
            }
        }

        public ConfigurationToken this[int index]
        {
            get { return _tokens[index]; }
        }

        public int Count
        {
            get { return _tokens.Count; }
        }

        public ConfigurationTokenStream(IEnumerable<ConfigurationToken> tokens)
        {
            _tokens = new List<ConfigurationToken>(tokens ?? Enumerable.Empty<ConfigurationToken>());
        }

        public ConfigurationToken Peek()
        {
            return _position >= Count ? null : _tokens[_position];
        }

        public ConfigurationToken Expect(ConfigurationTokenKind tokenType, string message)
        {
            if (Current == null || Current.Kind != tokenType)
            {
                throw new InvalidOperationException(message);
            }
            return Current;
        }

        public ConfigurationToken Consume()
        {
            if (_position >= Count)
            {
                return null;
            }
            var token = _tokens[_position];
            _position++;
            return token;
        }

        public IEnumerator<ConfigurationToken> GetEnumerator()
        {
            return _tokens.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
