// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Text;

namespace Cake.Scripting.Mono.CodeGen.Parsing
{
    internal sealed class ScriptTokenizer : IDisposable
    {
        private readonly ScriptBuffer _buffer;

        public ScriptTokenizer(string content)
        {
            _buffer = new ScriptBuffer(content);
        }

        public void Dispose()
        {
            _buffer.Dispose();
        }

        public ScriptToken GetNextToken()
        {
            _buffer.EatWhiteSpace();

            // Reached the end?
            if (_buffer.ReachedEnd)
            {
                return null;
            }

            // String?
            if (_buffer.CurrentToken == '\"')
            {
                return ParseString();
            }

            // Word or Identifier?
            if (IsWordCharacter((char)_buffer.CurrentToken))
            {
                return ParseWord();
            }

            // Single line comment?
            if (_buffer.CurrentToken == '/' && _buffer.NextToken == '/')
            {
                return SkipSingleLineComment();
            }

            // Multi line comment?
            if (_buffer.CurrentToken == '/' && _buffer.NextToken == '*')
            {
                return SkipMultiLineComment();
            }

            // Some character.
            var tokenType = GetCharacterTokenType();
            return new ScriptToken(
                tokenType,
                ((char)_buffer.CurrentToken).ToString(),
                _buffer.Position, 1);
        }

        private ScriptToken ParseString()
        {
            var start = _buffer.Position;

            var accumulator = new StringBuilder();
            accumulator.Append((char)_buffer.CurrentToken);

            while (true)
            {
                if (!_buffer.Read())
                {
                    throw new InvalidOperationException("Unterminated string literal.");
                }

                accumulator.Append((char)_buffer.CurrentToken);

                var current = (char)_buffer.CurrentToken;
                if (current == '\"')
                {
                    var last = (char)_buffer.PreviousToken;
                    if (last != '\\')
                    {
                        break;
                    }
                }
            }

            return new ScriptToken(
                ScriptTokenType.String,
                accumulator.ToString(),
                start,
                accumulator.Length);
        }

        private ScriptToken ParseWord()
        {
            var start = _buffer.Position;

            var accumulator = new StringBuilder();
            accumulator.Append((char)_buffer.CurrentToken);

            while (true)
            {
                var current = _buffer.Peek();
                if (current == -1 || !IsWordCharacter((char)current))
                {
                    break;
                }
                accumulator.Append((char)current);
                _buffer.Read();  // Nom nom nom
            }

            var type = ScriptTokenType.Word;

            var identity = accumulator.ToString();
            if (identity.Equals("if", StringComparison.Ordinal))
            {
                type = ScriptTokenType.If;
            }
            if (identity.Equals("else", StringComparison.Ordinal))
            {
                type = ScriptTokenType.Else;
            }
            if (identity.Equals("while", StringComparison.Ordinal))
            {
                type = ScriptTokenType.While;
            }
            if (identity.Equals("switch", StringComparison.Ordinal))
            {
                type = ScriptTokenType.Switch;
            }

            return new ScriptToken(
                type,
                accumulator.ToString(),
                start,
                accumulator.Length);
        }

        private ScriptToken SkipSingleLineComment()
        {
            while (true)
            {
                if ((_buffer.CurrentToken == '\r' && _buffer.NextToken == '\n') ||
                    _buffer.CurrentToken == '\n')
                {
                    break;
                }
                if (!_buffer.Read())
                {
                    break;
                }
            }
            return GetNextToken();
        }

        private ScriptToken SkipMultiLineComment()
        {
            while (true)
            {
                if (_buffer.CurrentToken == '*' && _buffer.NextToken == '/')
                {
                    _buffer.Read(); // Nom nom nom
                    break;
                }
                if (!_buffer.Read())
                {
                    break;
                }
            }
            return GetNextToken();
        }

        private ScriptTokenType GetCharacterTokenType()
        {
            switch (_buffer.CurrentToken)
            {
                case '{': return ScriptTokenType.LeftBrace;
                case '}': return ScriptTokenType.RightBrace;
                case '(': return ScriptTokenType.LeftParenthesis;
                case ')': return ScriptTokenType.RightParenthesis;
                case ';': return ScriptTokenType.Semicolon;
            }
            return ScriptTokenType.Character;
        }

        private static bool IsWordCharacter(char current)
        {
            return char.IsLetterOrDigit(current) || current == '_';
        }
    }
}
